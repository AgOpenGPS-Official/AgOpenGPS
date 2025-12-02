namespace AgOpenGPS.Core.Models
{
    public struct GeoLineSegment
    {
        public GeoLineSegment(GeoCoord coordA, GeoCoord coordB)
        {
            CoordA = coordA;
            CoordB = coordB;
        }

        public GeoCoord CoordA { get; }
        public GeoCoord CoordB { get; }
        public double Length => CoordA.Distance(CoordB);
        public GeoDelta Delta => new GeoDelta(CoordA, CoordB);

        public GeoDir Direction => new GeoDir(Delta);

        // Returns a new GeoLineSegment shifted over 'offset' w.r.t the original
        public GeoLineSegment Shifted(GeoDelta offset)
        {
            return new GeoLineSegment(CoordA + offset, CoordB + offset);
        }

        public GeoCoord? IntersectionPoint(GeoLineSegment otherSegment)
        {
            const double epsilon = 1e-9;

            GeoCoord? intersectionPoint = null;
            GeoDelta delta = Delta;
            GeoDelta otherDelta = otherSegment.Delta;
            double denominator = delta.CrossProductZ(otherDelta);
            if (denominator != 0.0)
            {
                GeoDelta aToOtherADelta = new GeoDelta(CoordA, otherSegment.CoordA);

                double s = aToOtherADelta.CrossProductZ(delta) / denominator;
                if (-epsilon <= s && s <= 1.0 + epsilon)
                {
                    double t = aToOtherADelta.CrossProductZ(otherDelta) / denominator;
                    if (-epsilon <= t && t <= 1.0 + epsilon)
                    {
                        intersectionPoint = CoordA + t * delta;
                    }
                }
            }
            return intersectionPoint;
        }

    }

}
