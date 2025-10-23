using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgOpenGPS.Classes.AgShare.Helpers
{
    public static class AgShareFieldParser
    {
        public static LocalFieldModel Parse(AgShareFieldDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Field DTO cannot be null");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Field name cannot be null or empty", nameof(dto));
            }

            // Validate coordinates are within valid ranges
            if (dto.Latitude < -90 || dto.Latitude > 90)
            {
                throw new ArgumentException($"Invalid latitude: {dto.Latitude}", nameof(dto));
            }

            if (dto.Longitude < -180 || dto.Longitude > 180)
            {
                throw new ArgumentException($"Invalid longitude: {dto.Longitude}", nameof(dto));
            }

            var result = new LocalFieldModel
            {
                FieldId = dto.Id,
                Name = dto.Name,
                Origin = new Wgs84(dto.Latitude, dto.Longitude),
                Boundaries = new List<List<LocalPoint>>(),
                AbLines = new List<AbLineLocal>()
            };

            var converter = new GeoConverter(dto.Latitude, dto.Longitude);

            // Convert boundary rings from WGS84 to local NE
            if (dto.Boundaries != null)
            {
                foreach (var ring in dto.Boundaries)
                {
                    if (ring == null || ring.Count == 0)
                    {
                        // Skip empty or null rings
                        continue;
                    }

                    var ringList = new List<LocalPoint>();
                    foreach (var point in ring)
                    {
                        if (point == null)
                        {
                            continue; // Skip null points
                        }

                        // Validate coordinates
                        if (point.Latitude < -90 || point.Latitude > 90 ||
                            point.Longitude < -180 || point.Longitude > 180)
                        {
                            continue; // Skip invalid coordinates
                        }

                        var local = converter.ToLocal(point.Latitude, point.Longitude);
                        ringList.Add(new LocalPoint(local.Easting, local.Northing));
                    }

                    // Only add rings with at least 3 points (minimum for a valid polygon)
                    if (ringList.Count >= 3)
                    {
                        result.Boundaries.Add(ringList);
                    }
                }
            }

            // Convert AB-lines and curves
            if (dto.AbLines != null)
            {
                foreach (var ab in dto.AbLines)
                {
                    if (ab == null || ab.Coords == null || ab.Coords.Count < 2)
                    {
                        continue; // Skip invalid AB lines
                    }

                    // Validate first two points
                    if (ab.Coords[0] == null || ab.Coords[1] == null)
                    {
                        continue;
                    }

                    if (ab.Coords[0].Latitude < -90 || ab.Coords[0].Latitude > 90 ||
                        ab.Coords[0].Longitude < -180 || ab.Coords[0].Longitude > 180 ||
                        ab.Coords[1].Latitude < -90 || ab.Coords[1].Latitude > 90 ||
                        ab.Coords[1].Longitude < -180 || ab.Coords[1].Longitude > 180)
                    {
                        continue; // Skip AB lines with invalid coordinates
                    }

                    var vA = converter.ToLocal(ab.Coords[0].Latitude, ab.Coords[0].Longitude);
                    var vB = converter.ToLocal(ab.Coords[1].Latitude, ab.Coords[1].Longitude);
                    double heading = GeoConverter.HeadingFromPoints(vA, vB);

                    var abLine = new AbLineLocal
                    {
                        Name = ab.Name ?? "Unnamed",
                        Heading = heading,
                        PtA = new LocalPoint(vA.Easting, vA.Northing),
                        PtB = new LocalPoint(vB.Easting, vB.Northing),
                        CurvePoints = new List<LocalPoint>()
                    };

                    if (ab.Coords.Count > 2)
                    {
                        for (int i = 0; i < ab.Coords.Count; i++)
                        {
                            var p = ab.Coords[i];

                            if (p == null)
                            {
                                continue; // Skip null points
                            }

                            // Validate coordinates
                            if (p.Latitude < -90 || p.Latitude > 90 ||
                                p.Longitude < -180 || p.Longitude > 180)
                            {
                                continue; // Skip invalid coordinates
                            }

                            var local = converter.ToLocal(p.Latitude, p.Longitude);
                            double localHeading = 0;

                            if (i < ab.Coords.Count - 1 && ab.Coords[i + 1] != null)
                            {
                                var next = ab.Coords[i + 1];

                                // Validate next point coordinates
                                if (next.Latitude >= -90 && next.Latitude <= 90 &&
                                    next.Longitude >= -180 && next.Longitude <= 180)
                                {
                                    var nextLocal = converter.ToLocal(next.Latitude, next.Longitude);

                                    // Correct volgorde: Northing, Easting
                                    var localCoord = new GeoCoord(local.Northing, local.Easting);
                                    var nextCoord = new GeoCoord(nextLocal.Northing, nextLocal.Easting);
                                    localHeading = new GeoDir(new GeoDelta(localCoord, nextCoord)).AngleInRadians;
                                }
                            }

                            abLine.CurvePoints.Add(new LocalPoint(local.Easting, local.Northing, localHeading));
                        }
                    }

                    result.AbLines.Add(abLine);
                }
            }

            return result;
        }
    }
}
