using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using AgOpenGPS.Core.AgShare;
using System.Linq;
using AgOpenGPS.Core.Models;
using System.Drawing;
using AgLibrary.Logging;
using System.IO;

namespace AgOpenGPS.Forms.Field
{
    public partial class FormAgShareFields : Form
    {
        private readonly AgShareClient _agShareClient;
        private FieldDownloadDto _currentField;
        private List<FieldItem> _fields = new List<FieldItem>();
        private readonly FormGPS _gps;

        public FormAgShareFields(AgShareClient agShareClient)
        {
            InitializeComponent();
            _agShareClient = agShareClient;
            this.Load += FormAgShareFields_Load;
            glControlPreview.Load += GlControlPreview_Load;
            glControlPreview.Paint += GlControlPreview_Paint;
        }

        private async void FormAgShareFields_Load(object sender, EventArgs e)
        {

            lblLoading.Visible = true;
            listViewFields.Enabled = false;

            _fields = await _agShareClient.GetFieldsAsync();

            listViewFields.Enabled = true;

            if (_fields == null)
            {
                lblLoading.Text = "Failed to load fields. Check API Key";
                return;
            }

            if (_fields.Count == 0)
            {
                lblLoading.Text = "Connected, but no fields available.";
                return;
            }


            if (_fields != null)
            {
                listViewFields.Items.Clear();
                lblLoading.Visible = false;

                foreach (var field in _fields)
                {
                    var item = new ListViewItem(field.Name);
                    item.Tag = field;
                    listViewFields.Items.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Failed to load fields.");
            }
        }

        private async void listViewFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFields.SelectedItems.Count > 0)
            {
                var selectedItem = listViewFields.SelectedItems[0];
                if (selectedItem.Tag is FieldItem field)
                {
                    labelSelectedField.Text = $"Selected Field: {field.Name} ({field.Id})";

                    await PreviewSelectedFieldAsync(field);
                }
            }
        }

        private async Task PreviewSelectedFieldAsync(FieldItem selectedField)
        {
            var (success, message, field) = await _agShareClient.DownloadFieldPreviewAsync(selectedField.Id.ToString());

            if (success)
            {
                _currentField = field;
                glControlPreview.Invalidate();
            }
            else
            {
                Log.EventWriter($"Failed to preview field: {message}");
            }
        }


            private async void buttonSaveAndUse_Click(object sender, EventArgs e)
            {
                if (_currentField != null)
                {
                    Wgs84 origin = new Wgs84(_currentField.OriginLat, _currentField.OriginLon);

                    SharedFieldProperties sharedFieldProperties = new SharedFieldProperties();

                    LocalPlane localPlane = new LocalPlane(origin, sharedFieldProperties);

                    bool success = await AgShareDownloader.SaveDownloadedFieldToDiskAsync(_currentField);

                    if (success)
                    {
                        Log.EventWriter("AgShare - Field saved and ready to use.");

                        this.Close();
                    }
                    else
                    {
                        _gps.TimedMessageBox(2000,"Agshare","Failed to save the field.");
                    }
                }
            }

        private void GlControlPreview_Load(object sender, EventArgs e)
        {
            glControlPreview.MakeCurrent();
            GL.ClearColor(System.Drawing.Color.WhiteSmoke);
            GL.Viewport(0, 0, glControlPreview.Width, glControlPreview.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, glControlPreview.Width, glControlPreview.Height, 0, -1, 1);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }


        private void GlControlPreview_Paint(object sender, PaintEventArgs e)
        {

            glControlPreview.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, glControlPreview.Width, glControlPreview.Height);

            if (_currentField == null || _currentField.ParsedBoundary == null || !_currentField.ParsedBoundary.Any())
            {
                glControlPreview.SwapBuffers();
                return;
            }

            double minX = _currentField.ParsedBoundary.Min(p => p.Longitude);
            double maxX = _currentField.ParsedBoundary.Max(p => p.Longitude);
            double minY = _currentField.ParsedBoundary.Min(p => p.Latitude);
            double maxY = _currentField.ParsedBoundary.Max(p => p.Latitude);

            double centerX = (minX + maxX) / 2.0;
            double centerY = (minY + maxY) / 2.0;

            double scale = Math.Min(
                glControlPreview.Width / (maxX - minX + 0.0001),
                glControlPreview.Height / (maxY - minY + 0.0001)
            ) * 0.9;

            GL.Color3(System.Drawing.Color.Red);
            GL.LineWidth(5.0f);
            GL.Begin(PrimitiveType.LineLoop);

            foreach (var point in _currentField.ParsedBoundary)
            {
                float x = (float)((point.Longitude - centerX) * scale + glControlPreview.Width / 2.0);
                float y = (float)((point.Latitude - centerY) * -scale + glControlPreview.Height / 2.0);

                GL.Vertex2(x, y);
            }

            GL.End();
            if (_currentField.ParsedTracks != null)
            {
                foreach (var track in _currentField.ParsedTracks)
                {
                    if (track.Type == "AB" && track.PtA != null && track.PtB != null)
                    {
                        GL.Color3(Color.Green);
                        GL.LineWidth(3.0f);
                        GL.Begin(PrimitiveType.Lines);

                        float x1 = (float)((track.PtA.Longitude - centerX) * scale + glControlPreview.Width / 2.0);
                        float y1 = (float)((track.PtA.Latitude - centerY) * -scale + glControlPreview.Height / 2.0);
                        float x2 = (float)((track.PtB.Longitude - centerX) * scale + glControlPreview.Width / 2.0);
                        float y2 = (float)((track.PtB.Latitude - centerY) * -scale + glControlPreview.Height / 2.0);

                        GL.Vertex2(x1, y1);
                        GL.Vertex2(x2, y2);
                        GL.End();
                    }

                    if (track.Type == "Curve" && track.CurvePoints != null && track.CurvePoints.Count > 1)
                    {
                        GL.Color3(Color.Blue);
                        GL.LineWidth(3.0f);
                        GL.Begin(PrimitiveType.LineStrip);

                        foreach (var pt in track.CurvePoints)
                        {
                            float x = (float)((pt.Longitude - centerX) * scale + glControlPreview.Width / 2.0);
                            float y = (float)((pt.Latitude - centerY) * -scale + glControlPreview.Height / 2.0);
                            GL.Vertex2(x, y);
                        }

                        GL.End();
                    }
                }
            }

            glControlPreview.SwapBuffers();
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
                Log.EventWriter("GL ERROR: " + err);

        }
    }
}