using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using AgOpenGPS.Core.AgShare;
using System.Linq;
using AgOpenGPS.Core.Models;
using System.IO;


namespace AgOpenGPS.Forms.Field
{
    public partial class FormAgShareFields : Form
    {
        private readonly AgShareClient _agShareClient;
        private FieldDownloadDto _currentField;
        private List<FieldItem> _fields = new List<FieldItem>(); // List to store fields
       
        public FormAgShareFields(AgShareClient agShareClient)
        {
            InitializeComponent();
            _agShareClient = agShareClient;
            glControlPreview.Load += GlControlPreview_Load;
            glControlPreview.Paint += GlControlPreview_Paint;
        }

        // Load the fields in the ListBox when the Form is loaded
        private async void FormAgShareFields_Load(object sender, EventArgs e)
        {
            var fields = await _agShareClient.GetFieldsAsync();  // Haal velden op

            if (fields != null)
            {
                listViewFields.Items.Clear();  // Maak de ListView leeg

                // Voeg alleen de veldnamen toe aan de ListView
                foreach (var field in fields)
                {
                    ListViewItem item = new ListViewItem(field.Name);  // Voeg de naam toe
                    listViewFields.Items.Add(item);  // Voeg toe aan de ListView
                }
            }
            else
            {
                MessageBox.Show("Failed to load fields.");
            }
        }

        // Display the selected field name and ID in the label when the selection changes
        private void listViewFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Controleer of er een item is geselecteerd in de ListView
            if (listViewFields.SelectedItems.Count > 0)
            {
                var selectedItem = listViewFields.SelectedItems[0];  // Haal het geselecteerde item op

                // Haal de naam en ID van het geselecteerde veld
                string fieldName = selectedItem.Text;  // Dit is de veldnaam
                string fieldId = selectedItem.SubItems[1].Text;  // Dit is de ID (de tweede kolom)

                // Toont de geselecteerde veldnaam in het label
                labelSelectedField.Text = $"Selected Field: {fieldName} ({fieldId})";
            }
        }


        // Preview the selected field's boundary when the Preview button is clicked
        private async void buttonPreview_Click(object sender, EventArgs e)
        {
            if (listViewFields.SelectedItems.Count > 0)
            {
                var selectedItem = listViewFields.SelectedItems[0];  // Haal het geselecteerde item op
                string selectedFieldName = selectedItem.Text;  // Dit is de veldnaam

                // Zoek het veld op basis van de naam (gebruik de naam voor verdere verwerking)
                var selectedField = _fields.FirstOrDefault(f => f.Name == selectedFieldName);

                if (selectedField != null)
                {
                    var (success, message, field) = await _agShareClient.DownloadFieldPreviewAsync(selectedField.Id.ToString());

                    if (success)
                    {
                        _currentField = field;
                        glControlPreview.Invalidate(); // Force redraw
                    }
                    else
                    {
                        MessageBox.Show($"Failed to preview field: {message}");
                    }
                }
                else
                {
                    MessageBox.Show("Field not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a field first.");
            }
        }


        // Save the selected field locally when the Save and Use button is clicked
        private async void buttonSaveAndUse_Click(object sender, EventArgs e)
        {
            if (_currentField != null)
            {
                // Maak een Wgs84 object aan met de lat/lon van het veld
                Wgs84 origin = new Wgs84(_currentField.OriginLat, _currentField.OriginLon);

                // Maak een SharedFieldProperties object aan (indien nodig, vul de velden in)
                SharedFieldProperties sharedFieldProperties = new SharedFieldProperties
                {
                    // Vul hier de nodige eigenschappen in, bijvoorbeeld veldnaam, ID, etc.
                };

                // Maak de LocalPlane aan met de Wgs84 en SharedFieldProperties
                LocalPlane localPlane = new LocalPlane(origin, sharedFieldProperties);

                // Roep AgShareDownloader aan om het veld op te slaan met LocalPlane
                bool success = await AgShareDownloader.SaveDownloadedFieldToDiskAsync(_currentField, localPlane);

                if (success)
                {
                    MessageBox.Show($"Field '{_currentField.Name}' saved and ready to use.");

                    // Sluit het huidige formulier nadat het veld is opgeslagen
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to save the field.");
                }
            }
            else
            {
                MessageBox.Show("Please preview a field first.");
            }
        }







        // Initialize OpenGL control when it is loaded
        private void GlControlPreview_Load(object sender, EventArgs e)
        {
            glControlPreview.MakeCurrent();
            GL.ClearColor(System.Drawing.Color.Black); // Set background color
        }

        // Paint method to draw the field's boundary in the OpenGL control
        private void GlControlPreview_Paint(object sender, PaintEventArgs e)
        {
            glControlPreview.MakeCurrent();
            GL.Clear(ClearBufferMask.ColorBufferBit);  // Clear the screen

            if (_currentField == null || _currentField.ParsedBoundary == null || !_currentField.ParsedBoundary.Any())
            {
                glControlPreview.SwapBuffers();
                return; // No boundary to draw
            }

            // Find the bounding box of the boundary
            double minX = _currentField.ParsedBoundary.Min(p => p.Longitude);
            double maxX = _currentField.ParsedBoundary.Max(p => p.Longitude);
            double minY = _currentField.ParsedBoundary.Min(p => p.Latitude);
            double maxY = _currentField.ParsedBoundary.Max(p => p.Latitude);

            // Center the boundary and scale it to fit the GLControl size
            double centerX = (minX + maxX) / 2.0;
            double centerY = (minY + maxY) / 2.0;

            double scale = Math.Min(
                glControlPreview.Width / (maxX - minX + 0.0001),
                glControlPreview.Height / (maxY - minY + 0.0001)
            ) * 0.9; // Add margin

            GL.Color3(System.Drawing.Color.LimeGreen); // Set boundary color
            GL.Begin(PrimitiveType.LineLoop);

            // Draw each point of the boundary
            foreach (var point in _currentField.ParsedBoundary)
            {
                float x = (float)((point.Longitude - centerX) * scale + glControlPreview.Width / 2.0);
                float y = (float)((point.Latitude - centerY) * -scale + glControlPreview.Height / 2.0); // Invert Y axis

                GL.Vertex2(x, y);
            }

            GL.End();
            glControlPreview.SwapBuffers(); // Swap buffers to display the drawing
        }
    }

    // FieldItem class to represent the fields in the ListBox
    public class FieldItem
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

