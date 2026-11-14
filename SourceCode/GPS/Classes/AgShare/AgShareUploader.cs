// This file centralizes all coordinate conversion previously handled in CNMEA
// Now uses LocalPlane, Wgs84, GeoCoord, and related structs (C# 7.1 compatible)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgLibrary.Logging;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.AgShare.Models;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    // What happened as a result of AgShareUploader.UploadAsync - lets callers distinguish
    // "nothing to do" from an actual upload, e.g. to show a different status in the bulk uploader.
    public enum AgShareUploadOutcome
    {
        FullUpload,
        IncrementalUpload,
        Skipped,
        Failed
    }

    public class AgShareUploader
    {
        private readonly AgShareClient _client;

        public AgShareUploader(AgShareClient client)
        {
            _client = client;
        }

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

        // Uploads a snapshot to AgShare. Sends a full replace the first time a field is uploaded
        // (or when it was never synced incrementally before), and only the boundary and/or
        // individual guidance lines that actually changed on every upload after that.
        public async Task<AgShareUploadOutcome> UploadAsync(FieldSnapshot snapshot, FormGPS gps)
        {
            bool changed = false;
            try
            {
                string boundaryHash = AgShareSyncStore.ComputeBoundaryHash(snapshot.Boundaries);
                var lineHashes = new Dictionary<string, string>();
                foreach (CTrk track in SupportedTracks(snapshot.Tracks))
                {
                    lineHashes[track.name] = AgShareSyncStore.ComputeLineHash(track);
                }

                AgShareSyncState previous = AgShareSyncStore.Load(snapshot.FieldDirectory);

                // A baseline from another server says nothing about this one, so upload everything again.
                if (previous == null || previous.FieldId != snapshot.FieldId || previous.Server != CurrentServer())
                {
                    return await DoFullUploadAsync(snapshot, gps, boundaryHash, lineHashes);
                }

                if (boundaryHash != previous.BoundaryHash)
                {
                    var (outer, holes) = BuildBoundaryParts(snapshot);
                    var result = await _client.PatchBoundaryAsync(snapshot.FieldId, new PolygonDto { Outer = outer, Holes = holes });

                    if (IsNotFound(result))
                        return await DoFullUploadAsync(snapshot, gps, boundaryHash, lineHashes);

                    if (!result.IsSuccessful)
                    {
                        Log.EventWriter("Error patching AgShare boundary for '" + snapshot.FieldName + "': " + DescribeError(result.Error));
                        return FailIncremental(snapshot, changed);
                    }
                    changed = true;
                }

                foreach (CTrk track in SupportedTracks(snapshot.Tracks))
                {
                    if (previous.LineHashes.TryGetValue(track.name, out string prevHash) && prevHash == lineHashes[track.name])
                        continue;

                    GuidanceTrackDto dto = ConvertGuidanceTrack(track, snapshot.Converter);
                    if (dto == null) continue;

                    var result = await _client.UpsertGuidanceLineAsync(snapshot.FieldId, dto);

                    if (IsNotFound(result))
                        return await DoFullUploadAsync(snapshot, gps, boundaryHash, lineHashes);

                    if (!result.IsSuccessful)
                    {
                        Log.EventWriter("Error upserting AgShare guidance line '" + track.name + "' for '" + snapshot.FieldName + "': " + DescribeError(result.Error));
                        return FailIncremental(snapshot, changed);
                    }
                    changed = true;
                }

                foreach (string existingName in previous.LineHashes.Keys)
                {
                    if (lineHashes.ContainsKey(existingName)) continue;

                    var result = await _client.DeleteGuidanceLineAsync(snapshot.FieldId, existingName);

                    if (IsNotFound(result))
                        return await DoFullUploadAsync(snapshot, gps, boundaryHash, lineHashes);

                    if (!result.IsSuccessful)
                    {
                        Log.EventWriter("Error deleting AgShare guidance line '" + existingName + "' for '" + snapshot.FieldName + "': " + DescribeError(result.Error));
                        return FailIncremental(snapshot, changed);
                    }
                    changed = true;
                }

                if (!changed)
                {
                    Log.EventWriter("AgShare: field unchanged, skipping upload: " + snapshot.FieldName);
                    return AgShareUploadOutcome.Skipped;
                }

                AgShareSyncStore.Save(snapshot.FieldDirectory, new AgShareSyncState
                {
                    FieldId = snapshot.FieldId,
                    Server = CurrentServer(),
                    BoundaryHash = boundaryHash,
                    LineHashes = lineHashes
                });

                Log.EventWriter("AgShare: incrementally synced field: " + snapshot.FieldName);
                return AgShareUploadOutcome.IncrementalUpload;
            }
            catch (Exception ex)
            {
                Log.EventWriter("Error uploading field to AgShare: " + ex.Message);
                // An earlier request in this run may already have changed the cloud copy; do not trust the old baseline.
                if (changed) AgShareSyncStore.Delete(snapshot.FieldDirectory);
                return AgShareUploadOutcome.Failed;
            }
        }

        // Full replace of boundary + all guidance lines - used for the first-ever upload of a field,
        // whenever the field's cloud id changed, and as a fallback when an older server doesn't yet
        // support the incremental endpoints.
        private async Task<AgShareUploadOutcome> DoFullUploadAsync(FieldSnapshot snapshot, FormGPS gps, string boundaryHash, Dictionary<string, string> lineHashes)
        {
            var (outer, holes) = BuildBoundaryParts(snapshot);
            List<GuidanceTrackDto> abLines = ConvertGuidanceTracks(snapshot.Tracks, snapshot.Converter);

            bool isPublic = false;
            try
            {
                var downloadResult = await _client.DownloadFieldAsync(snapshot.FieldId);
                if (downloadResult.IsSuccessful)
                {
                    isPublic = downloadResult.Data.IsPublic;
                }
            }
            catch (Exception)
            {
                Log.EventWriter("Failed to check field visibility on AgShare, defaulting to private.");
            }

            var payload = new UploadFieldDto
            {
                Name = snapshot.FieldName,
                IsPublic = isPublic,
                Origin = new CoordinateDto { Latitude = snapshot.OriginLat, Longitude = snapshot.OriginLon },
                Boundary = new PolygonDto { Outer = outer, Holes = holes },
                AbLines = abLines
            };

            var result = await _client.UploadFieldAsync(snapshot.FieldId, payload);

            if (!result.IsSuccessful)
            {
                Log.EventWriter("Error uploading field to AgShare: " + DescribeError(result.Error));
                return AgShareUploadOutcome.Failed;
            }

            string txtPath = Path.Combine(snapshot.FieldDirectory, "agshare.txt");
            File.WriteAllText(txtPath, snapshot.FieldId.ToString());

            AgShareSyncStore.Save(snapshot.FieldDirectory, new AgShareSyncState
            {
                FieldId = snapshot.FieldId,
                Server = CurrentServer(),
                BoundaryHash = boundaryHash,
                LineHashes = lineHashes
            });

            Log.EventWriter("Field uploaded to AgShare: " + snapshot.FieldName + " (" + snapshot.FieldId + ")");
            return AgShareUploadOutcome.FullUpload;
        }

        // An earlier request in this run may already have changed the cloud copy, so the sidecar no longer
        // describes it. Dropping it makes the next upload a full replace that reconciles everything.
        private static AgShareUploadOutcome FailIncremental(FieldSnapshot snapshot, bool changed)
        {
            if (changed) AgShareSyncStore.Delete(snapshot.FieldDirectory);
            return AgShareUploadOutcome.Failed;
        }

        private static string CurrentServer()
        {
            return AgShareSyncStore.NormalizeServer(Settings.Default.AgShareServer);
        }

        private static bool IsNotFound(AgShareResult result)
        {
            return !result.IsSuccessful && result.Error is StatusCodeError sce && sce.StatusCode == HttpStatusCode.NotFound;
        }

        private static string DescribeError(AgShareError error)
        {
            if (error is StatusCodeError sce) return "HTTP " + (int)sce.StatusCode + ": " + sce.Body;
            if (error is HttpRequestError hre) return hre.Exception.Message;
            if (error is JsonError je) return je.Exception.Message;
            if (error is InvalidApiKeyError) return "invalid API key";
            return "unknown error";
        }

        // Tracks supported by AgShare: AB lines, and curves with at least 2 points.
        private static IEnumerable<CTrk> SupportedTracks(List<CTrk> tracks)
        {
            return tracks.Where(t => t.mode == TrackMode.AB || (t.mode == TrackMode.Curve && t.curvePts.Count >= 2));
        }

        private static (List<CoordinateDto> outer, List<List<CoordinateDto>> holes) BuildBoundaryParts(FieldSnapshot snapshot)
        {
            List<CoordinateDto> outer = new List<CoordinateDto>();
            List<List<CoordinateDto>> holes = new List<List<CoordinateDto>>();

            if (snapshot.Boundaries != null && snapshot.Boundaries.Count > 0)
            {
                var firstBoundary = snapshot.Boundaries.FirstOrDefault();
                if (firstBoundary != null)
                {
                    outer = ConvertBoundary(firstBoundary, snapshot.Converter);
                    if (outer == null) outer = new List<CoordinateDto>();
                }

                foreach (var innerBoundary in snapshot.Boundaries.Skip(1))
                {
                    List<CoordinateDto> hole = ConvertBoundary(innerBoundary, snapshot.Converter);
                    if (hole != null && hole.Count >= 4) holes.Add(hole);
                }
            }

            return (outer, holes);
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
        private static List<GuidanceTrackDto> ConvertGuidanceTracks(List<CTrk> tracks, LocalPlane converter)
        {
            List<GuidanceTrackDto> result = new List<GuidanceTrackDto>();

            foreach (CTrk ab in SupportedTracks(tracks))
            {
                GuidanceTrackDto dto = ConvertGuidanceTrack(ab, converter);
                if (dto != null) result.Add(dto);
            }

            return result;
        }

        // Convert a single track line from local NE to WGS84 format. Returns null for unsupported modes.
        private static GuidanceTrackDto ConvertGuidanceTrack(CTrk ab, LocalPlane converter)
        {
            if (ab.mode == TrackMode.AB)
            {
                GeoCoord a = new GeoCoord(ab.ptA.northing, ab.ptA.easting);
                GeoCoord b = new GeoCoord(ab.ptB.northing, ab.ptB.easting);
                Wgs84 wgsA = converter.ConvertGeoCoordToWgs84(a);
                Wgs84 wgsB = converter.ConvertGeoCoordToWgs84(b);

                return new GuidanceTrackDto
                {
                    Name = ab.name,
                    Type = "AB",
                    Coords = new List<CoordinateDto>
                    {
                        new CoordinateDto { Latitude = wgsA.Latitude, Longitude = wgsA.Longitude },
                        new CoordinateDto { Latitude = wgsB.Latitude, Longitude = wgsB.Longitude }
                    }
                };
            }

            if (ab.mode == TrackMode.Curve && ab.curvePts.Count >= 2)
            {
                List<CoordinateDto> coords = new List<CoordinateDto>();
                foreach (vec3 pt in ab.curvePts)
                {
                    GeoCoord geo = new GeoCoord(pt.northing, pt.easting);
                    Wgs84 wgs = converter.ConvertGeoCoordToWgs84(geo);
                    coords.Add(new CoordinateDto { Latitude = wgs.Latitude, Longitude = wgs.Longitude });
                }

                return new GuidanceTrackDto
                {
                    Name = ab.name,
                    Type = "Curve",
                    Coords = coords
                };
            }

            return null;
        }
    }
}
