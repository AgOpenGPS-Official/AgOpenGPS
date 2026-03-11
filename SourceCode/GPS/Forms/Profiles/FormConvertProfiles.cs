using System;
using System.Collections.Generic;
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
            checkBoxVehicle.Checked = true;
            textBoxVehicleName.Enabled = true;
            checkBoxEnvironment.Checked = false;
            textBoxEnvName.Enabled = false;
            panelDetails.Enabled = false;
            buttonConvert.Enabled = false;
        }

        private void listViewFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count > 0)
            {
                string selected = listViewFiles.SelectedItems[0].Text;
                textBoxVehicleName.Text = selected;
                textBoxToolName.Text = selected;
                textBoxEnvName.Text = selected;
                checkBoxVehicle.Checked = true;
                textBoxVehicleName.Enabled = true;
                checkBoxEnvironment.Checked = false;
                textBoxEnvName.Enabled = false;
                panelDetails.Enabled = true;
                buttonConvert.Enabled = true;
            }
            else
            {
                ClearDetails();
            }
        }

        private void checkBoxVehicle_CheckedChanged(object sender, EventArgs e)
        {
            textBoxVehicleName.Enabled = checkBoxVehicle.Checked;
            UpdateConvertButton();
        }

        private void checkBoxEnvironment_CheckedChanged(object sender, EventArgs e)
        {
            textBoxEnvName.Enabled = checkBoxEnvironment.Checked;
            UpdateConvertButton();
        }

        private void UpdateConvertButton()
        {
            buttonConvert.Enabled = listViewFiles.SelectedItems.Count > 0
                && (!checkBoxVehicle.Checked || !string.IsNullOrWhiteSpace(textBoxVehicleName.Text))
                && !string.IsNullOrWhiteSpace(textBoxToolName.Text)
                && (!checkBoxEnvironment.Checked || !string.IsNullOrWhiteSpace(textBoxEnvName.Text));
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
            bool exportVehicle = checkBoxVehicle.Checked;
            string vehicleName = exportVehicle ? textBoxVehicleName.Text.Trim() : null;
            string toolName = textBoxToolName.Text.Trim();
            bool exportEnv = checkBoxEnvironment.Checked;
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
