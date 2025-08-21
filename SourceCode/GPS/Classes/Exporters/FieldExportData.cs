// File: FieldExportData.cs
// All comments in English (C# 7.3 compatible)
using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.Exporters
{
    /// <summary>
    /// DTO used by the multi-field exporter. Matches single-field exporter inputs.
    /// </summary>
    public sealed class FieldExportData
    {
        // Field display name / designator in ISOXML
        public string Designator { get; set; }

        // Area in square meters
        public int Area { get; set; }

        // Boundaries + headland (as used by ISO exporter)
        public List<CBoundaryList> BoundaryList { get; set; }

        // Local plane (origin) for WGS84 conversions
        public LocalPlane LocalPlane { get; set; }

        // Tracks (AB + Curve) for the field
        public List<CTrk> Tracks { get; set; }
    }
}
