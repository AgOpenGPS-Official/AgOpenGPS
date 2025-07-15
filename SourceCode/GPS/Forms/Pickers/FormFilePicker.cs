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
        private readonly FormGPS mf = null;
        private int order;
        private readonly List<string> fileList = new List<string>();

        public FormFilePicker(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            // Translate all the controls
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

        private void LoadFieldList()
        {
            fileList.Clear();
            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            if (dirs == null || dirs.Length < 1)
            {
                ShowNoFieldsMessage();
                return;
            }

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string fieldFile = Path.Combine(dir, "Field.txt");

                if (!File.Exists(fieldFile))
                    continue;

                ProcessFieldFile(fieldDirectory, fieldFile);
                ProcessBoundaryFile(dir, fieldDirectory);
            }

            if (fileList.Count < 1)
            {
                ShowNoFieldsMessage();
            }
        }

        private void ProcessFieldFile(string fieldDirectory, string filename)
        {
            try
            {
                using (GeoStreamReader reader = new GeoStreamReader(filename))
                {
                    // Skip 8 lines
                    for (int i = 0; i < 8; i++)
                    {
                        reader.ReadLine();
                    }

                    if (!reader.EndOfStream)
                    {
                        Wgs84 startLatLon = reader.ReadWgs84();
                        double distance = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);
                        fileList.Add(fieldDirectory);
                        fileList.Add(Math.Round(distance, 2).ToString("N2").PadLeft(10));
                    }
                    else
                    {
                        HandleDamagedField(fieldDirectory);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleDamagedField(fieldDirectory);
                Log.EventWriter("Field.txt is Broken" + ex.ToString());
            }
        }

        private void ProcessBoundaryFile(string dir, string fieldDirectory)
        {
            string filename = Path.Combine(dir, "Boundary.txt");
            if (!File.Exists(filename))
            {
                fileList.Add("Error");
                FormDialog.Show(
                    gStr.gsFileError,
                    $"{fieldDirectory} is Damaged, Missing Boundary.Txt \r\nDelete Field or Fix",
                    MessageBoxButtons.OK);
                return;
            }

            try
            {
                double area = CalculateBoundaryArea(filename);
                fileList.Add(area == 0 ? "No Bndry" : Math.Round(area, 1).ToString("N1").PadLeft(10));
            }
            catch (Exception ex)
            {
                fileList.Add("Error");
                Log.EventWriter("Boundary.txt is Broken" + ex.ToString());
            }
        }

        private double CalculateBoundaryArea(string filename)
        {
            List<vec3> pointList = new List<vec3>();
            double area = 0;

            using (StreamReader reader = new StreamReader(filename))
            {
                // Read header
                string line = reader.ReadLine(); // Boundary

                if (reader.EndOfStream)
                    return 0;

                // True or False OR points from older boundary files
                line = reader.ReadLine();

                // Check for older boundary files
                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine(); // number of points
                }

                // Check for latest boundary files
                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine(); // number of points
                }

                int numPoints = int.Parse(line);
                if (numPoints <= 0)
                    return 0;

                // Load the points
                for (int i = 0; i < numPoints; i++)
                {
                    line = reader.ReadLine();
                    string[] words = line.Split(',');
                    pointList.Add(new vec3(
                        double.Parse(words[0], CultureInfo.InvariantCulture),
                        double.Parse(words[1], CultureInfo.InvariantCulture),
                        double.Parse(words[2], CultureInfo.InvariantCulture)));
                }

                // Calculate area if we have enough points
                int ptCount = pointList.Count;
                if (ptCount > 5)
                {
                    area = 0;
                    int j = ptCount - 1;

                    for (int i = 0; i < ptCount; j = i++)
                    {
                        area += (pointList[j].easting + pointList[i].easting) *
                               (pointList[j].northing - pointList[i].northing);
                    }

                    area = Math.Abs(area / 2) * (mf.isMetric ? 0.0001 : 0.00024711);
                }
            }

            return area;
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

        private void HandleDamagedField(string fieldDirectory)
        {
            FormDialog.Show(gStr.gsFileError, $"{fieldDirectory} is Damaged, Please Delete This Field", MessageBoxButtons.OK);
            fileList.Add(fieldDirectory);
            fileList.Add("Error");
        }

        private void btnByDistance_Click(object sender, EventArgs e)
        {
            order = (order + 1) % 3;
            UpdateListView();
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count == 0)
                return;

            var selectedItem = lvLines.SelectedItems[0];
            if (selectedItem.SubItems[0].Text == "Error" ||
                selectedItem.SubItems[1].Text == "Error" ||
                selectedItem.SubItems[2].Text == "Error")
            {
                FormDialog.Show(
                    gStr.gsFileError,
                    "This Field is Damaged, Please Delete \r\nALREADY TOLD YOU THAT :)",
                    MessageBoxButtons.OK);
                return;
            }

            string fieldName = order == 0 ? selectedItem.SubItems[0].Text : selectedItem.SubItems[1].Text;
            mf.filePickerFileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, fieldName, "Field.txt");
            Close();
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count == 0)
                return;

            string fieldName = order == 0 ?
                lvLines.SelectedItems[0].SubItems[0].Text :
                lvLines.SelectedItems[0].SubItems[1].Text;

            string dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, fieldName);

            if (FormDialog.Show(dir2Delete, gStr.gsDeleteForSure, MessageBoxButtons.YesNo) != DialogResult.OK)
                return;

            Directory.Delete(dir2Delete, true);
            LoadFieldList();
            UpdateListView();
        }
    }
}