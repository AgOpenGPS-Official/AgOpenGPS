using System;
using System.Collections.Generic;
using System.IO;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Manages task lifecycle operations: create, resume, close, finish.
    /// Handles coverage import/migration and task state transitions.
    /// </summary>
    public class TaskManager
    {
        private readonly FormGPS mf;

        public TaskManager(FormGPS formGPS)
        {
            mf = formGPS;
        }

        #region Task Lifecycle

        /// <summary>
        /// Creates and starts a new task for the specified field.
        /// </summary>
        /// <param name="fieldDir">Full path to the field directory</param>
        /// <param name="fieldName">Display name of the field</param>
        /// <param name="profileName">Name of the vehicle profile</param>
        /// <param name="workType">Type of work (e.g., Spraying, Seeding)</param>
        /// <param name="taskName">Name for the new task</param>
        /// <param name="importCoverage">Whether to import existing coverage data</param>
        /// <returns>The created task, or null on failure</returns>
        public CTask CreateTask(string fieldDir, string fieldName, string profileName,
            string workType, string taskName, bool importCoverage = false)
        {
            // Check if field is already open and it's the same field
            bool fieldAlreadyOpen = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == fieldDir;

            // Save and close current task if needed
            if (mf.isJobStarted)
            {
                if (!fieldAlreadyOpen || mf.currentTask != null)
                {
                    SaveAndCloseCurrentTask();
                }
            }

            // Create the new task
            var task = new CTask(fieldName, profileName, workType, mf.tool.width, taskName);

            // Initialize task files
            TaskFiles.InitializeTaskFiles(task, fieldDir);

            // Handle coverage import
            if (importCoverage)
            {
                ImportCoverageToTask(task, fieldDir);
            }

            // Always clean up legacy coverage when creating a new task
            DeleteLegacyCoverage(fieldDir);

            // Set the current task
            mf.currentTask = task;

            // Open the field if not already open
            if (!fieldAlreadyOpen)
            {
                string fieldFile = Path.Combine(fieldDir, "Field.txt");
                mf.FileOpenField(fieldFile);
            }

            // Load task-specific coverage data
            mf.FileLoadTaskData();

            Log.EventWriter($"Task created: {task.Name}");
            return task;
        }

        /// <summary>
        /// Resumes an existing task.
        /// </summary>
        /// <param name="task">The task to resume</param>
        /// <param name="fieldDir">Full path to the field directory</param>
        /// <returns>True if successful</returns>
        public bool ResumeTask(CTask task, string fieldDir)
        {
            if (task == null || string.IsNullOrEmpty(fieldDir))
                return false;

            string fieldFile = Path.Combine(fieldDir, "Field.txt");
            if (!File.Exists(fieldFile))
            {
                Log.EventWriter($"Cannot resume task - field not found: {fieldDir}");
                return false;
            }

            // Check if we're resuming a task in the currently open field
            bool sameField = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == fieldDir;

            // Save and close current task if needed
            if (mf.isJobStarted)
            {
                SaveAndCloseCurrentTask();

                // Close field if different
                if (!sameField)
                {
                    mf.AppModel.Fields.CloseField();
                }
            }

            // Open the field
            mf.FileOpenField(fieldFile);

            // Set the current task BEFORE loading task data
            mf.currentTask = task;

            // Load task-specific coverage data
            mf.FileLoadTaskData();

            // Update task timestamp
            task.Touch();
            TaskFiles.Save(task, fieldDir);

            Log.EventWriter($"Task resumed: {task.Name}");
            return true;
        }

        /// <summary>
        /// Closes the current task without marking it as completed.
        /// Saves all data and clears the current task reference.
        /// </summary>
        public void CloseCurrentTask()
        {
            if (mf.currentTask == null) return;

            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            // Save task metadata
            TaskFiles.Save(mf.currentTask, fieldDir);

            Log.EventWriter($"Task closed: {mf.currentTask.Name}");
            mf.currentTask = null;
        }

        /// <summary>
        /// Finishes (completes) the current task.
        /// Marks as completed, saves all data, and clears the current task reference.
        /// </summary>
        public void FinishCurrentTask()
        {
            if (mf.currentTask == null) return;

            // Mark as completed
            mf.currentTask.Complete();

            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            // Save task metadata
            TaskFiles.Save(mf.currentTask, fieldDir);

            Log.EventWriter($"Task finished: {mf.currentTask.Name}");
            mf.currentTask = null;
        }

        /// <summary>
        /// Saves current task data and metadata, then clears the task reference.
        /// </summary>
        private void SaveAndCloseCurrentTask()
        {
            if (mf.currentTask != null)
            {
                var currentFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                TaskFiles.Save(mf.currentTask, currentFieldDir);
                mf.currentTask = null;
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
        /// Imports existing coverage data from field root to the task directory.
        /// </summary>
        /// <param name="task">The task to import coverage into</param>
        /// <param name="fieldDir">Full path to the field directory</param>
        public void ImportCoverageToTask(CTask task, string fieldDir)
        {
            try
            {
                string taskDir = Path.Combine(fieldDir, "Tasks", task.FolderName);

                if (!Directory.Exists(taskDir))
                {
                    Directory.CreateDirectory(taskDir);
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
                        string destPath = Path.Combine(taskDir, destFileName);

                        File.Copy(sourcePath, destPath, overwrite: true);
                        filesCopied++;
                    }
                }

                Log.EventWriter($"Coverage imported to task: {task.Name} ({filesCopied} files)");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error importing coverage: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes legacy coverage files from field root.
        /// Called after creating a new task to prevent old coverage from being loaded.
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
        /// Creates a new field and immediately starts a new task in it.
        /// </summary>
        /// <param name="fieldName">Name for the new field</param>
        /// <param name="profileName">Name of the vehicle profile</param>
        /// <param name="workType">Type of work</param>
        /// <param name="taskName">Name for the new task</param>
        /// <returns>The created task, or null on failure</returns>
        public CTask CreateFieldAndTask(string fieldName, string profileName, string workType, string taskName)
        {
            // Create the field first
            string fieldDir = CreateField(fieldName);
            if (string.IsNullOrEmpty(fieldDir))
                return null;

            // Now create the task in the new field
            var task = new CTask(fieldName.Trim(), profileName, workType, mf.tool.width, taskName);

            // Initialize task files
            TaskFiles.InitializeTaskFiles(task, fieldDir);

            // Set the current task
            mf.currentTask = task;

            // Load task-specific coverage data (empty for new field)
            mf.FileLoadTaskData();

            Log.EventWriter($"Field and task created: {fieldName} / {task.Name}");
            return task;
        }

        #endregion

        #region Task Queries

        /// <summary>
        /// Gets all active (incomplete) tasks across all fields.
        /// </summary>
        public List<(CTask Task, string FieldDirectory)> GetAllActiveTasks()
        {
            return TaskFiles.ListAllActiveTasks(RegistrySettings.fieldsDirectory);
        }

        /// <summary>
        /// Gets the most recently opened active task.
        /// </summary>
        public (CTask Task, string FieldDirectory) GetLastActiveTask()
        {
            var tasks = GetAllActiveTasks();
            return tasks.Count > 0 ? tasks[0] : (null, null);
        }

        #endregion
    }
}
