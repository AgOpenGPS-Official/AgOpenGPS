using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing.Controllers
{
    public class FieldController : IFieldController
    {
        private readonly FormGPS mf;

        public bool IsFieldOpen => mf.isJobStarted;

        public FieldController(FormGPS formGPS)
        {
            mf = formGPS ?? throw new ArgumentNullException(nameof(formGPS));
        }

        public void CreateNewField(string fieldName, double centerLat, double centerLon)
        {
            // Set the local plane origin
            var origin = new Wgs84(centerLat, centerLon);
            mf.AppModel.LocalPlane.Origin = origin;
            mf.AppModel.CurrentLatLon = origin;

            // Initialize the field name
            mf.AppModel.Fields.SetCurrentFieldByName(fieldName);

            // Open the field (creates necessary structures)
            if (!mf.isJobStarted)
            {
                mf.JobNew();
            }
        }

        public void AddBoundary(List<vec3> boundaryPoints, bool isOuter = true)
        {
            if (!IsFieldOpen)
            {
                throw new InvalidOperationException("Cannot add boundary without an open field");
            }

            if (boundaryPoints == null || boundaryPoints.Count < 3)
            {
                throw new ArgumentException("Boundary must have at least 3 points", nameof(boundaryPoints));
            }

            // Create a new boundary list
            CBoundaryList bndList = new CBoundaryList
            {
                isSet = true,
                isOuter = isOuter
            };

            // Add all the points
            foreach (var point in boundaryPoints)
            {
                bndList.fenceLineEar.Add(point);
            }

            // Calculate area (simplified)
            bndList.CalculateFenceArea();

            // Add to the boundary manager
            mf.bnd.bndList.Add(bndList);
        }

        public void CreateTrack(vec3 pointA, vec3 pointB, double headingDegrees)
        {
            if (!IsFieldOpen)
            {
                throw new InvalidOperationException("Cannot create track without an open field");
            }

            // Create a new track
            CTrk track = new CTrk(mf)
            {
                name = "TestTrack",
                mode = TrackMode.AB,
                isVisible = true
            };

            // Set the track points
            track.ptA = pointA;
            track.ptB = pointB;

            // Calculate the heading
            double dx = pointB.easting - pointA.easting;
            double dy = pointB.northing - pointA.northing;
            track.heading = Math.Atan2(dx, dy);

            // Add to track array
            mf.trk.gArr.Add(track);
        }

        public void SelectTrack(int trackIndex)
        {
            if (!IsFieldOpen)
            {
                throw new InvalidOperationException("Cannot select track without an open field");
            }

            if (trackIndex < 0 || trackIndex >= mf.trk.gArr.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(trackIndex), "Track index out of range");
            }

            mf.trk.idx = trackIndex;
        }

        public Field GetCurrentField()
        {
            return mf.AppModel.Fields.ActiveField;
        }
    }
}
