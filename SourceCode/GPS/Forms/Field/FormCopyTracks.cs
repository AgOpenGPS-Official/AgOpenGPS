using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AgOpenGPS.Core.Models;
using AgOpenGPS.IO;

namespace AgOpenGPS.Forms.Field
{
    /// <summary>
    /// Form that allows the user to copy tracks from other fields to the current field.
    /// Handles coordinate conversion between different field origins.
    /// </summary>
    public partial class FormCopyTracks : Form
    {
        private readonly FormGPS mf;
        private string selectedFieldDirectory;

        public FormCopyTracks(FormGPS gpsContext)
        {
            InitializeComponent();
            mf = gpsContext;
        }

        private void FormCopyTracks_Load(object sender, EventArgs e)
        {
            LoadFieldList();
        }

        private void LoadFieldList()
        {
            try
            {
                string fieldsDir = RegistrySettings.fieldsDirectory;
                if (!Directory.Exists(fieldsDir))
                {
                    lblStatus.Text = "Fields directory not found";
                    return;
                }

                var fieldDirs = Directory.GetDirectories(fieldsDir);
                lbFields.BeginUpdate();
                lbFields.Items.Clear();

                foreach (var fieldDir in fieldDirs)
                {
                    // Check if field has Field.txt and TrackLines.txt
                    string fieldFile = Path.Combine(fieldDir, "Field.txt");
                    string trackFile = Path.Combine(fieldDir, "TrackLines.txt");

                    if (!File.Exists(fieldFile) || !File.Exists(trackFile))
                        continue;

                    // Don't show the current field in the list
                    string fieldName = Path.GetFileName(fieldDir);
                    if (mf.currentFieldDirectory != null && fieldName == mf.currentFieldDirectory)
                        continue;

                    var fieldDirInfo = new DirectoryInfo(fieldDir);
                    var item = new ListViewItem(fieldDirInfo.Name);
                    item.Tag = fieldDirInfo;
                    lbFields.Items.Add(item);
                }

                lbFields.EndUpdate();
                lblStatus.Text = $"Found {lbFields.Items.Count} field(s) with tracks";

                if (lbFields.Items.Count > 0)
                    lbFields.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(2000, "Import Tracks", "Failed to load field list: " + ex.Message);
                lblStatus.Text = "Error loading fields";
            }
        }

        private void lbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0) return;

            var selectedItem = lbFields.SelectedItems[0];
            if (selectedItem.Tag is DirectoryInfo fieldDirInfo)
            {
                selectedFieldDirectory = fieldDirInfo.FullName;
                LoadTracksFromField(fieldDirInfo.FullName);
            }
        }

        private void LoadTracksFromField(string fieldDirectory)
        {
            try
            {
                var availableTracks = TrackFiles.Load(fieldDirectory);
                flpTrackList.Controls.Clear();

                if (availableTracks.Count == 0)
                {
                    lblStatus.Text = "No tracks found in this field";
                    return;
                }

                foreach (var track in availableTracks)
                {
                    string trackName = track.name ?? "Unnamed Track";
                    string trackType = track.mode == TrackMode.AB ? "AB Line" :
                                      track.mode == TrackMode.Curve ? "Curve" :
                                      track.mode.ToString();

                    var checkbox = CreateTrackCheckbox(track, $"{trackName} ({trackType})");
                    flpTrackList.Controls.Add(checkbox);
                }

                lblStatus.Text = $"{flpTrackList.Controls.Count} track(s) available for importing";
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(2000, "Import Tracks", "Failed to load tracks: " + ex.Message);
                lblStatus.Text = "Error loading tracks";
            }
        }

        private CheckBox CreateTrackCheckbox(CTrk track, string displayText)
        {
            var checkbox = new CheckBox
            {
                Text = displayText,
                Checked = false,
                AutoSize = false,
                Width = flpTrackList.Width - 25,
                Height = 45,
                Tag = track,
                Font = new Font("Tahoma", 18F, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 8, 0, 0),
                BackColor = Color.Transparent,
                ForeColor = Color.Black
            };

            checkbox.CheckedChanged += OnTrackSelectionChanged;
            return checkbox;
        }

        private void OnTrackSelectionChanged(object sender, EventArgs e)
        {
            if (sender is CheckBox checkbox)
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


        private void btnSelectAllTracks_Click(object sender, EventArgs e)
        {
            foreach (CheckBox checkbox in flpTrackList.Controls)
            {
                checkbox.Checked = true;
            }
        }

        private void btnDeselectAllTracks_Click(object sender, EventArgs e)
        {
            foreach (CheckBox checkbox in flpTrackList.Controls)
            {
                checkbox.Checked = false;
            }
        }

        private void btnCopyToCurrentField_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a field is selected
                if (string.IsNullOrEmpty(selectedFieldDirectory))
                {
                    mf.TimedMessageBox(2000, "Import Tracks", "Please select a field first.");
                    return;
                }

                // Get selected tracks
                var selectedTracks = new List<CTrk>();
                foreach (CheckBox checkbox in flpTrackList.Controls)
                {
                    if (checkbox.Checked && checkbox.Tag is CTrk track)
                    {
                        selectedTracks.Add(track);
                    }
                }

                if (selectedTracks.Count == 0)
                {
                    mf.TimedMessageBox(2000, "Import Tracks", "Please select at least one track to import.");
                    return;
                }

                // Verify current field is open
                if (string.IsNullOrEmpty(mf.currentFieldDirectory))
                {
                    mf.TimedMessageBox(2000, "Import Tracks", "No field is currently open.");
                    return;
                }

                // Build full path for current field directory
                string currentFieldFullPath = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

                lblStatus.Text = "Saving current tracks...";
                Application.DoEvents();

                // First save any changes to current tracks
                mf.FileSaveTracks();

                lblStatus.Text = "Converting tracks...";
                Application.DoEvents();

                // Use TrackCopier to convert and copy tracks
                int copiedCount = TrackCopier.CopyTracksToField(
                    selectedFieldDirectory,
                    currentFieldFullPath,
                    selectedTracks,
                    mf.AppModel.SharedFieldProperties);

                lblStatus.Text = "Saving tracks...";
                Application.DoEvents();

                // Clear and reload tracks to ensure all indices and references are correct
                mf.trk.gArr?.Clear();
                mf.FileLoadTracks();

                // Set index to first visible track if available, otherwise -1
                if (mf.trk.gArr.Count > 0)
                {
                    mf.trk.idx = 0;
                    // Find first visible track
                    for (int i = 0; i < mf.trk.gArr.Count; i++)
                    {
                        if (mf.trk.gArr[i].isVisible)
                        {
                            mf.trk.idx = i;
                            break;
                        }
                    }
                }

                lblStatus.Text = $"Successfully imported {copiedCount} track(s)";

                mf.TimedMessageBox(2000, "Import Tracks",
                    $"Successfully imported {copiedCount} track(s) to current field.");
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                mf.TimedMessageBox(3000, "Import Tracks", "Error importing tracks: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
