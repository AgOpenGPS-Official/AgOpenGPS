// Purpose: Skeleton for translating AgIO UDP packets into SensorInputs.
// For now only the signature exists; implementation follows in later PRs.

using System;
using AgOpenGPS.Core.Positioning;

namespace AgOpenGPS.Core
{
    public static class SensorInputsMapper
    {
        /// <summary>
        /// Translates a raw AgIO UDP packet into SensorInputs.
        /// </summary>
        public static SensorInputs MapFromAgIoPacket(byte[] packet)
        {
            throw new NotImplementedException("Mapper not implemented yet.");
        }
    }
}
