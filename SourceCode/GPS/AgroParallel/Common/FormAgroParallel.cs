// ============================================================================
// FormAgroParallel.cs - Panel de configuración central de AgroParallel
// Ubicación: SourceCode/GPS/AgroParallel/Common/FormAgroParallel.cs
// Target: net48 (C# 7.3)
// ============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class FormAgroParallel : Form
    {
        private static readonly Color CBgDark    = Color.FromArgb(30, 30, 30);
        private static readonly Color CBgPanel   = Color.FromArgb(45, 45, 48);
        private static readonly Color CBgCard    = Color.FromArgb(55, 55, 60);
        private static readonly Color CAccent    = Color.FromArgb(0, 180, 80);
        private static readonly Color CTextWhite = Color.FromArgb(240, 240, 240);
        private static readonly Color CTextDim   = Color.FromArgb(160, 160, 160);
        private static readonly Color CRed       = Color.FromArgb(231, 76, 60);
        private static readonly Color COrange    = Color.FromArgb(230, 150, 30);
        private static readonly Color CBlue      = Color.FromArgb(52, 152, 219);
        private static readonly Color CBorder    = Color.FromArgb(70, 70, 75);

        private ModuleCard cardVistaX;
        private ModuleCard cardQuantiX;
        private ModuleCard cardFlowX;

        public event Action<string> OpenModulePanel;
        public event Action<string, bool> ModuleToggled;

        public FormAgroParallel()
        {
            InitForm();
            BuildUI();
            LoadModuleStates();
        }

        // Drag support for custom title bar
        private bool _dragging;
        private Point _dragStart;

        private void InitForm()
        {
            Text = "AgroParallel - Módulos";
            Size = new Size(720, 560);
            MinimumSize = new Size(600, 480);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            BackColor = CBgDark;
            Font = new Font("Segoe UI", 9.5f);
            DoubleBuffered = true;
        }

        private void BuildUI()
        {
            // Custom title bar (no native chrome)
            var pnlHeader = new Panel();
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 70;
            pnlHeader.BackColor = CBgPanel;
            pnlHeader.Cursor = Cursors.SizeAll;
            pnlHeader.Paint += delegate (object s, PaintEventArgs e)
            {
                using (var pen = new Pen(CAccent, 2))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
                // Subtle border around entire form
                using (var borderPen = new Pen(CBorder))
                    e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, 0);
            };
            pnlHeader.MouseDown += delegate (object s, MouseEventArgs me)
            {
                if (me.Button == MouseButtons.Left) { _dragging = true; _dragStart = me.Location; }
            };
            pnlHeader.MouseMove += delegate (object s, MouseEventArgs me)
            {
                if (_dragging) this.Location = new Point(
                    this.Location.X + me.X - _dragStart.X,
                    this.Location.Y + me.Y - _dragStart.Y);
            };
            pnlHeader.MouseUp += delegate { _dragging = false; };

            var lblTitle = new Label();
            lblTitle.Text = "AgroParallel";
            lblTitle.Font = new Font("Segoe UI", 18f, FontStyle.Bold);
            lblTitle.ForeColor = CTextWhite;
            lblTitle.Location = new Point(20, 10);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblTitle);

            var lblSub = new Label();
            lblSub.Text = "Módulos de agricultura de precisión para AgOpenGPS";
            lblSub.Font = new Font("Segoe UI", 9f);
            lblSub.ForeColor = CTextDim;
            lblSub.Location = new Point(22, 42);
            lblSub.AutoSize = true;
            lblSub.BackColor = Color.Transparent;
            pnlHeader.Controls.Add(lblSub);

            // Close button (custom, no native)
            var btnX = new Button();
            btnX.Text = "✕";
            btnX.FlatStyle = FlatStyle.Flat;
            btnX.BackColor = CBgPanel;
            btnX.ForeColor = CTextDim;
            btnX.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnX.Size = new Size(40, 36);
            btnX.Location = new Point(670, 4);
            btnX.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnX.Cursor = Cursors.Hand;
            btnX.FlatAppearance.BorderSize = 0;
            btnX.FlatAppearance.MouseOverBackColor = CRed;
            btnX.Click += delegate { Close(); };
            pnlHeader.Controls.Add(btnX);

            Controls.Add(pnlHeader);

            // Body
            var pnlBody = new FlowLayoutPanel();
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Padding = new Padding(20, 20, 20, 20);
            pnlBody.AutoScroll = true;
            pnlBody.BackColor = CBgDark;
            pnlBody.FlowDirection = FlowDirection.TopDown;
            pnlBody.WrapContents = false;
            Controls.Add(pnlBody);
            pnlBody.BringToFront();

            cardVistaX = CreateModuleCard("VistaX", "Monitor de semilla vía MQTT — tasa, fallas por fila, alarmas",
                CBlue, "vistaX.json", true);
            pnlBody.Controls.Add(cardVistaX);

            cardQuantiX = CreateModuleCard("QuantiX", "Dosis variable con mapas de prescripción (shapefile)",
                COrange, "quantiX.json", false);
            pnlBody.Controls.Add(cardQuantiX);

            cardFlowX = CreateModuleCard("FlowX", "Pulverización con dosis variable/fija y corte por sección",
                CAccent, "flowX.json", false);
            pnlBody.Controls.Add(cardFlowX);

            // Footer
            var pnlFooter = new Panel();
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 50;
            pnlFooter.BackColor = CBgPanel;

            var btnClose = MkButton("Cerrar", CBgCard, CTextWhite);
            btnClose.Size = new Size(100, 34);
            btnClose.Location = new Point(590, 8);
            btnClose.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            btnClose.Click += delegate { Close(); };
            pnlFooter.Controls.Add(btnClose);

            var lblVer = new Label();
            lblVer.Text = "AgroParallel v1.0.0";
            lblVer.Font = new Font("Segoe UI", 8f);
            lblVer.ForeColor = CTextDim;
            lblVer.Location = new Point(20, 16);
            lblVer.AutoSize = true;
            lblVer.BackColor = Color.Transparent;
            pnlFooter.Controls.Add(lblVer);

            Controls.Add(pnlFooter);
        }

        private ModuleCard CreateModuleCard(string name, string description,
            Color accentColor, string configFile, bool hasPanel)
        {
            var card = new ModuleCard();
            card.ModuleName = name;
            card.Size = new Size(650, 130);
            card.Margin = new Padding(0, 0, 0, 12);

            var pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.BackColor = CBgCard;
            pnl.Padding = new Padding(16);
            pnl.Paint += delegate (object s, PaintEventArgs e)
            {
                using (var brush = new SolidBrush(accentColor))
                    e.Graphics.FillRectangle(brush, 0, 0, 4, pnl.Height);
                using (var borderPen = new Pen(CBorder))
                    e.Graphics.DrawRectangle(borderPen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };

            var lblName = new Label();
            lblName.Text = name;
            lblName.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblName.ForeColor = CTextWhite;
            lblName.Location = new Point(16, 12);
            lblName.AutoSize = true;
            lblName.BackColor = Color.Transparent;
            pnl.Controls.Add(lblName);

            var chkEnabled = new CheckBox();
            chkEnabled.Text = "Habilitado";
            chkEnabled.Font = new Font("Segoe UI", 9f);
            chkEnabled.ForeColor = CTextDim;
            chkEnabled.Location = new Point(520, 16);
            chkEnabled.Size = new Size(110, 24);
            chkEnabled.BackColor = Color.Transparent;
            chkEnabled.Appearance = Appearance.Button;
            chkEnabled.FlatStyle = FlatStyle.Flat;
            chkEnabled.TextAlign = ContentAlignment.MiddleCenter;
            chkEnabled.FlatAppearance.BorderColor = CBorder;
            chkEnabled.FlatAppearance.CheckedBackColor = CAccent;
            chkEnabled.FlatAppearance.MouseOverBackColor = Color.FromArgb(65, 65, 70);
            chkEnabled.Tag = configFile;
            chkEnabled.CheckedChanged += OnModuleToggled;
            pnl.Controls.Add(chkEnabled);
            card.ChkEnabled = chkEnabled;

            var lblDesc = new Label();
            lblDesc.Text = description;
            lblDesc.Font = new Font("Segoe UI", 8.5f);
            lblDesc.ForeColor = CTextDim;
            lblDesc.Location = new Point(16, 42);
            lblDesc.Size = new Size(500, 30);
            lblDesc.BackColor = Color.Transparent;
            pnl.Controls.Add(lblDesc);

            var lblStatus = new Label();
            lblStatus.Text = "Sin iniciar";
            lblStatus.Font = new Font("Segoe UI", 8.5f);
            lblStatus.ForeColor = CTextDim;
            lblStatus.Location = new Point(16, 80);
            lblStatus.Size = new Size(200, 20);
            lblStatus.BackColor = Color.Transparent;
            pnl.Controls.Add(lblStatus);
            card.LblStatus = lblStatus;

            var btnConfig = MkButton("Configurar", CBgPanel, CTextWhite);
            btnConfig.Location = new Point(340, 76);
            btnConfig.Size = new Size(120, 30);
            btnConfig.Tag = configFile;
            btnConfig.Click += OnConfigClicked;
            pnl.Controls.Add(btnConfig);

            if (hasPanel)
            {
                var btnOpen = MkButton("Abrir Panel", accentColor, Color.White);
                btnOpen.Location = new Point(470, 76);
                btnOpen.Size = new Size(120, 30);
                btnOpen.Tag = name;
                btnOpen.Click += delegate
                {
                    var h = OpenModulePanel;
                    if (h != null) h(name);
                };
                pnl.Controls.Add(btnOpen);
                card.BtnOpenPanel = btnOpen;
            }
            else
            {
                var lblSoon = new Label();
                lblSoon.Text = "Próximamente";
                lblSoon.Font = new Font("Segoe UI", 8f, FontStyle.Italic);
                lblSoon.ForeColor = Color.FromArgb(100, 100, 100);
                lblSoon.Location = new Point(490, 82);
                lblSoon.AutoSize = true;
                lblSoon.BackColor = Color.Transparent;
                pnl.Controls.Add(lblSoon);
            }

            card.Controls.Add(pnl);
            return card;
        }

        private void OnModuleToggled(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var configFile = (string)chk.Tag;
            bool enabled = chk.Checked;

            chk.Text = enabled ? "Activo" : "Habilitado";
            chk.ForeColor = enabled ? Color.White : CTextDim;

            ToggleModuleInConfig(configFile, enabled);

            string moduleName = configFile.Replace(".json", "");
            var handler = ModuleToggled;
            if (handler != null) handler(moduleName, enabled);
        }

        private void OnConfigClicked(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var configFile = (string)btn.Tag;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    string.Format("El archivo {0} no existe.\nSe creará al habilitar el módulo.\nRuta: {1}",
                        configFile, path),
                    "AgroParallel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var editor = new FormJsonEditor(path, configFile))
            {
                if (editor.ShowDialog(this) == DialogResult.OK)
                    LoadModuleStates();
            }
        }

        private void LoadModuleStates()
        {
            LoadModuleState(cardVistaX, "vistaX.json");
            LoadModuleState(cardQuantiX, "quantiX.json");
            LoadModuleState(cardFlowX, "flowX.json");
        }

        private void LoadModuleState(ModuleCard card, string configFile)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

            if (!File.Exists(path))
            {
                card.ChkEnabled.Checked = false;
                card.LblStatus.Text = "No configurado";
                card.LblStatus.ForeColor = CTextDim;
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                using (var doc = JsonDocument.Parse(json))
                {
                    JsonElement prop;
                    bool enabled = doc.RootElement.TryGetProperty("Enabled", out prop) && prop.GetBoolean();

                    card.ChkEnabled.CheckedChanged -= OnModuleToggled;
                    card.ChkEnabled.Checked = enabled;
                    card.ChkEnabled.Text = enabled ? "Activo" : "Habilitado";
                    card.ChkEnabled.ForeColor = enabled ? Color.White : CTextDim;
                    card.ChkEnabled.CheckedChanged += OnModuleToggled;

                    card.LblStatus.Text = enabled ? "Habilitado" : "Deshabilitado";
                    card.LblStatus.ForeColor = enabled ? CAccent : CTextDim;
                }
            }
            catch
            {
                card.LblStatus.Text = "Error en config";
                card.LblStatus.ForeColor = CRed;
            }
        }

        private static void ToggleModuleInConfig(string configFile, bool enabled)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

            try
            {
                if (!File.Exists(path))
                {
                    string minimal = string.Format("{{ \"Enabled\": {0} }}", enabled ? "true" : "false");
                    File.WriteAllText(path, minimal);
                    return;
                }

                string json = File.ReadAllText(path);
                using (var doc = JsonDocument.Parse(json))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var writer = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true }))
                        {
                            writer.WriteStartObject();
                            writer.WriteBoolean("Enabled", enabled);

                            foreach (var p in doc.RootElement.EnumerateObject())
                            {
                                if (p.Name == "Enabled") continue;
                                p.WriteTo(writer);
                            }
                            writer.WriteEndObject();
                        }
                        File.WriteAllText(path, System.Text.Encoding.UTF8.GetString(ms.ToArray()));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[AgroParallel] Error toggle: " + ex.Message);
            }
        }

        public void UpdateModuleStatus(string moduleName, bool connected, string statusText)
        {
            ModuleCard card = null;
            if (moduleName == "VistaX") card = cardVistaX;
            else if (moduleName == "QuantiX") card = cardQuantiX;
            else if (moduleName == "FlowX") card = cardFlowX;

            if (card == null || card.LblStatus == null) return;

            card.LblStatus.Text = statusText;
            card.LblStatus.ForeColor = connected ? CAccent : CRed;
        }

        private Button MkButton(string text, Color bg, Color fg)
        {
            var btn = new Button();
            btn.Text = text;
            btn.FlatStyle = FlatStyle.Flat;
            btn.BackColor = bg;
            btn.ForeColor = fg;
            btn.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderColor = CBorder;
            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(Math.Min(bg.R + 20, 255), Math.Min(bg.G + 20, 255), Math.Min(bg.B + 20, 255));
            return btn;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(CBorder))
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
        }

        private class ModuleCard : Panel
        {
            public string ModuleName { get; set; }
            public CheckBox ChkEnabled { get; set; }
            public Label LblStatus { get; set; }
            public Button BtnOpenPanel { get; set; }
        }
    }
}
