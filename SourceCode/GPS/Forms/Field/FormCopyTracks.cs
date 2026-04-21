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
            // Force tall rows for touch-friendly targets
            var il = new ImageList { ImageSize = new Size(1, 40) };
            lvTracks.SmallImageList = il;

            // Size column to fill the list width
            chTrackName.Width = lvTracks.ClientSize.Width - 4;

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
                FormDialog.Show("Import Tracks", "Failed to load field list: " + ex.Message, DialogSeverity.Error);
                lblStatus.Text = "Error loading fields";
            }
        }

        private void lbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0) return;

            // Reset all items, highlight selected orange
            foreach (ListViewItem it in lbFields.Items)
            {
                it.BackColor = Color.WhiteSmoke;
                it.ForeColor = Color.Black;
            }

            var selectedItem = lbFields.SelectedItems[0];
            selectedItem.BackColor = Color.DarkOrange;
            selectedItem.ForeColor = Color.White;

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
                lvTracks.Items.Clear();

                if (availableTracks.Count == 0)
                {
                    lblStatus.Text = "No tracks found in this field";
                    return;
                }

                lvTracks.BeginUpdate();
                foreach (var track in availableTracks)
                {
                    string trackName = track.name ?? "Unnamed Track";
                    string trackType = track.mode == TrackMode.AB ? "AB Line" :
                                      track.mode == TrackMode.Curve ? "Curve" :
                                      track.mode.ToString();

                    var item = new ListViewItem($"{trackName} ({trackType})");
                    item.Tag = track;
                    item.BackColor = Color.White;
                    item.ForeColor = Color.Black;
                    lvTracks.Items.Add(item);
                }
                lvTracks.EndUpdate();

                lblStatus.Text = $"{lvTracks.Items.Count} track(s) available for importing";
            }
            catch (Exception ex)
            {
                FormDialog.Show("Import Tracks", "Failed to load tracks: " + ex.Message, DialogSeverity.Error);
                lblStatus.Text = "Error loading tracks";
            }
        }

        private static readonly Color SelectedColor = Color.FromArgb(0, 150, 0);

        private void lvTracks_MouseClick(object sender, MouseEventArgs e)
        {
            var hit = lvTracks.HitTest(e.Location);
            if (hit.Item == null) return;

            bool isSelected = hit.Item.BackColor == SelectedColor;
            hit.Item.BackColor = isSelected ? Color.White : SelectedColor;
            hit.Item.ForeColor = isSelected ? Color.Black : Color.White;
        }


        private void btnSelectAllTracks_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTracks.Items)
            {
                item.BackColor = SelectedColor;
                item.ForeColor = Color.White;
            }
        }

        private void btnDeselectAllTracks_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvTracks.Items)
            {
                item.BackColor = Color.White;
                item.ForeColor = Color.Black;
            }
        }

        private void btnCopyToCurrentField_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a field is selected
                if (string.IsNullOrEmpty(selectedFieldDirectory))
                {
                    FormDialog.Show("Import Tracks", "Please select a field first.", DialogSeverity.Error);
                    return;
                }

                // Get selected tracks
                var selectedTracks = new List<CTrk>();
                foreach (ListViewItem item in lvTracks.Items)
                {
                    if (item.BackColor == SelectedColor && item.Tag is CTrk track)
                    {
                        selectedTracks.Add(track);
                    }
                }

                if (selectedTracks.Count == 0)
                {
                    FormDialog.Show("Import Tracks", "Please select at least one track to import.", DialogSeverity.Error);
                    return;
                }

                // Verify current field is open
                if (string.IsNullOrEmpty(mf.currentFieldDirectory))
                {
                    FormDialog.Show("Import Tracks", "No field is currently open.", DialogSeverity.Error);
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

                FormDialog.Show("Import Tracks", $"Successfully imported {copiedCount} track(s) to current field.", DialogSeverity.Info);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                FormDialog.Show("Import Tracks", "Error importing tracks: " + ex.Message, DialogSeverity.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
