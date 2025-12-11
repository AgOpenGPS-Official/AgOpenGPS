using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgLibrary.Logging;
using Newtonsoft.Json;

namespace AgOpenGPS
{
    /// <summary>
    /// Represents a type of work that can be performed (e.g., Spraying, Seeding, Tillage)
    /// </summary>
    public class CWorkType
    {
        /// <summary>Unique identifier for this work type</summary>
        public string Id { get; set; }

        /// <summary>Display name for this work type</summary>
        public string Name { get; set; }

        /// <summary>Icon name (for future use)</summary>
        public string Icon { get; set; }

        public CWorkType() { }

        public CWorkType(string id, string name, string icon = null)
        {
            Id = id;
            Name = name;
            Icon = icon ?? id.ToLowerInvariant();
        }

        public override string ToString() => Name;
    }

    /// <summary>
    /// Manages the collection of available work types
    /// </summary>
    public static class WorkTypes
    {
        private const string FileName = "WorkTypes.json";
        private static List<CWorkType> _workTypes;
        private static string _settingsDirectory;

        /// <summary>
        /// Default work types to use when no config file exists
        /// </summary>
        public static readonly List<CWorkType> Defaults = new List<CWorkType>
        {
            new CWorkType("spraying", "Spraying", "spray"),
            new CWorkType("seeding", "Seeding", "seed"),
            new CWorkType("tillage", "Tillage", "plow"),
            new CWorkType("harvest", "Harvest", "combine"),
            new CWorkType("mowing", "Mowing", "mower"),
            new CWorkType("fertilizing", "Fertilizing", "fertilizer"),
            new CWorkType("other", "Other", "other")
        };

        /// <summary>
        /// Gets all available work types
        /// </summary>
        public static List<CWorkType> All
        {
            get
            {
                if (_workTypes == null)
                    Load();
                return _workTypes;
            }
        }

        /// <summary>
        /// Initialize the work types manager with the settings directory
        /// </summary>
        public static void Initialize(string settingsDirectory)
        {
            _settingsDirectory = settingsDirectory;
            _workTypes = null; // Force reload
        }

        /// <summary>
        /// Gets the file path for work types config
        /// </summary>
        private static string GetFilePath()
        {
            if (string.IsNullOrEmpty(_settingsDirectory))
                return null;
            return Path.Combine(_settingsDirectory, FileName);
        }

        /// <summary>
        /// Loads work types from config file, or creates defaults if not found
        /// </summary>
        public static void Load()
        {
            string filePath = GetFilePath();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                _workTypes = new List<CWorkType>(Defaults);
                return;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                var container = JsonConvert.DeserializeObject<WorkTypesContainer>(json);
                _workTypes = container?.WorkTypes ?? new List<CWorkType>(Defaults);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error loading work types: {ex.Message}");
                _workTypes = new List<CWorkType>(Defaults);
            }
        }

        /// <summary>
        /// Saves current work types to config file
        /// </summary>
        public static void Save()
        {
            string filePath = GetFilePath();
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                var container = new WorkTypesContainer { WorkTypes = _workTypes };
                string json = JsonConvert.SerializeObject(container, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error saving work types: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new work type
        /// </summary>
        public static void Add(CWorkType workType)
        {
            if (_workTypes == null) Load();
            if (workType == null) return;

            // Generate ID if not provided
            if (string.IsNullOrEmpty(workType.Id))
                workType.Id = workType.Name.ToLowerInvariant().Replace(" ", "_");

            // Check for duplicate ID
            if (_workTypes.Any(w => w.Id.Equals(workType.Id, StringComparison.OrdinalIgnoreCase)))
                return;

            _workTypes.Add(workType);
            Save();
        }

        /// <summary>
        /// Removes a work type by ID
        /// </summary>
        public static bool Remove(string id)
        {
            if (_workTypes == null) Load();

            var toRemove = _workTypes.FirstOrDefault(w => w.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (toRemove != null)
            {
                _workTypes.Remove(toRemove);
                Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a work type by ID
        /// </summary>
        public static CWorkType GetById(string id)
        {
            if (_workTypes == null) Load();
            return _workTypes.FirstOrDefault(w => w.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Resets to default work types
        /// </summary>
        public static void ResetToDefaults()
        {
            _workTypes = new List<CWorkType>(Defaults);
            Save();
        }

        /// <summary>
        /// Container class for JSON serialization
        /// </summary>
        private class WorkTypesContainer
        {
            [JsonProperty("workTypes")]
            public List<CWorkType> WorkTypes { get; set; }
        }
    }
}
