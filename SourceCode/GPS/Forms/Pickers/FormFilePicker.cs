using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFilePicker : Form
    {
        // Reference to main form (FormGPS)
        private readonly FormGPS mf = null;

        // 0 = Name|Distance|Area, 1 = Distance|Name|Area, 2 = Area|Name|Distance
        private int order;

        // Triplets: [name, distance, area]
        private readonly List<string> fileList = new List<string>();

        public FormFilePicker(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            // Translate UI
            this.Text = gStr.gsFieldPicker;
            btnByDistance.Text = gStr.gsSort;
            btnOpenExistingLv.Text = gStr.gsUseSelected;
            labelDeleteField.Text = gStr.gsDeleteField;
            labelCancel.Text = gStr.gsCancel;
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            order = 0;
            timer1.Enabled = true;

            LoadFieldList();
            UpdateListView();
        }

        /// <summary>
        /// Populate the list from folders that contain Field.txt only.
        /// This form does not show error dialogs; it remains neutral.
        /// </summary>
        private void LoadFieldList()
        {
            fileList.Clear();

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);
            }
            catch
            {
                dirs = Array.Empty<string>();
            }

            if (dirs == null || dirs.Length < 1)
            {
                ShowNoFieldsMessage();
                return;
            }

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string fieldFile = Path.Combine(dir, "Field.txt");

                // Only show folders that actually have Field.txt
                if (!File.Exists(fieldFile))
                    continue;

                // Append name and distance (or neutral placeholder on failure)
                AppendFieldNameAndDistance(fieldDirectory, fieldFile);

                // Append area (or neutral placeholder on failure)
                AppendBoundaryArea(dir);
            }

            if (fileList.Count < 1)
            {
                ShowNoFieldsMessage();
            }
        }

        /// <summary>
        /// Adds [name, distance] pair to fileList, using a neutral placeholder if distance cannot be computed.
        /// </summary>
        private void AppendFieldNameAndDistance(string fieldDirectory, string filename)
        {
            try
            {
                using (var reader = new GeoStreamReader(filename))
                {
                    // Legacy format: skip 8 lines, then expect a WGS84 position.
                    for (int i = 0; i < 8; i++) reader.ReadLine();

                    string distanceStr;
                    if (!reader.EndOfStream)
                    {
                        var startLatLon = reader.ReadWgs84();
                        double km = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);
                        distanceStr = Math.Round(km, 2).ToString("N2").PadLeft(10);
                    }
                    else
                    {
                        // Incomplete file → neutral placeholder
                        distanceStr = "---".PadLeft(10);
                    }

                    fileList.Add(fieldDirectory);
                    fileList.Add(distanceStr);
                }
            }
            catch (Exception ex)
            {
                // Neutral placeholder; no popup here. Central loader will validate again.
                Log.EventWriter("Field.txt read failed (picker): " + ex);
                fileList.Add(fieldDirectory);
                fileList.Add("---".PadLeft(10));
            }
        }

        /// <summary>
        /// Adds [area] to fileList: computed from Boundary.txt if present; neutral placeholder otherwise.
        /// </summary>
        private void AppendBoundaryArea(string dir)
        {
            string filename = Path.Combine(dir, "Boundary.txt");

            if (!File.Exists(filename))
            {
                fileList.Add("---".PadLeft(10));
                return;
            }

            try
            {
                double area = CalculateBoundaryArea(filename);
                // Show "No Bndry" if area = 0 (legacy behavior), otherwise display the numeric value.
                fileList.Add(area == 0 ? "No Bndry" : Math.Round(area, 1).ToString("N1").PadLeft(10));
            }
            catch (Exception ex)
            {
                Log.EventWriter("Boundary.txt read failed (picker): " + ex);
                fileList.Add("---".PadLeft(10));
            }
        }

        /// <summary>
        /// Computes boundary area in hectares (metric) or acres (imperial).
        /// Returns 0 on insufficient points or degenerate polygon.
        /// </summary>
        private double CalculateBoundaryArea(string filename)
        {
            var pointList = new List<vec3>();
            using (var reader = new StreamReader(filename))
            {
                string line;

                // Header
                line = reader.ReadLine();
                if (line == null) return 0;

                // Next line(s): may contain True/False flags and a count; handle legacy variations safely
                line = reader.ReadLine();
                if (line == null) return 0;

                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine();
                    if (line == null) return 0;
                }
                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine();
                    if (line == null) return 0;
                }

                int numPoints;
                if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out numPoints))
                    return 0;

                if (numPoints <= 0) return 0;

                for (int i = 0; i < numPoints; i++)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) return 0;

                    var words = line.Split(',');
                    if (words.Length < 3) return 0;

                    double e, n, h;
                    if (!double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e)) return 0;
                    if (!double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n)) return 0;
                    if (!double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h)) return 0;

                    pointList.Add(new vec3(e, n, h));
                }
            }

            int ptCount = pointList.Count;
            if (ptCount <= 5) return 0;

            // Shoelace algorithm
            double acc = 0;
            int j = ptCount - 1;
            for (int i = 0; i < ptCount; j = i++)
            {
                acc += (pointList[j].easting + pointList[i].easting) *
                       (pointList[j].northing - pointList[i].northing);
            }

            double areaM2 = Math.Abs(acc / 2.0);
            return mf.isMetric ? (areaM2 * 0.0001) : (areaM2 * 0.00024711);
        }

        private void UpdateListView()
        {
            lvLines.Items.Clear();

            for (int i = 0; i < fileList.Count; i += 3)
            {
                string[] fieldNames = GetFieldNames(i);
                lvLines.Items.Add(new ListViewItem(fieldNames));
            }

            if (lvLines.Items.Count > 0)
            {
                UpdateColumnHeaders();
            }
            else
            {
                ShowNoFieldsMessage();
            }
        }

        private string[] GetFieldNames(int index)
        {
            if (order == 0)
                return new[] { fileList[index], fileList[index + 1], fileList[index + 2] };
            else if (order == 1)
                return new[] { fileList[index + 1], fileList[index], fileList[index + 2] };
            else
                return new[] { fileList[index + 2], fileList[index], fileList[index + 1] };
        }

        private void UpdateColumnHeaders()
        {
            if (order == 0)
            {
                SetColumnProperties(chName, gStr.gsField, 680);
                SetColumnProperties(chDistance, gStr.gsDistance, 140);
                SetColumnProperties(chArea, gStr.gsArea, 140);
            }
            else if (order == 1)
            {
                SetColumnProperties(chName, gStr.gsDistance, 140);
                SetColumnProperties(chDistance, gStr.gsField, 680);
                SetColumnProperties(chArea, gStr.gsArea, 140);
            }
            else
            {
                SetColumnProperties(chName, gStr.gsArea, 140);
                SetColumnProperties(chDistance, gStr.gsField, 680);
                SetColumnProperties(chArea, gStr.gsDistance, 140);
            }
        }

        private void SetColumnProperties(ColumnHeader column, string text, int width)
        {
            column.Text = text;
            column.Width = width;
        }

        private void ShowNoFieldsMessage()
        {
            mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
            Log.EventWriter("File Picker, No Fields");
            Close();
        }

        private void btnByDistance_Click(object sender, EventArgs e)
        {
            order = (order + 1) % 3;
            UpdateListView();
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count == 0) return;

            var selectedItem = lvLines.SelectedItems[0];

            // Determine the field name depending on the current ordering
            string fieldName = order == 0
                ? selectedItem.SubItems[0].Text
                : selectedItem.SubItems[1].Text;

            if (string.IsNullOrWhiteSpace(fieldName) || fieldName == "---")
                return;

            // Return the selected Field.txt back to FormJob
            mf.filePickerFileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, fieldName, "Field.txt");

            // Explicitly set DialogResult; safe even if the designer sets it already
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count == 0) return;

            string fieldName = order == 0
                ? lvLines.SelectedItems[0].SubItems[0].Text
                : lvLines.SelectedItems[0].SubItems[1].Text;

            if (string.IsNullOrWhiteSpace(fieldName) || fieldName == "---")
                return;

            string dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, fieldName);

            // Keep this dialog here; it is a user action (delete confirmation), not loader validation.
            if (FormDialog.Show(dir2Delete, gStr.gsDeleteForSure, MessageBoxButtons.YesNo) != DialogResult.OK)
                return;

            try
            {
                Directory.Delete(dir2Delete, true);
            }
            catch (Exception ex)
            {
                Log.EventWriter("Field delete failed: " + ex);
            }

            LoadFieldList();
            UpdateListView();
        }
    }
}
