using System;
using Newtonsoft.Json;

namespace AgOpenGPS
{
    /// <summary>
    /// Represents a work session (job) for a field.
    /// Jobs store coverage data separately, allowing multiple work sessions per field.
    /// </summary>
    public class CJob
    {
        /// <summary>Unique identifier for this job</summary>
        public Guid Id { get; set; }

        /// <summary>Display name for this job (user-defined)</summary>
        public string Name { get; set; }

        /// <summary>Name of the field this job belongs to</summary>
        public string FieldName { get; set; }

        /// <summary>Name of the vehicle/implement profile used</summary>
        public string ProfileName { get; set; }

        /// <summary>Type of work being performed (e.g., Spraying, Seeding, Tillage)</summary>
        public string WorkType { get; set; }

        /// <summary>Implement width in meters at time of job creation</summary>
        public double ImplementWidth { get; set; }

        /// <summary>When this job was created</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>When this job was last opened/worked on</summary>
        public DateTime LastOpenedAt { get; set; }

        /// <summary>Whether the job is marked as completed</summary>
        public bool IsCompleted { get; set; }

        /// <summary>Total worked area in square meters</summary>
        public double WorkedArea { get; set; }

        /// <summary>AgShare session ID for real-time sync (null if not connected)</summary>
        public Guid? AgShareSessionId { get; set; }

        /// <summary>
        /// Creates a new job with default values
        /// </summary>
        public CJob()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            LastOpenedAt = DateTime.UtcNow;
            IsCompleted = false;
            WorkedArea = 0;
        }

        /// <summary>
        /// Creates a new job with the specified parameters
        /// </summary>
        public CJob(string fieldName, string profileName, string workType, double implementWidth, string jobName = null)
            : this()
        {
            FieldName = fieldName;
            ProfileName = profileName;
            WorkType = workType;
            ImplementWidth = implementWidth;
            Name = jobName ?? GenerateDefaultName(workType, profileName);
        }

        /// <summary>
        /// Generates a default job name based on date, work type, and profile
        /// </summary>
        private static string GenerateDefaultName(string workType, string profileName)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string work = string.IsNullOrEmpty(workType) ? "Work" : workType;
            string profile = string.IsNullOrEmpty(profileName) ? "Unknown" : profileName;
            return $"{date}_{work}_{profile}";
        }

        /// <summary>
        /// Gets the folder name for this job (sanitized for file system)
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
        /// Marks the job as completed
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
            Touch();
        }

        /// <summary>
        /// Serializes this job to JSON
        /// </summary>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        /// <summary>
        /// Deserializes a job from JSON
        /// </summary>
        public static CJob FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CJob>(json);
        }

        public override string ToString()
        {
            return $"{Name} ({WorkType}) - {FieldName}";
        }
    }
}
