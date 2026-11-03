using System;

namespace AgOpenGPS
{
    /// <summary>
    /// Crab (side slip) compensation for dual antenna heading.
    ///
    /// With a dual antenna, fixHeading is the direction the tractor body points.
    /// When a side force pushes the rear axle sideways (an offset or side-draft tool,
    /// a side slope), the tractor drives slightly crabwise: the nose points a little
    /// towards the line while the rear axle actually travels parallel to it.
    /// Pure pursuit then believes it is already driving back to the line and stops
    /// steering, which leaves a constant cross track error.
    ///
    /// This class measures the direction the pivot (rear axle) really travels over
    /// the ground and keeps a heavily filtered difference with the body heading:
    /// the crab angle. Pure pursuit uses fixHeading + crabAngle (the travel direction
    /// of the rear axle) instead of the body heading.
    ///
    /// Without side force a rear axle only rolls along its own length, so the crab
    /// angle stays near zero and guidance behaves exactly as before.
    ///
    /// Only used with dual antenna heading. Fix-to-fix heading is already derived
    /// from the travelled track, so the crab is already in it.
    /// </summary>
    public class CCrabCompensation
    {
        private readonly FormGPS mf;

        /// <summary>Filtered crab angle in radians. Positive when the rear axle travels to the right of the body heading.</summary>
        public double crabAngle;

        /// <summary>Last unfiltered sample in radians, for diagnostics.</summary>
        public double crabAngleRaw;

        /// <summary>True when compensation is enabled and the dual antenna heading is in use.</summary>
        public bool isActive;

        private vec2 lastPivot;
        private double lastSampleHeading;
        private bool hasLastPivot;

        // Rear axle travel per sample. Long enough to keep RTK noise small (about 0.5 degree raw at 1 cm noise).
        private const double sampleDistance = 1.0;

        // Low pass per sample (per metre), time constant of about 10 m of travel
        private const double filterFactor = 0.1;

        // Plausible crab range
        private const double maxCrab = 3.0 * Math.PI / 180.0;

        // Samples with a larger difference are treated as a GPS jump and ignored
        private const double maxRawCrab = 6.0 * Math.PI / 180.0;

        // Do not learn in sharp turns (body yaw per metre, 2 degrees = radius below about 29 m)
        private const double maxYawPerSample = 2.0 * Math.PI / 180.0;

        // Minimum speed in km/h to learn
        private const double minSpeed = 2.0;

        public CCrabCompensation(FormGPS _f)
        {
            mf = _f;
        }

        public void Reset()
        {
            crabAngle = 0;
            crabAngleRaw = 0;
            hasLastPivot = false;
            isActive = false;
        }

        /// <summary>
        /// Called once per fix after the pivot position is calculated.
        /// </summary>
        /// <param name="pivot">Pivot (rear axle) position</param>
        /// <param name="bodyHeading">Dual antenna heading in radians</param>
        /// <param name="isDualHeading">True when the dual antenna heading is the active heading source</param>
        public void Update(vec3 pivot, double bodyHeading, bool isDualHeading)
        {
            if (!Properties.ToolSettings.Default.setVehicle_isCrabCompensationOn || !isDualHeading)
            {
                Reset();
                return;
            }

            isActive = true;

            if (mf.isReverse || mf.avgSpeed < minSpeed || mf.yt.isYouTurnTriggered)
            {
                // Fade out instead of keeping a stale value, start a fresh sample later
                crabAngle *= 0.95;
                hasLastPivot = false;
                return;
            }

            if (!hasLastPivot)
            {
                lastPivot = new vec2(pivot.easting, pivot.northing);
                lastSampleHeading = bodyHeading;
                hasLastPivot = true;
                return;
            }

            double dE = pivot.easting - lastPivot.easting;
            double dN = pivot.northing - lastPivot.northing;
            if (dE * dE + dN * dN < sampleDistance * sampleDistance) return;

            // Direction the rear axle actually travelled over the last sample
            double trackHeading = Math.Atan2(dE, dN);

            // The chord of an arc points in the direction of the heading halfway the arc,
            // so compare against the mean body heading of this sample. This keeps the
            // measurement valid on curves as well.
            double yawChange = WrapPi(bodyHeading - lastSampleHeading);
            double midHeading = lastSampleHeading + yawChange * 0.5;

            lastPivot = new vec2(pivot.easting, pivot.northing);
            lastSampleHeading = bodyHeading;

            if (Math.Abs(yawChange) > maxYawPerSample) return;

            crabAngleRaw = WrapPi(trackHeading - midHeading);
            if (Math.Abs(crabAngleRaw) > maxRawCrab) return;

            crabAngle += (crabAngleRaw - crabAngle) * filterFactor;

            if (crabAngle > maxCrab) crabAngle = maxCrab;
            else if (crabAngle < -maxCrab) crabAngle = -maxCrab;
        }

        private static double WrapPi(double angle)
        {
            while (angle > Math.PI) angle -= glm.twoPI;
            while (angle < -Math.PI) angle += glm.twoPI;
            return angle;
        }
    }
}
