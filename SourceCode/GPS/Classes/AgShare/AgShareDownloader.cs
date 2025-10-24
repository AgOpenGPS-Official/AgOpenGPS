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
using AgLibrary.Logging;

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

                // Validate JSON response
                if (string.IsNullOrWhiteSpace(json))
                {
                    Log.EventWriter($"[AgShare] Download failed for fieldId={fieldId}: Empty response from server");
                    return false;
                }

                var dto = JsonConvert.DeserializeObject<AgShareFieldDto>(json);

                // Validate DTO
                if (dto == null)
                {
                    Log.EventWriter($"[AgShare] Download failed for fieldId={fieldId}: Failed to deserialize field data");
                    return false;
                }

                // Parse DTO - validation is now done inside Parse method
                var model = AgShareFieldParser.Parse(dto);

                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, model.Name);
                FieldFileWriter.WriteAllFiles(model, fieldDir);
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"[AgShare] Download failed for fieldId={fieldId}: {ex.GetType().Name} - {ex.Message}");
                Log.EventWriter(ex.StackTrace);
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
            try
            {
                string json = await client.DownloadFieldAsync(fieldId);

                if (string.IsNullOrWhiteSpace(json))
                {
                    Log.EventWriter($"[AgShare] Preview download failed for fieldId={fieldId}: Empty response from server");
                    return null;
                }

                var dto = JsonConvert.DeserializeObject<AgShareFieldDto>(json);

                if (dto == null)
                {
                    Log.EventWriter($"[AgShare] Preview download failed for fieldId={fieldId}: Failed to deserialize field data");
                    return null;
                }

                // Validation is done in Parse method
                // If validation fails, the outer catch will handle it
                AgShareFieldParser.Parse(dto);

                return dto;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"[AgShare] Preview download failed for fieldId={fieldId}: {ex.GetType().Name} - {ex.Message}");
                return null;
            }
        }
        public async Task<(int Downloaded, int Skipped, int Failed)> DownloadAllAsync(
            bool forceOverwrite = false,
            IProgress<int> progress = null)
        {
            var fields = await GetOwnFieldsAsync();

            if (fields == null || fields.Count == 0)
            {
                Log.EventWriter("[AgShare] DownloadAll: No fields available to download");
                return (0, 0, 0);
            }

            int skipped = 0, downloaded = 0, failed = 0;

            foreach (var field in fields)
            {
                try
                {
                    // Validate field metadata
                    if (field == null || string.IsNullOrWhiteSpace(field.Name))
                    {
                        Log.EventWriter($"[AgShare] DownloadAll: Skipping field with invalid name");
                        failed++;
                        continue;
                    }

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
                        Log.EventWriter($"[AgShare] DownloadAll: Skipping field {field.Name} (ID: {field.Id}) - already exists");
                        skipped++;
                    }
                    else
                    {
                        var preview = await DownloadFieldPreviewAsync(field.Id);
                        if (preview != null)
                        {
                            var model = AgShareFieldParser.Parse(preview);

                            // Extra validation before writing
                            if (model != null && !string.IsNullOrWhiteSpace(model.Name))
                            {
                                FieldFileWriter.WriteAllFiles(model, dir);
                                downloaded++;
                            }
                            else
                            {
                                Log.EventWriter($"[AgShare] DownloadAll: Failed to parse field {field.Name} (ID: {field.Id})");
                                failed++;
                            }
                        }
                        else
                        {
                            Log.EventWriter($"[AgShare] DownloadAll: Failed to download field {field.Name} (ID: {field.Id})");
                            failed++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"[AgShare] DownloadAll: Error processing field {field?.Name ?? "unknown"}: {ex.GetType().Name} - {ex.Message}");
                    failed++;
                }

                progress?.Report(downloaded + skipped + failed);
            }

            if (failed > 0)
            {
                Log.EventWriter($"[AgShare] DownloadAll completed: {downloaded} downloaded, {skipped} skipped, {failed} failed");
            }

            return (downloaded, skipped, failed);
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

                // Header for hole/outer ring (legacy AOG format)
                lines.Add(isHole ? "True" : "False");

                // ---- Normalize ring using CBoundaryList ----
                var bnd = new CBoundaryList();

                // Ensure fenceLine exists and is empty
                if (bnd.fenceLine == null)
                {
                    bnd.fenceLine = new List<vec3>();
                }
                else
                {
                    bnd.fenceLine.Clear();
                }

                // Inline conversion LocalPoint -> vec3 (heading will be recomputed)
                for (int p = 0; p < ring.Count; p++)
                {
                    var lp = ring[p];
                    bnd.fenceLine.Add(new vec3(lp.Easting, lp.Northing, 0.0));
                }

                // First calculate area / winding (FixFenceLine spacing depends on area; winding may be reversed here)
                bnd.CalculateFenceArea(i);

                // Then fix spacing and recalc headings (also populates fenceLineEar if used elsewhere)
                bnd.FixFenceLine(i);

                // Use the fixed/normalized fenceLine for output
                int fixedCount = bnd.fenceLine.Count;
                lines.Add(fixedCount.ToString(CultureInfo.InvariantCulture));

                for (int k = 0; k < fixedCount; k++)
                {
                    var pt = bnd.fenceLine[k];
                    lines.Add(
                        pt.easting.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.northing.ToString("0.###", CultureInfo.InvariantCulture) + "," +
                        pt.heading.ToString("0.#####", CultureInfo.InvariantCulture)
                    );
                }
                // ---- end normalization ----
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
