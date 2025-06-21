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
            foreach (var ring in dto.Boundaries)
            {
                var ringList = new List<LocalPoint>();
                foreach (var point in ring)
                {
                    var local = converter.ToLocal(point.Latitude, point.Longitude);
                    ringList.Add(new LocalPoint(local.Easting, local.Northing));
                }
                result.Boundaries.Add(ringList);
            }

            // Convert AB-lines and curves
            foreach (var ab in dto.AbLines)
            {
                if (ab.Coords == null || ab.Coords.Count < 2) continue;

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
                        var local = converter.ToLocal(p.Latitude, p.Longitude);
                        double localHeading = 0;

                        if (i < ab.Coords.Count - 1)
                        {
                            var next = ab.Coords[i + 1];
                            var nextLocal = converter.ToLocal(next.Latitude, next.Longitude);

                            // Correct volgorde: Northing, Easting
                            var localCoord = new GeoCoord(local.Northing, local.Easting);
                            var nextCoord = new GeoCoord(nextLocal.Northing, nextLocal.Easting);
                            localHeading = new GeoDir(new GeoDelta(localCoord, nextCoord)).Angle;
                        }

                        abLine.CurvePoints.Add(new LocalPoint(local.Easting, local.Northing, localHeading));
                    }
                }

                result.AbLines.Add(abLine);
            }

            return result;
        }
    }
}
