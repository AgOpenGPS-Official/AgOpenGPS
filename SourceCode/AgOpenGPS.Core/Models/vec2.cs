// AgOpenGPS.Core/Models/vec2.cs
// Purpose: Minimal placeholder for vec2 used by the Positioning footprint.
// NOTE: Replace with the project's canonical vec2 when available.
namespace AgOpenGPS.Core.Models
{
    /// <summary>
    /// Minimal 2D point in local-plane meters.
    /// </summary>
    public struct vec2
    {
        public double easting;
        public double northing;

        public vec2(double easting, double northing)
        {
            this.easting = easting;
            this.northing = northing;
        }
    }
}
