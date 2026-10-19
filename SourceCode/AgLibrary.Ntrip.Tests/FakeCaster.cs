using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip.Tests
{
    /// <summary>One accepted connection, as seen by a script.</summary>
    internal sealed class FakeSession
    {
        private readonly NetworkStream _stream;
        private readonly CancellationToken _stop;
        private readonly StringBuilder _received = new StringBuilder();

        public string Request { get; internal set; } = "";

        /// <summary>Everything the client sent after the request, e.g. GGA sentences.</summary>
        public string ReceivedAfterRequest
        {
            get { lock (_received) return _received.ToString(); }
        }

        internal FakeSession(NetworkStream stream, CancellationToken stop)
        {
            _stream = stream;
            _stop = stop;
        }

        public Task SendAsync(string text) => SendAsync(Encoding.ASCII.GetBytes(text));

        public async Task SendAsync(byte[] data)
        {
            try { await _stream.WriteAsync(data, 0, data.Length, _stop); }
            catch (Exception) { /* the client went away */ }
        }

        public Task WaitAsync(int ms) => Task.Delay(ms, _stop);

        internal void Pump(CancellationToken ct)
        {
            Task.Run(async () =>
            {
                var buf = new byte[1024];
                try
                {
                    while (!ct.IsCancellationRequested)
                    {
                        int n = await _stream.ReadAsync(buf, 0, buf.Length, ct);
                        if (n == 0) break;
                        lock (_received) _received.Append(Encoding.ASCII.GetString(buf, 0, n));
                    }
                }
                catch (Exception) { }
            });
        }
    }

    /// <summary>
    /// A caster on a loopback port that plays one script per connection: the n-th connection runs the
    /// n-th script, and the connection closes when the script ends. A connection with no script left
    /// is closed at once.
    /// </summary>
    internal sealed class FakeCaster : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _stop = new CancellationTokenSource();
        private readonly ConcurrentQueue<Func<FakeSession, Task>> _scripts = new ConcurrentQueue<Func<FakeSession, Task>>();
        private readonly List<FakeSession> _sessions = new List<FakeSession>();

        public int Port { get; }

        /// <summary>The requests received, in connection order.</summary>
        public IReadOnlyList<string> Requests
        {
            get { lock (_sessions) return _sessions.ConvertAll(s => s.Request); }
        }

        public IReadOnlyList<FakeSession> Sessions
        {
            get { lock (_sessions) return new List<FakeSession>(_sessions); }
        }

        public int Connections
        {
            get { lock (_sessions) return _sessions.Count; }
        }

        public FakeCaster()
        {
            _listener = new TcpListener(IPAddress.Loopback, 0);
            _listener.Start();
            Port = ((IPEndPoint)_listener.LocalEndpoint).Port;
            Task.Run(AcceptLoop);
        }

        public FakeCaster Script(Func<FakeSession, Task> script)
        {
            _scripts.Enqueue(script);
            return this;
        }

        /// <summary>The same script for the next <paramref name="count"/> connections.</summary>
        public FakeCaster Script(int count, Func<FakeSession, Task> script)
        {
            for (int i = 0; i < count; i++) _scripts.Enqueue(script);
            return this;
        }

        private async Task AcceptLoop()
        {
            while (!_stop.IsCancellationRequested)
            {
                TcpClient client;
                try { client = await _listener.AcceptTcpClientAsync(); }
                catch (Exception) { return; }
                var ignored = Task.Run(() => Serve(client));
            }
        }

        private async Task Serve(TcpClient client)
        {
            using (client)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    var session = new FakeSession(stream, _stop.Token);
                    lock (_sessions) _sessions.Add(session);

                    // Read the request up to the blank line.
                    var sb = new StringBuilder();
                    var buf = new byte[1];
                    while (!sb.ToString().EndsWith("\r\n\r\n"))
                    {
                        int n = await stream.ReadAsync(buf, 0, 1, _stop.Token);
                        if (n == 0) return;
                        sb.Append((char)buf[0]);
                    }
                    session.Request = sb.ToString();
                    session.Pump(_stop.Token);

                    if (_scripts.TryDequeue(out Func<FakeSession, Task> script))
                        await script(session);
                }
                catch (Exception) { /* cancelled or the client closed */ }
            }
        }

        public void Dispose()
        {
            _stop.Cancel();
            _listener.Stop();
        }
    }
}
