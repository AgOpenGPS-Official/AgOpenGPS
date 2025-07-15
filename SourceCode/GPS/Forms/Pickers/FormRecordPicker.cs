using AgLibrary.Logging;
using AgOpenGPS.Core.Translations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS.Forms.Pickers
{
    public partial class FormRecordPicker : Form
    {
        private readonly FormGPS mf = null;

        private readonly List<string> fileList = new List<string>();

        public FormRecordPicker(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            //translate all the controls
            this.Text = gStr.gsRecordedPathPicker;
            buttonOpenExistingLv.Text = gStr.gsUseSelected;
            labelCancel.Text = gStr.gsCancel;
            labelDeleteRecord.Text = gStr.gsDelete;
            labelPathOff.Text = gStr.gsTurnOffRecordedPath;
        }

        private void FormRecordPicker_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void LoadList()
        {
            ListViewItem itm;

            string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            string[] files = Directory.GetFiles(fieldDir);

            fileList?.Clear();
            lvLines.Items.Clear();

            // Here we use the filename of all .rec files in the current field dir.
            // The path and postfix is stripped off.

            foreach (string file in files)
            {
                if (file.EndsWith(".rec"))
                {
                    string recordName = Path.GetFileNameWithoutExtension(file);
                    itm = new ListViewItem(recordName);
                    lvLines.Items.Add(itm);
                }
            }

            if (lvLines.Items.Count == 0)
            {
                FormDialog.Show(
                    "No Recorded Paths",
                    "Create A Path First",
                    MessageBoxButtons.OK);

                Close();
            }
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                string selectedRecord = lvLines.SelectedItems[0].SubItems[0].Text;
                string selectedRecordPath = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, selectedRecord + ".rec");

                // Copy the selected record file to the original record name inside the field dir:
                // ( this will load the last selected path automatically when this field is opened again)
                File.Copy(selectedRecordPath, Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, "RecPath.txt"), true);
                // and load the selected path into the recPath object:
                string line;
                if (File.Exists(selectedRecordPath))
                {
                    using (StreamReader reader = new StreamReader(selectedRecordPath))
                    {
                        try
                        {
                            //read header
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);
                            mf.recPath.recList.Clear();

                            while (!reader.EndOfStream)
                            {
                                for (int v = 0; v < numPoints; v++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    CRecPathPt point = new CRecPathPt(
                                        double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture),
                                        double.Parse(words[3], CultureInfo.InvariantCulture),
                                        bool.Parse(words[4]));

                                    //add the point
                                    mf.recPath.recList.Add(point);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            mf.TimedMessageBox(2000, gStr.gsRecordedPathFileIsCorrupt, gStr.gsButFieldIsLoaded);
                            Log.EventWriter("Load Recorded Path" + ex.ToString());
                        }
                    }
                }
            }
        }

        private void btnDeleteField_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            string dir2Delete;
            if (count > 0)
            {
                string selectedRecord = lvLines.SelectedItems[0].SubItems[0].Text;
                dir2Delete = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory, selectedRecord + ".rec");

                // Ask confirmation before deleting the file
                DialogResult result3 = FormDialog.Show(
                    gStr.gsDeleteForSure,
                    dir2Delete,
                    MessageBoxButtons.YesNo);

                if (result3 == DialogResult.OK)
                {
                    System.IO.File.Delete(dir2Delete);
                }
                else return;

            }
            else return;

            LoadList();
        }

        private void btnTurnOffRecPath_Click(object sender, EventArgs e)
        {
            mf.recPath.StopDrivingRecordedPath();
            mf.recPath.recList.Clear();
            mf.FileSaveRecPath();
            mf.panelDrag.Visible = false;
            Close();
        }
    }
}