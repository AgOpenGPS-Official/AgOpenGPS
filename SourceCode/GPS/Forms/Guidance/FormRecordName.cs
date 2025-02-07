using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgOpenGPS.Controls;
using AgOpenGPS.Helpers;

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
            //translate all the controls
            labelEnterRecordName.Text = Culture.gStr.gsEnterRecordName;

        }

        private void FormRecordName_Load(object sender, EventArgs e)
        {
            buttonSave.Enabled = false;
            labelFilename.Text = "";
            tboxFieldName.Focus();

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
                buttonSave.Enabled = false;
            }
            else
            {
                buttonSave.Enabled = true;
            }

            labelFilename.Text = tboxFieldName.Text.Trim();
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
                ((TextBox)sender).ShowKeyboard(this);
                buttonRecordCancel.Focus();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (checkBoxRecordAddDate.Checked) labelFilename.Text += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (checkBoxRecordAddTime.Checked) labelFilename.Text += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);

            filename = labelFilename.Text;
            DialogResult = DialogResult.OK;
        }
    }
}