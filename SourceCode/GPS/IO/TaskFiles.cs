using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgLibrary.Logging;

namespace AgOpenGPS.IO
{
    /// <summary>
    /// Handles file I/O operations for Task data.
    /// Tasks are stored in Fields/{FieldName}/Tasks/{TaskName}/ folders.
    /// </summary>
    public static class TaskFiles
    {
        public const string TasksFolder = "Tasks";
        public const string TaskFileName = "task.json";

        /// <summary>
        /// Gets the Tasks directory path for a field
        /// </summary>
        public static string GetTasksDirectory(string fieldDirectory)
        {
            return Path.Combine(fieldDirectory, TasksFolder);
        }

        /// <summary>
        /// Gets the full path to a specific task folder
        /// </summary>
        public static string GetTaskDirectory(string fieldDirectory, string taskFolderName)
        {
            return Path.Combine(fieldDirectory, TasksFolder, taskFolderName);
        }

        /// <summary>
        /// Gets the path to the task.json file
        /// </summary>
        public static string GetTaskFilePath(string taskDirectory)
        {
            return Path.Combine(taskDirectory, TaskFileName);
        }

        /// <summary>
        /// Saves a task to its directory. Creates the directory if it doesn't exist.
        /// </summary>
        public static void Save(CTask task, string fieldDirectory)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrEmpty(fieldDirectory)) throw new ArgumentNullException(nameof(fieldDirectory));

            string taskDir = GetTaskDirectory(fieldDirectory, task.FolderName);

            // Create Tasks folder and task subfolder if needed
            Directory.CreateDirectory(taskDir);

            // Write task.json
            string taskFilePath = GetTaskFilePath(taskDir);
            File.WriteAllText(taskFilePath, task.ToJson());

            Log.EventWriter($"Task saved: {task.Name} in {taskDir}");
        }

        /// <summary>
        /// Loads a task from a task directory
        /// </summary>
        public static CTask Load(string taskDirectory)
        {
            if (string.IsNullOrEmpty(taskDirectory)) return null;

            string taskFilePath = GetTaskFilePath(taskDirectory);
            if (!File.Exists(taskFilePath)) return null;

            try
            {
                string json = File.ReadAllText(taskFilePath);
                return CTask.FromJson(json);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error loading task from {taskDirectory}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Lists all tasks for a field, ordered by last opened date (most recent first)
        /// </summary>
        public static List<CTask> ListTasks(string fieldDirectory)
        {
            var tasks = new List<CTask>();

            string tasksDir = GetTasksDirectory(fieldDirectory);
            if (!Directory.Exists(tasksDir)) return tasks;

            foreach (string subDir in Directory.GetDirectories(tasksDir))
            {
                var task = Load(subDir);
                if (task != null)
                {
                    tasks.Add(task);
                }
            }

            // Sort by last opened, most recent first
            return tasks.OrderByDescending(t => t.LastOpenedAt).ToList();
        }

        /// <summary>
        /// Gets all incomplete (active) tasks for a field
        /// </summary>
        public static List<CTask> ListActiveTasks(string fieldDirectory)
        {
            return ListTasks(fieldDirectory)
                .Where(t => !t.IsCompleted)
                .ToList();
        }

        /// <summary>
        /// Gets the most recently opened task for a field
        /// </summary>
        public static CTask GetLastTask(string fieldDirectory)
        {
            return ListTasks(fieldDirectory).FirstOrDefault();
        }

        /// <summary>
        /// Gets the most recently opened incomplete task for a field
        /// </summary>
        public static CTask GetLastActiveTask(string fieldDirectory)
        {
            return ListActiveTasks(fieldDirectory).FirstOrDefault();
        }

        /// <summary>
        /// Checks if a field has any tasks
        /// </summary>
        public static bool HasTasks(string fieldDirectory)
        {
            string tasksDir = GetTasksDirectory(fieldDirectory);
            if (!Directory.Exists(tasksDir)) return false;
            return Directory.GetDirectories(tasksDir).Length > 0;
        }

        /// <summary>
        /// Deletes a task and all its files
        /// </summary>
        public static bool DeleteTask(string fieldDirectory, CTask task)
        {
            if (task == null) return false;

            string taskDir = GetTaskDirectory(fieldDirectory, task.FolderName);
            if (!Directory.Exists(taskDir)) return false;

            try
            {
                Directory.Delete(taskDir, true);
                Log.EventWriter($"Task deleted: {task.Name}");
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error deleting task {task.Name}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Creates the task directory structure and initializes empty coverage files
        /// </summary>
        public static void InitializeTaskFiles(CTask task, string fieldDirectory)
        {
            string taskDir = GetTaskDirectory(fieldDirectory, task.FolderName);
            Directory.CreateDirectory(taskDir);

            // Save the task metadata
            Save(task, fieldDirectory);

            // Create empty Sections.txt for coverage
            SectionsFiles.CreateEmpty(taskDir);

            // Create empty Contour.txt
            ContourFiles.CreateEmpty(taskDir);

            // Create empty Flags.txt
            FlagsFiles.CreateEmpty(taskDir);

            // Create empty RecPath.txt
            RecPathFiles.CreateEmpty(taskDir);

            Log.EventWriter($"Task initialized: {task.Name} in {taskDir}");
        }

        /// <summary>
        /// Finds a task by its ID across all fields
        /// </summary>
        public static (CTask Task, string FieldDirectory) FindTaskById(Guid taskId, string fieldsDirectory)
        {
            if (!Directory.Exists(fieldsDirectory)) return (null, null);

            foreach (string fieldDir in Directory.GetDirectories(fieldsDirectory))
            {
                foreach (var task in ListTasks(fieldDir))
                {
                    if (task.Id == taskId)
                    {
                        return (task, fieldDir);
                    }
                }
            }

            return (null, null);
        }

        /// <summary>
        /// Lists all incomplete tasks across all fields, sorted by last opened
        /// </summary>
        public static List<(CTask Task, string FieldDirectory)> ListAllActiveTasks(string fieldsDirectory)
        {
            var result = new List<(CTask Task, string FieldDirectory)>();

            if (!Directory.Exists(fieldsDirectory)) return result;

            foreach (string fieldDir in Directory.GetDirectories(fieldsDirectory))
            {
                foreach (var task in ListActiveTasks(fieldDir))
                {
                    result.Add((task, fieldDir));
                }
            }

            return result.OrderByDescending(x => x.Task.LastOpenedAt).ToList();
        }
    }
}
