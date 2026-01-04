namespace AgOpenGPS.Core.Models
{
    public struct XyDeltaInt
    {
        public XyDeltaInt(int deltaX, int deltaY)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
        }

        public int DeltaX { get; }
        public int DeltaY { get; }

        public static XyDelta operator *(double factor, XyDeltaInt delta)
        {
            return new XyDelta(factor * delta.DeltaX, factor * delta.DeltaY);
        }
    }
}
