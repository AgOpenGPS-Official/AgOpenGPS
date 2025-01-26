namespace AgOpenGPS.Core.Models
{
    public abstract class GeoPathBase
    {
        public abstract int Count { get; }

        public abstract GeoCoord this[int i] { get; }

        public int GetClosestPointIndex(GeoCoord coord)
        {
            double closestDistanceSquared = double.MaxValue;
            int closestIndex = -1;
            for (int i = 0; i < Count; i++)
            {
                double distanceSquared = coord.DistanceSquared(this[i]);
                if (distanceSquared < closestDistanceSquared)
                {
                    closestDistanceSquared = distanceSquared;
                    closestIndex = i;
                }
            }
            return closestIndex;
        }

    }
}
