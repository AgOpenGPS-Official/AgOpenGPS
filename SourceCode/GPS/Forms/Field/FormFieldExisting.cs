using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFieldExisting : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private int order;

        private readonly List<string> fileList = new List<string>();

        public FormFieldExisting(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();

            label1.Text = gStr.gsEditFieldName;
            btnSort.Text = gStr.gsSort;
            lblTemplateChosen.Text = "---";
        }

        private void FormFieldExisting_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            mf.CloseTopMosts();

            order = 0;
            timer1.Enabled = true;
            ListViewItem itm;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            if (dirs == null || dirs.Length < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsCreateNewField, gStr.gsFileError);
                Log.EventWriter("File Error Load Existing Field");

                Close();
                return;
            }

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (GeoStreamReader reader = new GeoStreamReader(filename))
                    {
                        try
                        {
                            // Skip 8 lines
                            for (int i = 0; i < 8; i++)
                            {
                                reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                Wgs84 startLatLon = reader.ReadWgs84();
                                double distance = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);

                                fileList.Add(fieldDirectory);
                                fileList.Add(Math.Round(distance, 2).ToString("N2").PadLeft(10));
                            }
                            else
                            {
                                MessageBox.Show(fieldDirectory + " is Damaged, Please Delete This Field", gStr.gsFileError,
                                MessageBoxButtons.OK, MessageBoxIcon.Error);

                                fileList.Add(fieldDirectory);
                                fileList.Add("Error");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(fieldDirectory + " is Damaged, Please Delete, Field.txt is Broken", gStr.gsFileError,
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Log.EventWriter(fieldDirectory + " is Damaged, Please Delete,Field.txt is Broken" + ex.ToString());
                            fileList.Add(fieldDirectory);
                            fileList.Add("Error");
                        }
                    }
                }
                else continue;

                //grab the boundary area
                filename = Path.Combine(dir, "Boundary.txt");
                if (File.Exists(filename))
                {
                    string line;
                    List<vec3> pointList = new List<vec3>();
                    double area = 0;

                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();//Boundary

                            if (!reader.EndOfStream)
                            {
                                //True or False OR points from older boundary files
                                line = reader.ReadLine();

                                //Check for older boundary files, then above line string is num of points
                                if (line == "True" || line == "False")
                                {
                                    line = reader.ReadLine(); //number of points
                                }

                                //Check for latest boundary files, then above line string is num of points
                                if (line == "True" || line == "False")
                                {
                                    line = reader.ReadLine(); //number of points
                                }

                                int numPoints = int.Parse(line);

                                if (numPoints > 0)
                                {
                                    //load the line
                                    for (int i = 0; i < numPoints; i++)
                                    {
                                        line = reader.ReadLine();
                                        string[] words = line.Split(',');
                                        vec3 vecPt = new vec3(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));

                                        pointList.Add(vecPt);
                                    }

                                    int ptCount = pointList.Count;
                                    if (ptCount > 5)
                                    {
                                        area = 0;         // Accumulates area in the loop
                                        int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                                        for (int i = 0; i < ptCount; j = i++)
                                        {
                                            area += (pointList[j].easting + pointList[i].easting) * (pointList[j].northing - pointList[i].northing);
                                        }
                                        if (mf.isMetric)
                                        {
                                            area = (Math.Abs(area / 2)) * 0.0001;
                                        }
                                        else
                                        {
                                            area = (Math.Abs(area / 2)) * 0.00024711;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ef)
                        {
                            area = 0;
                            Log.EventWriter(fieldDirectory + " Boundary.Txt error " + ef.ToString());
                        }
                    }
                    if (area == 0)
                    {
                        fileList.Add("No Bndry");
                        Log.EventWriter("Boundary is Broken, no Area");
                    }
                    else fileList.Add(Math.Round(area, 1).ToString("N1").PadLeft(10));
                }
                else
                {
                    fileList.Add("Error");
                    MessageBox.Show(fieldDirectory + " is Damaged, Missing Boundary.Txt " +
                        "               \r\n Delete Field or Fix ", gStr.gsFileError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.EventWriter(fieldDirectory + " is Damaged, Missing Boundary.Txt ");
                }

                filename = Path.Combine(dir, "Field.txt");
            }

            if (fileList == null || fileList.Count < 1)
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Log.EventWriter("Create New Field, No Fields Found");

                Close();
                return;
            }
            for (int i = 0; i < fileList.Count; i += 3)
            {
                string[] fieldNames = { fileList[i], fileList[i + 1], fileList[i + 2] };
                itm = new ListViewItem(fieldNames);
                lvLines.Items.Add(itm);
            }

            //string fieldName = Path.GetDirectoryName(dir).ToString(CultureInfo.InvariantCulture);

            if (lvLines.Items.Count > 0)
            {
                this.chName.Text = gStr.gsField;
                this.chName.Width = 680;

                this.chDistance.Text = gStr.gsDistance;
                this.chDistance.Width = 140;

                this.chArea.Text = gStr.gsArea;
                this.chArea.Width = 140;
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsCreateNewField);
                Log.EventWriter("Field Existing, No Fields to List");

                Close();
                return;
            }

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

        }

        private void tboxFieldName_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                return;
            }

            string fileStr = Path.Combine(RegistrySettings.fieldsDirectory, lblTemplateChosen.Text, "Field.txt");

            if (!File.Exists(fileStr))
            {
               mf.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                return;
            }

            if (mf.isJobStarted) 
                await mf.FileSaveEverythingBeforeClosingField();

            //get the directory and make sure it exists, create if not
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, tboxFieldName.Text.Trim());

            // create from template
            if (Directory.Exists(directoryName))
            {
                MessageBox.Show(gStr.gsChooseADifferentName, gStr.gsDirectoryExists, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                //create the new directory
                if ((!string.IsNullOrEmpty(directoryName)) && (!Directory.Exists(directoryName)))
                { Directory.CreateDirectory(directoryName); }
            }

            string line;
            string offsets, convergence, startFix;

            using (StreamReader reader = new StreamReader(fileStr))
            {
                try
                {
                    line = reader.ReadLine();
                    line = reader.ReadLine();
                    line = reader.ReadLine();
                    line = reader.ReadLine();

                    //read the Offsets  - all we really need from template field file
                    offsets = reader.ReadLine();

                    line = reader.ReadLine();
                    convergence = reader.ReadLine();

                    line = reader.ReadLine();
                    startFix = reader.ReadLine();
                }
                catch (Exception ex)
                {
                    Log.EventWriter("While Opening Field" + ex);

                    mf.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                    mf.JobClose();
                    return;
                }

                const string myFileName = "Field.txt";

                using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, myFileName)))
                {
                    //Write out the date
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                    writer.WriteLine("$FieldDir");
                    writer.WriteLine("FromExisting");

                    //write out the easting and northing Offsets
                    writer.WriteLine("$Offsets");
                    writer.WriteLine(offsets);

                    writer.WriteLine("$Convergence");
                    writer.WriteLine(convergence);

                    writer.WriteLine("StartFix");
                    writer.WriteLine(startFix);
                }

                //create txt file copies
                string templateDirectoryName = Path.Combine(RegistrySettings.fieldsDirectory, lblTemplateChosen.Text);
                string fileToCopy = "";
                string destinationDirectory = "";

                if (chkApplied.Checked)
                {
                    fileToCopy = Path.Combine(templateDirectoryName, "Contour.txt");
                    destinationDirectory = Path.Combine(directoryName, "Contour.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);

                    fileToCopy = Path.Combine(templateDirectoryName, "Sections.txt");
                    destinationDirectory = Path.Combine(directoryName, "Sections.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);
                }
                else
                {
                    //create blank Contour and Section files
                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Sections.txt")))
                    {
                        //blank
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Contour.txt")))
                    {
                        writer.WriteLine("$Contour");
                    }
                }

                fileToCopy = Path.Combine(templateDirectoryName, "BackPic.txt");
                destinationDirectory = Path.Combine(directoryName, "BackPic.txt");
                if (File.Exists(fileToCopy))
                    File.Copy(fileToCopy, destinationDirectory);

                fileToCopy = Path.Combine(templateDirectoryName, "BackPic.png");
                destinationDirectory = Path.Combine(directoryName, "BackPic.png");
                if (File.Exists(fileToCopy))
                    File.Copy(fileToCopy, destinationDirectory);

                fileToCopy = Path.Combine(templateDirectoryName, "Boundary.txt");
                destinationDirectory = Path.Combine(directoryName, "Boundary.txt");
                if (File.Exists(fileToCopy))
                    File.Copy(fileToCopy, destinationDirectory);


                fileToCopy = Path.Combine(templateDirectoryName, "Headlines.txt");
                destinationDirectory = Path.Combine(directoryName, "Headlines.txt");
                if (File.Exists(fileToCopy))
                    File.Copy(fileToCopy, destinationDirectory);
                else
                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Headlines.txt")))
                    {
                        writer.WriteLine("$Headlines");
                    }

                if (chkFlags.Checked)
                {
                    fileToCopy = Path.Combine(templateDirectoryName, "Flags.txt");
                    destinationDirectory = Path.Combine(directoryName, "Flags.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "Flags.txt")))
                    {
                        writer.WriteLine("$Flags");
                        writer.WriteLine("0");
                    }
                }

                if (chkGuidanceLines.Checked)
                {
                    fileToCopy = Path.Combine(templateDirectoryName, "ABLines.txt");
                    destinationDirectory = Path.Combine(directoryName, "ABLines.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);

                    fileToCopy = Path.Combine(templateDirectoryName, "RecPath.txt");
                    destinationDirectory = Path.Combine(directoryName, "RecPath.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);

                    fileToCopy = Path.Combine(templateDirectoryName, "CurveLines.txt");
                    destinationDirectory = Path.Combine(directoryName, "CurveLines.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);

                    fileToCopy = Path.Combine(templateDirectoryName, "Tram.txt");
                    destinationDirectory = Path.Combine(directoryName, "Tram.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);

                    fileToCopy = Path.Combine(templateDirectoryName, "TrackLines.txt");
                    destinationDirectory = Path.Combine(directoryName, "TrackLines.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(Path.Combine(directoryName, "RecPath.txt")))
                    {
                        writer.WriteLine("$RecPath");
                        writer.WriteLine("0");
                    }
                }

                if (chkHeadland.Checked)
                {
                    fileToCopy = Path.Combine(templateDirectoryName, "Headland.txt");
                    destinationDirectory = Path.Combine(directoryName, "Headland.txt");
                    if (File.Exists(fileToCopy))
                        File.Copy(fileToCopy, destinationDirectory);
                }

                //now open the newly cloned field
                mf.FileOpenField(Path.Combine(directoryName, myFileName));
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
                btnSerialCancel.Focus();
            }
        }

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void btnAddVehicleName_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + RegistrySettings.vehicleFileName;
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            ListViewItem itm;

            lvLines.Items.Clear();
            order += 1;
            if (order == 3) order = 0;

            for (int i = 0; i < fileList.Count; i += 3)
            {
                if (order == 0)
                {
                    string[] fieldNames = { fileList[i], fileList[i + 1], fileList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }
                else if (order == 1)
                {
                    string[] fieldNames = { fileList[i + 1], fileList[i], fileList[i + 2] };
                    itm = new ListViewItem(fieldNames);
                }
                else
                {
                    string[] fieldNames = { fileList[i + 2], fileList[i], fileList[i + 1] };
                    itm = new ListViewItem(fieldNames);
                }

                lvLines.Items.Add(itm);
            }

            if (lvLines.Items.Count > 0)
            {
                if (order == 0)
                {
                    this.chName.Text = gStr.gsField;
                    this.chName.Width = 680;

                    this.chDistance.Text = gStr.gsDistance;
                    this.chDistance.Width = 140;

                    this.chArea.Text = gStr.gsArea;
                    this.chArea.Width = 140;
                }
                else if (order == 1)
                {
                    this.chName.Text = gStr.gsDistance;
                    this.chName.Width = 140;

                    this.chDistance.Text = gStr.gsField;
                    this.chDistance.Width = 680;

                    this.chArea.Text = gStr.gsArea;
                    this.chArea.Width = 140;
                }
                else
                {
                    this.chName.Text = gStr.gsArea;
                    this.chName.Width = 140;

                    this.chDistance.Text = gStr.gsField;
                    this.chDistance.Width = 680;

                    this.chArea.Text = gStr.gsDistance;
                    this.chArea.Width = 140;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                if (lvLines.SelectedItems[0].SubItems[0].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[1].Text == "Error" ||
                    lvLines.SelectedItems[0].SubItems[2].Text == "Error")
                {
                    MessageBox.Show("This Field is Damaged, Please Delete \r\n ALREADY TOLD YOU THAT :)", gStr.gsFileError,
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (order == 0)
                    {
                        lblTemplateChosen.Text = lvLines.SelectedItems[0].SubItems[0].Text;
                        tboxFieldName.Text = lvLines.SelectedItems[0].SubItems[0].Text.Trim();
                    }
                    else
                    {
                        lblTemplateChosen.Text = lvLines.SelectedItems[0].SubItems[1].Text;
                        tboxFieldName.Text = lvLines.SelectedItems[0].SubItems[1].Text.Trim();
                    }
                    //else 
                    //{
                    //    lblTemplateChosen.Text = lvLines.SelectedItems[0].SubItems[2].Text;
                    //    tboxFieldName.Text = lvLines.SelectedItems[0].SubItems[2].Text.Trim();
                    //}
                    btnSave.Enabled = true;
                }
            }
        }

        private void btnBackSpace_MouseDown(object sender, MouseEventArgs e)
        {
            if (tboxFieldName.Text.Length > 0)
                tboxFieldName.Text = tboxFieldName.Text.Remove(tboxFieldName.Text.Length - 1, 1);
        }
    }
}