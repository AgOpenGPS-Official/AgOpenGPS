// Purpose: Minimal snapshot of raw inputs for a single tick (includes legacy pn.fix).
using AgOpenGPS.Core.Models; // vec2

namespace AgOpenGPS.Core.Positioning
{
    /// <summary>
    /// Minimal sensor snapshot. Distances in meters (local plane), angles in degrees unless noted.
    /// </summary>
    public sealed class SensorInputs
    {
        /// <summary>Raw local-plane position before any antenna/roll correction (legacy pn.fix).</summary>
        public vec2 RawFixLocal { get; set; }

        /// <summary>Vehicle speed in km/h if known; null if not available.</summary>
        public double? SpeedKmh { get; set; }

        /// <summary>True heading from VTG/RMC in degrees (0..360); null if unknown.</summary>
        public double? HeadingTrueDeg { get; set; }

        /// <summary>True heading from dual-antenna in degrees (0..360); null if unknown.</summary>
        public double? HeadingDualDeg { get; set; }

        /// <summary>IMU heading in degrees (0..360); null if IMU disconnected.</summary>
        public double? ImuHeadingDeg { get; set; }

        /// <summary>IMU roll in degrees; null if IMU disconnected.</summary>
        public double? ImuRollDeg { get; set; }

        /// <summary>IMU yaw rate in degrees per second; null if not provided.</summary>
        public double? ImuYawRateDegPerSec { get; set; }

        /// <summary>Altitude in meters; null if not provided.</summary>
        public double? AltitudeMeters { get; set; }

        /// <summary>GNSS satellites tracked; null if not provided.</summary>
        public int? SatellitesTracked { get; set; }

        /// <summary>GNSS fix quality code; null if not provided.</summary>
        public byte? FixQuality { get; set; }

        /// <summary>Horizontal dilution of precision; null if not provided.</summary>
        public double? Hdop { get; set; }

        /// <summary>Differential age in seconds; null if not provided.</summary>
        public double? AgeSeconds { get; set; }

        /// <summary>Selected heading source policy for this tick.</summary>
        public HeadingSource HeadingSource { get; set; }

        /// <summary>Elapsed time since previous tick in seconds.</summary>
        public double DeltaTimeSeconds { get; set; }

        /// <summary>Estimated GNSS update rate (Hz).</summary>
        public double GpsHz { get; set; }
    }
}
