using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgLibrary.Logging;

namespace AgOpenGPS.IO
{
    /// <summary>
    /// Handles file I/O operations for Job data.
    /// Jobs are stored in Fields/{FieldName}/Jobs/{JobName}/ folders.
    /// </summary>
    public static class JobFiles
    {
        public const string JobsFolder = "Jobs";
        public const string JobFileName = "job.json";

        /// <summary>
        /// Gets the Jobs directory path for a field
        /// </summary>
        public static string GetJobsDirectory(string fieldDirectory)
        {
            return Path.Combine(fieldDirectory, JobsFolder);
        }

        /// <summary>
        /// Gets the full path to a specific job folder
        /// </summary>
        public static string GetJobDirectory(string fieldDirectory, string jobFolderName)
        {
            return Path.Combine(fieldDirectory, JobsFolder, jobFolderName);
        }

        /// <summary>
        /// Gets the path to the job.json file
        /// </summary>
        public static string GetJobFilePath(string jobDirectory)
        {
            return Path.Combine(jobDirectory, JobFileName);
        }

        /// <summary>
        /// Saves a job to its directory. Creates the directory if it doesn't exist.
        /// </summary>
        public static void Save(CJob job, string fieldDirectory)
        {
            if (job == null) throw new ArgumentNullException(nameof(job));
            if (string.IsNullOrEmpty(fieldDirectory)) throw new ArgumentNullException(nameof(fieldDirectory));

            string jobDir = GetJobDirectory(fieldDirectory, job.FolderName);

            // Create Jobs folder and job subfolder if needed
            Directory.CreateDirectory(jobDir);

            // Write job.json
            string jobFilePath = GetJobFilePath(jobDir);
            File.WriteAllText(jobFilePath, job.ToJson());

            Log.EventWriter($"Job saved: {job.Name} in {jobDir}");
        }

        /// <summary>
        /// Loads a job from a job directory
        /// </summary>
        public static CJob Load(string jobDirectory)
        {
            if (string.IsNullOrEmpty(jobDirectory)) return null;

            string jobFilePath = GetJobFilePath(jobDirectory);
            if (!File.Exists(jobFilePath)) return null;

            try
            {
                string json = File.ReadAllText(jobFilePath);
                return CJob.FromJson(json);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error loading job from {jobDirectory}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lists all jobs for a field, ordered by last opened date (most recent first)
        /// </summary>
        public static List<CJob> ListJobs(string fieldDirectory)
        {
            var jobs = new List<CJob>();

            string jobsDir = GetJobsDirectory(fieldDirectory);
            if (!Directory.Exists(jobsDir)) return jobs;

            foreach (string subDir in Directory.GetDirectories(jobsDir))
            {
                var job = Load(subDir);
                if (job != null)
                {
                    jobs.Add(job);
                }
            }

            // Sort by last opened, most recent first
            return jobs.OrderByDescending(j => j.LastOpenedAt).ToList();
        }

        /// <summary>
        /// Gets all incomplete (active) jobs for a field
        /// </summary>
        public static List<CJob> ListActiveJobs(string fieldDirectory)
        {
            return ListJobs(fieldDirectory)
                .Where(j => !j.IsCompleted)
                .ToList();
        }

        /// <summary>
        /// Gets the most recently opened job for a field
        /// </summary>
        public static CJob GetLastJob(string fieldDirectory)
        {
            return ListJobs(fieldDirectory).FirstOrDefault();
        }

        /// <summary>
        /// Gets the most recently opened incomplete job for a field
        /// </summary>
        public static CJob GetLastActiveJob(string fieldDirectory)
        {
            return ListActiveJobs(fieldDirectory).FirstOrDefault();
        }

        /// <summary>
        /// Checks if a field has any jobs
        /// </summary>
        public static bool HasJobs(string fieldDirectory)
        {
            string jobsDir = GetJobsDirectory(fieldDirectory);
            if (!Directory.Exists(jobsDir)) return false;
            return Directory.GetDirectories(jobsDir).Length > 0;
        }

        /// <summary>
        /// Deletes a job and all its files
        /// </summary>
        public static bool DeleteJob(string fieldDirectory, CJob job)
        {
            if (job == null) return false;

            string jobDir = GetJobDirectory(fieldDirectory, job.FolderName);
            if (!Directory.Exists(jobDir)) return false;

            try
            {
                Directory.Delete(jobDir, true);
                Log.EventWriter($"Job deleted: {job.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error deleting job {job.Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Creates the job directory structure and initializes empty coverage files
        /// </summary>
        public static void InitializeJobFiles(CJob job, string fieldDirectory)
        {
            string jobDir = GetJobDirectory(fieldDirectory, job.FolderName);
            Directory.CreateDirectory(jobDir);

            // Save the job metadata
            Save(job, fieldDirectory);

            // Create empty Sections.txt for coverage
            SectionsFiles.CreateEmpty(jobDir);

            // Create empty Contour.txt
            ContourFiles.CreateEmpty(jobDir);

            // Create empty Flags.txt
            FlagsFiles.CreateEmpty(jobDir);

            // Create empty RecPath.txt
            RecPathFiles.CreateEmpty(jobDir);

            Log.EventWriter($"Job initialized: {job.Name} in {jobDir}");
        }

        /// <summary>
        /// Finds a job by its ID across all fields
        /// </summary>
        public static (CJob Job, string FieldDirectory) FindJobById(Guid jobId, string fieldsDirectory)
        {
            if (!Directory.Exists(fieldsDirectory)) return (null, null);

            foreach (string fieldDir in Directory.GetDirectories(fieldsDirectory))
            {
                foreach (var job in ListJobs(fieldDir))
                {
                    if (job.Id == jobId)
                    {
                        return (job, fieldDir);
                    }
                }
            }

            return (null, null);
        }

        /// <summary>
        /// Lists all incomplete jobs across all fields, sorted by last opened
        /// </summary>
        public static List<(CJob Job, string FieldDirectory)> ListAllActiveJobs(string fieldsDirectory)
        {
            var result = new List<(CJob Job, string FieldDirectory)>();

            if (!Directory.Exists(fieldsDirectory)) return result;

            foreach (string fieldDir in Directory.GetDirectories(fieldsDirectory))
            {
                foreach (var job in ListActiveJobs(fieldDir))
                {
                    result.Add((job, fieldDir));
                }
            }

            return result.OrderByDescending(x => x.Job.LastOpenedAt).ToList();
        }
    }
}
