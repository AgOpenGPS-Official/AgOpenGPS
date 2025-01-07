using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS.Forms
{
    public partial class FormRecordName : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public string filename = String.Empty;

        public FormRecordName(Form _callingForm)
        {
            //get copy of the calling main form
            mf = _callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormRecordName_Load(object sender, EventArgs e)
        {
            buttonSave.Enabled = false;
            lblFilename.Text = "";
            tboxFieldName.Focus();

            if (!mf.IsOnScreen(Location, Size, 1))
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
                buttonSave.Enabled = false;
            }
            else
            {
                buttonSave.Enabled = true;
            }

            lblFilename.Text = tboxFieldName.Text.Trim();
        }

        private void buttonRecordCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tboxFieldName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                buttonRecordCancel.Focus();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (checkBoxRecordAddDate.Checked) lblFilename.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (checkBoxRecordAddTime.Checked) lblFilename.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);

            filename = lblFilename.Text;
            DialogResult = DialogResult.OK;
        }
    }
}