using System;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormDrivePicker : Form
    {
        private readonly FormGPS mf = null;
        private readonly ListViewItem itm;

        public FormDrivePicker(Form callingForm, string _fileList)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            //translate all the controls
            this.Text = Culture.gStr.gsFieldPicker;
            btnOpenExistingLv.Text = Culture.gStr.gsUseSelected;

            string[] fileList = _fileList.Split(',');
            for (int i = 0; i < fileList.Length; i++)
            {
                itm = new ListViewItem(fileList[i]);
                lvLines.Items.Add(itm);
            }
        }

        private void FormFilePicker_Load(object sender, EventArgs e)
        {
            btnOpenExistingLv.Text = Culture.gStr.gsUseSelected;
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