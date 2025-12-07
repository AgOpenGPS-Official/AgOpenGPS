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
        // Parses an AgShare field DTO into domain types
        public static ParsedField Parse(AgShareFieldDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Field DTO cannot be null");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Field name cannot be null, empty or whitespace", nameof(dto));
            }

            var origin = new Wgs84(dto.Latitude, dto.Longitude);
            if (!origin.IsValid)
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
                Origin = origin
            };

            var converter = new GeoConverter(origin.Latitude, origin.Longitude);

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
                        var wgs = new Wgs84(point.Latitude, point.Longitude);
                        if (!wgs.IsValid)
                        {
                            Log.EventWriter($"[AgShare] Skipping invalid boundary coordinate: Lat={point.Latitude}, Lon={point.Longitude}");
                            continue;
                        }

                        var local = converter.ToLocal(wgs.Latitude, wgs.Longitude);
                        bnd.fenceLine.Add(new vec3(local.Easting, local.Northing, 0.0));
                    }

                    if (bnd.fenceLine.Count >= 3)
                    {
                        // AgShare does not yet provide isDriveThru metadata per boundary,
                        // so we default to false (user must manually enable drive-through for holes)
                        bnd.isDriveThru = false;

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

                    var wgsA = new Wgs84(ab.Coords[0].Latitude, ab.Coords[0].Longitude);
                    var wgsB = new Wgs84(ab.Coords[1].Latitude, ab.Coords[1].Longitude);
                    if (!wgsA.IsValid || !wgsB.IsValid)
                    {
                        Log.EventWriter($"[AgShare] Skipping AB line '{ab.Name ?? "Unnamed"}' - invalid coordinates");
                        continue;
                    }

                    var vA = converter.ToLocal(wgsA.Latitude, wgsA.Longitude);
                    var vB = converter.ToLocal(wgsB.Latitude, wgsB.Longitude);
                    var ptA = new vec3(vA.Easting, vA.Northing, 0);
                    var ptB = new vec3(vB.Easting, vB.Northing, 0);
                    bool isCurve = ab.Coords.Count > 2;

                    var trk = new CTrk
                    {
                        name = ab.Name ?? "Unnamed",
                        mode = isCurve ? TrackMode.Curve : TrackMode.AB,
                        ptA = ptA.ToVec2(),
                        ptB = ptB.ToVec2(),
                        heading = new GeoDir(ptA.ToGeoCoord(), ptB.ToGeoCoord()).AngleInRadians,
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
                            if (p == null) continue;
                            var wgs = new Wgs84(p.Latitude, p.Longitude);
                            if (!wgs.IsValid) continue;

                            var local = converter.ToLocal(wgs.Latitude, wgs.Longitude);
                            var pt = new vec3(local.Easting, local.Northing, 0);
                            double heading = 0;

                            // Calculate heading to next point
                            if (i < ab.Coords.Count - 1 && ab.Coords[i + 1] != null)
                            {
                                var next = ab.Coords[i + 1];
                                var nextWgs = new Wgs84(next.Latitude, next.Longitude);
                                if (nextWgs.IsValid)
                                {
                                    var nextLocal = converter.ToLocal(nextWgs.Latitude, nextWgs.Longitude);
                                    var nextPt = new vec3(nextLocal.Easting, nextLocal.Northing, 0);
                                    heading = new GeoDir(pt.ToGeoCoord(), nextPt.ToGeoCoord()).AngleInRadians;
                                }
                            }

                            trk.curvePts.Add(new vec3(pt.easting, pt.northing, heading));
                        }

                        // Update ptA/ptB to first/last curve points
                        if (trk.curvePts.Count >= 2)
                        {
                            trk.ptA = trk.curvePts[0].ToVec2();
                            trk.ptB = trk.curvePts[trk.curvePts.Count - 1].ToVec2();
                        }
                    }

                    result.Tracks.Add(trk);
                }
            }

            return result;
        }
    }
}
