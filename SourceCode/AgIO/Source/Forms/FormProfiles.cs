using AgIO.Controls;
using AgIO.Properties;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormProfiles : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormProfiles(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormCommSaver_Load(object sender, EventArgs e)
        {
            btnSaveAs.BackColor = Color.Transparent;
            btnSaveNewProfile.BackColor = Color.Transparent;

            lblLast.Text = "Using Profile: " + RegistrySettings.profileName;
            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.profileDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.xml");

            foreach (FileInfo file in Files)
            {
                string temp = Path.GetFileNameWithoutExtension(file.Name);
                cboxOverWrite.Items.Add(temp);
            }

            if (cboxOverWrite.Items.Count == 0)
            {
                cboxOverWrite.Enabled = false;
            }

            lblCurrentProfile.Text = RegistrySettings.profileName;

            DirectoryInfo dinfo2 = new DirectoryInfo(RegistrySettings.profileDirectory);
            FileInfo[] Files2 = dinfo2.GetFiles("*.xml");
            foreach (FileInfo file in Files2)
            {
                string temp = Path.GetFileNameWithoutExtension(file.Name);
                if (temp != RegistrySettings.profileName)
                    cboxChooseExisting.Items.Add(temp);
            }

            cboxChooseExisting.Enabled = cboxChooseExisting.Items.Count > 0;
        }

        private void cboxOverWrite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newProfile = SanitizeFileName(cboxOverWrite.SelectedItem.ToString()).Trim();
            DialogResult result3 = MessageBox.Show(
                "Overwrite: " + newProfile + ".xml",
                "Save And Return",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (result3 == DialogResult.Yes)
            {
                //save profile in registry
                RegistrySettings.Save(RegKeys.profileName, newProfile);

                Close();
            }
        }

        private void tboxNewProfile_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;
            if (textboxSender.Text.Length > 0)
            {
                btnSaveNewProfile.BackColor = Color.LightGreen;
            }
            else
            {
                btnSaveNewProfile.BackColor = Color.Transparent;
            }
        }

        private void btnSaveNewProfile_Click(object sender, EventArgs e)
        {
            string newProfile = SanitizeFileName(tboxCreateNew.Text).Trim();

            if (newProfile.Length > 0)
            {
                //save profile in registry
                RegistrySettings.Save(RegKeys.profileName, newProfile);
                
                //reset to Default Profile and save
                Settings.Default.Reset();

                DialogResult = DialogResult.Yes;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Enter a File Name To Save...",
                "Save And Return", MessageBoxButtons.OK);
            }
        }

        private void tboxNewProfile_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
                btnSaveNewProfile.Focus();
            }
        }

        //save As
        private void tboxSaveAs_TextChanged(object sender, EventArgs e)
        {
            TextBox textboxSender = (TextBox)sender;
            int cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");

            textboxSender.SelectionStart = cursorPosition;
            if (textboxSender.Text.Length > 0)
            {
                btnSaveAs.BackColor = Color.LightGreen;
            }
            else
            {
                btnSaveAs.BackColor = Color.Transparent;
            }

        }

        private void tboxSaveAs_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
                btnSaveNewProfile.Focus();
            }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            string newProfile = SanitizeFileName(tboxSaveAs.Text.ToString()).Trim();
            if (newProfile.Length > 0)
            {
                //save profile in registry
                RegistrySettings.Save(RegKeys.profileName, newProfile);

                Settings.Default.Save();

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                _ = MessageBox.Show("Enter a File Name To Save...",
                "Save And Return", MessageBoxButtons.OK);
            }
        }

        //Load Existing Profile
        private void cboxChooseExisting_SelectedIndexChanged(object sender, EventArgs e)
        {
            string newProfile = SanitizeFileName(cboxChooseExisting.SelectedItem.ToString()).Trim();
            //save current profile
            RegistrySettings.Save(RegKeys.profileName, newProfile);

            Settings.Default.Load();

            DialogResult = DialogResult.Yes;
            Close();
        }

        //functions
        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));

        public static string SanitizeFileName(string fileName)
        {
            return InvalidFileRegex.Replace(fileName, string.Empty);
        }

    }
}