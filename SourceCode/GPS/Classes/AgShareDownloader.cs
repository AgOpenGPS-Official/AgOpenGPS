using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;
using NetTopologySuite.Geometries;


namespace AgOpenGPS
{
    public class AgShareDownloader
    {
        // Download and save the field data to disk
        public static async Task<bool> SaveDownloadedFieldToDiskAsync(FieldDownloadDto field, LocalPlane localPlane)
        {
            try
            {
                // Determine the full field directory path
                string baseFieldPath = Path.Combine(RegistrySettings.fieldsDirectory, field.Name);

                // If the directory already exists, ask for overwrite confirmation
                if (Directory.Exists(baseFieldPath))
                {
                    var result = MessageBox.Show(
                        $"Field '{field.Name}' already exists. Do you want to overwrite it?",
                        "Field Exists",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                        return false;

                    // Delete only ABLines.txt and Curvelines.txt if they exist
                    string abLinesPath = Path.Combine(baseFieldPath, "ABLines.txt");
                    if (File.Exists(abLinesPath)) File.Delete(abLinesPath);

                    string curveLinesPath = Path.Combine(baseFieldPath, "Curvelines.txt");
                    if (File.Exists(curveLinesPath)) File.Delete(curveLinesPath);

                }
                else
                {
                    Directory.CreateDirectory(baseFieldPath);
                }

                // Save agshare.txt
                string agsharePath = Path.Combine(baseFieldPath, "agshare.txt");
                await WriteTextAsync(agsharePath, field.Id.ToString());

                // Save Field.txt
                string fieldTxtPath = Path.Combine(baseFieldPath, "Field.txt");
                await SaveFieldTxtAsync(fieldTxtPath, field);

                // Save Boundary.txt
                string boundaryTxtPath = Path.Combine(baseFieldPath, "Boundary.txt");
                await SaveBoundaryTxtAsync(boundaryTxtPath, field.ParsedBoundary, field.OriginLat, field.OriginLon, localPlane);

                // Save TrackLines.txt
                string tracklinesTxtPath = Path.Combine(baseFieldPath, "TrackLines.txt");
                await SaveTrackLinesTxtAsync(tracklinesTxtPath, field.ParsedTracks, localPlane);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving field data: " + ex.Message);
                return false;
            }
        }

        // Write text to file asynchronously using StreamWriter
        private static async Task WriteTextAsync(string path, string text)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteAsync(text);
            }
        }

        // Save Field.txt (basic metadata)
        private static async Task SaveFieldTxtAsync(string path, FieldDownloadDto field)
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
                await writer.WriteLineAsync($"{field.OriginLat.ToString(CultureInfo.InvariantCulture)},{field.OriginLon.ToString(CultureInfo.InvariantCulture)}");
            }
        }


        // Save Boundary.txt (field boundary) using LocalPlane coordinates
        private static async Task SaveBoundaryTxtAsync(string path, List<ParsedLatLon> parsedBoundary, double originLat, double originLon, LocalPlane localPlane)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync("$Boundary");

                // Zet de WGS84-coördinaten om naar LocalPlane
                foreach (var point in parsedBoundary)
                {
                    // Gebruik de LocalPlane converter uit AgOpenGPS.Core.Models.Base
                    var localPoint = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(point.Latitude, point.Longitude));

                    // Schrijf de LocalPlane-coördinaten naar het bestand
                    await writer.WriteLineAsync($"{localPoint.Northing},{localPoint.Easting}");
                }
            }
        }

        // Save TrackLines.txt (AB lines and curves) using LocalPlane coordinates
        private static async Task SaveTrackLinesTxtAsync(string path, List<ParsedTrackLatLon> parsedTracks, LocalPlane localPlane)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync("$TrackLines");

                foreach (var track in parsedTracks)
                {
                    await writer.WriteLineAsync(track.Name);

                    // Voor AB lijnen: Zet de coördinaten om naar LocalPlane
                    if (track.Type == "AB" && track.PtA != null && track.PtB != null)
                    {
                        // Zet WGS84 naar LocalPlane voor A-punt en B-punt
                        var localPtA = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtA.Latitude, track.PtA.Longitude));
                        var localPtB = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(track.PtB.Latitude, track.PtB.Longitude));

                        // Schrijf de omgezette LocalPlane coördinaten naar TrackLines.txt
                        await writer.WriteLineAsync($"AB line: {track.Name}");
                        await writer.WriteLineAsync($"Start: {localPtA.Northing},{localPtA.Easting}");
                        await writer.WriteLineAsync($"End: {localPtB.Northing},{localPtB.Easting}");
                    }

                    // Voor Curves: Zet de curvepunten om naar LocalPlane
                    if (track.Type == "Curve" && track.CurvePoints != null)
                    {
                        await writer.WriteLineAsync("Curve:");
                        foreach (var point in track.CurvePoints)
                        {
                            // Zet WGS84 naar LocalPlane voor elk curvepunt
                            var localPoint = localPlane.ConvertWgs84ToGeoCoord(new Wgs84(point.Latitude, point.Longitude));
                            await writer.WriteLineAsync($"{localPoint.Northing},{localPoint.Easting}");
                        }
                    }
                }
            }
        }
    }
}
