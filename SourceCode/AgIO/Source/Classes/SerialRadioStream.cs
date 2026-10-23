using System;
using System.IO;
using System.IO.Ports;

namespace AgIO
{
    /// <summary>
    /// A serial radio as a read-only stream for the NTRIP fallback. The port is opened when the stream is
    /// created and closed when it is disposed, which also ends a read that is waiting.
    /// </summary>
    internal sealed class SerialRadioStream : Stream
    {
        private readonly SerialPort port;

        /// <summary>Opens the port; throws when that fails (port in use or not there).</summary>
        public SerialRadioStream(string portName, int baudRate)
        {
            port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            port.Open();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return port.BaseStream.Read(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { if (port.IsOpen) port.Close(); }
                catch (Exception) { /* unplugged */ }
                port.Dispose();
            }
            base.Dispose(disposing);
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }
}
