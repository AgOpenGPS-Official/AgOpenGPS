using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Reads a stream with a time limit per call. A read that times out keeps running, and the next
    /// call waits for that same read, so two reads are never active on one stream.
    /// </summary>
    internal sealed class TimedReader
    {
        private readonly Stream _stream;
        private Task<int> _pending;

        public byte[] Buffer { get; } = new byte[16384];

        public TimedReader(Stream stream)
        {
            _stream = stream;
        }

        /// <summary>Bytes read into <see cref="Buffer"/>, 0 when the other side closed, -1 on timeout.</summary>
        public async Task<int> ReadAsync(TimeSpan timeout, CancellationToken ct)
        {
            if (_pending == null) _pending = _stream.ReadAsync(Buffer, 0, Buffer.Length);
            if (timeout < TimeSpan.Zero) timeout = TimeSpan.Zero;

            using (CancellationTokenSource linked = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                Task delay = Task.Delay(timeout, linked.Token);
                Task done = await Task.WhenAny(_pending, delay).ConfigureAwait(false);
                if (done != _pending)
                {
                    ct.ThrowIfCancellationRequested();
                    return -1;
                }
                linked.Cancel();
                Task<int> read = _pending;
                _pending = null;
                return await read.ConfigureAwait(false);
            }
        }

        /// <summary>Make sure an abandoned read that fails after the stream is closed raises nothing unobserved.</summary>
        public void Abandon()
        {
            Task<int> p = _pending;
            _pending = null;
            if (p != null) p.ContinueWith(t => { var ignored = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
