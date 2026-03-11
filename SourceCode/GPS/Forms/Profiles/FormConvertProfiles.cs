using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Forms.Profiles
{
    public partial class FormConvertProfiles : Form
    {
        public FormConvertProfiles()
        {
            InitializeComponent();
        }

        private void FormConvertProfiles_Load(object sender, EventArgs e)
        {
            RefreshFileList();
            ClearDetails();
            UpdateToggleButtons();
        }

        private void RefreshFileList()
        {
            listViewFiles.Items.Clear();

            string[] files = CSettingsMigration.GetConvertibleFiles();
            foreach (string name in files.OrderBy(n => n))
            {
                listViewFiles.Items.Add(new ListViewItem(name) { Name = name });
            }

            labelStatus.Text = listViewFiles.Items.Count == 0
                ? "No old format files found."
                : $"{listViewFiles.Items.Count} old file(s) found.";

            ClearDetails();
        }

        private void ClearDetails()
        {
            textBoxVehicleName.Text = "";
            textBoxToolName.Text = "";
            textBoxEnvName.Text = "";
            vehicleEnabled = true;
            environmentEnabled = false;
            panelDetails.Enabled = false;
            buttonConvert.Enabled = false;
            UpdateToggleButtons();
        }

        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                string selected = listViewFiles.SelectedItems[0].Text;
                textBoxVehicleName.Text = selected;
                textBoxToolName.Text = selected;
                textBoxEnvName.Text = selected;
                vehicleEnabled = true;
                environmentEnabled = false;
                panelDetails.Enabled = true;
                buttonConvert.Enabled = true;
            }
            else
            {
                ClearDetails();
            }
            UpdateToggleButtons();
        }

        private void btnToggleVehicle_Click(object sender, EventArgs e)
        {
            vehicleEnabled = !vehicleEnabled;
            UpdateToggleButtons();
            UpdateConvertButton();
        }

        private void btnToggleEnvironment_Click(object sender, EventArgs e)
        {
            environmentEnabled = !environmentEnabled;
            UpdateToggleButtons();
            UpdateConvertButton();
        }

        private void UpdateToggleButtons()
        {
            // Vehicle button
            if (vehicleEnabled)
            {
                btnToggleVehicle.Text = "Vehicle: ON";
                btnToggleVehicle.BackColor = Color.LightGreen;
                textBoxVehicleName.Enabled = true;
            }
            else
            {
                btnToggleVehicle.Text = "Vehicle: OFF";
                btnToggleVehicle.BackColor = Color.LightGray;
                textBoxVehicleName.Enabled = false;
            }

            // Environment button
            if (environmentEnabled)
            {
                btnToggleEnvironment.Text = "Environment: ON";
                btnToggleEnvironment.BackColor = Color.LightGreen;
                textBoxEnvName.Enabled = true;
            }
            else
            {
                btnToggleEnvironment.Text = "Environment: OFF";
                btnToggleEnvironment.BackColor = Color.LightGray;
                textBoxEnvName.Enabled = false;
            }
        }

        private void UpdateConvertButton()
        {
            buttonConvert.Enabled = listViewFiles.SelectedItems.Count > 0
                && (!vehicleEnabled || !string.IsNullOrWhiteSpace(textBoxVehicleName.Text))
                && !string.IsNullOrWhiteSpace(textBoxToolName.Text)
                && (!environmentEnabled || !string.IsNullOrWhiteSpace(textBoxEnvName.Text));
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            var tb = (TextBox)sender;
            var pos = tb.SelectionStart;
            tb.Text = Regex.Replace(tb.Text, glm.fileRegex, "");
            tb.SelectionStart = pos;

            UpdateConvertButton();
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0) return;

            string sourceFile = listViewFiles.SelectedItems[0].Text;
            bool exportVehicle = vehicleEnabled;
            string vehicleName = exportVehicle ? textBoxVehicleName.Text.Trim() : null;
            string toolName = textBoxToolName.Text.Trim();
            bool exportEnv = environmentEnabled;
            string envName = textBoxEnvName.Text.Trim();

            if (exportVehicle && string.IsNullOrEmpty(vehicleName)) return;
            if (string.IsNullOrEmpty(toolName)) return;
            if (exportEnv && string.IsNullOrEmpty(envName)) return;

            // Check for existing files
            var overwrites = new List<string>();
            string toolPath = Path.Combine(RegistrySettings.toolsDirectory, toolName + ".xml");

            if (exportVehicle)
            {
                string vehiclePath = Path.Combine(RegistrySettings.vehiclesDirectory, vehicleName + ".xml");
                if (File.Exists(vehiclePath) && CSettingsMigration.IsSettingsType(vehiclePath, "VehicleSettings"))
                    overwrites.Add($"Vehicle: {vehicleName}");
            }
            if (File.Exists(toolPath) && CSettingsMigration.IsSettingsType(toolPath, "ToolSettings"))
                overwrites.Add($"Tool: {toolName}");
            if (exportEnv)
            {
                string envPath = Path.Combine(RegistrySettings.environmentDirectory, envName + ".xml");
                if (File.Exists(envPath))
                    overwrites.Add($"Environment: {envName}");
            }

            string confirmMsg = $"Convert '{sourceFile}' to:\n\n" +
                (exportVehicle ? $"  Vehicle: {vehicleName}\n" : "") +
                $"  Tool: {toolName}\n" +
                (exportEnv ? $"  Environment: {envName}\n" : "") +
                "\nOriginal will be backed up.";

            if (overwrites.Count > 0)
                confirmMsg += $"\n\nWARNING: These files will be overwritten:\n  " + string.Join("\n  ", overwrites);

            var confirm = FormDialog.ShowQuestion("Convert Profile", confirmMsg);
            if (confirm != DialogResult.OK) return;

            var errors = new List<string>();

            // Convert Tool FIRST: als vehicleName == sourceFile overschrijft MigrateVehicle de source,
            // waarna MigrateTool verkeerde (VehicleSettings) data inleest.
            var tSettings = new ToolSettings();
            var tResult = CSettingsMigration.MigrateTool(sourceFile, toolName, tSettings);
            if (tResult != LoadResult.Ok)
                errors.Add($"Tool: {tResult}");

            // Convert Vehicle (optional)
            if (exportVehicle)
            {
                var vSettings = new VehicleSettings();
                var vResult = CSettingsMigration.MigrateVehicle(sourceFile, vehicleName, vSettings);
                if (vResult != LoadResult.Ok)
                    errors.Add($"Vehicle: {vResult}");
            }

            // Convert Environment (optional)
            if (exportEnv)
            {
                var eResult = CSettingsMigration.MigrateEnvironment(sourceFile, envName);
                if (eResult != LoadResult.Ok)
                    errors.Add($"Environment: {eResult}");
            }

            if (errors.Count == 0)
            {
                // Backup old file
                CSettingsMigration.BackupOldFile(sourceFile);

                Log.EventWriter($"Converted '{sourceFile}' -> " +
                    (exportVehicle ? $"Vehicle:'{vehicleName}', " : "") +
                    $"Tool:'{toolName}'" +
                    (exportEnv ? $", Env:'{envName}'" : ""));

                FormDialog.Show("Conversion Complete",
                    $"'{sourceFile}' converted successfully!", DialogSeverity.Info);
            }
            else
            {
                Log.EventWriter($"Errors converting '{sourceFile}': {string.Join(", ", errors)}");
                FormDialog.Show("Conversion Errors",
                    $"Errors occurred:\n\n" + string.Join("\n", errors), DialogSeverity.Warning);
            }

            RefreshFileList();
        }
    }
}
