using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Testing
{
    public interface IFieldController
    {
        void CreateNewField(string fieldName, double centerLat, double centerLon);
        void AddBoundary(List<vec3> boundaryPoints, bool isOuter = true);
        void CreateTrack(vec3 pointA, vec3 pointB, double headingDegrees);
        void SelectTrack(int trackIndex);
        Field GetCurrentField();
        bool IsFieldOpen { get; }
    }
}
