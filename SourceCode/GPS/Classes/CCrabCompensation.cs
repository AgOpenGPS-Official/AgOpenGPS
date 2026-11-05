using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    /// <summary>
    /// Crab (side slip) compensation for dual antenna heading.
    ///
    /// With a dual antenna, fixHeading is the direction the tractor body points.
    /// When a side force pushes the rear axle sideways (side slope, off-centre draft,
    /// soil), the tractor drives slightly crabwise: the nose points a little towards
    /// the line while the rear axle actually travels parallel to it. Pure pursuit then
    /// believes it is already driving back to the line and stops steering, which
    /// leaves a constant cross track error of about crabAngle * lookAheadDistance.
    ///
    /// This class measures the direction the pivot (rear axle) really travels over
    /// the ground and keeps a filtered difference with the body heading: the crab
    /// angle. Pure pursuit uses fixHeading + crabAngle (the travel direction of the
    /// rear axle) instead of the body heading.
    ///
    /// Measurement: a least squares line is fitted through all pivot positions of the
    /// last few metres, in a frame aligned with the mean body heading over the same
    /// points. The slope of that line is the raw crab angle. Fitting many points gives
    /// a much cleaner direction than two points, so the filter can be fast.
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

        /// <summary>Last unfiltered measurement in radians, for diagnostics.</summary>
        public double crabAngleRaw;

        /// <summary>True when compensation is enabled and the dual antenna heading is in use.</summary>
        public bool isActive;

        private struct Sample
        {
            public double easting, northing, heading;
        }

        /// <summary>Sideways shift of the guidance pivot in metres, positive to the right. Zero unless tool-on-line is active.</summary>
        public double toolShift;

        private readonly List<Sample> window = new List<Sample>();
        private vec3 lastShiftPivot;
        private bool hasLastShiftPivot;
        private double distanceSinceUpdate;

        // Length of rear axle track used for one fit
        private const double windowLength = 3.0;

        // Minimum pivot movement before a new point is stored, avoids piling up points when slow
        private const double minPointSpacing = 0.02;

        // A new fit is made every this many metres
        private const double updateDistance = 0.25;

        // Low pass per fit. With 1 cm RTK noise a 3 m fit is about 0.13 degree, so this can be fast.
        private const double filterFactor = 0.5;

        // Plausible crab range
        private const double maxCrab = 3.0 * Math.PI / 180.0;

        // Fits with a larger value are treated as a GPS jump and ignored
        private const double maxRawCrab = 6.0 * Math.PI / 180.0;

        // Do not learn in sharp turns: body yaw over the window (6 degrees over 3 m = radius below about 29 m)
        private const double maxYawInWindow = 6.0 * Math.PI / 180.0;

        // Largest sideways shift of the guidance pivot for tool-on-line
        private const double maxToolShift = 0.10;

        // The shift may change by this much per metre travelled, so switching on or leaving a headland is no step
        private const double toolShiftRampPerMetre = 0.02;

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
            toolShift = 0;
            hasLastShiftPivot = false;
            isActive = false;
            ClearWindow();
        }

        private void ClearWindow()
        {
            window.Clear();
            distanceSinceUpdate = 0;
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
            UpdateToolShift(pivot);

            if (mf.isReverse || mf.avgSpeed < minSpeed || mf.yt.isYouTurnTriggered)
            {
                // Fade out instead of keeping a stale value, start a fresh window later
                crabAngle *= 0.95;
                ClearWindow();
                return;
            }

            // Store a point once the pivot has moved a little
            if (window.Count > 0)
            {
                Sample last = window[window.Count - 1];
                double step = Math.Sqrt(Sq(pivot.easting - last.easting) + Sq(pivot.northing - last.northing));
                if (step < minPointSpacing) return;
                distanceSinceUpdate += step;
            }

            window.Add(new Sample { easting = pivot.easting, northing = pivot.northing, heading = bodyHeading });

            // Keep only the last windowLength metres
            while (window.Count > 2 && Span(1) >= windowLength) window.RemoveAt(0);

            // Wait for a full window, then fit every updateDistance
            if (Span(0) < windowLength * 0.9 || distanceSinceUpdate < updateDistance) return;
            distanceSinceUpdate = 0;

            if (!TryFit(out double raw)) return;

            crabAngleRaw = raw;
            crabAngle += (raw - crabAngle) * filterFactor;

            if (crabAngle > maxCrab) crabAngle = maxCrab;
            else if (crabAngle < -maxCrab) crabAngle = -maxCrab;
        }

        /// <summary>
        /// True for a tool that is mounted rigidly on the tractor (three point, front linkage). Only then the
        /// tool position follows exactly from the tractor position and heading.
        /// </summary>
        /// <summary>Shift actually applied to the guidance pivot. Stanley uses the plain pivot.</summary>
        public double AppliedShift => mf.isStanleyUsed ? 0 : toolShift;

        /// <summary>True when the tool XTE is shown: tool-on-line is on and in use.</summary>
        public bool isToolOnLineActive =>
            isActive && !mf.isStanleyUsed && IsRigidTool && Properties.ToolSettings.Default.setVehicle_isCrabToolOnLine;

        /// <summary>
        /// Position of the hitch of a rigid tool: the rear axle plus the hitch length along the body heading.
        /// </summary>
        public void GetToolPoint(out double easting, out double northing)
        {
            double hitch = mf.tool.GetHitchLengthFromVehiclePivot();
            easting = mf.pivotAxlePos.easting + Math.Sin(mf.fixHeading) * hitch;
            northing = mf.pivotAxlePos.northing + Math.Cos(mf.fixHeading) * hitch;
        }

        public bool IsRigidTool => !mf.tool.isToolTrailing && !mf.tool.isToolTBT;

        // Move toolShift towards its target, limited per metre travelled
        private void UpdateToolShift(vec3 pivot)
        {
            double step = hasLastShiftPivot
                ? Math.Sqrt(Sq(pivot.easting - lastShiftPivot.easting) + Sq(pivot.northing - lastShiftPivot.northing))
                : 0;
            lastShiftPivot = pivot;
            hasLastShiftPivot = true;

            double target = 0;
            if (Properties.ToolSettings.Default.setVehicle_isCrabToolOnLine && IsRigidTool)
            {
                // Lateral offset of the hitch relative to the track of the pivot, positive to the right of travel.
                // Uses the filtered crab angle, not the live heading, so the steering loop stays stable.
                target = -mf.tool.GetHitchLengthFromVehiclePivot() * Math.Sin(crabAngle);
                target = Math.Max(-maxToolShift, Math.Min(maxToolShift, target));
            }

            double maxChange = toolShiftRampPerMetre * step;
            toolShift += Math.Max(-maxChange, Math.Min(maxChange, target - toolShift));
        }

        /// <summary>
        /// Pivot to steer onto the line. Normally the rear axle. With the tool-on-line option it is shifted
        /// sideways so the hitch, and with it a rigidly mounted tool, ends up on the line in steady state while
        /// the rear axle runs beside it. The shift is limited and built up slowly, see UpdateToolShift.
        ///
        /// Only correct for a real crab. A dual heading offset error gives the same measurement but no
        /// real crab, so enable it only when the crab reads about zero on flat ground.
        /// </summary>
        public vec3 GetGuidancePivot(vec3 pivot)
        {
            if (glm.IsZero(AppliedShift)) return pivot;

            double travelHeading = mf.guidanceHeading;

            return new vec3(
                pivot.easting + Math.Cos(travelHeading) * AppliedShift,
                pivot.northing - Math.Sin(travelHeading) * AppliedShift,
                pivot.heading);
        }
        // Straight distance from window[first] to the newest point
        private double Span(int first)
        {
            Sample a = window[first];
            Sample b = window[window.Count - 1];
            return Math.Sqrt(Sq(b.easting - a.easting) + Sq(b.northing - a.northing));
        }

        private bool TryFit(out double raw)
        {
            raw = 0;
            int n = window.Count;
            if (n < 5) return false;

            // Mean body heading over the window, and yaw span to reject turns
            double h0 = window[0].heading;
            double sumDh = 0, minDh = 0, maxDh = 0;
            for (int i = 0; i < n; i++)
            {
                double dh = WrapPi(window[i].heading - h0);
                sumDh += dh;
                if (dh < minDh) minDh = dh;
                if (dh > maxDh) maxDh = dh;
            }
            if (maxDh - minDh > maxYawInWindow) return false;

            double meanHeading = h0 + sumDh / n;
            double sin = Math.Sin(meanHeading);
            double cos = Math.Cos(meanHeading);

            // Rotate positions into a frame along the mean heading:
            // along = forward distance, lateral = distance to the right
            double e0 = window[0].easting, n0 = window[0].northing;
            double sumA = 0, sumL = 0;
            double[] along = new double[n];
            double[] lateral = new double[n];
            for (int i = 0; i < n; i++)
            {
                double de = window[i].easting - e0;
                double dn = window[i].northing - n0;
                along[i] = de * sin + dn * cos;
                lateral[i] = de * cos - dn * sin;
                sumA += along[i];
                sumL += lateral[i];
            }

            double meanA = sumA / n, meanL = sumL / n;
            double sxx = 0, sxy = 0;
            for (int i = 0; i < n; i++)
            {
                double da = along[i] - meanA;
                sxx += da * da;
                sxy += da * (lateral[i] - meanL);
            }
            if (sxx < 1e-6) return false;

            // Slope of the rear axle track relative to the body heading
            raw = Math.Atan(sxy / sxx);
            return Math.Abs(raw) <= maxRawCrab;
        }

        private static double Sq(double v) => v * v;

        private static double WrapPi(double angle)
        {
            while (angle > Math.PI) angle -= glm.twoPI;
            while (angle < -Math.PI) angle += glm.twoPI;
            return angle;
        }
    }
}
