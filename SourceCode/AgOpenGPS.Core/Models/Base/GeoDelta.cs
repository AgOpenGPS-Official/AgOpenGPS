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


        // Returns the Z component of the cross product of this GeoDelta and otherDelta,
        // (when this GeoDelta and otherDelta are considered as 3D-vector with Z-component equal to 0)
        public double CrossProductZ(GeoDelta otherDelta)
        {
            return NorthingDelta * otherDelta.EastingDelta - EastingDelta * otherDelta.NorthingDelta;
        }

        public static GeoDelta operator *(double factor, GeoDelta delta)
        {
            return new GeoDelta(factor * delta.NorthingDelta, factor * delta.EastingDelta);
        }
    }
}
