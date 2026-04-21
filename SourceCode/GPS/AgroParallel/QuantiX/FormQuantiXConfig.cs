// ============================================================================
// FormQuantiXConfig.cs - UI nativa para configurar QuantiX UDP (paso 10)
// Ubicación: SourceCode/GPS/AgroParallel/QuantiX/FormQuantiXConfig.cs
// Target: net48 (C# 7.3)
//
// Permite editar quantiX.json sin tocar el archivo a mano:
//   - Habilitar / deshabilitar
//   - Host + Puerto
//   - Sample rate (Hz)
//   - Valor "afuera"
//   - SendOnlyOnChange
//   - IncludePosition
//   - Unidad (etiqueta)
// Ademas un boton "Probar UDP" que envia un paquete sintetico sin tocar el
// sender real — sirve para validar que el listener esta escuchando.
// ============================================================================

using System;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgroParallel.QuantiX
{
    public class FormQuantiXConfig : Form
    {
        private readonly QuantiXConfig _cfg;

        private CheckBox _chkEnabled;
        private TextBox _txtHost;
        private NumericUpDown _numPort;
        private NumericUpDown _numRate;
        private NumericUpDown _numOutside;
        private CheckBox _chkOnlyChange;
        private CheckBox _chkIncludePos;
        private TextBox _txtUnit;
        private Button _btnTest;
        private Button _btnOk;
        private Button _btnCancel;
        private Label _lblStatus;

        public FormQuantiXConfig(QuantiXConfig cfg)
        {
            _cfg = cfg ?? throw new ArgumentNullException("cfg");
            BuildUI();
            LoadFromConfig();
        }

        private void BuildUI()
        {
            Text = "QuantiX — Salida UDP de dosis";
            Size = new Size(480, 430);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            Font = new Font("Segoe UI", 10f);

            int x0 = 18, labelW = 170, fieldX = 200;
            int y = 18, dy = 36;

            _chkEnabled = new CheckBox
            {
                Text = "Habilitado",
                Location = new Point(x0, y),
                AutoSize = true
            };
            Controls.Add(_chkEnabled);
            y += dy;

            AddLabel("Host (IP):", x0, y + 3, labelW);
            _txtHost = new TextBox { Location = new Point(fieldX, y), Size = new Size(230, 24) };
            Controls.Add(_txtHost);
            y += dy;

            AddLabel("Puerto:", x0, y + 3, labelW);
            _numPort = new NumericUpDown
            {
                Location = new Point(fieldX, y), Size = new Size(120, 24),
                Minimum = 1, Maximum = 65535
            };
            Controls.Add(_numPort);
            y += dy;

            AddLabel("Frecuencia (Hz):", x0, y + 3, labelW);
            _numRate = new NumericUpDown
            {
                Location = new Point(fieldX, y), Size = new Size(120, 24),
                Minimum = 0, Maximum = 20, DecimalPlaces = 1, Increment = 0.5m
            };
            Controls.Add(_numRate);
            y += dy;

            AddLabel("Valor fuera de area:", x0, y + 3, labelW);
            _numOutside = new NumericUpDown
            {
                Location = new Point(fieldX, y), Size = new Size(120, 24),
                Minimum = -1000000, Maximum = 1000000, DecimalPlaces = 3, Increment = 1m
            };
            Controls.Add(_numOutside);
            y += dy;

            AddLabel("Unidad (etiqueta):", x0, y + 3, labelW);
            _txtUnit = new TextBox { Location = new Point(fieldX, y), Size = new Size(120, 24) };
            Controls.Add(_txtUnit);
            y += dy;

            _chkOnlyChange = new CheckBox
            {
                Text = "Enviar solo al cambiar de valor",
                Location = new Point(x0, y),
                AutoSize = true
            };
            Controls.Add(_chkOnlyChange);
            y += dy - 6;

            _chkIncludePos = new CheckBox
            {
                Text = "Incluir posicion (lat/lon/heading)",
                Location = new Point(x0, y),
                AutoSize = true
            };
            Controls.Add(_chkIncludePos);
            y += dy + 4;

            _lblStatus = new Label
            {
                Location = new Point(x0, y), Size = new Size(440, 20),
                ForeColor = Color.DimGray, Text = ""
            };
            Controls.Add(_lblStatus);

            _btnTest = new Button
            {
                Text = "Probar UDP",
                Location = new Point(x0, 340), Size = new Size(120, 32)
            };
            _btnTest.Click += OnTestClick;
            Controls.Add(_btnTest);

            _btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(240, 340), Size = new Size(100, 32),
                DialogResult = DialogResult.Cancel
            };
            Controls.Add(_btnCancel);

            _btnOk = new Button
            {
                Text = "Aplicar",
                Location = new Point(348, 340), Size = new Size(100, 32),
                DialogResult = DialogResult.OK
            };
            _btnOk.Click += OnApplyClick;
            Controls.Add(_btnOk);

            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }

        private void AddLabel(string text, int x, int y, int w)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(w, 20)
            });
        }

        private void LoadFromConfig()
        {
            _chkEnabled.Checked = _cfg.Enabled;
            _txtHost.Text = _cfg.UdpHost ?? "127.0.0.1";
            _numPort.Value = Clamp(_cfg.UdpPort, 1, 65535);
            _numRate.Value = (decimal)ClampD(_cfg.SampleRateHz, 0.2, 20);
            _numOutside.Value = (decimal)_cfg.OutsideValue;
            _txtUnit.Text = _cfg.DoseUnit ?? "";
            _chkOnlyChange.Checked = _cfg.SendOnlyOnChange;
            _chkIncludePos.Checked = _cfg.IncludePosition;
        }

        private void OnApplyClick(object sender, EventArgs e)
        {
            if (!TryParseHost(_txtHost.Text, out _))
            {
                _lblStatus.ForeColor = Color.FromArgb(200, 40, 40);
                _lblStatus.Text = "Host invalido: " + _txtHost.Text;
                DialogResult = DialogResult.None;
                return;
            }

            _cfg.Enabled = _chkEnabled.Checked;
            _cfg.UdpHost = _txtHost.Text.Trim();
            _cfg.UdpPort = (int)_numPort.Value;
            _cfg.SampleRateHz = (double)_numRate.Value;
            _cfg.OutsideValue = (double)_numOutside.Value;
            _cfg.DoseUnit = _txtUnit.Text?.Trim() ?? "";
            _cfg.SendOnlyOnChange = _chkOnlyChange.Checked;
            _cfg.IncludePosition = _chkIncludePos.Checked;
            _cfg.Save();
        }

        private void OnTestClick(object sender, EventArgs e)
        {
            _lblStatus.Text = "";
            _lblStatus.ForeColor = Color.DimGray;

            if (!TryParseHost(_txtHost.Text, out IPAddress ip))
            {
                _lblStatus.ForeColor = Color.FromArgb(200, 40, 40);
                _lblStatus.Text = "Host invalido: " + _txtHost.Text;
                return;
            }

            try
            {
                using (var udp = new UdpClient())
                {
                    string json = "{\"dose\":0.0,\"inside\":false,\"field\":\"__test__\","
                        + "\"ts\":\"" + DateTime.UtcNow.ToString("o") + "\"}";
                    var bytes = Encoding.UTF8.GetBytes(json);
                    var ep = new IPEndPoint(ip, (int)_numPort.Value);
                    udp.Send(bytes, bytes.Length, ep);
                }
                _lblStatus.ForeColor = Color.FromArgb(0, 140, 0);
                _lblStatus.Text = "Paquete de prueba enviado a "
                    + _txtHost.Text + ":" + (int)_numPort.Value;
            }
            catch (Exception ex)
            {
                _lblStatus.ForeColor = Color.FromArgb(200, 40, 40);
                _lblStatus.Text = "Error: " + ex.Message;
            }
        }

        private static bool TryParseHost(string host, out IPAddress ip)
        {
            ip = null;
            if (string.IsNullOrWhiteSpace(host)) return false;
            host = host.Trim();
            if (IPAddress.TryParse(host, out ip)) return true;
            try
            {
                var addrs = Dns.GetHostAddresses(host);
                if (addrs != null && addrs.Length > 0)
                {
                    ip = addrs[0];
                    return true;
                }
            }
            catch { }
            return false;
        }

        private static decimal Clamp(int v, int lo, int hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }

        private static double ClampD(double v, double lo, double hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }
    }
}
