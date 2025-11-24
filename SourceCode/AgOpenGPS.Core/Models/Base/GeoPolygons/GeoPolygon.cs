using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class GeoPolygon : GeoPathBase
    {
        protected readonly List<GeoCoord> _coords;
        protected bool _signedAreaValid;
        private double _signedArea;
        private bool _bbValid;
        private GeoBoundingBox _boundingBox;

        public GeoPolygon()
        {
            _coords = new List<GeoCoord>();
            Invalidate();
        }

        public override int Count => _coords.Count;

        public override GeoCoord this[int index]
        {
            get { return _coords[index]; }
        }

        public GeoCoord Last => this[Count - 1];

        public double Area => Math.Abs(SignedArea);
        public bool IsClockwise => SignedArea > 0;

        public GeoBoundingBox BoundingBox
        {
            get
            {
                if (!_bbValid)
                {
                    _boundingBox = CalculateBoundingBox();
                    _bbValid = true;
                }
                return _boundingBox;
            }
        }

        private double SignedArea
        {
            get
            {
                if (!_signedAreaValid)
                {
                    _signedArea = CalculateSignedArea();
                    _signedAreaValid = true;
                }
                return _signedArea;
            }
        }

        public void Clear()
        {
            _coords.Clear();
            Invalidate();
        }

        public virtual void Add(GeoCoord coord)
        {
            _coords.Add(coord);
            Invalidate();
        }

        public bool IsFarAwayFromPath(GeoCoord testCoord, double minimumDistanceSquared)
        {
            return _coords.TrueForAll(coordOnPath => coordOnPath.DistanceSquared(testCoord) >= minimumDistanceSquared);
        }

        public bool IsInside(GeoCoord testPoint)
        {
            bool result = false;
            for (int i = 0; i < Count; i++)
            {
                var iCoord = this[i];
                var jCoord = i == 0 ? Last : this[i - 1];
                if ((iCoord.Easting < testPoint.Easting && jCoord.Easting >= testPoint.Easting)
                    || (jCoord.Easting < testPoint.Easting && iCoord.Easting >= testPoint.Easting))
                {
                    if (iCoord.Northing + (testPoint.Easting - iCoord.Easting)
                        / (jCoord.Easting - iCoord.Easting) * (jCoord.Northing - iCoord.Northing)
                        < testPoint.Northing)
                    {
                        result = !result;
                    }
                }
            }
            return result;
        }

        public void ForceClockwiseWinding()
        {
            if (!IsClockwise)
            {
                ReverseWinding();
            }
        }

        public void ForceCounterClockwiseWinding()
        {
            if (IsClockwise)
            {
                ReverseWinding();
            }
        }

        private void ReverseWinding()
        {
            _coords.Reverse();
            _signedArea = -_signedArea;
        }

        private double CalculateSignedArea()
        {
            double area = 0.0;
            int ptCount = _coords.Count;

            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (_coords[j].Easting + _coords[i].Easting) * (_coords[j].Northing - _coords[i].Northing);
            }
            return 0.5 * area;
        }

        private GeoBoundingBox CalculateBoundingBox()
        {
            GeoBoundingBox bb = GeoBoundingBox.CreateEmpty();

            foreach (GeoCoord coord in _coords)
            {
                bb.Include(coord);
            }
            return bb;
        }

        private void Invalidate()
        {
            _signedAreaValid = false;
            _bbValid = false;
        }
    }

}
