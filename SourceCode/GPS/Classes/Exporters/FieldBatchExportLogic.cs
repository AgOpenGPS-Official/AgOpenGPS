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
        public void MoveAll(IList<string> source, IList<string> target)
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
            return !Properties.Settings.Default.AgShareEnabled;
        }
    }

    /// <summary>
    /// Small DTO used by multi-field exporter (matches your single-field exporter inputs).
    /// </summary>
    public sealed class FieldExportData
    {
        // Field display name / designator in ISOXML
        public string Designator { get; set; }

        // Area in square meters (cast to ulong inside exporter)
        public int Area { get; set; }

        // Boundary + headland structures expected by your exporter
        public List<CBoundaryList> BoundaryList { get; set; }

        // Local plane (origin) for WGS84 conversions
        public LocalPlane LocalPlane { get; set; }

        // Tracks (AB + Curve) for the field
        public List<CTrk> Tracks { get; set; }
    }

    /// <summary>
    /// Loader placeholder that gathers the per-field models needed by the exporter.
    /// Replace the bodies with your real file readers (Boundary, Headland, Tracks, Origin, Area).
    /// </summary>
    internal static class FieldDataLoader
    {
        /// <summary>
        /// Loads all data needed for exporting a single field.
        /// </summary>
        public static FieldExportData LoadForField(string fieldDirectory, string designator)
        {
            var localPlane = TryLoadLocalPlane(fieldDirectory);
            var boundaries = TryLoadBoundaries(fieldDirectory) ?? new List<CBoundaryList>();
            var tracks = TryLoadTracks(fieldDirectory) ?? new List<CTrk>();

            var area = TryComputeArea(boundaries);

            return new FieldExportData
            {
                Designator = designator,
                Area = area,
                BoundaryList = boundaries,
                LocalPlane = localPlane,
                Tracks = tracks
            };
        }


        // --- Stubs to be replaced with your existing readers. ---

        /// <summary>
        /// Reads the field origin / local plane from the field folder.
        /// </summary>
        private static LocalPlane TryLoadLocalPlane(string fieldDirectory)
        {
            // TODO: Implement: read stored origin (e.g., Origin.txt / Field.txt / JSON)
            return null;
        }

        /// <summary>
        /// Reads boundaries/headlands into List&lt;CBoundaryList&gt;.
        /// </summary>
        private static List<CBoundaryList> TryLoadBoundaries(string fieldDirectory)
        {
            // TODO: Implement: parse Boundary.txt / bnd files → CBoundaryList
            return null;
        }

        /// <summary>
        /// Reads AB and Curve tracks into CTrack (gArr filled with CTrk).
        /// </summary>
        private static List<CTrk> TryLoadTracks(string fieldDirectory)
        {
            // TODO: lees hier TrackLines.txt of vergelijkbare opslag
            // en geef een lijst van CTrk terug
            return null;
        }


        /// <summary>
        /// Computes area from the outer boundary if available, else 0.
        /// </summary>
        private static int TryComputeArea(List<CBoundaryList> bndList)
        {
            if (bndList == null || bndList.Count == 0) return 0;
            try
            {
                // TODO: Implement with your existing area logic.
                return 0;
            }
            catch { return 0; }
        }
    }
}
