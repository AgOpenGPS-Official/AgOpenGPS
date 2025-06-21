using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using AgOpenGPS.Properties;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Classes.AgShare.Helpers;

namespace AgOpenGPS
{
    /// <summary>
    /// Central helper class for downloading, parsing and saving AgShare fields locally.
    /// </summary>
    public class CAgShareDownloader
    {
        private readonly AgShareClient client;

        public CAgShareDownloader()
        {
            // Initialize AgShare client using stored settings
            client = new AgShareClient(Settings.Default.AgShareServer, Settings.Default.AgShareApiKey);
        }

        // Downloads a field and saves it to disk
        public async Task<bool> DownloadAndSaveAsync(Guid fieldId)
        {
            try
            {
                string json = await client.DownloadFieldAsync(fieldId);
                var dto = JsonConvert.DeserializeObject<AgShareFieldDto>(json);
                var model = AgShareFieldParser.Parse(dto);
                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, model.Name);
                FieldFileWriter.WriteAllFiles(model, fieldDir);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Retrieves a list of user-owned fields
        public async Task<List<AgShareGetOwnFieldDto>> GetOwnFieldsAsync()
        {
            return await client.GetOwnFieldsAsync();
        }

        // Downloads a field DTO for preview only
        public async Task<AgShareFieldDto> DownloadFieldPreviewAsync(Guid fieldId)
        {
            string json = await client.DownloadFieldAsync(fieldId);
            return JsonConvert.DeserializeObject<AgShareFieldDto>(json);
        }
        public async Task<(int Downloaded, int Skipped)> DownloadAllAsync(
            bool forceOverwrite = false,
            IProgress<int> progress = null)
        {
            var fields = await GetOwnFieldsAsync();
            int skipped = 0, downloaded = 0;

            foreach (var field in fields)
            {
                string dir = Path.Combine(RegistrySettings.fieldsDirectory, field.Name);
                string agsharePath = Path.Combine(dir, "agshare.txt");

                bool alreadyExists = false;
                if (File.Exists(agsharePath))
                {
                    try
                    {
                        var id = File.ReadAllText(agsharePath).Trim();
                        alreadyExists = Guid.TryParse(id, out Guid guid) && guid == field.Id;
                    }
                    catch { }
                }

                if (alreadyExists && !forceOverwrite)
                {
                    skipped++;
                }
                else
                {
                    var preview = await DownloadFieldPreviewAsync(field.Id);
                    if (preview != null)
                    {
                        var model = AgShareFieldParser.Parse(preview);
                        FieldFileWriter.WriteAllFiles(model, dir);
                        downloaded++;
                    }
                }

                progress?.Report(downloaded + skipped);
            }

            return (downloaded, skipped);
        }


    }

    /// <summary>
    /// Utility class that writes a LocalFieldModel to standard AgOpenGPS-compatible files.
    /// </summary>
    public static class FieldFileWriter
    {
        // Writes all files required for a field
        public static void WriteAllFiles(LocalFieldModel field, string fieldDir)
        {
            if (!Directory.Exists(fieldDir))
                Directory.CreateDirectory(fieldDir);

            WriteAgShareId(fieldDir, field.FieldId);
            WriteFieldTxt(fieldDir, field.Origin);
            WriteBoundaryTxt(fieldDir, field.Boundaries);
            WriteTrackLinesTxt(fieldDir, field.AbLines);
            WriteStaticFiles(fieldDir); // Flags, Headland
        }

        // Writes agshare.txt with the field ID
        private static void WriteAgShareId(string fieldDir, Guid fieldId)
        {
            File.WriteAllText(Path.Combine(fieldDir, "agshare.txt"), fieldId.ToString());
        }

        // Writes origin and metadata to Field.txt
        private static void WriteFieldTxt(string fieldDir, Wgs84 origin)
        {
            var fieldTxt = new List<string>
            {
                DateTime.Now.ToString("yyyy-MMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture),
                "$FieldDir",
                "AgShare Downloaded",
                "$Offsets",
                "0,0",
                "Convergence",
                "0", // Always 0
                "StartFix",
                origin.Latitude.ToString(CultureInfo.InvariantCulture) + "," + origin.Longitude.ToString(CultureInfo.InvariantCulture)
            };

            File.WriteAllLines(Path.Combine(fieldDir, "Field.txt"), fieldTxt);
        }

        // Writes outer and inner boundary rings to Boundary.txt
        private static void WriteBoundaryTxt(string fieldDir, List<List<LocalPoint>> boundaries)
        {
            if (boundaries == null || boundaries.Count == 0) return;

            var lines = new List<string> { "$Boundary" };

            for (int i = 0; i < boundaries.Count; i++)
            {
                var ring = boundaries[i];
                bool isHole = i != 0;

                lines.Add(isHole ? "True" : "False");
                lines.Add(ring.Count.ToString());

                var enriched = BoundaryHelper.WithHeadings(ring);

                foreach (var pt in enriched)
                {
                    lines.Add(
                        pt.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.Northing.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.Heading.ToString("0.#####", CultureInfo.InvariantCulture)
                    );
                }
            }

            File.WriteAllLines(Path.Combine(fieldDir, "Boundary.txt"), lines);
        }


        // Writes AB-lines and optional curve points to TrackLines.txt
        private static void WriteTrackLinesTxt(string fieldDir, List<AbLineLocal> abLines)
        {
            var lines = new List<string> { "$TrackLines" };

            foreach (var ab in abLines)
            {
                lines.Add(ab.Name ?? "Unnamed");

                bool isCurve = ab.CurvePoints != null && ab.CurvePoints.Count > 1;

                LocalPoint ptA = ab.PtA;
                LocalPoint ptB = ab.PtB;
                double heading = ab.Heading;

                if (isCurve)
                {
                    ptA = ab.CurvePoints[0];
                    ptB = ab.CurvePoints[ab.CurvePoints.Count - 1];
                    heading = GeoConverter.HeadingFromPoints(
                        new Vec2(ptA.Easting, ptA.Northing),
                        new Vec2(ptB.Easting, ptB.Northing)
                    );
                }

                lines.Add(heading.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add(ptA.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," + ptA.Northing.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add(ptB.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," + ptB.Northing.ToString("0.###", CultureInfo.InvariantCulture));
                lines.Add("0"); // Nudge

                if (isCurve)
                {
                    lines.Add("4"); // Curve mode
                    lines.Add("True");
                    lines.Add(ab.CurvePoints.Count.ToString());

                    foreach (var pt in ab.CurvePoints)
                    {
                        lines.Add(
                            pt.Easting.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                            pt.Northing.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                            pt.Heading.ToString("0.#####", CultureInfo.InvariantCulture)
                        );
                    }
                }
                else
                {
                    lines.Add("2"); // AB mode
                    lines.Add("True");
                    lines.Add("0");
                }
            }

            File.WriteAllLines(Path.Combine(fieldDir, "TrackLines.txt"), lines);
        }

        // Writes default placeholder files like Flags.txt and Headland.txt
        private static void WriteStaticFiles(string fieldDir)
        {
            File.WriteAllLines(Path.Combine(fieldDir, "Flags.txt"), new[] { "$Flags", "0" });
            File.WriteAllLines(Path.Combine(fieldDir, "Headland.txt"), new[] { "$Headland", "0" });
            File.WriteAllLines(Path.Combine(fieldDir, "Contour.txt"), new[] { "$Contour", "0" });
            File.WriteAllLines(Path.Combine(fieldDir, "Sections.txt"), new[] { "Sections", "0" });  
        }
    }

}
