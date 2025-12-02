using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using AgLibrary.Logging;

namespace AgOpenGPS.Classes.AgShare.Helpers
{
    /// <summary>
    /// Parses AgShare field DTOs into domain types ready for file writing.
    /// </summary>
    public static class AgShareFieldParser
    {
        // Validates if coordinates are within valid WGS84 ranges
        private static bool IsValidCoordinate(double latitude, double longitude)
        {
            return latitude >= -90 && latitude <= 90 &&
                   longitude >= -180 && longitude <= 180;
        }

        // Parses an AgShare field DTO into domain types
        public static ParsedField Parse(AgShareFieldDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Field DTO cannot be null");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Field name cannot be null or empty", nameof(dto));
            }

            if (!IsValidCoordinate(dto.Latitude, dto.Longitude))
            {
                throw new ArgumentException($"Invalid origin coordinates: Lat={dto.Latitude}, Lon={dto.Longitude}", nameof(dto));
            }

            bool hasBoundaries = dto.Boundaries != null && dto.Boundaries.Count > 0;
            bool hasAbLines = dto.AbLines != null && dto.AbLines.Count > 0;

            if (!hasBoundaries && !hasAbLines)
            {
                throw new ArgumentException($"Field '{dto.Name}' has no boundaries or AB lines", nameof(dto));
            }

            var result = new ParsedField
            {
                FieldId = dto.Id,
                Name = dto.Name,
                Origin = new Wgs84(dto.Latitude, dto.Longitude)
            };

            var converter = new GeoConverter(dto.Latitude, dto.Longitude);

            // Parse boundaries directly to CBoundaryList
            if (dto.Boundaries != null)
            {
                int boundaryIndex = 0;
                foreach (var ring in dto.Boundaries)
                {
                    if (ring == null || ring.Count < 3) continue;

                    var bnd = new CBoundaryList();
                    if (bnd.fenceLine == null) bnd.fenceLine = new List<vec3>();

                    foreach (var point in ring)
                    {
                        if (point == null) continue;
                        if (!IsValidCoordinate(point.Latitude, point.Longitude))
                        {
                            Log.EventWriter($"[AgShare] Skipping invalid boundary coordinate: Lat={point.Latitude}, Lon={point.Longitude}");
                            continue;
                        }

                        var local = converter.ToLocal(point.Latitude, point.Longitude);
                        bnd.fenceLine.Add(new vec3(local.Easting, local.Northing, 0.0));
                    }

                    if (bnd.fenceLine.Count >= 3)
                    {
                        // First ring is outer boundary, subsequent rings are holes (drive-through)
                        bnd.isDriveThru = boundaryIndex > 0;

                        // Normalize boundary (calculate area, fix spacing, compute headings)
                        bnd.CalculateFenceArea(boundaryIndex);
                        bnd.FixFenceLine(boundaryIndex);
                        result.Boundaries.Add(bnd);
                        boundaryIndex++;
                    }
                }
            }

            // Parse tracks directly to CTrk
            if (dto.AbLines != null)
            {
                foreach (var ab in dto.AbLines)
                {
                    if (ab == null || ab.Coords == null || ab.Coords.Count < 2) continue;
                    if (ab.Coords[0] == null || ab.Coords[1] == null) continue;

                    if (!IsValidCoordinate(ab.Coords[0].Latitude, ab.Coords[0].Longitude) ||
                        !IsValidCoordinate(ab.Coords[1].Latitude, ab.Coords[1].Longitude))
                    {
                        Log.EventWriter($"[AgShare] Skipping AB line '{ab.Name ?? "Unnamed"}' - invalid coordinates");
                        continue;
                    }

                    var vA = converter.ToLocal(ab.Coords[0].Latitude, ab.Coords[0].Longitude);
                    var vB = converter.ToLocal(ab.Coords[1].Latitude, ab.Coords[1].Longitude);
                    bool isCurve = ab.Coords.Count > 2;

                    var trk = new CTrk
                    {
                        name = ab.Name ?? "Unnamed",
                        mode = isCurve ? TrackMode.Curve : TrackMode.AB,
                        ptA = new vec2(vA.Easting, vA.Northing),
                        ptB = new vec2(vB.Easting, vB.Northing),
                        heading = GeoConverter.HeadingFromPoints(vA, vB),
                        nudgeDistance = 0,
                        isVisible = true,
                        curvePts = new List<vec3>()
                    };

                    // Parse curve points if present
                    if (isCurve)
                    {
                        for (int i = 0; i < ab.Coords.Count; i++)
                        {
                            var p = ab.Coords[i];
                            if (p == null || !IsValidCoordinate(p.Latitude, p.Longitude)) continue;

                            var local = converter.ToLocal(p.Latitude, p.Longitude);
                            double heading = 0;

                            // Calculate heading to next point
                            if (i < ab.Coords.Count - 1 && ab.Coords[i + 1] != null)
                            {
                                var next = ab.Coords[i + 1];
                                if (IsValidCoordinate(next.Latitude, next.Longitude))
                                {
                                    var nextLocal = converter.ToLocal(next.Latitude, next.Longitude);
                                    var delta = new GeoDelta(
                                        new GeoCoord(local.Northing, local.Easting),
                                        new GeoCoord(nextLocal.Northing, nextLocal.Easting));
                                    heading = new GeoDir(delta).AngleInRadians;
                                }
                            }

                            trk.curvePts.Add(new vec3(local.Easting, local.Northing, heading));
                        }

                        // Update ptA/ptB to first/last curve points
                        if (trk.curvePts.Count >= 2)
                        {
                            trk.ptA = new vec2(trk.curvePts[0].easting, trk.curvePts[0].northing);
                            trk.ptB = new vec2(trk.curvePts[trk.curvePts.Count - 1].easting,
                                               trk.curvePts[trk.curvePts.Count - 1].northing);
                        }
                    }

                    result.Tracks.Add(trk);
                }
            }

            return result;
        }
    }
}
