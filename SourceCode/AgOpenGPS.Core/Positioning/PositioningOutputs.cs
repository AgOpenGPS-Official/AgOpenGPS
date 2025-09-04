// Purpose: Pure outputs of Positioning (placeholder; will expand later).
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Positioning
{
    /// <summary>
    /// Outputs produced by the Positioning pipeline. Angles in radians.
    /// </summary>
    public sealed class PositioningOutputs
    {
        /// <summary>
        /// Corrected local-plane position after antenna-offset and roll corrections.
        /// Until we add corrections, this will equal RawFixLocal.
        /// </summary>
        public GeoCoord CorrectedFixLocal { get; set; }

        /// <summary>Body heading in radians, normalized to [0, 2π).</summary>
        public double FixHeadingRad { get; set; }

        /// <summary>Detected motion direction.</summary>
        public ReverseStatus ReverseStatus { get; set; }
    }
}
