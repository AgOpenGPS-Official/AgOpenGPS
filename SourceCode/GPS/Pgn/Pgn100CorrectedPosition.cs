using System;

namespace AgOpenGPS
{
    public class Pgn100CorrectedPosition : PgnBase
    {
        private const int _longitudeIndex = 5;
        private const int _latitudeIndex = 13;
        private const int _headingIndex = 21;

        public Pgn100CorrectedPosition(
            IPgnErrorPresenter errorPresenter
        )
            : base(100, 24, errorPresenter)
        {
        }

        public double Longitude
        {
            set { SetDoubleToDouble(_longitudeIndex, value); }
        }

        public double Latitude
        {
            set { SetDoubleToDouble(_latitudeIndex, value); }
        }

        public double Heading
        {
            set { SetDoubleToDouble(_headingIndex, value); }
        }

        private void SetDoubleToDouble(int startIndex, double value)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, _message, startIndex, sizeof(double));
        }

    }
}
