using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFieldDir : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormFieldDir(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();

            labelEnterFieldName.Text = gStr.gsEnterFieldName;
            this.Text = gStr.gsCreateNewField;
        }

        private void FormFieldDir_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;

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

        private void btnAddDate_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            tboxFieldName.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //fill something in
            if (String.IsNullOrEmpty(tboxFieldName.Text.Trim()))
            {
                return;
            }

            //append date time to name

            mf.currentFieldDirectory = tboxFieldName.Text.Trim();

            //get the directory and make sure it exists, create if not
            DirectoryInfo dirNewField = new DirectoryInfo(Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory));

            mf.menustripLanguage.Enabled = false;
            //if no template set just make a new file.
            try
            {
                //start a new job
                mf.JobNew();

                //create it for first save
                if (dirNewField.Exists)
                {
                    FormDialog.Show(gStr.gsDirectoryExists, gStr.gsChooseADifferentName, MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    mf.pn.DefineLocalPlane(mf.AppModel.CurrentLatLon, false);

                    dirNewField.Create();

                    //create the field file header info
                    mf.FileCreateField();
                    mf.FileCreateSections();
                    mf.FileCreateRecPath();
                    mf.FileCreateContour();
                    mf.FileCreateElevation();
                    mf.FileSaveFlags();
                    mf.FileCreateBoundary();
                    //mf.FileSaveABLine();
                    //mf.FileSaveCurveLine();
                    //mf.FileSaveHeadland();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Creating new field " + ex);

                FormDialog.Show(gStr.gsError, ex.ToString(), MessageBoxButtons.OK);
                mf.currentFieldDirectory = "";
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
    }
}