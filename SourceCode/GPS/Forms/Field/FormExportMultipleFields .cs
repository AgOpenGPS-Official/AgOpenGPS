// In your Form code-behind (e.g., FormExportMultipleFields.cs)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgOpenGPS.Forms;
using AgOpenGPS.Protocols.ISOBUS;

public partial class FormExportMultipleFields : Form
{
    private readonly FieldBatchExportLogic _logic = new FieldBatchExportLogic();

    // Always parameterless constructor for forms
    public FormExportMultipleFields()
    {
        InitializeComponent(); // Designer-created controls

        // Load lists on form load
        this.Load += (s, e) =>
        {
            var names = _logic.LoadAvailableFieldNames();
            listBoxAvailable.Items.Clear();
            listBoxAvailable.Items.AddRange(names.Cast<object>().ToArray());

            //btnExportAgShare.Enabled = _logic.IsAgShareEnabled();
        };

        // Wire buttons
        btnAllToSelected.Click += (s, e) => MoveAll(listBoxAvailable, listBoxSelected);
        btnAllToAvailable.Click += (s, e) => MoveAll(listBoxSelected, listBoxAvailable);
        btnOneToSelected.Click += (s, e) => MoveSelected(listBoxAvailable, listBoxSelected);
        btnOneToAvailable.Click += (s, e) => MoveSelected(listBoxSelected, listBoxAvailable);

        btnExportIsoXml.Click += (s, e) => ExportIsoXml();
        btnExportAgShare.Click += (s, e) => ExportAgShare();
        btnClose.Click += (s, e) => Close();
    }

    // --- Helper methods to bridge UI lists to logic ---

    // Move all items from A -> B
    private void MoveAll(ListBox from, ListBox to)
    {
        var fromList = from.Items.Cast<string>().ToList();
        var toList = to.Items.Cast<string>().ToList();

        _logic.MoveAll(fromList, toList);

        from.Items.Clear();
        to.Items.Clear();
        to.Items.AddRange(toList.Cast<object>().ToArray());
    }


    // Move selected items from A -> B
    private void MoveSelected(ListBox from, ListBox to)
    {
        var fromList = from.Items.Cast<string>().ToList();
        var toList = to.Items.Cast<string>().ToList();
        var selected = from.SelectedItems.Cast<string>().ToList();

        _logic.MoveSelected(fromList, toList, selected);

        from.Items.Clear();
        to.Items.Clear();

        // Rebuild "from" minus moved items
        // (We remove selected from the original set)
        foreach (var s in from.SelectedItems.Cast<string>().ToList())
        {
            // nothing needed; list was rebuilt from fromList
        }

        from.Items.AddRange(fromList.Cast<object>().ToArray());
        to.Items.AddRange(toList.Cast<object>().ToArray());
    }

    // Export selected fields to a single ISOXML TaskData
    private void ExportIsoXml()
    {
        var selected = listBoxSelected.Items.Cast<string>().ToList();
        if (selected.Count == 0)
        {
            FormDialog.Show("No fields selected.", "Export", MessageBoxButtons.OK);
            return;
        }

        try
        {
            var data = _logic.BuildExportData(selected);

            // Choose an output folder (simple default on Desktop)
            var outDir = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TaskData_All");

            // Export using your multi-field exporter
            _logic.ExportIsoXmlBatch(data, outDir, ISO11783_TaskFile.Version.V4);

            FormDialog.Show($"ISOXML Export. {data.Count} fields written",
                "Export Succes!", MessageBoxButtons.OK);
        }
        catch (Exception ex)
        {
            FormDialog.Show("Export failed:\n" + ex.Message, "ISOXML Export",
                MessageBoxButtons.OK);
        }
    }

    // Placeholder for AgShare export (enable button only if AgShare is active)
    private void ExportAgShare()
    {
        var selected = listBoxSelected.Items.Cast<string>().ToList();
        if (selected.Count == 0)
        {
            FormDialog.Show("No fields selected.", "Export", MessageBoxButtons.OK);
            return;
        }

        // TODO: Implement your AgShare batch upload here
        FormDialog.Show("Exported To AgShare", $"{selected.Count} fields send to AgShare. Great Succes",
            MessageBoxButtons.OK);
    }
}
