using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS
{
    public class CAgShareUpload
    {
        private readonly CTrack trk;
        private readonly FormGPS gps;

        public CAgShareUpload(FormGPS gpsInstance)
        {
            gps = gpsInstance;
            trk = gps.trk;
        }

        // Holds both the field ID and its public visibility status
        public class FieldIdentity
        {
            public Guid Id { get; set; }
            public bool IsPublic { get; set; }

            public FieldIdentity(Guid id, bool isPublic)
            {
                Id = id;
                IsPublic = isPublic;
            }
        }

        // Determines the field ID based on agshare.txt, or generates a new one
        // Then checks whether the field already exists in the cloud and if it's public
        public async Task<FieldIdentity> GetOrCreateFieldIdentityAsync(string fieldDirectory)
        {
            string agshareFile = Path.Combine(fieldDirectory, "agshare.txt");

            Guid id;
            if (File.Exists(agshareFile))
            {
                string text = File.ReadAllText(agshareFile).Trim();
                if (!Guid.TryParse(text, out id))
                {
                    id = Guid.NewGuid();
                    File.WriteAllText(agshareFile, id.ToString());
                }
            }
            else
            {
                id = Guid.NewGuid();
                File.WriteAllText(agshareFile, id.ToString());
            }

            var client = new AgShareClient();
            client.SetServer(Properties.Settings.Default.AgShareServer);
            client.SetApiKey(Properties.Settings.Default.AgShareApiKey);

            var existing = await client.GetFieldByIdAsync(id.ToString());
            bool isPublic = existing != null && existing.IsPublic;

            return new FieldIdentity(id, isPublic);
        }

        // Uploads the field to AgShare, with proper structure and public status
        public async Task UploadAsync(
    FieldIdentity identity,
    string fieldDirectory,
    string fieldName,
    List<vec2> boundary,
    LocalPlane plane)
        {
            if (boundary == null || boundary.Count < 3)
            {
                gps.TimedMessageBox(2000, "AgShare - No Boundary", "Upload Failed");
                return;
            }

            // Calculate origin based on average of boundary points
            double avgLat = 0;
            double avgLon = 0;
            var latlonPoints = new List<(double Lat, double Lon)>();

            foreach (var p in boundary)
            {
                var wgs = plane.ConvertGeoCoordToWgs84(new GeoCoord(p.northing, p.easting));
                avgLat += wgs.Latitude;
                avgLon += wgs.Longitude;
                latlonPoints.Add((wgs.Latitude, wgs.Longitude));
            }

            avgLat /= boundary.Count;
            avgLon /= boundary.Count;

            // Ensure the polygon is closed
            if (latlonPoints.Count > 0 && (latlonPoints[0] != latlonPoints[latlonPoints.Count - 1]))
            {
                latlonPoints.Add(latlonPoints[0]);
            }

            var boundaryList = latlonPoints.Select(pt => new
            {
                latitude = pt.Lat,
                longitude = pt.Lon
            }).ToList();

            // Convert AB lines and curves to cloud-friendly format
            var abLines = new List<object>();
            foreach (var track in trk.gArr)
            {
                List<vec2> pts = (track.curvePts.Count > 0)
                    ? track.curvePts.Select(p => new vec2(p.easting, p.northing)).ToList()
                    : new List<vec2> { track.ptA, track.ptB };

                if (pts.Count < 2) continue;

                var coords = pts.Select(p =>
                {
                    var wgs = plane.ConvertGeoCoordToWgs84(new GeoCoord(p.northing, p.easting));
                    return new
                    {
                        latitude = wgs.Latitude,
                        longitude = wgs.Longitude
                    };
                }).ToList();

                abLines.Add(new
                {
                    name = track.name ?? "Unnamed",
                    type = track.curvePts.Count > 0 ? "Curve" : "AB",
                    coords = coords
                });
            }

            // Prepare JSON payload for upload
            var client = new AgShareClient();
            client.SetServer(Properties.Settings.Default.AgShareServer);
            client.SetApiKey(Properties.Settings.Default.AgShareApiKey);

            var payload = new
            {
                id = identity.Id,
                name = fieldName,
                isPublic = identity.IsPublic,
                origin = new { latitude = avgLat, longitude = avgLon },
                convergence = 0,
                sourceId = (string)null,
                boundary = boundaryList,
                abLines = abLines
            };

            bool success = await client.UploadAsync(identity.Id.ToString(), payload);

            if (success)
            {
                File.WriteAllText(Path.Combine(fieldDirectory, "agshare.txt"), identity.Id.ToString());
                gps.TimedMessageBox(2000, "AgShare", "Uploaded successfully.");
            }
            else
            {
                gps.TimedMessageBox(2000, "AgShare", "Upload failed.");
            }
        }
    }
}
