namespace AgOpenGPS.Core.Models
{
    public struct GeoBoundingBox
    {
        private GeoCoord _minCoord;
        private GeoCoord _maxCoord;

        public static GeoBoundingBox CreateEmpty()
        {
            GeoCoord minCoord = new GeoCoord(double.MaxValue, double.MaxValue);
            GeoCoord maxCoord = new GeoCoord(double.MinValue, double.MinValue);
            return new GeoBoundingBox(minCoord, maxCoord);
        }

        public GeoBoundingBox(GeoCoord minCoord, GeoCoord maxCoord)
        {
            _minCoord = minCoord;
            _maxCoord = maxCoord;
        }

        public bool IsEmpty =>
            _maxCoord.Northing < _minCoord.Northing &&
            _maxCoord.Easting < _minCoord.Easting;
        public double MinNorthing => _minCoord.Northing;
        public double MaxNorthing => _maxCoord.Northing;
        public double MinEasting => _minCoord.Easting;
        public double MaxEasting => _maxCoord.Easting;
        public GeoCoord MinCoord => _minCoord;
        public GeoCoord MaxCoord => _maxCoord;
        public GeoCoord CenterCoord => _minCoord.Average(_maxCoord);

        public void Include(GeoCoord geoCoord)
        {
            _minCoord = _minCoord.Min(geoCoord);
            _maxCoord = _maxCoord.Max(geoCoord);
        }

        public void Include(GeoBoundingBox bb)
        {
            _minCoord = _minCoord.Min(bb.MinCoord);
            _maxCoord = _maxCoord.Max(bb.MaxCoord);
        }

        public bool IsInside(GeoCoord testCoord)
        {
            return
                _minCoord.Northing <= testCoord.Northing && testCoord.Northing <= _maxCoord.Northing &&
                _minCoord.Easting <= testCoord.Easting && testCoord.Easting <= _maxCoord.Easting;
        }

        public GeoBoundingBox Scaled(double northingFactor, double eastingFactor)
        {
            GeoCoord center = CenterCoord;
            GeoDelta maxCornerDelta = new GeoDelta(center, MaxCoord);
            GeoDelta resizedMaxCornerDelta = new GeoDelta(
                maxCornerDelta.NorthingDelta * northingFactor,
                maxCornerDelta.EastingDelta * eastingFactor);
            return new GeoBoundingBox(center - resizedMaxCornerDelta, center + resizedMaxCornerDelta);
        }

    }

}
