using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AgLibrary.Logging;
using AgOpenGPS.Classes.AgShare.Helpers;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.AgShare.Models;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Central helper class for downloading, parsing and saving AgShare fields locally.
    /// </summary>
    public class AgShareDownloader
    {
        private readonly AgShareClient _client;

        public AgShareDownloader(AgShareClient client)
        {
            _client = client;
        }

        // Downloads a field and saves it to disk
        public async Task<bool> DownloadAndSaveAsync(Guid fieldId)
        {
            try
            {
                var result = await _client.DownloadFieldAsync(fieldId);

                if (!result.IsSuccessful)
                {
                    Log.EventWriter($"[AgShare] Download failed for fieldId={fieldId}: Failed to deserialize field data");
                    return false;
                }

                // Parse DTO - validation is now done inside Parse method
                var model = AgShareFieldParser.Parse(result.Data);

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
        public async Task<List<GetOwnFieldDto>> GetOwnFieldsAsync()
        {
            var result = await _client.GetOwnFieldsAsync();

            if (!result.IsSuccessful)
            {
                Log.EventWriter($"[AgShare] Download own fields failed");
                return null;
            }

            return result.Data;
        }

        // Downloads a field DTO for preview only
        public async Task<GetFieldDto> DownloadFieldPreviewAsync(Guid fieldId)
        {
            try
            {
                var result = await _client.DownloadFieldAsync(fieldId);

                if (!result.IsSuccessful)
                {
                    Log.EventWriter($"[AgShare] Preview download failed for fieldId={fieldId}: Failed to deserialize field data");
                    return null;
                }

                // Validation is done in Parse method
                // If validation fails, the outer catch will handle it
                AgShareFieldParser.Parse(result.Data);

                return result.Data;
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
    /// Utility class that writes a ParsedField to standard AgOpenGPS-compatible files.
    /// Uses static IO classes from GPS\IO for consistent file formatting.
    /// </summary>
    public static class FieldFileWriter
    {
        // Writes all files required for a field using static IO classes
        public static void WriteAllFiles(ParsedField field, string fieldDir)
        {
            if (!Directory.Exists(fieldDir))
                Directory.CreateDirectory(fieldDir);

            File.WriteAllText(Path.Combine(fieldDir, "agshare.txt"), field.FieldId.ToString());
            FieldPlaneFiles.Save(fieldDir, DateTime.Now, field.Origin);
            BoundaryFiles.Save(fieldDir, field.Boundaries);
            TrackFiles.Save(fieldDir, field.Tracks);

            // Empty placeholder files
            FlagsFiles.Save(fieldDir, new List<CFlag>());
            HeadlandFiles.Save(fieldDir, new List<CBoundaryList>());
            ContourFiles.CreateFile(fieldDir);
            SectionsFiles.CreateEmpty(fieldDir);
        }
    }

}
