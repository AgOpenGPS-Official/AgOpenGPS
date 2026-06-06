using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Forms.Profiles
{
    public partial class FormLoadVehicleTool : Form
    {
        private readonly FormGPS _formGPS;
        private string _selectedVehicle;
        private string _selectedTool;

        private static readonly Color ColorCurrent = Color.Orange;
        private static readonly Color ColorSelected = Color.LightGreen;

        public FormLoadVehicleTool(FormGPS formGPS)
        {
            _formGPS = formGPS;
            InitializeComponent();
        }

        private void FormLoadVehicleTool_Load(object sender, EventArgs e)
        {
            // Set localized form title and labels
            this.Text = gStr.gsVehicleTool;
            buttonLoad.Text = gStr.gsLoad;
            buttonCancel.Text = gStr.gsCancel;
            buttonConvertOld.Text = gStr.gsConvertOldProfiles;

            RefreshVehicleList();
            RefreshToolList();

            // Pre-select and preview the currently active vehicle/tool
            if (!string.IsNullOrEmpty(RegistrySettings.vehicleProfileName))
            {
                var vehicleItem = listViewVehicles.Items[RegistrySettings.vehicleProfileName];
                if (vehicleItem != null)
                {
                    vehicleItem.Selected = true;
                    vehicleItem.Focused = true;
                }
            }
            if (!string.IsNullOrEmpty(RegistrySettings.toolProfileName))
            {
                var toolItem = listViewTools.Items[RegistrySettings.toolProfileName];
                if (toolItem != null)
                {
                    toolItem.Selected = true;
                    toolItem.Focused = true;
                }
            }

            UpdateCurrentLabels();
            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void UpdateCurrentLabels()
        {
            lblCurrentVehicle.Text = !string.IsNullOrEmpty(RegistrySettings.vehicleProfileName)
                ? gStr.gsCurrentVehicle + ": " + RegistrySettings.vehicleProfileName +
                  (RegistrySettings.vehicleProfileLoadResult == LoadResult.Ok ? "" : " (load failed)")
                : gStr.gsCurrentVehicle + ": " + gStr.gsNone;
            lblCurrentTool.Text = !string.IsNullOrEmpty(RegistrySettings.toolProfileName)
                ? gStr.gsCurrentTool + ": " + RegistrySettings.toolProfileName +
                  (RegistrySettings.toolProfileLoadResult == LoadResult.Ok ? "" : " (load failed)")
                : gStr.gsCurrentTool + ": " + gStr.gsNone;
        }

        private void UpdateSelectedLabels()
        {
            lblSelectedVehicle.Text = _selectedVehicle != null
                ? gStr.gsSelected + ": " + _selectedVehicle
                : gStr.gsSelected + ": -";
            lblSelectedTool.Text = _selectedTool != null
                ? gStr.gsSelected + ": " + _selectedTool
                : gStr.gsSelected + ": -";
        }

        #region Vehicle List

        private void RefreshVehicleList()
        {
            listViewVehicles.Items.Clear();

            foreach (string name in GetFiles(RegistrySettings.vehiclesDirectory, "VehicleSettings"))
            {
                var item = new ListViewItem(name) { Name = name };
                if (name == RegistrySettings.vehicleProfileName)
                {
                    item.BackColor = ColorCurrent;
                    item.Font = new Font(listViewVehicles.Font, FontStyle.Bold);
                }
                listViewVehicles.Items.Add(item);
            }

            _selectedVehicle = null;
            ClearVehiclePreview();
        }

        private void listViewVehicles_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset all non-current items back to default
            foreach (ListViewItem li in listViewVehicles.Items)
            {
                if (li.Text != RegistrySettings.vehicleProfileName)
                    li.BackColor = listViewVehicles.BackColor;
            }

            if (listViewVehicles.SelectedItems.Count > 0)
            {
                _selectedVehicle = listViewVehicles.SelectedItems[0].Text;
                buttonDeleteVehicle.Enabled = _selectedVehicle != RegistrySettings.vehicleProfileName;
                buttonRenameVehicle.Enabled = true;

                // Color selected item green (unless it's current, keep orange)
                if (_selectedVehicle != RegistrySettings.vehicleProfileName)
                    listViewVehicles.SelectedItems[0].BackColor = ColorSelected;

                LoadVehiclePreview(_selectedVehicle);
            }
            else
            {
                _selectedVehicle = null;
                buttonDeleteVehicle.Enabled = false;
                buttonRenameVehicle.Enabled = false;
                ClearVehiclePreview();
            }

            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void buttonDeleteVehicle_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted || string.IsNullOrEmpty(_selectedVehicle)) return;
            if (_selectedVehicle == RegistrySettings.vehicleProfileName)
            {
                FormDialog.Show(gStr.gsVehicleInUse, gStr.gsCannotDeleteActiveVehicle, DialogSeverity.Error);
                return;
            }

            var result = FormDialog.ShowQuestion(gStr.gsDelete,
                gStr.gsDeleteVehicleConfirm + " '" + _selectedVehicle + "'?");

            if (result == DialogResult.OK)
            {
                string path = Path.Combine(RegistrySettings.vehiclesDirectory, _selectedVehicle + ".xml");
                if (File.Exists(path)) File.Delete(path);
                Log.EventWriter($"Vehicle deleted: {_selectedVehicle}");
                RefreshVehicleList();
                UpdateSelectedLabels();
                UpdateLoadButton();
            }
        }

        private void buttonRenameVehicle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedVehicle)) return;

            string oldName = _selectedVehicle;
            string newName = PromptForName(gStr.gsRenameVehicle, gStr.gsEnterNewVehicleName, oldName);
            if (string.IsNullOrEmpty(newName) || newName == oldName) return;

            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, oldName + ".xml");
            string newPath = Path.Combine(RegistrySettings.vehiclesDirectory, newName + ".xml");

            // Allow case-only renames (e.g. "tractor" -> "Tractor") — File.Exists returns true for same file on Windows
            if (File.Exists(newPath) && !string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
            {
                FormDialog.Show(gStr.gsExists, gStr.gsVehicleExists + " '" + newName + "'", DialogSeverity.Error);
                return;
            }

            File.Move(oldPath, newPath);
            Log.EventWriter($"Vehicle renamed: {oldName} -> {newName}");

            // If we renamed the active profile, update registry
            if (oldName == RegistrySettings.vehicleProfileName)
            {
                RegistrySettings.Save(RegKeys.vehicleProfileName, newName);
            }

            RefreshVehicleList();
            _selectedVehicle = null;
            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        #endregion

        #region Tool List

        private void RefreshToolList()
        {
            listViewTools.Items.Clear();

            foreach (string name in GetFiles(RegistrySettings.toolsDirectory, "ToolSettings"))
            {
                var item = new ListViewItem(name) { Name = name };
                if (name == RegistrySettings.toolProfileName)
                {
                    item.BackColor = ColorCurrent;
                    item.Font = new Font(listViewTools.Font, FontStyle.Bold);
                }
                listViewTools.Items.Add(item);
            }

            _selectedTool = null;
            ClearToolPreview();
        }

        private void listViewTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem li in listViewTools.Items)
            {
                if (li.Text != RegistrySettings.toolProfileName)
                    li.BackColor = listViewTools.BackColor;
            }

            if (listViewTools.SelectedItems.Count > 0)
            {
                _selectedTool = listViewTools.SelectedItems[0].Text;
                buttonDeleteTool.Enabled = _selectedTool != RegistrySettings.toolProfileName;
                buttonRenameTool.Enabled = true;

                if (_selectedTool != RegistrySettings.toolProfileName)
                    listViewTools.SelectedItems[0].BackColor = ColorSelected;

                LoadToolPreview(_selectedTool);
            }
            else
            {
                _selectedTool = null;
                buttonDeleteTool.Enabled = false;
                buttonRenameTool.Enabled = false;
                ClearToolPreview();
            }

            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void buttonDeleteTool_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted || string.IsNullOrEmpty(_selectedTool)) return;
            if (_selectedTool == RegistrySettings.toolProfileName)
            {
                FormDialog.Show(gStr.gsToolInUse, gStr.gsCannotDeleteActiveTool, DialogSeverity.Error);
                return;
            }

            var result = FormDialog.ShowQuestion(gStr.gsDelete,
                gStr.gsDeleteToolConfirm + " '" + _selectedTool + "'?");

            if (result == DialogResult.OK)
            {
                string path = Path.Combine(RegistrySettings.toolsDirectory, _selectedTool + ".xml");
                if (File.Exists(path)) File.Delete(path);
                Log.EventWriter($"Tool deleted: {_selectedTool}");
                RefreshToolList();
                UpdateSelectedLabels();
                UpdateLoadButton();
            }
        }

        private void buttonRenameTool_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedTool)) return;

            string oldName = _selectedTool;
            string newName = PromptForName(gStr.gsRenameTool, gStr.gsEnterNewToolName, oldName);
            if (string.IsNullOrEmpty(newName) || newName == oldName) return;

            string oldPath = Path.Combine(RegistrySettings.toolsDirectory, oldName + ".xml");
            string newPath = Path.Combine(RegistrySettings.toolsDirectory, newName + ".xml");

            // Allow case-only renames (e.g. "tool" -> "Tool") — File.Exists returns true for same file on Windows
            if (File.Exists(newPath) && !string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
            {
                FormDialog.Show(gStr.gsExists, gStr.gsToolExists + " '" + newName + "'", DialogSeverity.Error);
                return;
            }

            File.Move(oldPath, newPath);
            Log.EventWriter($"Tool renamed: {oldName} -> {newName}");

            // If we renamed the active profile, update registry
            if (oldName == RegistrySettings.toolProfileName)
            {
                RegistrySettings.Save(RegKeys.toolProfileName, newName);
            }

            RefreshToolList();
            _selectedTool = null;
            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        #endregion

        #region Preview

        private void LoadVehiclePreview(string name)
        {
            var preview = new VehicleSettings();
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, name + ".xml");
            if (File.Exists(path))
                XmlSettingsHandler.LoadXMLFile(path, preview);

            string type = preview.setVehicle_vehicleType == 0 ? gStr.gsTractor
                : preview.setVehicle_vehicleType == 1 ? gStr.gsHarvester : gStr.gsArticulated;

            lblVehType.Text = gStr.gsType + ": " + type;
            lblVehWheelbase.Text = gStr.gsWheelbase + ": " + preview.setVehicle_wheelbase.ToString("N2") + " m";
            lblVehAntPivot.Text = gStr.gsAntennaPivot + ": " + preview.setVehicle_antennaPivot.ToString("N2") + " m";
            lblVehAntOffset.Text = gStr.gsAntennaOffset + ": " + preview.setVehicle_antennaOffset.ToString("N2") + " m";
            lblVehTrackWidth.Text = gStr.gsTrackWidth + ": " + preview.setVehicle_trackWidth.ToString("N2") + " m";
        }

        private void ClearVehiclePreview()
        {
            lblVehType.Text = gStr.gsType + ":";
            lblVehWheelbase.Text = gStr.gsWheelbase + ":";
            lblVehAntPivot.Text = gStr.gsAntennaPivot + ":";
            lblVehAntOffset.Text = gStr.gsAntennaOffset + ":";
            lblVehTrackWidth.Text = gStr.gsTrackWidth + ":";
            lblVehHitch.Text = gStr.gsHitch + ":";
        }

        private void LoadToolPreview(string name)
        {
            var preview = new ToolSettings();
            string path = Path.Combine(RegistrySettings.toolsDirectory, name + ".xml");
            if (File.Exists(path))
                XmlSettingsHandler.LoadXMLFile(path, preview);

            string attach = preview.setTool_isToolFront ? gStr.gsFront
                : preview.setTool_isToolTBT ? gStr.gsTBT
                : preview.setTool_isToolRearFixed ? gStr.gsRearFixed
                : preview.setTool_isToolTrailing ? gStr.gsTrailing : "?";

            lblToolWidth.Text = gStr.gsWidth + ": " + preview.setVehicle_toolWidth.ToString("N2") + " m";
            lblToolOverlap.Text = gStr.gsOverlap + ": " + preview.setVehicle_toolOverlap.ToString("N2") + " m";
            lblToolOffset.Text = gStr.gsOffset + ": " + preview.setVehicle_toolOffset.ToString("N2") + " m";
            lblToolSections.Text = gStr.gsSections + ": " + preview.setVehicle_numSections.ToString();
            lblToolAttach.Text = gStr.gsAttach + ": " + attach;
            lblToolHitch.Text = gStr.gsTrailingHitch + ": " + preview.setVehicle_toolTrailingHitchLength.ToString("N2") + " m";
            lblVehHitch.Text = gStr.gsHitch + ": " + preview.setVehicle_hitchLength.ToString("N2") + " m";

        }

        private void ClearToolPreview()
        {
            lblToolWidth.Text = gStr.gsWidth + ":";
            lblToolOverlap.Text = gStr.gsOverlap + ":";
            lblToolOffset.Text = gStr.gsOffset + ":";
            lblToolSections.Text = gStr.gsSections + ":";
            lblToolAttach.Text = gStr.gsAttach + ":";
            lblToolHitch.Text = gStr.gsTrailingHitch + ":";
        }

        #endregion

        #region Load

        private void UpdateLoadButton()
        {
            bool vehicleChanged = _selectedVehicle != null && _selectedVehicle != RegistrySettings.vehicleProfileName;
            bool toolChanged = _selectedTool != null && _selectedTool != RegistrySettings.toolProfileName;
            buttonLoad.Enabled = vehicleChanged || toolChanged;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted)
            {
                _formGPS.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            bool vehicleChanged = _selectedVehicle != null && _selectedVehicle != RegistrySettings.vehicleProfileName;
            bool toolChanged = _selectedTool != null && _selectedTool != RegistrySettings.toolProfileName;

            // Save current profiles before switching so unsaved changes are not lost
            if (vehicleChanged) VehicleSettings.Default.Save();
            if (toolChanged) ToolSettings.Default.Save();

            if (vehicleChanged)
            {
                var result = VehicleSettings.Default.Load(_selectedVehicle);
                if (result != LoadResult.Ok)
                {
                    Log.EventWriter($"Error loading vehicle {_selectedVehicle}.xml ({result})");
                    FormDialog.Show(gStr.gsError,
                        gStr.gsErrorLoadingVehicle + " '" + _selectedVehicle + ".xml' " + Environment.NewLine + gStr.gsFileResult + ": " + result + Environment.NewLine,
                        DialogSeverity.Error);
                    return;
                }
                RegistrySettings.Save(RegKeys.vehicleProfileName, _selectedVehicle);
                Log.EventWriter($"Vehicle loaded: {_selectedVehicle}");
            }

            if (toolChanged)
            {
                var result = ToolSettings.Default.Load(_selectedTool);
                if (result != LoadResult.Ok)
                {
                    Log.EventWriter($"Error loading tool {_selectedTool}.xml ({result})");
                    FormDialog.Show(gStr.gsError,
                        gStr.gsErrorLoadingTool + " '" + _selectedTool + ".xml' " + Environment.NewLine + gStr.gsFileResult + ": " + result + Environment.NewLine,
                        DialogSeverity.Error);
                    return;
                }
                RegistrySettings.Save(RegKeys.toolProfileName, _selectedTool);
                Log.EventWriter($"Tool loaded: {_selectedTool}");
            }

            if (vehicleChanged || toolChanged)
            {
                _formGPS.vehicle = new CVehicle(_formGPS);
                _formGPS.tool = new CTool(_formGPS);

                _formGPS.LoadSettings();
                _formGPS.SetVehicleTextures();
                _formGPS.SendSettings();
                _formGPS.SendRelaySettingsToMachineModule();

                string msg = "";
                if (vehicleChanged) msg += $"Vehicle: {_selectedVehicle}";
                if (vehicleChanged && toolChanged) msg += "  |  ";
                if (toolChanged) msg += $"Tool: {_selectedTool}";

                _formGPS.TimedMessageBox(2500, gStr.gsLoad, msg);
                Close();
            }
        }

        #endregion

        #region New / Reset / Convert

        private void buttonNewVehicle_Click(object sender, EventArgs e)
        {
            string name = PromptForName(gStr.gsNewVehicle, gStr.gsEnterVehicleName);
            if (string.IsNullOrEmpty(name)) return;

            string path = Path.Combine(RegistrySettings.vehiclesDirectory, name + ".xml");
            if (File.Exists(path) && CSettingsMigration.IsSettingsType(path, "VehicleSettings"))
            {
                FormDialog.Show(gStr.gsExists, gStr.gsVehicleExists + " '" + name + "'", DialogSeverity.Error);
                return;
            }

            // Copy from current
            XmlSettingsHandler.SaveXMLFile(path, VehicleSettings.Default);
            Log.EventWriter($"New vehicle created (from current): {name}");
            RefreshVehicleList();
        }

        private void buttonNewTool_Click(object sender, EventArgs e)
        {
            string name = PromptForName(gStr.gsNewTool, gStr.gsEnterToolName);
            if (string.IsNullOrEmpty(name)) return;

            string path = Path.Combine(RegistrySettings.toolsDirectory, name + ".xml");
            if (File.Exists(path) && CSettingsMigration.IsSettingsType(path, "ToolSettings"))
            {
                FormDialog.Show(gStr.gsExists, gStr.gsToolExists + " '" + name + "'", DialogSeverity.Error);
                return;
            }

            // Copy from current
            XmlSettingsHandler.SaveXMLFile(path, ToolSettings.Default);
            Log.EventWriter($"New tool created (from current): {name}");
            RefreshToolList();
        }

        private void buttonResetVehicle_Click(object sender, EventArgs e)
        {
            var result = FormDialog.ShowQuestion(gStr.gsCreateDefaultVehicle,
                gStr.gsCreateDefaultVehicleConfirm + Environment.NewLine + Environment.NewLine + "This will create and load the new profile.",
                DialogSeverity.Info);
            if (result != DialogResult.OK) return;

            string defaultName = "Default";
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, defaultName + ".xml");

            if (File.Exists(path))
            {
                var overwrite = FormDialog.ShowQuestion(gStr.gsOverwrite,
                    gStr.gsProfileExistsOverwrite + " '" + defaultName + "'?", DialogSeverity.Warning);
                if (overwrite != DialogResult.OK) return;
            }

            var fresh = new VehicleSettings();
            XmlSettingsHandler.SaveXMLFile(path, fresh);
            Log.EventWriter($"Default vehicle profile created: {defaultName}");

            // Load the Default profile
            var loadResult = VehicleSettings.Default.Load(defaultName);
            if (loadResult != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading vehicle {defaultName}.xml ({loadResult})");
                FormDialog.Show(gStr.gsError,
                    gStr.gsErrorLoadingVehicle + " '" + defaultName + ".xml' " + Environment.NewLine + gStr.gsFileResult + ": " + loadResult,
                    DialogSeverity.Error);
                RefreshVehicleList();
                return;
            }

            RegistrySettings.Save(RegKeys.vehicleProfileName, defaultName);
            Log.EventWriter($"Vehicle loaded: {defaultName}");

            _formGPS.vehicle = new CVehicle(_formGPS);
            _formGPS.LoadSettings();
            _formGPS.SetVehicleTextures();
            _formGPS.SendSettings();

            RefreshVehicleList();
            _formGPS.TimedMessageBox(2500, gStr.gsLoad, gStr.gsDefaultVehicleLoaded);
        }

        private void buttonResetTool_Click(object sender, EventArgs e)
        {
            var result = FormDialog.ShowQuestion(gStr.gsCreateDefaultTool,
                gStr.gsCreateDefaultToolConfirm + Environment.NewLine + Environment.NewLine + "This will create and load the new profile.",
                DialogSeverity.Info);
            if (result != DialogResult.OK) return;

            string defaultName = "Default";
            string path = Path.Combine(RegistrySettings.toolsDirectory, defaultName + ".xml");

            if (File.Exists(path))
            {
                var overwrite = FormDialog.ShowQuestion(gStr.gsOverwrite,
                    gStr.gsProfileExistsOverwrite + " '" + defaultName + "'?", DialogSeverity.Warning);
                if (overwrite != DialogResult.OK) return;
            }

            var fresh = new ToolSettings();
            XmlSettingsHandler.SaveXMLFile(path, fresh);
            Log.EventWriter($"Default tool profile created: {defaultName}");

            // Load the Default profile
            var loadResult = ToolSettings.Default.Load(defaultName);
            if (loadResult != LoadResult.Ok)
            {
                Log.EventWriter($"Error loading tool {defaultName}.xml ({loadResult})");
                FormDialog.Show(gStr.gsError,
                    gStr.gsErrorLoadingTool + " '" + defaultName + ".xml' " + Environment.NewLine + gStr.gsFileResult + ": " + loadResult,
                    DialogSeverity.Error);
                RefreshToolList();
                return;
            }

            RegistrySettings.Save(RegKeys.toolProfileName, defaultName);
            Log.EventWriter($"Tool loaded: {defaultName}");

            _formGPS.tool = new CTool(_formGPS);
            _formGPS.LoadSettings();
            _formGPS.SendSettings();
            _formGPS.SendRelaySettingsToMachineModule();

            RefreshToolList();
            _formGPS.TimedMessageBox(2500, gStr.gsLoad, gStr.gsDefaultToolLoaded);
        }

        private void buttonConvertOld_Click(object sender, EventArgs e)
        {
            using (var form = new FormConvertProfiles(_formGPS))
            {
                form.ShowDialog(this);
            }
            RefreshVehicleList();
            RefreshToolList();
        }

        #endregion

        #region Helpers

        private string PromptForName(string title, string prompt)
        {
            return AgOpenGPS.Forms.FormInputDialog.ShowInput(title, prompt, _formGPS);
        }

        private string PromptForName(string title, string prompt, string defaultValue)
        {
            return AgOpenGPS.Forms.FormInputDialog.ShowInput(title, prompt, _formGPS, defaultValue);
        }

        private IEnumerable<string> GetFiles(string directory, string expectedType)
        {
            if (!Directory.Exists(directory))
                return Enumerable.Empty<string>();

            return new DirectoryInfo(directory)
                .GetFiles("*.xml")
                .Where(f => CSettingsMigration.IsSettingsType(f.FullName, expectedType))
                .Select(f => Path.GetFileNameWithoutExtension(f.Name))
                .OrderBy(n => n);
        }

        #endregion
    }
}
