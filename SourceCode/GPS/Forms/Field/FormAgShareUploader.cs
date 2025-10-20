using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.IO;

namespace AgOpenGPS.Forms
{
    public partial class FormAgShareUploader : Form
    {
        private readonly AgShareClient agShareClient;
        private List<FieldInfo> availableFields;
        private bool isUploading = false;

        public FormAgShareUploader()
        {
            InitializeComponent();
            // Create AgShareClient using settings directly
            agShareClient = new AgShareClient(
                Properties.Settings.Default.AgShareServer,
                Properties.Settings.Default.AgShareApiKey);
        }

        private void FormAgShareBulkUploader_Load(object sender, EventArgs e)
        {
            LoadAvailableFields();
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

                    // Check if field has necessary files (Boundary.txt contains boundary data)
                    string boundaryFile = Path.Combine(fieldDir, "Boundary.txt");
                    if (File.Exists(boundaryFile))
                    {
                        var fieldInfo = new FieldInfo
                        {
                            Name = fieldName,
                            DirectoryPath = fieldDir
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
                Text = fieldInfo.Name,
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
                FormDialog.Show("Please Wait", "Upload already in progress", MessageBoxButtons.OK);
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
                FormDialog.Show("No Selection", "Please select at least one field to upload", MessageBoxButtons.OK);
                return;
            }

            // Confirm upload
            DialogResult result = FormDialog.Show(
                "Confirm Upload",
                $"Upload {selectedFields.Count} field(s) to AgShare?",
                MessageBoxButtons.YesNo);

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
            int failCount = 0;

            try
            {
                foreach (var fieldInfo in selectedFields)
                {
                    lblStatus.Text = $"Uploading: {fieldInfo.Name}...";
                    Application.DoEvents(); // Keep UI responsive

                    try
                    {
                        await UploadField(fieldInfo);
                        successCount++;
                        lblStatus.Text = $"Uploaded: {fieldInfo.Name} ✓";
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        lblStatus.Text = $"Failed: {fieldInfo.Name} ✗ - {ex.Message}";
                        Log.EventWriter($"Failed to upload field {fieldInfo.Name}: {ex.Message}");
                    }

                    progressBar.Value++;
                    Application.DoEvents(); // Keep UI responsive
                    await Task.Delay(500); // Small delay to show status
                }

                // Show summary
                lblStatus.Text = $"Upload complete: {successCount} succeeded, {failCount} failed";
                FormDialog.Show(
                    "Upload Complete",
                    $"Upload Complete\n\nSuccessful: {successCount}\nFailed: {failCount}",
                    MessageBoxButtons.OK);
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

        private async Task UploadField(FieldInfo fieldInfo)
        {
            // Load field data from directory
            var snapshot = await LoadFieldSnapshot(fieldInfo);

            if (snapshot == null)
            {
                throw new Exception("Failed to load field data");
            }

            // Use existing upload logic
            await CAgShareUploader.UploadAsync(snapshot, agShareClient, null);
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

                    if (boundaries.Count == 0)
                        return null;

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
                FormDialog.Show("Upload in Progress", "Please wait for upload to complete", MessageBoxButtons.OK);
                return;
            }

            Close();
        }

        private class FieldInfo
        {
            public string Name { get; set; }
            public string DirectoryPath { get; set; }
        }
    }
}
