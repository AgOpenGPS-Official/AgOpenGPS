using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;
using AgLibrary.Logging;

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
                    await writer.WriteLineAsync(track.Name);

                    if (track.Type == "AB" && track.PtA != null && track.PtB != null)
                    {
                        var localPtA = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtA.Latitude, track.PtA.Longitude));
                        var localPtB = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtB.Latitude, track.PtB.Longitude));

                        await writer.WriteLineAsync($"AB line: {track.Name}");
                        await writer.WriteLineAsync($"Start: {localPtA.Northing},{localPtA.Easting}");
                        await writer.WriteLineAsync($"End: {localPtB.Northing},{localPtB.Easting}");
                    }

                    if (track.Type == "Curve" && track.CurvePoints != null)
                    {
                        await writer.WriteLineAsync("Curve:");
                        foreach (var point in track.CurvePoints)
                        {
                            var localPoint = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(point.Latitude, point.Longitude));
                            await writer.WriteLineAsync($"{localPoint.Northing},{localPoint.Easting}");
                        }
                    }
                }
            }
        }
    }
}
