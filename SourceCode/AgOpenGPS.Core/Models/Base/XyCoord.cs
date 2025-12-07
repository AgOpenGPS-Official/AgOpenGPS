namespace AgOpenGPS.Core.Models
{
    public struct XyCoord
    {
        public XyCoord(double x, double y)
        {
            X = x;
            Y = y;
        }

        // Do not change the order of X and Y. Vertex2Array expects X before Y
        public double X { get; }
        public double Y { get; }

        public static XyCoord operator +(XyCoord coord, XyDelta delta)
        {
            return new XyCoord(coord.X + delta.DeltaX, coord.Y + delta.DeltaY);
        }

        public static XyCoord operator -(XyCoord coord, XyDelta delta)
        {
            return new XyCoord(coord.X - delta.DeltaX, coord.Y - delta.DeltaY);
        }

    }
}
