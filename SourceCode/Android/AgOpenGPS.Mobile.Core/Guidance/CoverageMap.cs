using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>A tile of the coverage map, for drawing.</summary>
    public readonly struct CoverageTileInfo
    {
        public CoverageTileInfo(int tx, int ty, int version)
        {
            TileX = tx;
            TileY = ty;
            Version = version;
        }

        public int TileX { get; }
        public int TileY { get; }
        /// <summary>Changes whenever a cell of the tile changes: redraw cached bitmaps then.</summary>
        public int Version { get; }
    }

    /// <summary>
    /// Worked ground as a grid of <see cref="CellSize"/> cells on its own local plane, stored in
    /// tiles of 64 x 64 cells (16 x 16 m) that exist only where something was worked. Each cell
    /// counts how often it was covered, so the worked area counts ground once and overlap is known.
    /// Thread safe.
    /// </summary>
    public sealed class CoverageMap
    {
        public const double CellSize = 0.25;
        public const int TileCells = 64;
        public const double TileMeters = TileCells * CellSize;
        private const string Magic = "AOGCOV1";

        private readonly object _lock = new object();
        private readonly Dictionary<long, Tile> _tiles = new Dictionary<long, Tile>();
        private List<GeoPoint> _boundary; // on this map's plane
        private long _covered, _overlap, _coveredInside;

        public CoverageMap(double originLatitude, double originLongitude)
        {
            Plane = new LocalPlane(originLatitude, originLongitude);
        }

        public LocalPlane Plane { get; }

        /// <summary>True when something changed since the last <see cref="Save"/>.</summary>
        public bool Dirty { get; private set; }

        public double CoveredSquareMeters { get { lock (_lock) return _covered * CellSize * CellSize; } }
        public double OverlapSquareMeters { get { lock (_lock) return _overlap * CellSize * CellSize; } }
        /// <summary>Worked area inside the boundary; NaN without a boundary.</summary>
        public double CoveredInsideSquareMeters
        {
            get { lock (_lock) return _boundary == null ? double.NaN : _coveredInside * CellSize * CellSize; }
        }

        /// <summary>Sets the field boundary (latitude/longitude points) for the "inside" count, or clears it.</summary>
        public void SetBoundary(IReadOnlyList<TrackPoint> boundary)
        {
            lock (_lock)
            {
                if (boundary == null || boundary.Count < 3)
                {
                    _boundary = null;
                    _coveredInside = 0;
                    return;
                }
                _boundary = new List<GeoPoint>(boundary.Count);
                foreach (TrackPoint p in boundary) _boundary.Add(Plane.ToLocal(p.Latitude, p.Longitude));

                _coveredInside = 0;
                foreach (KeyValuePair<long, Tile> kv in _tiles)
                {
                    Unpack(kv.Key, out int tx, out int ty);
                    byte[] c = kv.Value.Counts;
                    for (int i = 0; i < c.Length; i++)
                        if (c[i] > 0 && InsideBoundary(tx * TileCells + (i % TileCells), ty * TileCells + (i / TileCells))) _coveredInside++;
                }
            }
        }

        /// <summary>
        /// Marks the cells whose centre lies in the quadrilateral a-b-c-d (corners in order, on this
        /// map's plane) as covered once more.
        /// </summary>
        public void PaintQuad(GeoPoint a, GeoPoint b, GeoPoint c, GeoPoint d)
        {
            var quad = new[] { a, b, c, d };
            double minN = Math.Min(Math.Min(a.Northing, b.Northing), Math.Min(c.Northing, d.Northing));
            double maxN = Math.Max(Math.Max(a.Northing, b.Northing), Math.Max(c.Northing, d.Northing));
            double minE = Math.Min(Math.Min(a.Easting, b.Easting), Math.Min(c.Easting, d.Easting));
            double maxE = Math.Max(Math.Max(a.Easting, b.Easting), Math.Max(c.Easting, d.Easting));
            if (maxN - minN > 200 || maxE - minE > 200) return; // a jump, not a strip

            int x0 = (int)Math.Floor(minE / CellSize), x1 = (int)Math.Floor(maxE / CellSize);
            int y0 = (int)Math.Floor(minN / CellSize), y1 = (int)Math.Floor(maxN / CellSize);

            lock (_lock)
            {
                for (int y = y0; y <= y1; y++)
                {
                    for (int x = x0; x <= x1; x++)
                    {
                        var centre = new GeoPoint((y + 0.5) * CellSize, (x + 0.5) * CellSize);
                        if (Geometry.Contains(quad, centre)) Increment(x, y);
                    }
                }
            }
        }

        private void Increment(int x, int y)
        {
            int tx = FloorDiv(x, TileCells), ty = FloorDiv(y, TileCells);
            long key = Pack(tx, ty);
            if (!_tiles.TryGetValue(key, out Tile tile))
            {
                tile = new Tile();
                _tiles[key] = tile;
            }
            int i = (y - ty * TileCells) * TileCells + (x - tx * TileCells);
            byte before = tile.Counts[i];
            if (before == byte.MaxValue) return;
            tile.Counts[i] = (byte)(before + 1);
            tile.Version++;
            Dirty = true;
            if (before == 0)
            {
                _covered++;
                if (_boundary != null && InsideBoundary(x, y)) _coveredInside++;
            }
            else if (before == 1)
            {
                _overlap++;
            }
        }

        private bool InsideBoundary(int x, int y)
            => Geometry.Contains(_boundary, new GeoPoint((y + 0.5) * CellSize, (x + 0.5) * CellSize));

        /// <summary>Tiles overlapping the given area of this map's plane.</summary>
        public List<CoverageTileInfo> TilesIn(double minN, double maxN, double minE, double maxE)
        {
            int tx0 = (int)Math.Floor(minE / TileMeters), tx1 = (int)Math.Floor(maxE / TileMeters);
            int ty0 = (int)Math.Floor(minN / TileMeters), ty1 = (int)Math.Floor(maxN / TileMeters);
            var list = new List<CoverageTileInfo>();
            lock (_lock)
            {
                if ((long)(tx1 - tx0 + 1) * (ty1 - ty0 + 1) > _tiles.Count)
                {
                    foreach (KeyValuePair<long, Tile> kv in _tiles)
                    {
                        Unpack(kv.Key, out int tx, out int ty);
                        if (tx >= tx0 && tx <= tx1 && ty >= ty0 && ty <= ty1) list.Add(new CoverageTileInfo(tx, ty, kv.Value.Version));
                    }
                }
                else
                {
                    for (int ty = ty0; ty <= ty1; ty++)
                        for (int tx = tx0; tx <= tx1; tx++)
                            if (_tiles.TryGetValue(Pack(tx, ty), out Tile t)) list.Add(new CoverageTileInfo(tx, ty, t.Version));
                }
            }
            return list;
        }

        /// <summary>A copy of a tile's counts, row 0 = southernmost row. Null if the tile is empty.</summary>
        public byte[] CopyTile(int tx, int ty)
        {
            lock (_lock)
            {
                return _tiles.TryGetValue(Pack(tx, ty), out Tile t) ? (byte[])t.Counts.Clone() : null;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _tiles.Clear();
                _covered = _overlap = _coveredInside = 0;
                Dirty = true;
            }
        }

        // ---------------------------------------------------------------- file

        public void Save(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            string tmp = path + ".tmp";
            lock (_lock)
            {
                using (var w = new BinaryWriter(File.Create(tmp), Encoding.ASCII))
                {
                    w.Write(Magic);
                    w.Write(Plane.OriginLatitude);
                    w.Write(Plane.OriginLongitude);
                    w.Write(CellSize);
                    w.Write(_tiles.Count);
                    foreach (KeyValuePair<long, Tile> kv in _tiles)
                    {
                        Unpack(kv.Key, out int tx, out int ty);
                        w.Write(tx);
                        w.Write(ty);
                        w.Write(kv.Value.Counts);
                    }
                }
                Dirty = false;
            }
            File.Move(tmp, path, true);
        }

        /// <summary>Loads a saved map, or returns null if there is none or it cannot be read.</summary>
        public static CoverageMap Load(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;
                using (var r = new BinaryReader(File.OpenRead(path), Encoding.ASCII))
                {
                    if (r.ReadString() != Magic) return null;
                    var map = new CoverageMap(r.ReadDouble(), r.ReadDouble());
                    if (Math.Abs(r.ReadDouble() - CellSize) > 1e-9) return null;
                    int n = r.ReadInt32();
                    for (int k = 0; k < n; k++)
                    {
                        int tx = r.ReadInt32(), ty = r.ReadInt32();
                        byte[] counts = r.ReadBytes(TileCells * TileCells);
                        if (counts.Length != TileCells * TileCells) return null;
                        var tile = new Tile { Counts = counts };
                        map._tiles[Pack(tx, ty)] = tile;
                        foreach (byte b in counts)
                        {
                            if (b > 0) map._covered++;
                            if (b > 1) map._overlap++;
                        }
                    }
                    return map;
                }
            }
            catch (IOException) { return null; }
            catch (UnauthorizedAccessException) { return null; }
        }

        // ---------------------------------------------------------------- helpers

        private static long Pack(int tx, int ty) => ((long)tx << 32) | (uint)ty;

        private static void Unpack(long key, out int tx, out int ty)
        {
            tx = (int)(key >> 32);
            ty = (int)(key & 0xFFFFFFFF);
        }

        private static int FloorDiv(int a, int b) => a >= 0 ? a / b : -((-a + b - 1) / b);

        private sealed class Tile
        {
            public byte[] Counts = new byte[TileCells * TileCells];
            public int Version;
        }
    }
}
