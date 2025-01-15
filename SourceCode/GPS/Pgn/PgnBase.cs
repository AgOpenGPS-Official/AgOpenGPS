using System.Diagnostics;

namespace AgOpenGPS
{
    public class PgnBase
    {
        private readonly byte _id;
        private readonly byte _contentSize;
        protected readonly byte[] message;

        public PgnBase(byte id, byte contentSize)
        {
            _id = id;
            _contentSize = contentSize;
            message = new byte[5 + contentSize + 1];
            message[0] = 0x80;
            message[1] = 0x81;
            message[2] = 0x7f;
            message[3] = id;
            message[4] = _contentSize;
        }

        // This exposes the message to all kind of harm from outside the pgn classes.
        // I would prefer to return a ReadOnlySpan<byte> here, but unfortunately, the method Socket.BeginSendTo
        // only accepts byte[].
        public byte[] Message
        {
            get
            {
                ComputeCrc();
                return message;
            }
        }

        public void FindRefactoringBugs(byte[] oldMessage)
        {
            for (int i = 0; i < message.Length - 1; i++)
            {
                Debug.Assert(message[i] == oldMessage[i]);
            }
        }

        private void ComputeCrc()
        {
            int crc = 0;
            for (int i = 2; i < message.Length - 1; i++)
            {
                crc += message[i];
            }
            message[message.Length - 1] = (byte)crc;
        }
    }

}
