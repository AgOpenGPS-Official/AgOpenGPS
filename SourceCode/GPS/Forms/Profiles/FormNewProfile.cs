using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgLibrary.Forms;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Forms.Profiles
{
    public partial class FormNewProfile : Form
    {
        private const string EmptyProfile = "<Empty Profile>";
        private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
        private readonly FormGPS _formGPS;

        public FormNewProfile(FormGPS formGPS)
        {
            _formGPS = formGPS;

            InitializeComponent();
        }

        private void FormNewProfile_Load(object sender, EventArgs e)
        {
            listViewProfiles.Items.Clear();
            listViewProfiles.Items.Add(new ListViewItem(EmptyProfile) { Name = EmptyProfile });
            listViewProfiles.Items.AddRange(LoadProfiles().Select(profile => new ListViewItem(profile) { Name = profile }).ToArray());
            listViewProfiles.SelectedItems.Clear();

            ListViewItem currentProfile = listViewProfiles.Items[RegistrySettings.vehicleFileName];
            if (currentProfile != null)
            {
                currentProfile.SubItems.Add("(Current)");
                currentProfile.Selected = true;
            }
            else
            {
                // This is a fallback for the initial setup, when no profiles exist yet
                listViewProfiles.Items[0].Selected = true;
            }
        }

        private IEnumerable<string> LoadProfiles()
        {
            DirectoryInfo directory = new DirectoryInfo(RegistrySettings.vehiclesDirectory);
            FileInfo[] files = directory.GetFiles("*.xml");
            return files.Select(file => Path.GetFileNameWithoutExtension(file.Name));
        }

        private void textBoxName_Click(object sender, EventArgs e)
        {
            if (!_formGPS.isJobStarted)
            {
                if (_formGPS.isKeyboardOn)
                {
                    ((TextBox)sender).ShowKeyboard(_formGPS);
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                textBoxName.Enabled = false;
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            var cursorPosition = textBoxName.SelectionStart;
            textBoxName.Text = Regex.Replace(textBoxName.Text, glm.fileRegex, "");
            textBoxName.SelectionStart = cursorPosition;

            buttonOK.Enabled = !string.IsNullOrEmpty(textBoxName.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string newProfileName = SanitizeFileName(textBoxName.Text.Trim()).Trim();
            if (string.IsNullOrEmpty(newProfileName))
                return;

            string newProfilePath = Path.Combine(RegistrySettings.vehiclesDirectory, newProfileName + ".xml");

            if (File.Exists(newProfilePath))
            {
                var overwrite = FormDialog.Show(
                    gStr.gsSaveAndReturn,
                    $"Profile '{newProfileName}' already exists.\r\n\r\nOverwrite?",
                    MessageBoxButtons.OKCancel);

                if (overwrite != DialogResult.OK)
                    return;
            }

            if (listViewProfiles.SelectedItems.Count <= 0)
                return;

            string existingProfileName = listViewProfiles.SelectedItems[0].Name;

            if (existingProfileName.Equals(EmptyProfile))
            {
                var confirmReset = FormDialog.Show(
                    "!! WARNING !!",
                    "This will reset all Tractor measurements and control, Are you Sure??",
                    MessageBoxButtons.OKCancel);

                if (confirmReset != DialogResult.OK)
                    return;

                CreateNewEmptyProfile(newProfileName);
            }
            else if (existingProfileName.Equals(RegistrySettings.vehicleFileName))
            {
                CreateNewProfileFromCurrent(newProfileName);
            }
            else
            {
                CreateNewProfileFromExisting(newProfileName, existingProfileName);
            }
        }


        private void CreateNewEmptyProfile(string profileName)
        {
            RegistrySettings.Save(RegKeys.vehicleFileName, profileName);

            Settings.Default.Reset();
            Settings.Default.Save();

            Log.EventWriter($"New profile created: {profileName}.xml");

            _formGPS.vehicle = new CVehicle(_formGPS);
            _formGPS.tool = new CTool(_formGPS);

            _formGPS.LoadSettings();

            _formGPS.SendSettings();
            _formGPS.SendRelaySettingsToMachineModule();

            _formGPS.TimedMessageBox(2500, $"New profile '{profileName}' created", "Steer settings reset!");
        }

        private void CreateNewProfileFromCurrent(string profileName)
        {
            RegistrySettings.Save(RegKeys.vehicleFileName, profileName);

            Settings.Default.Save();
        }

        private void CreateNewProfileFromExisting(string profileName, string existingProfileName)
        {
            RegistrySettings.Save(RegKeys.vehicleFileName, existingProfileName);

            var result = Settings.Default.Load();
            if (result != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading profile {existingProfileName}.xml ({result})");

                FormDialog.Show(
                    gStr.gsError,
                    "Error loading profile " + existingProfileName + ".xml\n\nResult: " + result,
                    MessageBoxButtons.OK);
            }

            Log.EventWriter($"Profile loaded: {existingProfileName}.xml");

            _formGPS.vehicle = new CVehicle(_formGPS);
            _formGPS.tool = new CTool(_formGPS);

            _formGPS.LoadSettings();

            _formGPS.SendSettings();
            _formGPS.SendRelaySettingsToMachineModule();

            RegistrySettings.Save(RegKeys.vehicleFileName, profileName);

            Settings.Default.Save();

            _formGPS.TimedMessageBox(2500, $"New profile '{profileName}' created", "Steer settings reset!");
        }

        private static string SanitizeFileName(string fileName)
        {
            return InvalidFileRegex.Replace(fileName, string.Empty);
        }
    }
}
