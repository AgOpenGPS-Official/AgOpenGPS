using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.AgShare;

namespace AgOpenGPS.Core.AgShare
{
    public static class AgShareDownloader
    {
        public static void BuildLocalFieldFromCloud(FieldDownloadDto field, string folderPath)
        {
            Directory.CreateDirectory(folderPath);

            // 1. agshare.txt
            File.WriteAllText(Path.Combine(folderPath, "agshare.txt"), field.Id.ToString());

            // 2. Field.txt
            var fieldTxt = new List<string>
            {
                DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture),
                "$FieldDir",
                "KML Derived",
                "$Offsets",
                "0,0",
                "Convergence",
                "0",
                "StartFix",
                string.Format(CultureInfo.InvariantCulture, "{0},{1}", field.OriginLat, field.OriginLon)
            };
            File.WriteAllLines(Path.Combine(folderPath, "Field.txt"), fieldTxt);

            // 3. Init local coordinate system
            var origin = new Wgs84(field.OriginLat, field.OriginLon);
            var sharedProps = new SharedFieldProperties
            {
                DriftCompensation = new GeoDelta(0, 0)
            };
            var local = new LocalPlane(origin, sharedProps);

            // 4. Boundary.txt
            var boundaryLines = new List<string> { "$Boundary", "False", field.Boundary.Count.ToString() };
            foreach (var coord in field.Boundary)
            {
                var geo = new Wgs84(coord[0], coord[1]);
                var vec = local.ConvertWgs84ToGeoCoord(geo);
                boundaryLines.Add(string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", vec.Easting, vec.Northing, 0));
            }
            File.WriteAllLines(Path.Combine(folderPath, "Boundary.txt"), boundaryLines);

            // 5. TrackLines.txt
            var trackLines = new List<string> { "$TrackLines" };
            foreach (var ab in field.AbLines)
            {
                trackLines.Add(ab.Name);
                trackLines.Add("0"); // heading placeholder

                if (ab.Coords.Count >= 2 && ab.Type == "AB")
                {
                    var geoA = new Wgs84(ab.Coords[0][0], ab.Coords[0][1]);
                    var geoB = new Wgs84(ab.Coords[1][0], ab.Coords[1][1]);
                    var ptA = local.ConvertWgs84ToGeoCoord(geoA);
                    var ptB = local.ConvertWgs84ToGeoCoord(geoB);

                    trackLines.Add(string.Format(CultureInfo.InvariantCulture, "{0},{1}", ptA.Easting, ptA.Northing));
                    trackLines.Add(string.Format(CultureInfo.InvariantCulture, "{0},{1}", ptB.Easting, ptB.Northing));
                }
                else
                {
                    trackLines.Add("0,0");
                    trackLines.Add("0,0");
                }

                trackLines.Add("0"); // nudgeDistance
                trackLines.Add("0"); // mode
                trackLines.Add("true"); // isVisible

                trackLines.Add(ab.Coords.Count.ToString());
                foreach (var coord in ab.Coords)
                {
                    var geo = new Wgs84(coord[0], coord[1]);
                    var vec = local.ConvertWgs84ToGeoCoord(geo);
                    trackLines.Add(string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", vec.Easting, vec.Northing, 0));
                }
            }
            File.WriteAllLines(Path.Combine(folderPath, "TrackLines.txt"), trackLines);

            // 6. Headlines.txt
            File.WriteAllText(Path.Combine(folderPath, "Headlines.txt"), "$HeadLines\n");

            // 7. CurveLines.txt
            File.WriteAllText(Path.Combine(folderPath, "CurveLines.txt"), "$CurveLines\n");

            // 8. ABLines.txt (optioneel invullen)
            using (StreamWriter writer = new StreamWriter(Path.Combine(folderPath, "ABLines.txt"), false))
            {
                foreach (var ab in field.AbLines)
                {
                    if (ab.Type == "AB" && ab.Coords.Count >= 2)
                    {
                        var geoA = new Wgs84(ab.Coords[0][0], ab.Coords[0][1]);
                        var ptA = local.ConvertWgs84ToGeoCoord(geoA);
                        writer.WriteLine($"{ab.Name},0,{ptA.Easting},{ptA.Northing}");
                    }
                }
            }

            // 9. Contour.txt
            File.WriteAllText(Path.Combine(folderPath, "Contour.txt"), "$Contour\n");

            // 10. Flags.txt
            File.WriteAllText(Path.Combine(folderPath, "Flags.txt"), "$Flags\n0\n");

            // 11. RecPath.txt
            File.WriteAllLines(Path.Combine(folderPath, "RecPath.txt"), new[] { "$RecPath", "0" });
        }
    }
}
