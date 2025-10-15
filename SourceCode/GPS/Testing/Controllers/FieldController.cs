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
            // Set the local plane origin via DefineLocalPlane
            var origin = new Wgs84(centerLat, centerLon);
            mf.pn.DefineLocalPlane(origin, false);
            mf.AppModel.CurrentLatLon = origin;

            // Initialize the field name
            mf.AppModel.Fields.SetCurrentFieldByName(fieldName);

            // Open the field (creates necessary structures)
            if (!mf.isJobStarted)
            {
                mf.JobNew();
            }
        }

        public void AddBoundary(List<TestPoint> boundaryPoints, bool isOuter = true)
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
            CBoundaryList bndList = new CBoundaryList();

            // Add all the points (convert TestPoint to vec2 for fence line)
            foreach (var point in boundaryPoints)
            {
                bndList.fenceLineEar.Add(new vec2(point.Easting, point.Northing));
            }

            // Calculate area
            int idx = mf.bnd.bndList.Count;
            bndList.CalculateFenceArea(idx);

            // Add to the boundary manager
            mf.bnd.bndList.Add(bndList);
        }

        public void CreateTrack(TestPoint pointA, TestPoint pointB, double headingDegrees)
        {
            if (!IsFieldOpen)
            {
                throw new InvalidOperationException("Cannot create track without an open field");
            }

            // Create a new track
            CTrk track = new CTrk()
            {
                name = "TestTrack",
                mode = TrackMode.AB,
                isVisible = true
            };

            // Set the track points (convert TestPoint to vec2)
            track.ptA = new vec2(pointA.Easting, pointA.Northing);
            track.ptB = new vec2(pointB.Easting, pointB.Northing);

            // Calculate the heading
            double dx = pointB.Easting - pointA.Easting;
            double dy = pointB.Northing - pointA.Northing;
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
