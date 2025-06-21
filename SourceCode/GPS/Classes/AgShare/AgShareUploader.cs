// This file centralizes all coordinate conversion previously handled in CNMEA
// Now uses LocalPlane, Wgs84, GeoCoord, and related structs (C# 7.1 compatible)

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Diagnostics;
using AgLibrary.Logging;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS
{
    public class CAgShareUploader
    {
        private readonly FormGPS gps;

        // Create a snapshot from the current GPS session to upload
        public static FieldSnapshot CreateSnapshot(FormGPS gps)
        {
            string dir = Path.Combine(RegistrySettings.fieldsDirectory, gps.currentFieldDirectory);
            string idPath = Path.Combine(dir, "agshare.txt");

            Guid fieldId;
            if (File.Exists(idPath))
            {
                string raw = File.ReadAllText(idPath).Trim();
                fieldId = Guid.Parse(raw);
            }
            else
            {
                fieldId = Guid.NewGuid();
            }

            List<List<vec3>> boundaries = new List<List<vec3>>();
            foreach (var b in gps.bnd.bndList)
            {
                boundaries.Add(b.fenceLine.ToList());
            }

            List<CTrk> tracks = gps.trk.gArr.ToList();

            Wgs84 origin = gps.AppModel.LocalPlane.Origin;
            LocalPlane plane = new LocalPlane(origin, new SharedFieldProperties());

            FieldSnapshot snapshot = new FieldSnapshot
            {
                FieldName = gps.displayFieldName,
                FieldDirectory = dir,
                FieldId = fieldId,
                OriginLat = origin.Latitude,
                OriginLon = origin.Longitude,
                Convergence = 0,
                Boundaries = boundaries,
                Tracks = tracks,
                Converter = plane
            };
            return snapshot;
        }

        // Upload snapshot to AgShare using boundary with holes
        public static async Task UploadAsync(FieldSnapshot snapshot, AgShareClient client, FormGPS gps)
        {
            try
            {
                if (snapshot.Boundaries == null || snapshot.Boundaries.Count == 0)
                    return;

                List<CoordinateDto> outer = ConvertBoundary(snapshot.Boundaries[0], snapshot.Converter);
                if (outer == null || outer.Count < 3) return;

                List<List<CoordinateDto>> holes = new List<List<CoordinateDto>>();
                for (int i = 1; i < snapshot.Boundaries.Count; i++)
                {
                    List<CoordinateDto> hole = ConvertBoundary(snapshot.Boundaries[i], snapshot.Converter);
                    if (hole.Count >= 4) holes.Add(hole);
                }

                List<AbLineUploadDto> abLines = ConvertAbLines(snapshot.Tracks, snapshot.Converter);

                bool isPublic = false;
                try
                {
                    string json = await client.DownloadFieldAsync(snapshot.FieldId);
                    AgShareFieldDto field = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                    if (field != null) isPublic = field.IsPublic;
                }
                catch (Exception)
                {
                    Log.EventWriter("Failed to check field visibility on AgShare, defaulting to private.");
                }

                var boundary = new
                {
                    outer = outer,
                    holes = holes
                };

                var payload = new
                {
                    name = snapshot.FieldName,
                    isPublic = isPublic,
                    origin = new { latitude = snapshot.OriginLat, longitude = snapshot.OriginLon },
                    boundary = boundary,
                    abLines = abLines,
                    convergence = snapshot.Convergence,
                    sourceId = (string)null
                };

                var uploadResult = await client.UploadFieldAsync(snapshot.FieldId, payload);
                bool ok = uploadResult.ok;
                string message = uploadResult.message;

                if (ok)
                {
                    string txtPath = Path.Combine(snapshot.FieldDirectory, "agshare.txt");
                    File.WriteAllText(txtPath, snapshot.FieldId.ToString());
                    gps.TimedMessageBox(1000, "AgShare", "Upload Succesfully");
                    Log.EventWriter("Field uploaded to AgShare: " + snapshot.FieldName + " (" + snapshot.FieldId + ")");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Error uploading field to AgShare: " + ex.Message);
            }
        }

        // Convert local NE boundary to WGS84
        private static List<CoordinateDto> ConvertBoundary(List<vec3> localFence, LocalPlane converter)
        {
            List<CoordinateDto> coords = new List<CoordinateDto>();
            for (int i = 0; i < localFence.Count; i++)
            {
                GeoCoord geo = new GeoCoord(localFence[i].northing, localFence[i].easting);
                Wgs84 wgs = converter.ConvertGeoCoordToWgs84(geo);
                coords.Add(new CoordinateDto { Latitude = wgs.Latitude, Longitude = wgs.Longitude });
            }

            if (coords.Count > 1)
            {
                CoordinateDto first = coords[0];
                CoordinateDto last = coords[coords.Count - 1];
                if (first.Latitude != last.Latitude || first.Longitude != last.Longitude)
                {
                    coords.Add(first);
                }
            }

            return coords;
        }

        // Convert track lines from local NE to WGS84 format
        private static List<AbLineUploadDto> ConvertAbLines(List<CTrk> tracks, LocalPlane converter)
        {
            List<AbLineUploadDto> result = new List<AbLineUploadDto>();

            foreach (CTrk ab in tracks)
            {
                if (ab.mode == TrackMode.AB)
                {
                    GeoCoord a = new GeoCoord(ab.ptA.northing, ab.ptA.easting);
                    GeoCoord b = new GeoCoord(ab.ptB.northing, ab.ptB.easting);
                    Wgs84 wgsA = converter.ConvertGeoCoordToWgs84(a);
                    Wgs84 wgsB = converter.ConvertGeoCoordToWgs84(b);

                    result.Add(new AbLineUploadDto
                    {
                        Name = ab.name,
                        Type = "AB",
                        Coords = new List<CoordinateDto>
                        {
                            new CoordinateDto { Latitude = wgsA.Latitude, Longitude = wgsA.Longitude },
                            new CoordinateDto { Latitude = wgsB.Latitude, Longitude = wgsB.Longitude }
                        }
                    });
                }
                else if (ab.mode == TrackMode.Curve && ab.curvePts.Count >= 2)
                {
                    List<CoordinateDto> coords = new List<CoordinateDto>();
                    foreach (vec3 pt in ab.curvePts)
                    {
                        GeoCoord geo = new GeoCoord(pt.northing, pt.easting);
                        Wgs84 wgs = converter.ConvertGeoCoordToWgs84(geo);
                        coords.Add(new CoordinateDto { Latitude = wgs.Latitude, Longitude = wgs.Longitude });
                    }

                    result.Add(new AbLineUploadDto
                    {
                        Name = ab.name,
                        Type = "Curve",
                        Coords = coords
                    });
                }
            }

            return result;
        }
    }
}
