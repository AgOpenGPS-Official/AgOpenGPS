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
            listViewProfiles.Items.Clear();
            listViewProfiles.Items.AddRange(LoadProfiles().Select(profile => new ListViewItem(profile)).ToArray());
            listViewProfiles.SelectedItems.Clear();
        }

        private IEnumerable<string> LoadProfiles()
        {
            DirectoryInfo directory = new DirectoryInfo(RegistrySettings.vehiclesDirectory);
            FileInfo[] files = directory.GetFiles("*.xml");
            return files.Select(file => Path.GetFileNameWithoutExtension(file.Name));
        }

        private void listViewProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool profileSelected = listViewProfiles.SelectedItems.Count > 0;
            buttonOK.Enabled = profileSelected;
            buttonProfileDelete.Enabled = profileSelected;
        }

        private void buttonProfileDelete_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted) return;

            if (listViewProfiles.SelectedItems.Count <= 0) return;

            string profileName = listViewProfiles.SelectedItems[0].Text;
            if (RegistrySettings.vehicleFileName != profileName)
            {
                DialogResult result = FormDialog.Show(
                    gStr.gsSaveAndReturn,
                    $"Delete {profileName}.xml ?",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.OK)
                {
                    File.Delete(Path.Combine(RegistrySettings.vehiclesDirectory, profileName + ".XML"));
                }
            }
            else
            {
                _formGPS.TimedMessageBox(2000, "Profile currently in use", "Select different profile");
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
                DialogResult result = FormDialog.Show(
                    gStr.gsSaveAndReturn,
                    $"Load {profileName}.xml ?",
                    MessageBoxButtons.YesNo);

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
            RegistrySettings.Save(RegKeys.vehicleFileName, profileName);

            var result = Settings.Default.Load();
            if (result != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading profile {profileName}.xml ({result})");

                FormDialog.Show(
                    gStr.gsError,
                    $"Error loading profile {profileName}.xml\n\nResult: {result}",
                    MessageBoxButtons.OK);
            }

            Log.EventWriter($"Profile loaded: {profileName}.xml");

            _formGPS.vehicle = new CVehicle(_formGPS);
            _formGPS.tool = new CTool(_formGPS);

            _formGPS.LoadSettings();

            _formGPS.SendSettings();
            _formGPS.SendRelaySettingsToMachineModule();

            _formGPS.TimedMessageBox(2500, $"Profile '{profileName}' loaded", "Steer settings reset!");
        }
    }
}
