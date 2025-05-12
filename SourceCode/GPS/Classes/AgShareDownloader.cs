using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;
using AgLibrary.Logging;
using System.Linq;

namespace AgOpenGPS
{
    public class AgShareDownloader
    {
        // Download and save the field data to disk
        public static async Task<bool> SaveDownloadedFieldToDiskAsync(FieldDownloadDto field)
        {
            try
            {
                var origin = new Wgs84(field.OriginLat, field.OriginLon);
                var sharedProps = new SharedFieldProperties
                {
                    DriftCompensation = new GeoDelta(0, 0)
                };

                var localPlane = new LocalPlane(origin, sharedProps);
                string baseFieldPath = Path.Combine(RegistrySettings.fieldsDirectory, field.Name);

                if (Directory.Exists(baseFieldPath))
                {
                    var result = MessageBox.Show(
                        $"Field '{field.Name}' already exists. Do you want to overwrite it?",
                        "Field Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return false;

                    string abLinesPath = Path.Combine(baseFieldPath, "ABLines.txt");
                    if (File.Exists(abLinesPath)) File.Delete(abLinesPath);

                    string curveLinesPath = Path.Combine(baseFieldPath, "Curvelines.txt");
                    if (File.Exists(curveLinesPath)) File.Delete(curveLinesPath);
                }
                else
                {
                    Directory.CreateDirectory(baseFieldPath);
                }

                string agsharePath = Path.Combine(baseFieldPath, "agshare.txt");
                await WriteTextAsync(agsharePath, field.Id.ToString());

                string fieldTxtPath = Path.Combine(baseFieldPath, "Field.txt");
                await SaveFieldTxtAsync(fieldTxtPath, field, localPlane);

                string boundaryTxtPath = Path.Combine(baseFieldPath, "Boundary.txt");
                await SaveBoundaryTxtAsync(boundaryTxtPath, field.ParsedBoundary, localPlane);

                string tracklinesTxtPath = Path.Combine(baseFieldPath, "TrackLines.txt");
                await SaveTrackLinesTxtAsync(tracklinesTxtPath, field.ParsedTracks, localPlane);

                Log.EventWriter("AgShare - Succesfully Downloaded");
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter("AgShare - Error while saving field data: " + ex.Message);
                return false;
            }

        }

        // Write text to file asynchronously
        private static async Task WriteTextAsync(string path, string text)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(text);
            }
        }

        // Save Field.txt with metadata
        private static async Task SaveFieldTxtAsync(string path, FieldDownloadDto field, LocalPlane localPlane)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                await writer.WriteLineAsync("$FieldDir");
                await writer.WriteLineAsync(field.Name);
                await writer.WriteLineAsync("$Offsets");
                await writer.WriteLineAsync("0,0");
                await writer.WriteLineAsync("Convergence");
                await writer.WriteLineAsync("0");
                await writer.WriteLineAsync("StartFix");
                await writer.WriteLineAsync($"{localPlane.Origin.Latitude.ToString(CultureInfo.InvariantCulture)},{localPlane.Origin.Longitude.ToString(CultureInfo.InvariantCulture)}");
            }
        }

        // Save Boundary.txt using LocalPlane coordinates
        private static async Task SaveBoundaryTxtAsync(string path, List<ParsedLatLon> parsedBoundary, LocalPlane localPlane)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync("$Boundary");
                await writer.WriteLineAsync("False");
                await writer.WriteLineAsync(parsedBoundary.Count.ToString());

                var localPoints = new List<GeoCoord>();
                for (int i = 0; i < parsedBoundary.Count; i++)
                {
                    var wgs = parsedBoundary[i];
                    var local = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(wgs.Latitude, wgs.Longitude));
                    localPoints.Add(local);
                }

                for (int i = 0; i < localPoints.Count; i++)
                {
                    var current = localPoints[i];
                    var prev = localPoints[(i - 1 + localPoints.Count) % localPoints.Count];
                    var next = localPoints[(i + 1) % localPoints.Count];

                    double dx = next.Easting - prev.Easting;
                    double dy = next.Northing - prev.Northing;
                    double headingRad = Math.Atan2(dx, dy);
                    double headingDeg = (headingRad * 180.0 / Math.PI + 360.0) % 360.0;

                    await writer.WriteLineAsync($"{current.Easting:0.#####},{current.Northing:0.#####},{headingDeg:0.#####}");
                }
            }
        }

        // Save TrackLines.txt using LocalPlane coordinates
        private static async Task SaveTrackLinesTxtAsync(string path, List<ParsedTrackLatLon> parsedTracks, LocalPlane localPlane)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync("$TrackLines");

                foreach (var track in parsedTracks)
                {
                    await writer.WriteLineAsync(track.Name ?? "Unnamed");

                    if (track.Type == "AB" && track.PtA != null && track.PtB != null)
                    {
                        var ptALocal = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtA.Latitude, track.PtA.Longitude));
                        var ptBLocal = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtB.Latitude, track.PtB.Longitude));

                        double dx = ptBLocal.Easting - ptALocal.Easting;
                        double dy = ptBLocal.Northing - ptALocal.Northing;

                        double headingRad = Math.Atan2(dx, dy);
                        if (headingRad < 0) headingRad += 2 * Math.PI;

                        await writer.WriteLineAsync(headingRad.ToString(CultureInfo.InvariantCulture));
                        await writer.WriteLineAsync($"{ptALocal.Easting:0.###},{ptALocal.Northing:0.###}");
                        await writer.WriteLineAsync($"{ptBLocal.Easting:0.###},{ptBLocal.Northing:0.###}");
                        await writer.WriteLineAsync("0");     // Nudge
                        await writer.WriteLineAsync("2");     // Mode AB
                        await writer.WriteLineAsync("True");  // Visible
                        await writer.WriteLineAsync("0");     // Curve count
                    }
                    else if (track.Type == "Curve" && track.CurvePoints != null && track.CurvePoints.Count > 1)
                    {
                        var localPoints = track.CurvePoints
                            .Select(p => localPlane.ConvertWgs84ToGeoCoord(new Wgs84(p.Latitude, p.Longitude)))
                            .ToList();

                        var first = localPoints.First();
                        var last = localPoints.Last();

                        // Bereken gemiddelde heading uit opeenvolgende segmenten
                        double sumX = 0, sumY = 0;
                        for (int i = 1; i < localPoints.Count; i++)
                        {
                            var dx = localPoints[i].Easting - localPoints[i - 1].Easting;
                            var dy = localPoints[i].Northing - localPoints[i - 1].Northing;

                            if (dx == 0 && dy == 0) continue; // skip identieke punten

                            double angle = Math.Atan2(dx, dy);
                            sumX += Math.Cos(angle);
                            sumY += Math.Sin(angle);
                        }

                        double avgHeading = Math.Atan2(sumY, sumX);
                        if (avgHeading < 0) avgHeading += 2 * Math.PI;

                        await writer.WriteLineAsync(avgHeading.ToString(CultureInfo.InvariantCulture));
                        await writer.WriteLineAsync($"{first.Easting:0.###},{first.Northing:0.###}");
                        await writer.WriteLineAsync($"{last.Easting:0.###},{last.Northing:0.###}");
                        await writer.WriteLineAsync("0");     // Nudge
                        await writer.WriteLineAsync("4");     // Mode Curve
                        await writer.WriteLineAsync("True");  // Visible

                        await writer.WriteLineAsync(localPoints.Count.ToString(CultureInfo.InvariantCulture));

                        foreach (var p in localPoints)
                        {
                            await writer.WriteLineAsync($"{p.Easting:0.###},{p.Northing:0.###},0");
                        }
                    }
                }
            }
        }
    }
}


    
