// Purpose: Verifies that the SensorInputsMapper skeleton is present and throws NotImplementedException.

using NUnit.Framework;
using System;

namespace AgOpenGPS.Core.Tests.Positioning
{
    [TestFixture]
    public class SensorInputsMapperTests
    {
        [Test]
        public void MapFromAgIoPacket_NotYetImplemented_Throws()
        {
            // Arrange
            byte[] dummy = new byte[] { 0x80, 0x81, 0x00 };

            // Act & Assert
            Assert.Throws<NotImplementedException>(() =>
            {
                var _ = AgOpenGPS.Core.SensorInputsMapper.MapFromAgIoPacket(dummy);
            });
        }
    }
}
