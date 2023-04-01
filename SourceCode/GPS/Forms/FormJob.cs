﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJob : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormJob(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            btnJobOpen.Text = gStr.gsOpen;
            btnJobNew.Text = gStr.gsNew;
            btnJobResume.Text = gStr.gsResume;

            this.Text = gStr.gsStartNewField;
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnJobResume_Click(object sender, EventArgs e)
        {
            //open the Resume.txt and continue from last exit
            mf.FileOpenField("Resume");

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormJob_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnJobResume.Enabled = false;
            string directoryName = mf.fieldsDirectory + mf.currentFieldDirectory + "\\";

            string fileAndDirectory = directoryName + "Field.txt";

            if (!File.Exists(fileAndDirectory))
            {
                textBox1.Text = "";
                btnJobResume.Enabled = false;
                mf.currentFieldDirectory = "";


                Properties.Settings.Default.setF_CurrentDir = "";
                Properties.Settings.Default.Save();
            }
            else
            {
                textBox1.Text = mf.currentFieldDirectory;
            }

            lblLatitude.Text = mf.Latitude;
            lblLongitude.Text = mf.Longitude;

            //other sat and GPS info
            lblFixQuality.Text = mf.FixQuality;
            lblSatsTracked.Text = mf.SatsTracked;

            if (mf.isMetric)
            {
                lblAltitude.Text = mf.Altitude;
            }
            else //imperial
            {
                lblAltitude.Text = mf.AltitudeFeet;
            }
            setUseTemplate();

            mf.CloseTopMosts();
        }

        private void btnJobTouch_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";

            using (FormTouchPick form = new FormTouchPick(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnJobOpen_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";

            using (FormFilePicker form = new FormFilePicker(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnFromTemplate_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDriveInFromTemplate = !Properties.Settings.Default.setDriveInFromTemplate;
            setUseTemplate();
        }

        private void setUseTemplate()
        {
            if (!Properties.Settings.Default.setDriveInFromTemplate)
            {
                //toggle the drive in button to be standard drive in.
                btnInField.Text = "Drive In";
                btnInField.Image = Properties.Resources.AutoManualIsAuto;
                btnFromTemplate.BackgroundImage = Properties.Resources.DriveInFromTemplate;

            }
            else
            {
                //toggle the drive in button to be copy from template drive in.
                btnInField.Text = "Drive In (From Template)";
                btnInField.Image = Properties.Resources.DriveInFromTemplate;
                btnFromTemplate.BackgroundImage = Properties.Resources.AutoManualIsAuto;
            }
        }

        private void btnInField_Click(object sender, EventArgs e)
        {
            string infieldList = "";
            int numFields = 0;
            string[] dirs;

            if (Properties.Settings.Default.setDriveInFromTemplate)
            {
                dirs = Directory.GetDirectories(mf.templateFieldsDirectory);
            }
            else
            {
                dirs = Directory.GetDirectories(mf.fieldsDirectory);
            }
            

            foreach (string dir in dirs)
            {
                double lat = 0;
                double lon = 0;

                string fieldDirectory = Path.GetFileName(dir);
                string filename = dir + "\\Field.txt";
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                lat = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lon = (double.Parse(offs[1], CultureInfo.InvariantCulture));

                                double dist = GetDistance(lon, lat, mf.pn.longitude, mf.pn.latitude);

                                if (dist < 500)
                                {
                                    numFields++;
                                    if (string.IsNullOrEmpty(infieldList))
                                        infieldList += Path.GetFileName(dir);
                                    else
                                        infieldList += "," + Path.GetFileName(dir);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            FormTimedMessage form = new FormTimedMessage(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                        }
                    }



                }
            }

            if (!string.IsNullOrEmpty(infieldList))
            {
                mf.filePickerFileAndDirectory = "";

                if (numFields > 1)
                {
                    using (FormDrivePicker form = new FormDrivePicker(mf, infieldList))
                    {
                        //returns full field.txt file dir name
                        if (form.ShowDialog(this) == DialogResult.Yes)
                        {
                            if (Properties.Settings.Default.setDriveInFromTemplate)
                            {
                                char[] fielddir = mf.fieldsDirectory.ToCharArray();
                                char[] fieldfile = "\\Field.txt".ToCharArray();
                                infieldList = mf.filePickerFileAndDirectory.Substring(fielddir.Length, mf.filePickerFileAndDirectory.Length - fielddir.Length - fieldfile.Length);
                                CopyTemplateField(infieldList);
                            }

                            mf.FileOpenField(mf.filePickerFileAndDirectory);
                            Close();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else // 1 field found
                {
                    if (Properties.Settings.Default.setDriveInFromTemplate)
                    {
                        CopyTemplateField(infieldList);
                    }
                    else
                    {
                        mf.filePickerFileAndDirectory = mf.fieldsDirectory + infieldList + "\\Field.txt";
                    }
                    
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
            }
            else //no fields found
            {
                FormTimedMessage form2 = new FormTimedMessage(2000, gStr.gsNoFieldsFound, gStr.gsFieldNotOpen);
                form2.Show(this);
            }

        }

        private void CopyTemplateField(string fieldname)
        {
            string newdir = fieldname + "_" + mf.vehicleFileName + "_" + DateTime.Now.ToString("MMM.dd", CultureInfo.InvariantCulture)
    + " " + DateTime.Now.ToString("HH_mm", CultureInfo.InvariantCulture);

            Copy(mf.templateFieldsDirectory + fieldname, mf.fieldsDirectory + newdir);

            mf.filePickerFileAndDirectory = mf.fieldsDirectory + newdir + "\\Field.txt";
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            double d1 = latitude * (Math.PI / 180.0);
            double num1 = longitude * (Math.PI / 180.0);
            double d2 = otherLatitude * (Math.PI / 180.0);
            double num2 = otherLongitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private void btnFromKML_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.No;
            Close();
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}