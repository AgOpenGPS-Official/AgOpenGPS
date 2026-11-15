using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>Where the recorded boundary lies relative to the vehicle while driving round.</summary>
    public enum BoundarySide
    {
        /// <summary>The boundary is the path of the antenna.</summary>
        Antenna,
        /// <summary>Half the working width left of the antenna (the left end of the implement).</summary>
        LeftOfImplement,
        /// <summary>Half the working width right of the antenna.</summary>
        RightOfImplement,
    }

    /// <summary>A named field: its outer boundary and (through <see cref="AbLine.FieldId"/>) its lines.</summary>
    public sealed class Field
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = "";
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        /// <summary>Closed outer boundary (the last point connects back to the first). Empty until recorded.</summary>
        public List<TrackPoint> Boundary { get; set; } = new List<TrackPoint>();

        public bool HasBoundary => Boundary != null && Boundary.Count >= 3;

        /// <summary>A plane at the first boundary point, or null without a boundary.</summary>
        public LocalPlane CreatePlane() => HasBoundary ? new LocalPlane(Boundary[0].Latitude, Boundary[0].Longitude) : null;

        public List<GeoPoint> BoundaryLocal(LocalPlane plane)
        {
            var list = new List<GeoPoint>();
            if (Boundary != null) foreach (TrackPoint p in Boundary) list.Add(plane.ToLocal(p.Latitude, p.Longitude));
            return list;
        }

        public double AreaHectares
        {
            get
            {
                LocalPlane plane = CreatePlane();
                return plane == null ? 0 : Geometry.Area(BoundaryLocal(plane)) / 10000.0;
            }
        }
    }

    /// <summary>The saved fields and which one is open, kept in one JSON file.</summary>
    public sealed class FieldStore
    {
        private readonly string _path;
        private readonly object _lock = new object();
        private FieldFile _data = new FieldFile();

        public FieldStore(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            Load();
        }

        public IReadOnlyList<Field> Fields
        {
            get { lock (_lock) return _data.Fields.OrderBy(f => f.Name, StringComparer.CurrentCultureIgnoreCase).ToList(); }
        }

        public Field Active
        {
            get { lock (_lock) return _data.Fields.FirstOrDefault(f => f.Id == _data.ActiveId); }
        }

        public Field Create(string name)
        {
            lock (_lock)
            {
                var f = new Field { Name = string.IsNullOrWhiteSpace(name) ? NextNameLocked() : name.Trim() };
                _data.Fields.Add(f);
                _data.ActiveId = f.Id;
                Save();
                return f;
            }
        }

        public string NextName()
        {
            lock (_lock) return NextNameLocked();
        }

        private string NextNameLocked()
        {
            for (int i = 1; ; i++)
            {
                string name = "Field " + i;
                if (!_data.Fields.Any(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase))) return name;
            }
        }

        public void SetActive(string id)
        {
            lock (_lock)
            {
                _data.ActiveId = _data.Fields.Any(f => f.Id == id) ? id : null;
                Save();
            }
        }

        public void Rename(string id, string name)
        {
            lock (_lock)
            {
                Field f = _data.Fields.FirstOrDefault(x => x.Id == id);
                if (f == null || string.IsNullOrWhiteSpace(name)) return;
                f.Name = name.Trim();
                Save();
            }
        }

        public void SetBoundary(string id, List<TrackPoint> boundary)
        {
            lock (_lock)
            {
                Field f = _data.Fields.FirstOrDefault(x => x.Id == id);
                if (f == null) return;
                f.Boundary = boundary ?? new List<TrackPoint>();
                Save();
            }
        }

        public void Delete(string id)
        {
            lock (_lock)
            {
                _data.Fields.RemoveAll(f => f.Id == id);
                if (_data.ActiveId == id) _data.ActiveId = null;
                Save();
            }
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_path)) return;
                FieldFile data = JsonSerializer.Deserialize(File.ReadAllText(_path), StoreJson.Default.FieldFile);
                if (data?.Fields != null) _data = data;
            }
            catch (Exception ex) when (ex is IOException || ex is JsonException || ex is UnauthorizedAccessException)
            {
                try { File.Copy(_path, _path + ".bad", true); } catch (IOException) { }
                _data = new FieldFile();
            }
        }

        private void Save()
        {
            string dir = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            string tmp = _path + ".tmp";
            File.WriteAllText(tmp, JsonSerializer.Serialize(_data, StoreJson.Default.FieldFile));
            File.Move(tmp, _path, true);
        }

        internal sealed class FieldFile
        {
            public int Version { get; set; } = 1;
            public string ActiveId { get; set; }
            public List<Field> Fields { get; set; } = new List<Field>();
        }
    }
}
