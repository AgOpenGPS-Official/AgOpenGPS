using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Forms.Profiles
{
    public partial class FormLoadProfile : Form
    {
        private readonly FormGPS _formGPS;

        public FormLoadProfile(FormGPS formGPS)
        {
            _formGPS = formGPS;

            InitializeComponent();
        }

        private void FormLoadProfile_Load(object sender, EventArgs e)
        {
            Text = "Load Environment";
            labelLoadProfile.Text = "Load Environment:";
            buttonProfileDelete.Text = gStr.gsDelete;
            buttonLoad.Text = gStr.gsLoad;
            buttonCancel.Text = gStr.gsCancel;

            listViewProfiles.Items.Clear();
            listViewProfiles.Items.AddRange(LoadProfiles().Select(profile => new ListViewItem(profile)).ToArray());
            listViewProfiles.SelectedItems.Clear();
        }

        private IEnumerable<string> LoadProfiles()
        {
            if (!Directory.Exists(RegistrySettings.environmentDirectory))
                return Enumerable.Empty<string>();

            DirectoryInfo directory = new DirectoryInfo(RegistrySettings.environmentDirectory);
            FileInfo[] files = directory.GetFiles("*.xml");
            return files.Select(file => Path.GetFileNameWithoutExtension(file.Name));
        }

        private void listViewProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool profileSelected = listViewProfiles.SelectedItems.Count > 0;
            buttonLoad.Enabled = profileSelected;
            buttonProfileDelete.Enabled = profileSelected;
        }

        private void buttonProfileDelete_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted) return;

            if (listViewProfiles.SelectedItems.Count <= 0) return;

            string profileName = listViewProfiles.SelectedItems[0].Text;
            if (RegistrySettings.environmentFileName != profileName)
            {
                DialogResult result = FormDialog.ShowQuestion(
                    gStr.gsSaveAndReturn,
                    $"Delete {profileName}.xml ?");

                if (result == DialogResult.OK)
                {
                    File.Delete(Path.Combine(RegistrySettings.environmentDirectory, profileName + ".xml"));
                }
            }
            else
            {
                FormDialog.Show("Environment currently in use", "Select different environment", DialogSeverity.Error);
            }

            listViewProfiles.Items.Clear();
            listViewProfiles.Items.AddRange(LoadProfiles().Select(profile => new ListViewItem(profile)).ToArray());
            listViewProfiles.SelectedItems.Clear();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!_formGPS.isJobStarted)
            {
                if (listViewProfiles.SelectedItems.Count <= 0) return;

                string profileName = listViewProfiles.SelectedItems[0].Text;
                DialogResult result = FormDialog.ShowQuestion(
                    gStr.gsSaveAndReturn,
                    $"Load {profileName}.xml ?");

                if (result == DialogResult.OK)
                {
                    LoadProfile(profileName);
                }
            }
            else
            {
                _formGPS.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
            }
        }

        private void LoadProfile(string profileName)
        {
            RegistrySettings.Save(RegKeys.environmentFileName, profileName);

            var result = Settings.Default.Load();
            if (result != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading environment profile {profileName}.xml ({result})");

                FormDialog.Show(
                    gStr.gsError,
                    $"Error loading environment profile {profileName}.xml\n\nResult: {result}",
                    DialogSeverity.Error);
                return;
            }

            Log.EventWriter($"Environment profile loaded: {profileName}.xml");

            _formGPS.LoadSettings();

            _formGPS.TimedMessageBox(2500, $"Profile '{profileName}' loaded", "Environment settings loaded!");
        }
    }
}
