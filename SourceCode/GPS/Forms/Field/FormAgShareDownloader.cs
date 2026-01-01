using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AgOpenGPS.Classes.AgShare.Helpers;
using AgOpenGPS.Core.AgShare.Models;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Forms.Field
{
    /// <summary>
    /// Form that allows the user to preview and download their own AgShare fields,
    /// with OpenGL rendering of boundaries and AB lines.
    /// </summary>
    public partial class FormAgShareDownloader : Form
    {
        private readonly FormGPS gps;
        private readonly AgShareDownloader downloader;

        public FormAgShareDownloader(FormGPS gpsContext)
        {
            InitializeComponent();
            gps = gpsContext;
            downloader = new AgShareDownloader(gps.agShareClient);
            progressBarDownloadAll.Visible = false;
            lblDownloading.Visible = false;
            chkForceOverwrite.Text = gStr.gsForceOverwrite;
            btnSaveAll.Text = gStr.gsDownloadAll;
            btnGetSelected.Text = gStr.gsGetSelected;
            lblDownloading.Text = $"{gStr.gsDownloading}... {gStr.gsPleaseWait}";
            this.Text = gStr.gsAgShareDownloader;

        }

        // Called when the form loads: initialize OpenGL and load the list of available fields
        private async void FormAgShareDownloader_Load(object sender, EventArgs e)
        {
            glControl1.MakeCurrent();
            GL.ClearColor(Color.DarkSlateGray);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            glControl1.SwapBuffers();

            try
            {
                // Get user's own fields from the AgShare server
                var fields = await downloader.GetOwnFieldsAsync();

                if (fields == null)
                {
                    gps.TimedMessageBox(1000, "AgShare", "Failed to load field list.");
                    return;
                }

                lbFields.BeginUpdate();
                lbFields.Items.Clear();

                foreach (var field in fields)
                {
                    var item = new ListViewItem(field.Name) { Tag = field };
                    lbFields.Items.Add(item);
                }

                lbFields.EndUpdate();

                if (lbFields.Items.Count > 0)
                    lbFields.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                gps.TimedMessageBox(1000, "AgShare", "Failed to load field list.\n" + ex.Message);
            }
        }


        // Triggered when a user selects a field in the list; shows preview using OpenGL
        private async void lbFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0) return;

            var dto = lbFields.SelectedItems[0].Tag as GetOwnFieldDto;
            if (dto == null) return;

            lblSelectedField.Text = "Selected Field: " + dto.Name;
            lblSelectedField.ForeColor = Color.Red;

            // Download and parse field for preview
            var previewDto = await downloader.DownloadFieldPreviewAsync(dto.Id);

            if (previewDto == null)
            {
                gps.TimedMessageBox(2000, "AgShare", "Failed to download field preview. Check logs for details.");
                return;
            }

            var localModel = AgShareFieldParser.Parse(previewDto); // Already converted to NE

            RenderField(localModel);
        }


        // Called when the user clicks the "Open" button to download the field
        private async void btnOpen_Click(object sender, EventArgs e)
        {
            if (lbFields.SelectedItems.Count == 0)
            {
                gps.TimedMessageBox(1000, "AgShare", "No field selected.");
                return;
            }

            var selected = lbFields.SelectedItems[0].Tag as GetOwnFieldDto;
            if (selected == null)
            {
                gps.TimedMessageBox(1000, "AgShare", "Invalid selection.");
                return;
            }

            // Attempt to download and save field locally
            bool success = await downloader.DownloadAndSaveAsync(selected.Id);

            if (success)
            {
                gps.TimedMessageBox(2000, "AgShare", "Field downloaded and saved.");

                // Build full path to Field.txt
                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, selected.Name);
                string fieldFile = Path.Combine(fieldDir, "Field.txt");

                if (!File.Exists(fieldFile))
                {
                    gps.TimedMessageBox(2000, "AgShare", "Field saved but could not be opened (missing Field.txt).");
                    return;
                }

                // Close Current Field if necessary
                if (gps.isJobStarted)
                {
                    await gps.FileSaveEverythingBeforeClosingField();
                }

                gps.FileOpenField(fieldFile);
                Close();
            }
        }

        // Called when the "Download All" button is clicked
        private async void BtnDownloadAll_Click(object sender, EventArgs e)
        {
            // Disable UI during download
            lblDownloading.Visible = true;
            btnSaveAll.Enabled = false;
            btnClose.Enabled = false;
            btnGetSelected.Enabled = false;
            btnSaveAll.Enabled = false;
            chkForceOverwrite.Enabled = false;
            progressBarDownloadAll.Visible = true;
            progressBarDownloadAll.Value = 0;

            // Get list of fields to determine max for progress bar
            var fields = await downloader.GetOwnFieldsAsync();
            progressBarDownloadAll.Maximum = fields.Count;

            // Prepare progress reporting
            var progress = new Progress<int>(v =>
            {
                progressBarDownloadAll.Value = v;
                progressBarDownloadAll.Refresh();
            });

            // Use checkbox value to determine overwrite behavior
            bool forceOverwrite = chkForceOverwrite.Checked;

            // Start download
            var result = await downloader.DownloadAllAsync(forceOverwrite, progress);

            // Restore UI
            progressBarDownloadAll.Visible = false;
            lblDownloading.Visible = false;
            btnClose.Enabled = true;
            btnGetSelected.Enabled = true;
            btnSaveAll.Enabled = true;
            chkForceOverwrite.Enabled = true;

            // Show result
            string message = $"Downloaded {result.Downloaded} new field(s).";
            if (result.Skipped > 0)
            {
                message += $"\nSkipped {result.Skipped} existing.";
            }
            if (result.Failed > 0)
            {
                message += $"\nFailed {result.Failed} field(s).";
            }
            gps.TimedMessageBox(3000, "AgShare", message);
        }



        // Called when the user clicks the "Close" button
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region OpenGL Rendering

        // Draws the field boundaries and AB lines in the OpenGL context
        private void RenderField(ParsedField field)
        {
            glControl1.MakeCurrent();

            // Set OpenGL background
            GL.ClearColor(0.12f, 0.12f, 0.12f, 1f); // Anthracite gray
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Enable alpha blending
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            // Determine scaling based on boundary extents, or AB lines if no boundary
            GeoBoundingBox fieldBb = GetBoundingBox(field.Boundaries, field.Tracks);

            // Ensure non-zero margins even for vertical/horizontal lines or single points
            GeoDelta bbMargin = new GeoDelta(
                Math.Max(0.05 * (fieldBb.MaxNorthing - fieldBb.MinNorthing), 50),
                Math.Max(0.05 * (fieldBb.MaxEasting - fieldBb.MinEasting), 50));
            GeoBoundingBox bbWithMargin = new GeoBoundingBox(fieldBb.MinCoord - bbMargin, fieldBb.MaxCoord + bbMargin);

            // Configure orthographic projection
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(
                bbWithMargin.MinEasting, bbWithMargin.MaxEasting,
                bbWithMargin.MinNorthing, bbWithMargin.MaxNorthing, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw field boundaries in lime green
            GL.Color4(0f, 1f, 0f, 0.8f);
            foreach (var bnd in field.Boundaries)
            {
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var pt in bnd.fenceLine)
                    GL.Vertex2(pt.easting, pt.northing);
                GL.End();
            }

            // Draw AB lines and curves (dashed)
            foreach (var trk in field.Tracks)
            {
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F0F);
                GL.LineWidth(3.5f);

                if (trk.mode == TrackMode.Curve && trk.curvePts != null && trk.curvePts.Count > 0)
                {
                    // Render curve (red dashed line)
                    GL.Color4(1f, 0f, 0f, 0.9f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (var pt in trk.curvePts)
                        GL.Vertex2(pt.easting, pt.northing);
                    GL.End();
                }
                else
                {
                    // Render AB line (orange dashed line)
                    GL.Color4(1f, 0.65f, 0f, 0.9f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(trk.ptA.easting, trk.ptA.northing);
                    GL.Vertex2(trk.ptB.easting, trk.ptB.northing);
                    GL.End();
                }

                GL.Disable(EnableCap.LineStipple);
            }

            // Swap OpenGL buffers to show result
            glControl1.SwapBuffers();
        }

        private GeoBoundingBox GetBoundingBox(List<CBoundaryList> boundaries, List<CTrk> tracks)
        {
            GeoBoundingBox bb = GeoBoundingBox.CreateEmpty();

            // Check boundaries first
            if (boundaries != null)
            {
                foreach (var bnd in boundaries)
                {
                    foreach (var pt in bnd.fenceLine)
                    {
                        bb.Include(pt.ToGeoCoord());
                    }
                }
            }

            // If no boundary, use tracks to calculate bounds
            if (bb.IsEmpty && tracks != null)
            {
                foreach (var trk in tracks)
                {
                    if (trk.mode == TrackMode.Curve && trk.curvePts != null && trk.curvePts.Count > 0)
                    {
                        foreach (var pt in trk.curvePts)
                        {
                            bb.Include(pt.ToGeoCoord());
                        }
                    }
                    else
                    {
                        bb.Include(trk.ptA.ToGeoCoord());
                        bb.Include(trk.ptB.ToGeoCoord());
                    }
                }
            }

            // Fallback to default bounds if no data
            if (bb.IsEmpty)
            {
                bb.Include(new GeoCoord(-100.0, -100.0));
                bb.Include(new GeoCoord(100.0, 100.0));
            }
            return bb;
        }

        #endregion

        private void glControl1_Load(object sender, EventArgs e)
        {
        }
    }
}
