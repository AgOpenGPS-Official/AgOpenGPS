using System;
using System.Collections.Generic;
using System.IO;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Manages job lifecycle operations: create, resume, close, finish.
    /// Handles coverage import/migration and job state transitions.
    /// </summary>
    public class JobManager
    {
        private readonly FormGPS mf;

        public JobManager(FormGPS formGPS)
        {
            mf = formGPS;
        }

        #region Job Lifecycle

        /// <summary>
        /// Creates and starts a new job for the specified field.
        /// </summary>
        /// <param name="fieldDir">Full path to the field directory</param>
        /// <param name="fieldName">Display name of the field</param>
        /// <param name="profileName">Name of the vehicle profile</param>
        /// <param name="workType">Type of work (e.g., Spraying, Seeding)</param>
        /// <param name="jobName">Name for the new job</param>
        /// <param name="importCoverage">Whether to import existing coverage data</param>
        /// <returns>The created job, or null on failure</returns>
        public CJob CreateJob(string fieldDir, string fieldName, string profileName,
            string workType, string jobName, bool importCoverage = false)
        {
            // Check if field is already open and it's the same field
            bool fieldAlreadyOpen = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == fieldDir;

            // Save and close current job if needed
            if (mf.isJobStarted)
            {
                if (!fieldAlreadyOpen || mf.currentJob != null)
                {
                    SaveAndCloseCurrentJob();
                }
            }

            // Create the new job
            var job = new CJob(fieldName, profileName, workType, mf.tool.width, jobName);

            // Initialize job files
            JobFiles.InitializeJobFiles(job, fieldDir);

            // Handle coverage import
            if (importCoverage)
            {
                ImportCoverageToJob(job, fieldDir);
            }

            // Always clean up legacy coverage when creating a new job
            DeleteLegacyCoverage(fieldDir);

            // Set the current job
            mf.currentJob = job;

            // Open the field if not already open
            if (!fieldAlreadyOpen)
            {
                string fieldFile = Path.Combine(fieldDir, "Field.txt");
                mf.FileOpenField(fieldFile);
            }

            // Load job-specific coverage data
            mf.FileLoadJobData();

            Log.EventWriter($"Job created: {job.Name}");
            return job;
        }

        /// <summary>
        /// Resumes an existing job.
        /// </summary>
        /// <param name="job">The job to resume</param>
        /// <param name="fieldDir">Full path to the field directory</param>
        /// <returns>True if successful</returns>
        public bool ResumeJob(CJob job, string fieldDir)
        {
            if (job == null || string.IsNullOrEmpty(fieldDir))
                return false;

            string fieldFile = Path.Combine(fieldDir, "Field.txt");
            if (!File.Exists(fieldFile))
            {
                Log.EventWriter($"Cannot resume job - field not found: {fieldDir}");
                return false;
            }

            // Check if we're resuming a job in the currently open field
            bool sameField = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == fieldDir;

            // Save and close current job if needed
            if (mf.isJobStarted)
            {
                SaveAndCloseCurrentJob();

                // Close field if different
                if (!sameField)
                {
                    mf.AppModel.Fields.CloseField();
                }
            }

            // Open the field
            mf.FileOpenField(fieldFile);

            // Set the current job BEFORE loading job data
            mf.currentJob = job;

            // Load job-specific coverage data
            mf.FileLoadJobData();

            // Update job timestamp
            job.Touch();
            JobFiles.Save(job, fieldDir);

            Log.EventWriter($"Job resumed: {job.Name}");
            return true;
        }

        /// <summary>
        /// Closes the current job without marking it as completed.
        /// Saves all data and clears the current job reference.
        /// </summary>
        public void CloseCurrentJob()
        {
            if (mf.currentJob == null) return;

            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            // Save job metadata
            JobFiles.Save(mf.currentJob, fieldDir);

            Log.EventWriter($"Job closed: {mf.currentJob.Name}");
            mf.currentJob = null;
        }

        /// <summary>
        /// Finishes (completes) the current job.
        /// Marks as completed, saves all data, and clears the current job reference.
        /// </summary>
        public void FinishCurrentJob()
        {
            if (mf.currentJob == null) return;

            // Mark as completed
            mf.currentJob.Complete();

            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            // Save job metadata
            JobFiles.Save(mf.currentJob, fieldDir);

            Log.EventWriter($"Job finished: {mf.currentJob.Name}");
            mf.currentJob = null;
        }

        /// <summary>
        /// Saves current job data and metadata, then clears the job reference.
        /// </summary>
        private void SaveAndCloseCurrentJob()
        {
            if (mf.currentJob != null)
            {
                var currentFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                JobFiles.Save(mf.currentJob, currentFieldDir);
                mf.currentJob = null;
            }
        }

        #endregion

        #region Coverage Migration

        /// <summary>
        /// Checks if the specified field has existing coverage data that could be imported.
        /// </summary>
        /// <param name="fieldDir">Full path to the field directory</param>
        /// <returns>True if legacy coverage exists</returns>
        public bool HasExistingCoverage(string fieldDir)
        {
            if (string.IsNullOrEmpty(fieldDir) || !Directory.Exists(fieldDir))
                return false;

            string sectionsFile = Path.Combine(fieldDir, "Sections.txt");
            string tempSectionsFile = Path.Combine(fieldDir, ".temp_Sections.txt");

            return File.Exists(sectionsFile) || File.Exists(tempSectionsFile);
        }

        /// <summary>
        /// Imports existing coverage data from field root to the job directory.
        /// </summary>
        /// <param name="job">The job to import coverage into</param>
        /// <param name="fieldDir">Full path to the field directory</param>
        public void ImportCoverageToJob(CJob job, string fieldDir)
        {
            try
            {
                string jobDir = Path.Combine(fieldDir, "Jobs", job.FolderName);

                if (!Directory.Exists(jobDir))
                {
                    Directory.CreateDirectory(jobDir);
                }

                int filesCopied = 0;
                string[] coverageFiles = { "Sections.txt", ".temp_Sections.txt", "Contour.txt", "RecPath.txt" };

                foreach (string fileName in coverageFiles)
                {
                    string sourcePath = Path.Combine(fieldDir, fileName);
                    if (File.Exists(sourcePath))
                    {
                        // For temp files, copy to non-temp destination
                        string destFileName = fileName.StartsWith(".temp_") ? fileName.Substring(6) : fileName;
                        string destPath = Path.Combine(jobDir, destFileName);

                        File.Copy(sourcePath, destPath, overwrite: true);
                        filesCopied++;
                    }
                }

                Log.EventWriter($"Coverage imported to job: {job.Name} ({filesCopied} files)");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error importing coverage: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes legacy coverage files from field root.
        /// Called after creating a new job to prevent old coverage from being loaded.
        /// </summary>
        /// <param name="fieldDir">Full path to the field directory</param>
        public void DeleteLegacyCoverage(string fieldDir)
        {
            try
            {
                string[] filesToDelete = { "Sections.txt", ".temp_Sections.txt", "Contour.txt", "RecPath.txt" };

                foreach (string fileName in filesToDelete)
                {
                    string filePath = Path.Combine(fieldDir, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        Log.EventWriter($"Deleted legacy coverage file: {fileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error deleting legacy coverage: {ex.Message}");
            }
        }

        #endregion

        #region Profile Loading

        /// <summary>
        /// Loads a vehicle profile by name.
        /// </summary>
        /// <param name="profileName">Name of the profile to load</param>
        /// <returns>True if successful</returns>
        public bool LoadProfile(string profileName)
        {
            if (string.IsNullOrEmpty(profileName))
                return false;

            // Don't reload if it's already the current profile
            if (profileName == RegistrySettings.vehicleFileName)
                return true;

            RegistrySettings.Save(RegKeys.vehicleFileName, profileName);

            var result = Properties.Settings.Default.Load();
            if (result != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading profile {profileName}.xml ({result})");
                return false;
            }

            Log.EventWriter($"Profile loaded: {profileName}.xml");

            mf.vehicle = new CVehicle(mf);
            mf.tool = new CTool(mf);

            mf.LoadSettings();
            mf.SendSettings();
            mf.SendRelaySettingsToMachineModule();

            return true;
        }

        #endregion

        #region Field Creation

        /// <summary>
        /// Creates a new field with the given name at the current GPS position.
        /// </summary>
        /// <param name="fieldName">Name for the new field</param>
        /// <returns>The field directory path, or null on failure</returns>
        public string CreateField(string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
                return null;

            string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, fieldName.Trim());

            // Check if directory already exists
            if (Directory.Exists(fieldDir))
            {
                Log.EventWriter($"Field directory already exists: {fieldDir}");
                return null;
            }

            try
            {
                // Set the current field directory
                mf.currentFieldDirectory = fieldName.Trim();

                // Start new job (enables UI elements)
                mf.JobNew();

                // Define local plane at current position
                mf.pn.DefineLocalPlane(mf.AppModel.CurrentLatLon, false);

                // Create the directory
                Directory.CreateDirectory(fieldDir);

                // Create all required field files
                mf.FileCreateField();
                mf.FileCreateSections();
                mf.FileCreateRecPath();
                mf.FileCreateContour();
                mf.FileCreateElevation();
                mf.FileSaveFlags();
                mf.FileCreateBoundary();

                mf.menustripLanguage.Enabled = false;

                Log.EventWriter($"Field created: {fieldName}");
                return fieldDir;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error creating field: {ex.Message}");
                mf.currentFieldDirectory = "";
                return null;
            }
        }

        /// <summary>
        /// Creates a new field and immediately starts a new job in it.
        /// </summary>
        /// <param name="fieldName">Name for the new field</param>
        /// <param name="profileName">Name of the vehicle profile</param>
        /// <param name="workType">Type of work</param>
        /// <param name="jobName">Name for the new job</param>
        /// <returns>The created job, or null on failure</returns>
        public CJob CreateFieldAndJob(string fieldName, string profileName, string workType, string jobName)
        {
            // Create the field first
            string fieldDir = CreateField(fieldName);
            if (string.IsNullOrEmpty(fieldDir))
                return null;

            // Now create the job in the new field
            var job = new CJob(fieldName.Trim(), profileName, workType, mf.tool.width, jobName);

            // Initialize job files
            JobFiles.InitializeJobFiles(job, fieldDir);

            // Set the current job
            mf.currentJob = job;

            // Load job-specific coverage data (empty for new field)
            mf.FileLoadJobData();

            Log.EventWriter($"Field and job created: {fieldName} / {job.Name}");
            return job;
        }

        #endregion

        #region Job Queries

        /// <summary>
        /// Gets all active (incomplete) jobs across all fields.
        /// </summary>
        public List<(CJob Job, string FieldDirectory)> GetAllActiveJobs()
        {
            return JobFiles.ListAllActiveJobs(RegistrySettings.fieldsDirectory);
        }

        /// <summary>
        /// Gets the most recently opened active job.
        /// </summary>
        public (CJob Job, string FieldDirectory) GetLastActiveJob()
        {
            var jobs = GetAllActiveJobs();
            return jobs.Count > 0 ? jobs[0] : (null, null);
        }

        #endregion
    }
}
