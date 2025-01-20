using System;
using System.Diagnostics;

namespace AgOpenGPS.Pgn
{
    public class Pgn236RelayConfig : PgnBase
    {
        private const int _contentSize = 23;
        private const int _pin0Index = 5;

        public Pgn236RelayConfig(
            IPgnErrorPresenter errorPresenter
        )
            : base(236, _contentSize, errorPresenter)
        {
        }

        public void SetPin(int pinNumber, string word)
        {
            Debug.Assert(0 <= pinNumber);
            Debug.Assert(pinNumber < _contentSize);

            int pinIndex = _pin0Index + pinNumber;

            byte value = (byte)int.Parse(word);
            SetInt((byte)pinIndex, value);
        }

    }
}
