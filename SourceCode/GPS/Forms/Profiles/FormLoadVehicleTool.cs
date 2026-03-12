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
            RefreshVehicleList();
            RefreshToolList();

            // Pre-select and preview the currently active vehicle/tool
            if (!string.IsNullOrEmpty(RegistrySettings.vehicleFileName))
            {
                _selectedVehicle = RegistrySettings.vehicleFileName;
                LoadVehiclePreview(_selectedVehicle);
            }
            if (!string.IsNullOrEmpty(RegistrySettings.toolFileName))
            {
                _selectedTool = RegistrySettings.toolFileName;
                LoadToolPreview(_selectedTool);
            }

            UpdateCurrentLabels();
            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void UpdateCurrentLabels()
        {
            lblCurrentVehicle.Text = !string.IsNullOrEmpty(RegistrySettings.vehicleFileName)
                ? "Current Vehicle: " + RegistrySettings.vehicleFileName
                : "Current Vehicle: (none)";
            lblCurrentTool.Text = !string.IsNullOrEmpty(RegistrySettings.toolFileName)
                ? "Current Tool: " + RegistrySettings.toolFileName
                : "Current Tool: (none)";
        }

        private void UpdateSelectedLabels()
        {
            lblSelectedVehicle.Text = _selectedVehicle != null
                ? "Selected: " + _selectedVehicle
                : "Selected: -";
            lblSelectedTool.Text = _selectedTool != null
                ? "Selected: " + _selectedTool
                : "Selected: -";
        }

        #region Vehicle List

        private void RefreshVehicleList()
        {
            listViewVehicles.Items.Clear();

            foreach (string name in GetFiles(RegistrySettings.vehiclesDirectory, "VehicleSettings"))
            {
                var item = new ListViewItem(name) { Name = name };
                if (name == RegistrySettings.vehicleFileName)
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
                if (li.Text != RegistrySettings.vehicleFileName)
                    li.BackColor = listViewVehicles.BackColor;
            }

            if (listViewVehicles.SelectedItems.Count > 0)
            {
                _selectedVehicle = listViewVehicles.SelectedItems[0].Text;
                buttonDeleteVehicle.Enabled = _selectedVehicle != RegistrySettings.vehicleFileName;

                // Color selected item green (unless it's current, keep orange)
                if (_selectedVehicle != RegistrySettings.vehicleFileName)
                    listViewVehicles.SelectedItems[0].BackColor = ColorSelected;

                LoadVehiclePreview(_selectedVehicle);
            }
            else
            {
                _selectedVehicle = null;
                buttonDeleteVehicle.Enabled = false;
                ClearVehiclePreview();
            }

            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void buttonDeleteVehicle_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted || string.IsNullOrEmpty(_selectedVehicle)) return;
            if (_selectedVehicle == RegistrySettings.vehicleFileName)
            {
                FormDialog.Show("Vehicle in use", "Cannot delete the active vehicle", DialogSeverity.Error);
                return;
            }

            var result = FormDialog.ShowQuestion("Delete Vehicle",
                $"Delete vehicle '{_selectedVehicle}'?");

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

        #endregion

        #region Tool List

        private void RefreshToolList()
        {
            listViewTools.Items.Clear();

            foreach (string name in GetFiles(RegistrySettings.toolsDirectory, "ToolSettings"))
            {
                var item = new ListViewItem(name) { Name = name };
                if (name == RegistrySettings.toolFileName)
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
                if (li.Text != RegistrySettings.toolFileName)
                    li.BackColor = listViewTools.BackColor;
            }

            if (listViewTools.SelectedItems.Count > 0)
            {
                _selectedTool = listViewTools.SelectedItems[0].Text;
                buttonDeleteTool.Enabled = _selectedTool != RegistrySettings.toolFileName;

                if (_selectedTool != RegistrySettings.toolFileName)
                    listViewTools.SelectedItems[0].BackColor = ColorSelected;

                LoadToolPreview(_selectedTool);
            }
            else
            {
                _selectedTool = null;
                buttonDeleteTool.Enabled = false;
                ClearToolPreview();
            }

            UpdateSelectedLabels();
            UpdateLoadButton();
        }

        private void buttonDeleteTool_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted || string.IsNullOrEmpty(_selectedTool)) return;
            if (_selectedTool == RegistrySettings.toolFileName)
            {
                FormDialog.Show("Tool in use", "Cannot delete the active tool", DialogSeverity.Error);
                return;
            }

            var result = FormDialog.ShowQuestion("Delete Tool",
                $"Delete tool '{_selectedTool}'?");

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

        #endregion

        #region Preview

        private void LoadVehiclePreview(string name)
        {
            var preview = new VehicleSettings();
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, name + ".xml");
            if (File.Exists(path))
                XmlSettingsHandler.LoadXMLFile(path, preview);

            string type = preview.setVehicle_vehicleType == 0 ? "Tractor"
                : preview.setVehicle_vehicleType == 1 ? "Harvester" : "Articulated";

            lblVehType.Text = "Type: " + type;
            lblVehWheelbase.Text = "Wheelbase: " + preview.setVehicle_wheelbase.ToString("N2") + " m";
            lblVehAntPivot.Text = "Ant. Pivot: " + preview.setVehicle_antennaPivot.ToString("N2") + " m";
            lblVehAntOffset.Text = "Ant. Offset: " + preview.setVehicle_antennaOffset.ToString("N2") + " m";
            lblVehTrackWidth.Text = "Track Width: " + preview.setVehicle_trackWidth.ToString("N2") + " m";
        }

        private void ClearVehiclePreview()
        {
            lblVehType.Text = "Type:";
            lblVehWheelbase.Text = "Wheelbase:";
            lblVehAntPivot.Text = "Ant. Pivot:";
            lblVehAntOffset.Text = "Ant. Offset:";
            lblVehTrackWidth.Text = "Track Width:";
            lblVehHitch.Text = "Hitch:";
        }

        private void LoadToolPreview(string name)
        {
            var preview = new ToolSettings();
            string path = Path.Combine(RegistrySettings.toolsDirectory, name + ".xml");
            if (File.Exists(path))
                XmlSettingsHandler.LoadXMLFile(path, preview);

            string attach = preview.setTool_isToolFront ? "Front"
                : preview.setTool_isToolTBT ? "TBT"
                : preview.setTool_isToolRearFixed ? "Rear Fixed"
                : preview.setTool_isToolTrailing ? "Trailing" : "?";

            lblToolWidth.Text = "Width: " + preview.setVehicle_toolWidth.ToString("N2") + " m";
            lblToolOverlap.Text = "Overlap: " + preview.setVehicle_toolOverlap.ToString("N2") + " m";
            lblToolOffset.Text = "Offset: " + preview.setVehicle_toolOffset.ToString("N2") + " m";
            lblToolSections.Text = "Sections: " + preview.setVehicle_numSections.ToString();
            lblToolAttach.Text = "Attach: " + attach;
            lblToolHitch.Text = "Trail Hitch: " + preview.setVehicle_toolTrailingHitchLength.ToString("N2") + " m";
            lblVehHitch.Text = "Hitch: " + preview.setVehicle_hitchLength.ToString("N2") + " m";

        }

        private void ClearToolPreview()
        {
            lblToolWidth.Text = "Width:";
            lblToolOverlap.Text = "Overlap:";
            lblToolOffset.Text = "Offset:";
            lblToolSections.Text = "Sections:";
            lblToolAttach.Text = "Attach:";
            lblToolHitch.Text = "Trail Hitch:";
        }

        #endregion

        #region Load

        private void UpdateLoadButton()
        {
            bool vehicleChanged = _selectedVehicle != null && _selectedVehicle != RegistrySettings.vehicleFileName;
            bool toolChanged = _selectedTool != null && _selectedTool != RegistrySettings.toolFileName;
            buttonLoad.Enabled = vehicleChanged || toolChanged;
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted)
            {
                _formGPS.TimedMessageBox(2000, "Field is open", "Close field first");
                return;
            }

            bool vehicleChanged = _selectedVehicle != null && _selectedVehicle != RegistrySettings.vehicleFileName;
            bool toolChanged = _selectedTool != null && _selectedTool != RegistrySettings.toolFileName;

            if (vehicleChanged)
            {
                var result = VehicleSettings.Default.Load(_selectedVehicle);
                if (result != LoadResult.Ok)
                {
                    Log.EventWriter($"Error loading vehicle {_selectedVehicle}.xml ({result})");
                    FormDialog.Show("Error",
                        $"Error loading vehicle {_selectedVehicle}.xml\n\nResult: {result}",
                        DialogSeverity.Error);
                    return;
                }
                RegistrySettings.Save(RegKeys.vehicleFileName, _selectedVehicle);
                Log.EventWriter($"Vehicle loaded: {_selectedVehicle}");
            }

            if (toolChanged)
            {
                var result = ToolSettings.Default.Load(_selectedTool);
                if (result != LoadResult.Ok)
                {
                    Log.EventWriter($"Error loading tool {_selectedTool}.xml ({result})");
                    FormDialog.Show("Error",
                        $"Error loading tool {_selectedTool}.xml\n\nResult: {result}",
                        DialogSeverity.Error);
                    return;
                }
                RegistrySettings.Save(RegKeys.toolFileName, _selectedTool);
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

                _formGPS.TimedMessageBox(2500, "Loaded", msg);
                Close();
            }
        }

        #endregion

        #region New / Reset / Convert

        private void buttonNewVehicle_Click(object sender, EventArgs e)
        {
            string name = PromptForName("New Vehicle", "Enter vehicle name:");
            if (string.IsNullOrEmpty(name)) return;

            string path = Path.Combine(RegistrySettings.vehiclesDirectory, name + ".xml");
            if (File.Exists(path) && CSettingsMigration.IsSettingsType(path, "VehicleSettings"))
            {
                FormDialog.Show("Exists", $"Vehicle '{name}' already exists", DialogSeverity.Error);
                return;
            }

            // Copy from current
            XmlSettingsHandler.SaveXMLFile(path, VehicleSettings.Default);
            Log.EventWriter($"New vehicle created (from current): {name}");
            RefreshVehicleList();
        }

        private void buttonNewTool_Click(object sender, EventArgs e)
        {
            string name = PromptForName("New Tool", "Enter tool name:");
            if (string.IsNullOrEmpty(name)) return;

            string path = Path.Combine(RegistrySettings.toolsDirectory, name + ".xml");
            if (File.Exists(path) && CSettingsMigration.IsSettingsType(path, "ToolSettings"))
            {
                FormDialog.Show("Exists", $"Tool '{name}' already exists", DialogSeverity.Error);
                return;
            }

            // Copy from current
            XmlSettingsHandler.SaveXMLFile(path, ToolSettings.Default);
            Log.EventWriter($"New tool created (from current): {name}");
            RefreshToolList();
        }

        private void buttonResetVehicle_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted) return;
            var result = FormDialog.ShowQuestion("Reset Vehicle",
                "Reset all vehicle settings to defaults?", DialogSeverity.Warning);
            if (result != DialogResult.OK) return;

            var fresh = new VehicleSettings();
            // Copy all default values
            foreach (var field in typeof(VehicleSettings).GetFields())
            {
                if (!field.IsStatic)
                    field.SetValue(VehicleSettings.Default, field.GetValue(fresh));
            }
            VehicleSettings.Default.Save();

            _formGPS.vehicle = new CVehicle(_formGPS);
            _formGPS.LoadSettings();
            _formGPS.SetVehicleTextures();
            _formGPS.SendSettings();

            Log.EventWriter("Vehicle settings reset to defaults");
            _formGPS.TimedMessageBox(2000, "Reset", "Vehicle settings reset to defaults");
        }

        private void buttonResetTool_Click(object sender, EventArgs e)
        {
            if (_formGPS.isJobStarted) return;
            var result = FormDialog.ShowQuestion("Reset Tool",
                "Reset all tool settings to defaults?", DialogSeverity.Warning);
            if (result != DialogResult.OK) return;

            var fresh = new ToolSettings();
            foreach (var field in typeof(ToolSettings).GetFields())
            {
                if (!field.IsStatic)
                    field.SetValue(ToolSettings.Default, field.GetValue(fresh));
            }
            ToolSettings.Default.Save();

            _formGPS.tool = new CTool(_formGPS);
            _formGPS.LoadSettings();
            _formGPS.SendSettings();
            _formGPS.SendRelaySettingsToMachineModule();

            Log.EventWriter("Tool settings reset to defaults");
            _formGPS.TimedMessageBox(2000, "Reset", "Tool settings reset to defaults");
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
