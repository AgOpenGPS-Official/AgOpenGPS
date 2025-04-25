using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.DrawLib;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using AgOpenGPS;
using System.IO;

namespace AgOpenGPS.Forms.Field
{
    public partial class FormFieldBrowser : Form
    {
        private readonly AgShareClient _client = new AgShareClient();
        private List<FieldDownloadDto> _fields = new List<FieldDownloadDto>();

        public FormFieldBrowser()
        {
            InitializeComponent();
        }

        private async void FormFieldBrowser_Load(object sender, EventArgs e)
        {
            _client.ApiKey = Properties.Settings.Default.AgShareApiKey;
            await LoadFieldsAsync();
        }

        private async Task LoadFieldsAsync()
        {
            lblStatus.Text = "Loading fields....";
            lblStatus.Visible = true;
            lstFields.Enabled = false;
            btnUseField.Enabled = false;

            _fields = await _client.GetOwnFieldsAsync();

            lstFields.Items.Clear();
            foreach (var field in _fields)
            {
                var item = new ListViewItem(field.Name);
                item.SubItems.Add(field.CreatedAt.ToShortDateString());
                item.Tag = field;
                lstFields.Items.Add(item);
            }

            lblStatus.Visible = false;
            lstFields.Enabled = true;
            btnUseField.Enabled = true;
        }

        private void lstFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFields.SelectedItems.Count == 0) return;

            var selectedItem = lstFields.SelectedItems[0];
            if (selectedItem.Tag is FieldDownloadDto field)
            {
                lblName.Text = field.Name;
                DrawField(field); // of renderer.Draw(field);
                glControl.SwapBuffers();
            }
        }



        private void DrawField(FieldDownloadDto field)
        {
            GL.ClearColor(0f, 0f, 0f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            // ✅ Optioneel: bereken midden voor centrering
            var allPoints = field.Boundary.Concat(field.AbLines.SelectMany(a => a.Coords)).ToList();
            if (allPoints.Count == 0) return;

            double centerLat = allPoints.Average(p => p[0]);
            double centerLon = allPoints.Average(p => p[1]);
            float scale = 10000f; // pas eventueel aan

            void Transform(double lat, double lon, out float x, out float y)
            {
                x = (float)((lon - centerLon) * scale);
                y = (float)((lat - centerLat) * scale);
            }

            // ✅ Boundary tekenen
            GL.Color3(0f, 1f, 0f); // groen
            GL.Begin(PrimitiveType.LineLoop);
            foreach (var point in field.Boundary)
            {
                Transform(point[0], point[1], out float x, out float y);
                GL.Vertex2(x, y);
            }
            GL.End();

            // ✅ AB lijnen tekenen
            foreach (var ab in field.AbLines)
            {
                GL.Color3(ab.Type == "Curve" ? 1f : 0f, 0f, 1f); // roze of blauw
                GL.Begin(PrimitiveType.LineStrip);
                foreach (var point in ab.Coords)
                {
                    Transform(point[0], point[1], out float x, out float y);
                    GL.Vertex2(x, y);
                }
                GL.End();
            }
            Console.WriteLine($"Boundary: {field.Boundary.Count} punten");
            Console.WriteLine($"ABLines: {field.AbLines.Count} lijnen");

            // ⬇️ swap via GLControl of GameWindow
            glControl.SwapBuffers();
        }

        private void btnUseField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecteer eerst een veld.");
                return;
            }

            var selectedItem = lstFields.SelectedItems[0];
            if (!(selectedItem.Tag is FieldDownloadDto selectedField))
            {
                MessageBox.Show("Kon veld niet ophalen uit lijst.");
                return;
            }

            // 🔒 Haal hoofdmap op uit RegistrySettings
            string fieldsDir = RegistrySettings.fieldsDirectory;

            // ✅ Gebruik GUID-matching om bestaande map op te sporen
            string targetPath = null;
            if (Directory.Exists(fieldsDir))
            {
                foreach (var dir in Directory.GetDirectories(fieldsDir))
                {
                    var agsharePath = Path.Combine(dir, "agshare.txt");
                    if (File.Exists(agsharePath))
                    {
                        var existingId = File.ReadAllText(agsharePath).Trim();
                        if (existingId == selectedField.Id.ToString())
                        {
                            targetPath = dir;
                            break;
                        }
                    }
                }
            }

            // ✅ Geen bestaande map? Genereer nieuwe mapnaam
            if (targetPath == null)
            {
                string safeName = string.Join("_", selectedField.Name.Split(Path.GetInvalidFileNameChars()));
                targetPath = Path.Combine(fieldsDir, safeName);
            }

            try
            {
                AgShareDownloader.BuildLocalFieldFromCloud(selectedField, targetPath);
                MessageBox.Show($"Veld '{selectedField.Name}' is lokaal opgeslagen in:\n\n{targetPath}", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij opslaan:\n" + ex.Message, "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
