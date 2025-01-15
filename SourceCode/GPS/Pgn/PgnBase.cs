using System;
using System.Diagnostics;

namespace AgOpenGPS
{
    public class PgnBase
    {
        private readonly byte _id;
        private readonly byte _contentSize;
        protected readonly byte[] _message;

        public PgnBase(byte id, byte contentSize)
        {
            _id = id;
            _contentSize = contentSize;
            _message = new byte[5 + contentSize + 1];
            _message[0] = 0x80;
            _message[1] = 0x81;
            _message[2] = 0x7f;
            _message[3] = id;
            _message[4] = _contentSize;
        }

        // This exposes the message to all kind of harm from outside the pgn classes.
        // I would prefer to return a ReadOnlySpan<byte> here, but unfortunately, the method Socket.BeginSendTo
        // only accepts byte[].
        public byte[] Message
        {
            get
            {
                ComputeCrc();
                return _message;
            }
        }

        public void FindRefactoringBugs(byte[] oldMessage)
        {
            for (int i = 0; i < _message.Length - 1; i++)
            {
                if (_message[i] != oldMessage[i])
                {
                    // Set  breakpoint here
                }
            }
        }

        protected void SetBool(byte index, bool boolValue)
        {
            _message[index] = (byte)(boolValue ? 1 : 0);
        }

        protected void SetInt16LoHi(byte loIndex, Int16 int16Value)
        {
            _message[loIndex] = (byte)(int16Value & byte.MaxValue);
            _message[loIndex + 1] = unchecked((byte)(int16Value >> 8));
        }

        protected void SetUInt16LoHi(byte loIndex, UInt16 uint16Value)
        {
            _message[loIndex] = (byte)(uint16Value & byte.MaxValue);
            _message[loIndex + 1] = (byte)(uint16Value >> 8);
        }


        protected byte SetInt(byte index, int inputValue)
        {
            byte b = unchecked((byte)inputValue);
            _message[index] = b;
            return b;
        }

        public void SetDoubleLoHi(byte loIndex, double inputValue)
        {
            int intValue = (int)(inputValue);
            _message[loIndex] = unchecked((byte)intValue);
            _message[loIndex + 1] = checked((byte)(intValue >> 8));
        }

        // Overflow detection does not work very well here, because we
        // do not know if we are trying to send an Int16 or an UInt16
        protected void SetIntLoHi(byte loIndex, int inputValue)
        {
            UInt16 uint16 = unchecked((UInt16)inputValue);
            _message[loIndex] = unchecked((byte)(uint16));
            _message[loIndex + 1] = unchecked((byte)(uint16 >> 8));
        }

        private void ComputeCrc()
        {
            int crc = 0;
            for (int i = 2; i < _message.Length - 1; i++)
            {
                crc += _message[i];
            }
            _message[_message.Length - 1] = (byte)crc;
        }
    }

}
