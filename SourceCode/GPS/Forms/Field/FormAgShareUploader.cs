using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;
using AgOpenGPS.IO;

namespace AgOpenGPS.Forms
{
    public partial class FormAgShareUploader : Form
    {
        private readonly AgShareUploader uploader;
        private readonly AgShareClient client;
        private List<FieldInfo> availableFields;
        private bool isUploading = false;

        public FormAgShareUploader(AgShareClient agShareClient)
        {
            InitializeComponent();
            client = agShareClient;
            uploader = new AgShareUploader(agShareClient);
        }

        private void FormAgShareBulkUploader_Load(object sender, EventArgs e)
        {
            LoadAvailableFields();
            // Check cloud status async after loading
            _ = CheckCloudStatusAsync();
        }

        private void LoadAvailableFields()
        {
            availableFields = new List<FieldInfo>();
            flpFieldList.Controls.Clear();

            try
            {
                string fieldsDirectory = RegistrySettings.fieldsDirectory;
                if (!Directory.Exists(fieldsDirectory))
                {
                    lblStatus.Text = "Fields directory not found";
                    return;
                }

                // Get all subdirectories (each is a field)
                string[] fieldDirectories = Directory.GetDirectories(fieldsDirectory);

                foreach (string fieldDir in fieldDirectories)
                {
                    string fieldName = Path.GetFileName(fieldDir);

                    // Check if field has necessary files (Field.txt contains origin)
                    string fieldFile = Path.Combine(fieldDir, "Field.txt");
                    if (File.Exists(fieldFile))
                    {
                        var fieldInfo = new FieldInfo
                        {
                            Name = fieldName,
                            DirectoryPath = fieldDir,
                            IsOnCloud = false,  // Will be determined by cloud check
                            CloudFieldId = null
                        };

                        availableFields.Add(fieldInfo);

                        // Create checkbox for this field
                        var checkbox = CreateFieldCheckbox(fieldInfo);
                        flpFieldList.Controls.Add(checkbox);
                    }
                }

                lblStatus.Text = $"Found {availableFields.Count} fields";
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error loading fields: " + ex.Message;
                Log.EventWriter("Error loading fields for bulk upload: " + ex.Message);
            }
        }

        private CheckBox CreateFieldCheckbox(FieldInfo fieldInfo)
        {
            var checkbox = new CheckBox
            {
                Text = fieldInfo.Name,  // Cloud check will add ☁ later if needed
                Checked = false,
                AutoSize = false,
                Width = flpFieldList.Width - 25,
                Height = 45,
                Tag = fieldInfo,
                Font = new Font("Tahoma", 20F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 8, 0, 0),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            checkbox.CheckedChanged += OnFieldSelectionChanged;
            return checkbox;
        }

        private void OnFieldSelectionChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.Tag is FieldInfo fieldInfo)
            {
                if (checkbox.Checked)
                {
                    checkbox.BackColor = Color.FromArgb(0, 119, 190); // OceanBlue
                    checkbox.ForeColor = Color.White;
                }
                else
                {
                    checkbox.BackColor = Color.Transparent;
                    checkbox.ForeColor = Color.Black;
                }
            }
        }

        private async Task CheckCloudStatusAsync()
        {
            try
            {
                var result = await client.GetOwnFieldsAsync();
                if (!result.IsSuccessful || result.Data == null)
                {
                    return; // Silently fail if cloud check fails
                }

                var cloudFields = result.Data;

                // Update UI on UI thread
                this.Invoke((Action)(() =>
                {
                    foreach (CheckBox checkbox in flpFieldList.Controls)
                    {
                        if (checkbox.Tag is FieldInfo fieldInfo)
                        {
                            fieldInfo.IsOnCloud = false;
                            fieldInfo.CloudFieldId = null;

                            // First check if local agshare.txt exists and has a valid ID
                            string idPath = Path.Combine(fieldInfo.DirectoryPath, "agshare.txt");
                            if (File.Exists(idPath))
                            {
                                try
                                {
                                    string raw = File.ReadAllText(idPath).Trim();
                                    Guid localId = Guid.Parse(raw);

                                    // Check if this ID exists in current user's cloud fields
                                    var cloudField = cloudFields.FirstOrDefault(f => f.Id == localId);
                                    if (cloudField != null)
                                    {
                                        // ID belongs to current user - mark as on cloud
                                        fieldInfo.IsOnCloud = true;
                                        fieldInfo.CloudFieldId = localId;
                                    }
                                    else
                                    {
                                        // ID in agshare.txt but NOT in current user's cloud fields
                                        // Could be different user OR same user with different/outdated ID
                                        // Will check by name next
                                        Log.EventWriter($"AgShare: Field '{fieldInfo.Name}' has agshare.txt ID not found in cloud fields. Checking by name...");
                                    }
                                }
                                catch
                                {
                                    // Invalid agshare.txt, ignore - will check by name below
                                }
                            }

                            // Only check by name if we haven't found the field via ID yet
                            // (even if agshare.txt exists, the ID might be wrong/outdated)
                            if (!fieldInfo.IsOnCloud)
                            {
                                var cloudField = cloudFields.FirstOrDefault(f => f.Name == fieldInfo.Name);
                                if (cloudField != null)
                                {
                                    // Found field with same name on cloud - mark as on cloud
                                    // This handles the case where local agshare.txt has a different ID
                                    fieldInfo.IsOnCloud = true;
                                    fieldInfo.CloudFieldId = cloudField.Id;
                                }
                            }

                            // Update checkbox text based on cloud status
                            if (fieldInfo.IsOnCloud)
                            {
                                checkbox.Text = $"☁ {fieldInfo.Name}";
                                checkbox.ForeColor = Color.FromArgb(0, 119, 190);
                            }
                            else
                            {
                                checkbox.Text = fieldInfo.Name;
                                checkbox.ForeColor = Color.Black;
                            }
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                // Silently fail - cloud check is optional
                Log.EventWriter($"AgShare cloud status check failed: {ex.Message}");
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox checkbox in flpFieldList.Controls)
            {
                checkbox.Checked = true;
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox checkbox in flpFieldList.Controls)
            {
                checkbox.Checked = false;
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (isUploading)
            {
                FormDialog.Show("Please Wait", "Upload already in progress", DialogSeverity.Info);
                return;
            }

            // Get selected fields
            var selectedFields = new List<FieldInfo>();
            foreach (CheckBox checkbox in flpFieldList.Controls)
            {
                if (checkbox.Checked && checkbox.Tag is FieldInfo fieldInfo)
                {
                    selectedFields.Add(fieldInfo);
                }
            }

            if (selectedFields.Count == 0)
            {
                FormDialog.Show("No Selection", "Please select at least one field to upload", DialogSeverity.Info);
                return;
            }

            // Confirm upload
            DialogResult result = FormDialog.ShowQuestion(
                "Confirm Upload",
                $"Upload {selectedFields.Count} field(s) to AgShare?");

            if (result != DialogResult.OK)
                return;

            await PerformBulkUpload(selectedFields);
        }

        private async Task PerformBulkUpload(List<FieldInfo> selectedFields)
        {
            isUploading = true;
            btnUpload.Enabled = false;
            btnClose.Enabled = false;
            btnSelectAll.Enabled = false;
            btnDeselectAll.Enabled = false;

            progressBar.Maximum = selectedFields.Count;
            progressBar.Value = 0;

            int successCount = 0;
            int skippedCount = 0;
            int failCount = 0;

            try
            {
                foreach (var fieldInfo in selectedFields)
                {
                    lblStatus.Text = $"Uploading: {fieldInfo.Name}...";
                    Application.DoEvents(); // Keep UI responsive

                    try
                    {
                        var outcome = await UploadField(fieldInfo);
                        if (outcome == AgShareUploadOutcome.Skipped)
                        {
                            skippedCount++;
                            lblStatus.Text = $"Skipped: {fieldInfo.Name} (already up to date)";
                        }
                        else
                        {
                            successCount++;
                            lblStatus.Text = $"Uploaded: {fieldInfo.Name} ✓";
                        }
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        lblStatus.Text = $"Failed: {fieldInfo.Name} ✗";

                        // Log detailed error information
                        string errorDetails = $"AgShare Upload Failed - Field: {fieldInfo.Name}\n" +
                                            $"Error: {ex.Message}\n";

                        if (ex.InnerException != null)
                        {
                            errorDetails += $"Inner Error: {ex.InnerException.Message}\n";
                        }

                        errorDetails += $"StackTrace: {ex.StackTrace}";
                        Log.EventWriter(errorDetails);
                    }

                    progressBar.Value++;
                    Application.DoEvents(); // Keep UI responsive
                    await Task.Delay(500); // Small delay to show status
                }

                // Build summary message
                string summary = $"Upload Complete\n\nSuccessful: {successCount}\nSkipped: {skippedCount}\nFailed: {failCount}";

                if (failCount > 0)
                {
                    summary += "\n\nCheck Log Viewer to view problems";
                }

                lblStatus.Text = $"Upload complete: {successCount} succeeded, {skippedCount} skipped, {failCount} failed";
                FormDialog.Show("Upload Complete", summary, failCount > 0 ? DialogSeverity.Warning : DialogSeverity.Info);
            }
            finally
            {
                isUploading = false;
                btnUpload.Enabled = true;
                btnClose.Enabled = true;
                btnSelectAll.Enabled = true;
                btnDeselectAll.Enabled = true;
            }
        }

        private async Task<AgShareUploadOutcome> UploadField(FieldInfo fieldInfo)
        {
            // Load field data from directory
            var snapshot = await LoadFieldSnapshot(fieldInfo);

            if (snapshot == null)
            {
                throw new Exception("Failed to load field data");
            }

            string idPath = Path.Combine(fieldInfo.DirectoryPath, "agshare.txt");

            // Scenario: Cloud ID was found via name check, but local agshare.txt has different ID
            if (fieldInfo.CloudFieldId.HasValue && File.Exists(idPath))
            {
                try
                {
                    string raw = File.ReadAllText(idPath).Trim();
                    Guid localId = Guid.Parse(raw);

                    // If cloud ID (from name match) is different from local ID, ask user
                    if (localId != fieldInfo.CloudFieldId.Value)
                    {
                        DuplicateNameChoice choice = AskDifferentCloudIdChoice(snapshot.FieldName);

                        if (choice == DuplicateNameChoice.Cancel)
                        {
                            throw new Exception("Upload cancelled by user");
                        }
                        else if (choice == DuplicateNameChoice.Overwrite)
                        {
                            // Overwrite: Use cloud ID (overwrite cloud field with this ID)
                            snapshot.FieldId = fieldInfo.CloudFieldId.Value;
                            File.WriteAllText(idPath, fieldInfo.CloudFieldId.Value.ToString());
                        }
                        // else: CreateNew - generate new ID (already done in LoadFieldSnapshot)
                    }
                    else
                    {
                        // IDs match - use cloud ID
                        snapshot.FieldId = fieldInfo.CloudFieldId.Value;
                    }
                }
                catch
                {
                    // Invalid agshare.txt, use cloud ID
                    snapshot.FieldId = fieldInfo.CloudFieldId.Value;
                    File.WriteAllText(idPath, fieldInfo.CloudFieldId.Value.ToString());
                }
            }
            // Scenario: Cloud ID found via name check, no local agshare.txt
            else if (fieldInfo.CloudFieldId.HasValue)
            {
                snapshot.FieldId = fieldInfo.CloudFieldId.Value;
                File.WriteAllText(idPath, fieldInfo.CloudFieldId.Value.ToString());
            }
            // Scenario: No cloud ID found during initial check, no local agshare.txt
            else if (!File.Exists(idPath))
            {
                // Check if field with same name exists on cloud (might have been added since initial check)
                var existingFieldId = await FindFieldByNameOnCloud(snapshot.FieldName);
                if (existingFieldId.HasValue)
                {
                    // Ask user what to do
                    DuplicateNameChoice choice = AskDuplicateNameChoice(snapshot.FieldName);

                    if (choice == DuplicateNameChoice.Cancel)
                    {
                        throw new Exception("Upload cancelled by user");
                    }
                    else if (choice == DuplicateNameChoice.Overwrite)
                    {
                        // Use existing cloud ID and save it locally
                        snapshot.FieldId = existingFieldId.Value;
                        File.WriteAllText(idPath, existingFieldId.Value.ToString());
                    }
                    // else: CreateNew - keep the new GUID that was already created
                }
            }

            // Use existing upload logic
            var outcome = await uploader.UploadAsync(snapshot, null);
            if (outcome == AgShareUploadOutcome.Failed)
            {
                throw new Exception("AgShare upload failed - check the log for details.");
            }
            return outcome;
        }

        private async Task<Guid?> FindFieldByNameOnCloud(string fieldName)
        {
            try
            {
                var result = await client.GetOwnFieldsAsync();
                if (result.IsSuccessful && result.Data != null)
                {
                    var existing = result.Data.FirstOrDefault(f => f.Name == fieldName);
                    if (existing != null)
                    {
                        return existing.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error checking for duplicate field name '{fieldName}': {ex.Message}");
            }
            return null;
        }

        private DuplicateNameChoice AskDuplicateNameChoice(string fieldName)
        {
            // This must be called on UI thread
            string message = $"Field '{fieldName}' already exists on AgShare cloud.\n\n" +
                            "What do you want to do?\n\n" +
                            "Yes = Overwrite existing cloud field\n" +
                            "No = Create as new field with different ID\n" +
                            "Cancel = Skip this field";

            DialogResult result = FormDialog.ShowQuestion(
                "Duplicate Field Name",
                message);

            if (result == DialogResult.OK)
                return DuplicateNameChoice.Overwrite;
            else if (result == DialogResult.No)
                return DuplicateNameChoice.CreateNew;
            else
                return DuplicateNameChoice.Cancel;
        }

        private DuplicateNameChoice AskDifferentCloudIdChoice(string fieldName)
        {
            // This must be called on UI thread
            string message = $"Field '{fieldName}' exists on cloud with a different ID.\n\n" +
                            "Your local field has an ID in agshare.txt, but the cloud has a different ID for a field with the same name.\n\n" +
                            "What do you want to do?\n\n" +
                            "Yes = Overwrite the cloud field (use cloud ID)\n" +
                            "No = Create as new field with a new ID\n" +
                            "Cancel = Skip this field";

            DialogResult result = FormDialog.ShowQuestion(
                "Different Cloud ID",
                message);

            if (result == DialogResult.OK)
                return DuplicateNameChoice.Overwrite;
            else if (result == DialogResult.No)
                return DuplicateNameChoice.CreateNew;
            else
                return DuplicateNameChoice.Cancel;
        }

        private async Task<FieldSnapshot> LoadFieldSnapshot(FieldInfo fieldInfo)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Use existing IO classes to load field data
                    // Load origin from Field.txt
                    Wgs84 origin = FieldPlaneFiles.LoadOrigin(fieldInfo.DirectoryPath);

                    // Load boundaries from Boundary.txt
                    List<CBoundaryList> boundaryList = BoundaryFiles.Load(fieldInfo.DirectoryPath);
                    List<List<vec3>> boundaries = new List<List<vec3>>();
                    foreach (var bnd in boundaryList)
                    {
                        if (bnd.fenceLine != null && bnd.fenceLine.Count > 0)
                        {
                            boundaries.Add(bnd.fenceLine);
                        }
                    }

                    // Allow upload without boundary - boundary is optional

                    // Load tracks from TrackLines.txt
                    List<CTrk> tracks = TrackFiles.Load(fieldInfo.DirectoryPath);

                    // Get or create field ID
                    Guid fieldId;
                    string idPath = Path.Combine(fieldInfo.DirectoryPath, "agshare.txt");
                    if (File.Exists(idPath))
                    {
                        string raw = File.ReadAllText(idPath).Trim();
                        fieldId = Guid.Parse(raw);
                    }
                    else
                    {
                        fieldId = Guid.NewGuid();
                    }

                    // Create LocalPlane with the field's own origin
                    LocalPlane plane = new LocalPlane(origin, new SharedFieldProperties());

                    return new FieldSnapshot
                    {
                        FieldName = fieldInfo.Name,
                        FieldDirectory = fieldInfo.DirectoryPath,
                        FieldId = fieldId,
                        OriginLat = origin.Latitude,
                        OriginLon = origin.Longitude,
                        Convergence = 0,
                        Boundaries = boundaries,
                        Tracks = tracks,
                        Converter = plane
                    };
                }
                catch (Exception ex)
                {
                    Log.EventWriter($"Error loading field snapshot for {fieldInfo.Name}: {ex.Message}");
                    return null;
                }
            });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (isUploading)
            {
                FormDialog.Show("Upload in Progress", "Please wait for upload to complete", DialogSeverity.Warning);
                return;
            }

            Close();
        }

        private class FieldInfo
        {
            public string Name { get; set; }
            public string DirectoryPath { get; set; }
            public bool IsOnCloud { get; set; }
            public Guid? CloudFieldId { get; set; }
        }

        private enum DuplicateNameChoice
        {
            Overwrite,
            CreateNew,
            Cancel
        }
    }
}
