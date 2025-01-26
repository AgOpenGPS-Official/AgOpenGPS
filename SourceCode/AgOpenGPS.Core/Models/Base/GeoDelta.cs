using System;

namespace AgOpenGPS.Core.Models
{
    public struct GeoDelta
    {
        public GeoDelta(GeoCoord fromCoord, GeoCoord toCoord)
        {
            NorthingDelta = toCoord.Northing - fromCoord.Northing;
            EastingDelta = toCoord.Easting - fromCoord.Easting;
        }

        public GeoDelta(double northingDelta, double eastingDelta)
        {
            NorthingDelta = northingDelta;
            EastingDelta = eastingDelta;
        }

        public double NorthingDelta { get; }
        public double EastingDelta { get; }

        public double LengthSquared => NorthingDelta * NorthingDelta + EastingDelta * EastingDelta;
        public double Length => Math.Sqrt(LengthSquared);
    }
}
