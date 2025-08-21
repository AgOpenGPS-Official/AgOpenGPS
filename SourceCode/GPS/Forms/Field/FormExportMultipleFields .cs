using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AgOpenGPS.Protocols.ISOBUS;
using AgOpenGPS.Classes.Exporters;
using AgOpenGPS.Forms;

public partial class FormExportMultipleFields : Form
{
    // Logic is initialized in the form (no DI)
    private readonly FieldBatchExportLogic _logic = new FieldBatchExportLogic();

    public FormExportMultipleFields()
    {
        InitializeComponent();

        // Populate lists when the form is shown (controls are ready)
        this.Shown += (s, e) =>
        {
            RefreshAvailableList();
            SafeEnable(btnExportAgShare, FieldBatchExportLogic.IsAgShareEnabled());
        };

        // Wire buttons to compact helpers
        btnAllToSelected.Click += (s, e) => Transfer(listBoxAvailable, listBoxSelected, moveAll: true);
        btnAllToAvailable.Click += (s, e) => Transfer(listBoxSelected, listBoxAvailable, moveAll: true);
        btnOneToSelected.Click += (s, e) => Transfer(listBoxAvailable, listBoxSelected, moveAll: false);
        btnOneToAvailable.Click += (s, e) => Transfer(listBoxSelected, listBoxAvailable, moveAll: false);

        btnExportIsoXml.Click += (s, e) => ExportIsoXml();
        btnExportAgShare.Click += (s, e) => ExportAgShare();
        btnClose.Click += (s, e) => Close();

    }

    // -------- UI population --------

    /// <summary>
    /// Loads the available field names into the upper list.
    /// </summary>
    private void RefreshAvailableList()
    {
        TryUi(() =>
        {
            var names = _logic.LoadAvailableFieldNames() ?? new List<string>();
            ReplaceItems(listBoxAvailable, names);
        });
    }

    // -------- Transfer helpers --------

    /// <summary>
    /// Moves either all items or only the selected items from source to target.
    /// Keeps the UI lists in sync with logic output.
    /// </summary>
    private void Transfer(ListBox source, ListBox target, bool moveAll)
    {
        var fromList = GetItems(source);
        var toList = GetItems(target);

        if (moveAll)
        {
            _logic.MoveAll(fromList, toList);
        }
        else
        {
            var selected = source.SelectedItems.Cast<string>().ToList();
            _logic.MoveSelected(fromList, toList, selected);
        }

        ReplaceItems(source, fromList);
        ReplaceItems(target, toList);
    }

    // -------- Export actions --------

    /// <summary>
    /// Builds export data from selected field names and writes a single TaskData folder.
    /// </summary>
    private void ExportIsoXml()
    {
        var selected = GetItems(listBoxSelected);
        if (selected.Count == 0)
        {
            FormDialog.Show("No fields selected.", "Try again!", MessageBoxButtons.OK);
            return;
        }

        TryUi(() =>
        {
            var data = _logic.BuildExportData(selected);
            var outDir = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "TaskData_All");

            // Call multi-field exporter
            _logic.ExportIsoXmlBatch(data, outDir, ISO11783_TaskFile.Version.V4);

            FormDialog.Show($"ISOXML Export. {data.Count} fields written",
                "Export Succes!", MessageBoxButtons.OK);
        });
    }

    /// <summary>
    /// Placeholder for AgShare export. Keeps original text and behavior.
    /// </summary>
    private void ExportAgShare()
    {
        var selected = GetItems(listBoxSelected);
        if (selected.Count == 0)
        {
            FormDialog.Show("No fields selected.", "Export", MessageBoxButtons.OK);
            return;
        }

        // TODO: Replace with real AgShare batch upload
        // Untill then
        FormDialog.Show("Simulated Test To AgShare", $"{selected.Count} fields send to AgShare. Not Implemented",
            MessageBoxButtons.OK);
    }

    // -------- Small UI utilities (stateless) --------

    /// <summary>
    /// Reads all items from a ListBox as a string list.
    /// </summary>
    private static List<string> GetItems(ListBox box)
    {
        return box?.Items?.Cast<string>().ToList() ?? new List<string>();
    }

    /// <summary>
    /// Replaces all items of a ListBox with the given strings.
    /// </summary>
    private static void ReplaceItems(ListBox box, IList<string> items)
    {
        if (box == null) return;
        box.BeginUpdate();
        try
        {
            box.Items.Clear();
            if (items != null && items.Count > 0)
                box.Items.AddRange(items.Cast<object>().ToArray());
        }
        finally
        {
            box.EndUpdate();
        }
    }

    /// <summary>
    /// Wraps an action with UI wait cursor and exception dialog.
    /// </summary>
    private static void TryUi(Action action)
    {
        if (action == null) return;
        var prev = Cursor.Current;
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            action();
        }
        catch (Exception ex)
        {
            FormDialog.Show("Operation failed:\n" + ex.Message, "Error", MessageBoxButtons.OK);
        }
        finally
        {
            Cursor.Current = prev;
        }
    }

    /// <summary>
    /// Enables/disables a control safely (null-safe).
    /// </summary>
    private static void SafeEnable(Control ctrl, bool enabled)
    {
        if (ctrl != null) ctrl.Enabled = enabled;
    }
}
