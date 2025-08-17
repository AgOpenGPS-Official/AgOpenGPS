// File: FieldBatchExportLogic.cs
// C# 7.3 compatible
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Protocols.ISOBUS;

namespace AgOpenGPS.Forms
{
    /// <summary>
    /// Logic helper for the "Export Multiple Fields" form.
    /// Keeps UI light: loading names, moving selections, building export data, and invoking exporters.
    /// </summary>
    public sealed class FieldBatchExportLogic
    {
        // Always provide a parameterless constructor (no DI)
        public FieldBatchExportLogic() { }

        /// <summary>
        /// Returns the root directory that contains all field folders.
        /// </summary>
        public string GetFieldsRoot()
        {
            // Use the global/static setting as agreed
            return RegistrySettings.fieldsDirectory;
        }

        /// <summary>
        /// Scans the fields root and returns all field folder names (single level).
        /// </summary>
        public List<string> LoadAvailableFieldNames()
        {
            var root = GetFieldsRoot();
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(root) && Directory.Exists(root))
            {
                foreach (var dir in Directory.EnumerateDirectories(root))
                {
                    var name = Path.GetFileName(dir);
                    if (!string.IsNullOrWhiteSpace(name))
                        result.Add(name);
                }
            }

            result.Sort(StringComparer.OrdinalIgnoreCase);
            return result;
        }

        /// <summary>
        /// Moves all items from source to target (no duplicates).
        /// </summary>
        public static void MoveAll(IList<string> source, IList<string> target)
        {
            foreach (var it in source)
                if (!target.Contains(it)) target.Add(it);
            source.Clear();
        }

        /// <summary>
        /// Moves the selected subset from source to target (no duplicates).
        /// </summary>
        public void MoveSelected(IList<string> source, IList<string> target, IEnumerable<string> selectedItems)
        {
            if (selectedItems == null) return;
            var toMove = selectedItems.Distinct().ToList();

            foreach (var it in toMove)
                if (source.Contains(it) && !target.Contains(it))
                    target.Add(it);

            foreach (var it in toMove)
                source.Remove(it);
        }

        /// <summary>
        /// Builds FieldExportData for the selected field names by reading each field folder.
        /// </summary>
        public List<FieldExportData> BuildExportData(IEnumerable<string> selectedFieldNames)
        {
            var root = GetFieldsRoot();
            var list = new List<FieldExportData>();

            foreach (var name in selectedFieldNames ?? Enumerable.Empty<string>())
            {
                var fieldDir = Path.Combine(root, name);
                if (!Directory.Exists(fieldDir)) continue;

                var data = FieldDataLoader.LoadForField(fieldDir, name);
                if (data != null) list.Add(data);
            }

            return list;
        }

        /// <summary>
        /// Exports multiple fields into a single ISOXML TaskData directory using your extended exporter.
        /// </summary>
        public void ExportIsoXmlBatch(IEnumerable<FieldExportData> fields, string outputDirectory, ISO11783_TaskFile.Version version)
        {
            if (fields == null || !fields.Any())
                throw new InvalidOperationException("No fields to export.");

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentNullException(nameof(outputDirectory));

            Directory.CreateDirectory(outputDirectory);

            // Call the multi-field export you will add to ISO11783_TaskFile
            ISO11783_TaskFile.ExportMultiple(outputDirectory, version, fields);
        }

        /// <summary>
        /// Returns whether AgShare export should be enabled (adjust to your real condition).
        /// </summary>
        public bool IsAgShareEnabled()
        {
            // Example check; replace with your actual flag/toggle
            return !string.IsNullOrWhiteSpace(RegistrySettings.agShareApiKey);
        }
    }

    /// <summary>
    /// Small DTO used by multi-field exporter (matches your single-field exporter inputs).
    /// </summa
