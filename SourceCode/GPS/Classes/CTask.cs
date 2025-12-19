using System;
using Newtonsoft.Json;

namespace AgOpenGPS
{
    /// <summary>
    /// Represents a work session (task) for a field.
    /// Tasks store coverage data separately, allowing multiple work sessions per field.
    /// </summary>
    public class CTask
    {
        /// <summary>Unique identifier for this task</summary>
        public Guid Id { get; set; }

        /// <summary>Display name for this task (user-defined)</summary>
        public string Name { get; set; }

        /// <summary>Name of the field this task belongs to</summary>
        public string FieldName { get; set; }

        /// <summary>Name of the vehicle/implement profile used</summary>
        public string ProfileName { get; set; }

        /// <summary>Type of work being performed (e.g., Spraying, Seeding, Tillage)</summary>
        public string WorkType { get; set; }

        /// <summary>Implement width in meters at time of task creation</summary>
        public double ImplementWidth { get; set; }

        /// <summary>When this task was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>When this task was last opened/worked on</summary>
        public DateTime LastOpenedAt { get; set; }

        /// <summary>Whether the task is marked as completed</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Total worked area in square meters</summary>
        public double WorkedArea { get; set; }

        /// <summary>AgShare session ID for real-time sync (null if not connected)</summary>
        public Guid? AgShareSessionId { get; set; }

        /// <summary>
        /// Creates a new task with default values
        /// </summary>
        public CTask()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastOpenedAt = DateTime.UtcNow;
            IsCompleted = false;
            WorkedArea = 0;
        }

        /// <summary>
        /// Creates a new task with the specified parameters
        /// </summary>
        public CTask(string fieldName, string profileName, string workType, double implementWidth, string taskName = null)
            : this()
        {
            FieldName = fieldName;
            ProfileName = profileName;
            WorkType = workType;
            ImplementWidth = implementWidth;
            Name = taskName ?? GenerateDefaultName(workType, profileName);
        }

        /// <summary>
        /// Generates a default task name based on date, work type, and profile
        /// </summary>
        private static string GenerateDefaultName(string workType, string profileName)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string work = string.IsNullOrEmpty(workType) ? "Work" : workType;
            string profile = string.IsNullOrEmpty(profileName) ? "Unknown" : profileName;
            return $"{date}_{work}_{profile}";
        }

        /// <summary>
        /// Gets the folder name for this task (sanitized for file system)
        /// </summary>
        [JsonIgnore]
        public string FolderName
        {
            get
            {
                // Sanitize the name for use as folder name
                string sanitized = Name ?? Id.ToString();
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                {
                    sanitized = sanitized.Replace(c, '_');
                }
                return sanitized;
            }
        }

        /// <summary>
        /// Updates the LastOpenedAt timestamp to now
        /// </summary>
        public void Touch()
        {
            LastOpenedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the task as completed
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
            Touch();
        }

        /// <summary>
        /// Serializes this task to JSON
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Deserializes a task from JSON
        /// </summary>
        public static CTask FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CTask>(json);
        }

        public override string ToString()
        {
            return $"{Name} ({WorkType}) - {FieldName}";
        }
    }
}
