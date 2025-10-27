using AgOpenGPS.Core.Translations;
using System;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormDrivePicker : Form
    {
        private readonly FormGPS mf = null;
        private readonly ListViewItem itm;

        public FormDrivePicker(Form callingForm, string _fileList, string _distanceList)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            //translate all the controls
            this.Text = gStr.gsFieldPicker;
            btnOpenExistingLv.Text = gStr.gsUseSelected;

            // Set distance column header based on metric setting
            chDistance.Text = mf.isMetric ? "Distance (km)" : "Distance (mi)";

            string[] fileList = _fileList.Split(',');
            string[] distanceList = _distanceList.Split(',');

            for (int i = 0; i < fileList.Length; i++)
            {
                itm = new ListViewItem(fileList[i]);
                if (i < distanceList.Length)
                {
                    itm.SubItems.Add(distanceList[i]);
                }
                lvLines.Items.Add(itm);
            }
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            btnOpenExistingLv.Text = gStr.gsUseSelected;
        }

        private void btnOpenExistingLv_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                mf.filePickerFileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, lvLines.SelectedItems[0].SubItems[0].Text, "Field.txt");
                Close();
            }
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";
        }
    }
}