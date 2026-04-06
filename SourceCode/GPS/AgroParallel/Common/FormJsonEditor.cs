// ============================================================================
// FormJsonEditor.cs - Editor visual de configuración JSON
// Ubicación: SourceCode/GPS/AgroParallel/Common/FormJsonEditor.cs
// Target: net48 (C# 7.3)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class FormJsonEditor : Form
    {
        private static readonly Color CBg      = Color.FromArgb(30, 30, 30);
        private static readonly Color CBgField = Color.FromArgb(50, 50, 55);
        private static readonly Color CText    = Color.FromArgb(240, 240, 240);
        private static readonly Color CDim     = Color.FromArgb(160, 160, 160);
        private static readonly Color CAccent  = Color.FromArgb(0, 180, 80);
        private static readonly Color CBorder  = Color.FromArgb(70, 70, 75);
        private static readonly Color CRed     = Color.FromArgb(231, 76, 60);

        private readonly string _filePath;
        private readonly Dictionary<string, Control> _editors = new Dictionary<string, Control>();
        private JsonDocument _originalDoc;

        // Drag support for custom title bar
        private bool _dragging;
        private Point _dragStart;

        public FormJsonEditor(string filePath, string title)
        {
            _filePath = filePath;
            Text = "Configuración - " + title;
            Size = new Size(520, 600);
            MinimumSize = new Size(400, 400);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            BackColor = CBg;
            Font = new Font("Segoe UI", 9f);

            BuildUI();
        }

        private void BuildUI()
        {
            // Custom title bar (no native chrome)
            var pnlTitleBar = new Panel();
            pnlTitleBar.Dock = DockStyle.Top;
            pnlTitleBar.Height = 40;
            pnlTitleBar.BackColor = Color.FromArgb(40, 40, 45);
            pnlTitleBar.Cursor = Cursors.SizeAll;
            pnlTitleBar.MouseDown += delegate (object s, MouseEventArgs me)
            {
                if (me.Button == MouseButtons.Left) { _dragging = true; _dragStart = me.Location; }
            };
            pnlTitleBar.MouseMove += delegate (object s, MouseEventArgs me)
            {
                if (_dragging) this.Location = new Point(
                    this.Location.X + me.X - _dragStart.X,
                    this.Location.Y + me.Y - _dragStart.Y);
            };
            pnlTitleBar.MouseUp += delegate { _dragging = false; };

            var lblHeader = new Label();
            lblHeader.Text = Path.GetFileName(_filePath);
            lblHeader.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblHeader.ForeColor = CText;
            lblHeader.Location = new Point(16, 10);
            lblHeader.AutoSize = true;
            lblHeader.BackColor = Color.Transparent;
            pnlTitleBar.Controls.Add(lblHeader);

            var btnX = new Button();
            btnX.Text = "✕";
            btnX.FlatStyle = FlatStyle.Flat;
            btnX.BackColor = Color.FromArgb(40, 40, 45);
            btnX.ForeColor = CDim;
            btnX.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnX.Size = new Size(36, 32);
            btnX.Location = new Point(476, 4);
            btnX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnX.Cursor = Cursors.Hand;
            btnX.FlatAppearance.BorderSize = 0;
            btnX.FlatAppearance.MouseOverBackColor = CRed;
            btnX.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            pnlTitleBar.Controls.Add(btnX);

            Controls.Add(pnlTitleBar);

            var pnlBody = new Panel();
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.AutoScroll = true;
            pnlBody.Padding = new Padding(16, 8, 16, 8);
            pnlBody.BackColor = CBg;
            Controls.Add(pnlBody);
            pnlBody.BringToFront();

            var pnlFooter = new Panel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 50;
            pnlFooter.BackColor = Color.FromArgb(40, 40, 45);

            var btnSave = new Button();
            btnSave.Text = "Guardar";
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.BackColor = CAccent;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnSave.Size = new Size(110, 34);
            btnSave.Location = new Point(280, 8);
            btnSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnSave.FlatAppearance.BorderColor = CAccent;
            btnSave.Click += OnSave;
            pnlFooter.Controls.Add(btnSave);

            var btnCancel = new Button();
            btnCancel.Text = "Cancelar";
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.BackColor = CBgField;
            btnCancel.ForeColor = CText;
            btnCancel.Size = new Size(100, 34);
            btnCancel.Location = new Point(398, 8);
            btnCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnCancel.FlatAppearance.BorderColor = CBorder;
            btnCancel.Click += delegate { DialogResult = DialogResult.Cancel; Close(); };
            pnlFooter.Controls.Add(btnCancel);

            Controls.Add(pnlFooter);

            LoadJson(pnlBody);
        }

        private void LoadJson(Panel container)
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                _originalDoc = JsonDocument.Parse(json);

                int y = 8;
                foreach (var prop in _originalDoc.RootElement.EnumerateObject())
                {
                    var lbl = new Label();
                    lbl.Text = FormatPropertyName(prop.Name);
                    lbl.Font = new Font("Segoe UI", 8.5f);
                    lbl.ForeColor = CDim;
                    lbl.Location = new Point(0, y);
                    lbl.Size = new Size(460, 18);
                    lbl.BackColor = Color.Transparent;
                    container.Controls.Add(lbl);
                    y += 20;

                    Control editor;

                    if (prop.Value.ValueKind == JsonValueKind.True || prop.Value.ValueKind == JsonValueKind.False)
                    {
                        var chk = new CheckBox();
                        chk.Checked = prop.Value.GetBoolean();
                        chk.Text = prop.Value.GetBoolean() ? "Sí" : "No";
                        chk.ForeColor = CText;
                        chk.BackColor = Color.Transparent;
                        chk.Location = new Point(0, y);
                        chk.Size = new Size(460, 24);
                        chk.FlatStyle = FlatStyle.Flat;
                        chk.CheckedChanged += delegate { chk.Text = chk.Checked ? "Sí" : "No"; };
                        editor = chk;
                        y += 28;
                    }
                    else
                    {
                        var txt = new TextBox();
                        txt.Text = prop.Value.ToString();
                        txt.BackColor = CBgField;
                        txt.ForeColor = CText;
                        txt.BorderStyle = BorderStyle.FixedSingle;
                        txt.Font = new Font("Consolas", 9.5f);
                        txt.Location = new Point(0, y);
                        txt.Size = new Size(460, 26);
                        editor = txt;
                        y += 32;
                    }

                    _editors[prop.Name] = editor;
                    container.Controls.Add(editor);
                    y += 6;
                }
            }
            catch (Exception ex)
            {
                var lbl = new Label();
                lbl.Text = "Error cargando:\n" + ex.Message;
                lbl.ForeColor = Color.FromArgb(231, 76, 60);
                lbl.Dock = DockStyle.Fill;
                container.Controls.Add(lbl);
            }
        }

        private void OnSave(object sender, EventArgs e)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true }))
                    {
                        writer.WriteStartObject();

                        foreach (var prop in _originalDoc.RootElement.EnumerateObject())
                        {
                            Control editor;
                            if (!_editors.TryGetValue(prop.Name, out editor))
                            {
                                prop.WriteTo(writer);
                                continue;
                            }

                            writer.WritePropertyName(prop.Name);

                            if (prop.Value.ValueKind == JsonValueKind.True || prop.Value.ValueKind == JsonValueKind.False)
                            {
                                writer.WriteBooleanValue(((CheckBox)editor).Checked);
                            }
                            else if (prop.Value.ValueKind == JsonValueKind.Number)
                            {
                                string numText = ((TextBox)editor).Text;
                                long lval;
                                double dval;

                                if (numText.Contains(".") || numText.Contains(","))
                                {
                                    if (double.TryParse(numText,
                                        System.Globalization.NumberStyles.Any,
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        out dval))
                                        writer.WriteNumberValue(dval);
                                    else
                                        writer.WriteStringValue(numText);
                                }
                                else
                                {
                                    if (long.TryParse(numText, out lval))
                                        writer.WriteNumberValue(lval);
                                    else
                                        writer.WriteStringValue(numText);
                                }
                            }
                            else
                            {
                                writer.WriteStringValue(((TextBox)editor).Text);
                            }
                        }
                        writer.WriteEndObject();
                    }

                    File.WriteAllText(_filePath, System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                }

                MessageBox.Show("Configuración guardada.", "AgroParallel",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando:\n" + ex.Message, "AgroParallel",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string FormatPropertyName(string name)
        {
            var result = new System.Text.StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]) && !char.IsUpper(name[i - 1]))
                    result.Append(' ');
                result.Append(name[i]);
            }
            return result.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(CBorder))
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _originalDoc != null)
                _originalDoc.Dispose();
            base.Dispose(disposing);
        }
    }
}
