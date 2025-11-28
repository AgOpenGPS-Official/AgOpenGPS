using Accord.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;

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

        public GeoLineSegment GetLineSegment(int index)
        {
            int nextIndex = (index + 1) % _coords.Count;
            return new GeoLineSegment(this[index], this[nextIndex]);
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

        // Return the total length of all consecutive line segments between vertex A and B,
        // (when traveling from A to B in the direction of increasing indices)

        public double GetLength(int indexA, int indexB)
        {
            double len = 0.0;
            if (indexA < indexB)
            {
                for (int i = indexA; i < indexB; i++)
                {
                    len += GetLineSegment(i).Length;
                }
            }
            else
            {
                for (int i = indexA; i < indexB + Count; i++)
                {
                    len += GetLineSegment(i < Count ? i : i - Count).Length;
                }
            }
            return len;
        }

        public int RemoveSelfIntersections()
        {
            int nIntersections = 0;
            while (RemoveFirstIntersection())
            {
                nIntersections++;
            }
            if (0 < nIntersections)
            {
                RemoveCloseNeighbours(0.001);
            }
            return nIntersections;
        }

        private bool RemoveFirstIntersection()
        {
            for (int paIndex = 0; paIndex < Count; paIndex++)
            {
                int pbIndex = (paIndex + 1) % Count;
                GeoLineSegment segmentP = GetLineSegment(paIndex);

                // Skip neighbouring segments
                int qaLast = paIndex == 0 ? Count - 1 : Count;
                for (int qaIndex = paIndex + 2; qaIndex < qaLast; qaIndex++)
                {
                    GeoLineSegment segmentQ = GetLineSegment(qaIndex);
                    GeoCoord? intersectionPoint = segmentP.IntersectionPoint(segmentQ);
                    if (intersectionPoint.HasValue)
                    {
                        GeoCoord ip = intersectionPoint.Value;
                        int qbIndex = (qaIndex + 1) % Count;
                        // Compute lengths to remove the smaller 'half'.
                        double pbqaLength = ip.Distance(this[pbIndex]) + GetLength(pbIndex, qaIndex) + this[qaIndex].Distance(ip);
                        double qbpaLength = ip.Distance(this[qbIndex]) + GetLength(qbIndex, paIndex) + this[paIndex].Distance(ip);
                        if (pbqaLength < qbpaLength)
                        {
                            Replace(pbIndex, qaIndex, intersectionPoint.Value);
                        }
                        else
                        {
                            Replace(qbIndex, paIndex, intersectionPoint.Value);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void Replace(int startIndex, int endIndex, GeoCoord coord)
        {
            if (startIndex < endIndex)
            {
                _coords.RemoveRange(startIndex, endIndex + 1 - startIndex);
                _coords.Insert(startIndex, coord);
            }
            else
            {
                _coords.RemoveRange(startIndex, Count - startIndex);
                _coords.Insert(startIndex, coord);
                _coords.RemoveRange(0, endIndex + 1);
            }
            Invalidate();
        }

        // Returns the number of removed vertices
        public int RemoveCloseNeighbours(double tooCloseDistance)
        {
            double tooCloseDistanceSquared = tooCloseDistance * tooCloseDistance;
            GeoCoord prevVertex = Last;
            int dstIndex = 0;
            for (int srcIndex = 0; srcIndex < _coords.Count; srcIndex++)
            {
                GeoCoord srcVertex = _coords[srcIndex];
                if (tooCloseDistanceSquared < prevVertex.DistanceSquared(srcVertex))
                {
                    prevVertex = _coords[srcIndex];
                    _coords[dstIndex++] = _coords[srcIndex];
                }
            }
            int nRemoved = Count - dstIndex;
            if (0 < nRemoved)
            {
                _coords.RemoveRange(dstIndex, nRemoved);
                Invalidate();
            }
            return nRemoved;
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
