using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>The saved A-B lines and which one is active, kept in one JSON file.</summary>
    public sealed class AbLineStore
    {
        private readonly string _path;
        private readonly object _lock = new object();
        private StoreFile _data = new StoreFile();

        public AbLineStore(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
            Load();
        }

        public IReadOnlyList<AbLine> Lines
        {
            get { lock (_lock) return _data.Lines.ToList(); }
        }

        /// <summary>The lines of one field (null = lines without a field).</summary>
        public IReadOnlyList<AbLine> LinesFor(string fieldId)
        {
            lock (_lock) return _data.Lines.Where(l => l.FieldId == fieldId).ToList();
        }

        public AbLine Active
        {
            get { lock (_lock) return _data.Lines.FirstOrDefault(l => l.Id == _data.ActiveId); }
        }

        /// <summary>"AB 1", "AB 2", ...: the first number not used yet in that field.</summary>
        public string NextName(string fieldId = null)
        {
            lock (_lock)
            {
                for (int i = 1; ; i++)
                {
                    string name = "AB " + i;
                    if (!_data.Lines.Any(l => l.FieldId == fieldId && string.Equals(l.Name, name, StringComparison.OrdinalIgnoreCase))) return name;
                }
            }
        }

        /// <summary>Deletes all lines of a field (when the field is deleted).</summary>
        public void DeleteForField(string fieldId)
        {
            lock (_lock)
            {
                if (_data.Lines.RemoveAll(l => l.FieldId == fieldId) == 0) return;
                if (!_data.Lines.Any(l => l.Id == _data.ActiveId)) _data.ActiveId = null;
                Save();
            }
        }

        public void Add(AbLine line, bool makeActive)
        {
            lock (_lock)
            {
                _data.Lines.Add(line);
                if (makeActive) _data.ActiveId = line.Id;
                Save();
            }
        }

        public void SetActive(string id)
        {
            lock (_lock)
            {
                _data.ActiveId = _data.Lines.Any(l => l.Id == id) ? id : null;
                Save();
            }
        }

        public void Rename(string id, string name)
        {
            lock (_lock)
            {
                AbLine line = _data.Lines.FirstOrDefault(l => l.Id == id);
                if (line == null || string.IsNullOrWhiteSpace(name)) return;
                line.Name = name.Trim();
                Save();
            }
        }

        /// <summary>Saves the nudge of a line (the line object may already carry it).</summary>
        public void SetNudge(string id, double nudgeMeters)
        {
            lock (_lock)
            {
                AbLine line = _data.Lines.FirstOrDefault(l => l.Id == id);
                if (line == null) return;
                line.NudgeMeters = nudgeMeters;
                Save();
            }
        }

        public void Delete(string id)
        {
            lock (_lock)
            {
                _data.Lines.RemoveAll(l => l.Id == id);
                if (_data.ActiveId == id) _data.ActiveId = null;
                Save();
            }
        }

        private void Load()
        {
            try
            {
                if (!File.Exists(_path)) return;
                StoreFile data = JsonSerializer.Deserialize(File.ReadAllText(_path), StoreJson.Default.StoreFile);
                if (data?.Lines != null) _data = data;
            }
            catch (Exception ex) when (ex is IOException || ex is JsonException || ex is UnauthorizedAccessException)
            {
                // A damaged file must not stop the app: keep a copy and start empty.
                try { File.Copy(_path, _path + ".bad", true); } catch (IOException) { }
                _data = new StoreFile();
            }
        }

        private void Save()
        {
            string dir = Path.GetDirectoryName(_path);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            string tmp = _path + ".tmp";
            File.WriteAllText(tmp, JsonSerializer.Serialize(_data, StoreJson.Default.StoreFile));
            File.Move(tmp, _path, true); // never leave a half-written file behind
        }

        internal sealed class StoreFile
        {
            public int Version { get; set; } = 1;
            public string ActiveId { get; set; }
            public List<AbLine> Lines { get; set; } = new List<AbLine>();
        }
    }

    // Source-generated serializer: works with Android's trimming, no reflection.
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(AbLineStore.StoreFile))]
    [JsonSerializable(typeof(FieldStore.FieldFile))]
    internal partial class StoreJson : JsonSerializerContext
    {
    }
}
