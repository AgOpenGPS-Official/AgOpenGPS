// Purpose: Orchestrator placeholder; no behavior yet.
using System;

namespace AgOpenGPS.Core.Positioning
{
    /// <summary>
    /// Executes Positioning for a single tick. This footprint version is a no-op.
    /// </summary>
    public sealed class PositioningEngine
    {
        /// <summary>
        /// No-op implementation: forwards RawFixLocal as CorrectedFixLocal and returns defaults.
        /// </summary>
        public PositioningOutputs Step(SensorInputs inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));

            return new PositioningOutputs
            {
                CorrectedFixLocal = inputs.RawFixLocal,
                FixHeadingRad = 0.0,
                ReverseStatus = ReverseStatus.Unknown
            };
        }
    }
}
