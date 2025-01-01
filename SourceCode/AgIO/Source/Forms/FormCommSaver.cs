using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormCommSaver : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormCommSaver(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommSaver_Load(object sender, EventArgs e)
        {
            lblLast.Text = "Current " + Properties.RegistrySettings.ProfileName;
            DirectoryInfo dinfo = new DirectoryInfo(FormLoop.profileDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.xml");

            foreach (FileInfo file in Files)
            {
                string temp = Path.GetFileNameWithoutExtension(file.Name);
                cboxEnv.Items.Add(temp);
            }

            if (cboxEnv.Items.Count == 0)
            {
                cboxEnv.Enabled = false;
            }
        }

        private void cboxVeh_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(
                "Overwrite: " + cboxEnv.SelectedItem.ToString() + ".xml",
                "Save And Return",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                var newname = cboxEnv.SelectedItem.ToString().Trim();

                Properties.RegistrySettings.Save("ProfileName", newname);
                Properties.Settings.Default.Load();

                if (Properties.RegistrySettings.ProfileName == "")
                {
                    mf.YesMessageBox("Default Profile, Changes will NOT be Saved");
                }
                Close();
            }
        }

        private void tboxName_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");

            textboxSender.SelectionStart = cursorPosition;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tboxName.Text.Trim().Length > 0)
            {
                Properties.RegistrySettings.Save("ProfileName", tboxName.Text.ToString().Trim());

                if (Properties.RegistrySettings.ProfileName != "")
                    Properties.Settings.Default.Save();
                else
                    mf.YesMessageBox("Default Profile, Changes will NOT be Saved");
                Close();
            }
            else
            {
                _ = MessageBox.Show("Enter a File Name To Save...",
                "Save And Return", MessageBoxButtons.OK);
            }
        }

        private void tboxName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSave.Focus();
            }
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}