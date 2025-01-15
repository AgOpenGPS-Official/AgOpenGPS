﻿using System;

namespace AgOpenGPS
{
    public class PgnBase
    {
        private readonly IPgnErrorPresenter _errorPresenter;
        protected readonly byte[] _message;

        public PgnBase(byte id, byte contentSize, IPgnErrorPresenter errorPresenter)
        {
            _errorPresenter = errorPresenter;
            _message = new byte[5 + contentSize + 1];
            _message[0] = 0x80;
            _message[1] = 0x81;
            _message[2] = 0x7f;
            _message[3] = id;
            _message[4] = contentSize;
            for (int i = 5; i < 5 + contentSize; i++)
            {
                _message[i] = 0;
            }
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
            ComputeCrc();
            for (int i = 0; i < _message.Length; i++)
            {
                if (_message[i] != oldMessage[i])
                {
                    _errorPresenter.PresentRefactoringBug(GetType().ToString(), i, oldMessage[i], _message[i]);
                }
            }
        }

        private void OverflowErrorIntToByte(byte index, int intInputValue, byte byteErrorValue)
        {
            _errorPresenter.PresentOverflowErrorIntToByte(GetType().ToString(), index, intInputValue, byteErrorValue);

        }

        private void OverflowErrorIntToUInt16(byte index, int intInputValue, UInt16 uint16ErrorValue)
        {
            _errorPresenter.PresentOverflowErrorIntToUInt16(GetType().ToString(), index, intInputValue, uint16ErrorValue);
        }

        private void OverflowErrorDoubleToUInt16(byte index, double doubleInputValue, UInt16 uint16ErrorValue)
        {
            _errorPresenter.PresentOverflowErrorDoubleToUInt16(GetType().ToString(), index, doubleInputValue, uint16ErrorValue);
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
            if (byte.MaxValue < inputValue)
            {
                OverflowErrorIntToByte(index, inputValue, b);
                b = byte.MaxValue;
            }
            if (inputValue < byte.MinValue)
            {
                OverflowErrorIntToByte(index, inputValue, b);
                b = byte.MinValue;
            }
            _message[index] = b;
            return b;
        }

        public void SetDoubleLoHi(byte loIndex, double inputValue)
        {
            UInt16 outputValue = unchecked((UInt16)inputValue);
            if (UInt16.MaxValue < inputValue)
            {
                OverflowErrorDoubleToUInt16(loIndex, inputValue, outputValue);
                outputValue = UInt16.MaxValue;
            }
            if (inputValue < UInt16.MinValue)
            {
                OverflowErrorDoubleToUInt16(loIndex, inputValue, outputValue);
                outputValue = unchecked((UInt16)Int16.MinValue);
            }
            _message[loIndex] = checked((byte)(outputValue & byte.MaxValue)); 
            _message[loIndex + 1] = checked((byte)(outputValue >> 8));
        }

        // Overflow detection does not work very well here, because we
        // do not know if we are trying to send an Int16 or an UInt16
        protected void SetIntLoHi(byte loIndex, int inputValue)
        {
            UInt16 uint16 = unchecked((UInt16)inputValue);
            if (UInt16.MaxValue < inputValue)
            {
                OverflowErrorIntToUInt16(loIndex, inputValue, uint16);
                uint16 = UInt16.MaxValue;
            }
            if (inputValue < Int16.MinValue)
            {
                OverflowErrorIntToUInt16(loIndex, inputValue, uint16);
                uint16 = unchecked((UInt16)Int16.MinValue);
            }
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
