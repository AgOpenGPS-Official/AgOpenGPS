using System;
using System.Collections.Generic;
using System.Globalization;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>One square of a web map: zoom level and tile column/row (OpenStreetMap / Google / Esri scheme).</summary>
    public readonly struct TileId : IEquatable<TileId>
    {
        public TileId(int z, int x, int y)
        {
            Z = z;
            X = x;
            Y = y;
        }

        public int Z { get; }
        public int X { get; }
        public int Y { get; }

        public bool Equals(TileId o) => Z == o.Z && X == o.X && Y == o.Y;
        public override bool Equals(object obj) => obj is TileId t && Equals(t);
        public override int GetHashCode() => (Z * 397 ^ X) * 397 ^ Y;
        public override string ToString() => Z + "/" + X + "/" + Y;
    }

    /// <summary>Web Mercator tile maths for the satellite background map.</summary>
    public static class MapTiles
    {
        public const int TileSize = 256;
        public const int MinZoom = 1;
        /// <summary>Default most detailed level. Sources go to 19-22 in places; rural areas often stop at 17-18.</summary>
        public const int MaxZoom = 19;
        /// <summary>Highest level the app will ever ask for.</summary>
        public const int AbsoluteMaxZoom = 22;

        /// <summary>Esri World Imagery: free with attribution (check the terms for commercial use).</summary>
        public const string DefaultUrl = "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}";
        public const string DefaultAttribution = "Imagery © Esri, Maxar, Earthstar Geographics";

        private const double MaxLat = 85.05112878;

        /// <summary>Fractional tile coordinates of a point at a zoom level.</summary>
        public static void ToTile(double latitude, double longitude, int zoom, out double x, out double y)
        {
            double n = 1 << zoom;
            double lat = Math.Max(-MaxLat, Math.Min(MaxLat, latitude)) * Math.PI / 180.0;
            x = (longitude + 180.0) / 360.0 * n;
            y = (1.0 - Math.Log(Math.Tan(lat) + 1.0 / Math.Cos(lat)) / Math.PI) / 2.0 * n;
        }

        /// <summary>Latitude/longitude of a (fractional) tile coordinate; whole numbers give the top-left corner.</summary>
        public static void ToLatLon(double x, double y, int zoom, out double latitude, out double longitude)
        {
            double n = 1 << zoom;
            longitude = x / n * 360.0 - 180.0;
            latitude = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * y / n))) * 180.0 / Math.PI;
        }

        /// <summary>Metres per map pixel at a zoom level and latitude.</summary>
        public static double MetersPerPixel(int zoom, double latitude)
            => 156543.03392 * Math.Cos(latitude * Math.PI / 180.0) / (1 << zoom);

        /// <summary>
        /// The zoom whose map pixels are closest to (but not coarser than about 1.4x) the screen
        /// pixels at <paramref name="screenMetersPerPixel"/>, limited to <paramref name="maxZoom"/>.
        /// </summary>
        public static int ZoomFor(double screenMetersPerPixel, double latitude, int maxZoom = MaxZoom)
        {
            if (screenMetersPerPixel <= 0 || double.IsNaN(screenMetersPerPixel)) return maxZoom;
            double z = Math.Log(156543.03392 * Math.Cos(latitude * Math.PI / 180.0) / screenMetersPerPixel, 2);
            return Math.Max(MinZoom, Math.Min(maxZoom, (int)Math.Ceiling(z - 0.5)));
        }

        /// <summary>All tiles of a zoom level that touch the given latitude/longitude box.</summary>
        public static List<TileId> TilesFor(double south, double west, double north, double east, int zoom)
        {
            ToTile(north, west, zoom, out double x0, out double y0);
            ToTile(south, east, zoom, out double x1, out double y1);
            int max = (1 << zoom) - 1;
            var list = new List<TileId>();
            for (int y = Math.Max(0, (int)Math.Floor(y0)); y <= Math.Min(max, (int)Math.Floor(y1)); y++)
                for (int x = Math.Max(0, (int)Math.Floor(x0)); x <= Math.Min(max, (int)Math.Floor(x1)); x++)
                    list.Add(new TileId(zoom, x, y));
            return list;
        }

        /// <summary>
        /// The tile <paramref name="up"/> levels coarser that contains <paramref name="t"/>, and the
        /// part of it (in pixels of a 256 px tile) that covers <paramref name="t"/>.
        /// </summary>
        public static TileId Parent(TileId t, int up, out int srcX, out int srcY, out int srcSize)
        {
            var parent = new TileId(t.Z - up, t.X >> up, t.Y >> up);
            srcSize = TileSize >> up;
            srcX = (t.X - (parent.X << up)) * srcSize;
            srcY = (t.Y - (parent.Y << up)) * srcSize;
            return parent;
        }

        /// <summary>Fills {z}, {x}, {y} (and {s} with a, b or c) in a tile URL template.</summary>
        public static string Url(string template, TileId t)
        {
            string s = "abc"[(t.X + t.Y) % 3].ToString();
            return (template ?? DefaultUrl)
                .Replace("{z}", t.Z.ToString(CultureInfo.InvariantCulture))
                .Replace("{x}", t.X.ToString(CultureInfo.InvariantCulture))
                .Replace("{y}", t.Y.ToString(CultureInfo.InvariantCulture))
                .Replace("{s}", s);
        }

        /// <summary>A short stable name for a URL template, to keep each map source's tiles in its own folder.</summary>
        public static string SourceKey(string template)
        {
            unchecked
            {
                uint h = 2166136261;
                foreach (char c in template ?? DefaultUrl) h = (h ^ c) * 16777619;
                return h.ToString("x8", CultureInfo.InvariantCulture);
            }
        }
    }
}
