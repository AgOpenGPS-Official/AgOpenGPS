using System;

namespace AgOpenGPS.Core.Models
{
    public struct GeoLine
    {
        public GeoLine(GeoCoord coordA, GeoCoord coordB)
        {
            CoordA = coordA;
            CoordB = coordB;
        }

        public GeoCoord CoordA { get; }
        public GeoCoord CoordB { get; }

        public GeoDir Direction => new GeoDir(CoordA, CoordB);

        public GeoLine ParallelLine(GeoDelta offset)
        {
            return new GeoLine(CoordA + offset, CoordB + offset);
        }

    }
}