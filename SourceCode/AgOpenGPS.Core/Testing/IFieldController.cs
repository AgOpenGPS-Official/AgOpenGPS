using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Testing
{
    /// <summary>
    /// Simple point structure for testing (easting, northing, heading)
    /// </summary>
    public struct TestPoint
    {
        public double Easting;
        public double Northing;
        public double Heading;

        public TestPoint(double easting, double northing, double heading)
        {
            Easting = easting;
            Northing = northing;
            Heading = heading;
        }
    }

    public interface IFieldController
    {
        void CreateNewField(string fieldName, double centerLat, double centerLon);
        void AddBoundary(List<TestPoint> boundaryPoints, bool isOuter = true);
        void CreateTrack(TestPoint pointA, TestPoint pointB, double headingDegrees);
        void SelectTrack(int trackIndex);
        Field GetCurrentField();
        bool IsFieldOpen { get; }
    }
}
