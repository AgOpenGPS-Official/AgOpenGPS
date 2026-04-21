// ============================================================================
// QuantiXSender.cs - Timer + UDP client para enviar la dosis muestreada
// Ubicación: SourceCode/GPS/AgroParallel/QuantiX/QuantiXSender.cs
// Target: net48 (C# 7.3)
//
// Paso 9B: lee CurrentDose/CurrentInside del ShapefileLayer (actualizado en
// cada frame por el sampling de paso 9A) y lo manda como JSON via UDP al
// host:port configurados en quantiX.json.
//
// El sender corre sobre un System.Windows.Forms.Timer (hilo UI) porque el
// payload es chico y el ritmo tipico (~5 Hz) no requiere hilo separado.
// ============================================================================

using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgroParallel.QuantiX
{
    public class QuantiXSender : IDisposable
    {
        private readonly QuantiXConfig _cfg;
        private readonly UdpClient _client;
        private readonly IPEndPoint _endpoint;
        private readonly Timer _timer;
        private readonly Func<QuantiXSample> _sampleProvider;

        private double _lastSentDose = double.NaN;
        private bool _lastSentInside;
        private bool _hasSent;

        public bool IsRunning { get; private set; }
        public int PacketsSent { get; private set; }
        public DateTime? LastSendUtc { get; private set; }

        public QuantiXSender(QuantiXConfig cfg, Func<QuantiXSample> sampleProvider)
        {
            if (cfg == null) throw new ArgumentNullException("cfg");
            if (sampleProvider == null) throw new ArgumentNullException("sampleProvider");

            _cfg = cfg;
            _sampleProvider = sampleProvider;

            _client = new UdpClient();
            IPAddress ip;
            if (!IPAddress.TryParse(cfg.UdpHost, out ip))
            {
                try { ip = Dns.GetHostAddresses(cfg.UdpHost)[0]; }
                catch { ip = IPAddress.Loopback; }
            }
            _endpoint = new IPEndPoint(ip, cfg.UdpPort);

            double rate = cfg.SampleRateHz <= 0 ? 5.0 : cfg.SampleRateHz;
            int interval = (int)Math.Round(1000.0 / rate);
            if (interval < 50) interval = 50;       // minimo 50ms para no saturar la UI.
            if (interval > 5000) interval = 5000;   // maximo 5s.

            _timer = new Timer();
            _timer.Interval = interval;
            _timer.Tick += OnTick;
        }

        public void Start()
        {
            if (IsRunning || !_cfg.Enabled) return;
            _timer.Start();
            IsRunning = true;
            System.Diagnostics.Debug.WriteLine("[QuantiX] Start → "
                + _endpoint + " @ " + _timer.Interval + "ms");
        }

        public void Stop()
        {
            if (!IsRunning) return;
            _timer.Stop();
            IsRunning = false;
            System.Diagnostics.Debug.WriteLine("[QuantiX] Stop (" + PacketsSent + " paquetes)");
        }

        private void OnTick(object s, EventArgs e)
        {
            try
            {
                var p = _sampleProvider();
                if (p == null) return;

                // Si el tractor esta afuera, usamos el OutsideValue como dosis.
                double dose = p.Inside ? p.Dose : _cfg.OutsideValue;

                if (_cfg.SendOnlyOnChange
                    && _hasSent
                    && p.Inside == _lastSentInside
                    && Math.Abs(dose - _lastSentDose) < 1e-9)
                {
                    return;
                }

                string json = BuildJson(p, dose);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                _client.Send(bytes, bytes.Length, _endpoint);

                _lastSentDose = dose;
                _lastSentInside = p.Inside;
                _hasSent = true;

                PacketsSent++;
                LastSendUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] OnTick: " + ex.Message);
            }
        }

        private string BuildJson(QuantiXSample p, double dose)
        {
            var sb = new StringBuilder(256);
            sb.Append('{');
            sb.Append("\"dose\":").Append(dose.ToString("G17", CultureInfo.InvariantCulture));
            sb.Append(",\"inside\":").Append(p.Inside ? "true" : "false");
            if (!string.IsNullOrEmpty(p.FieldName))
                sb.Append(",\"field\":\"").Append(Escape(p.FieldName)).Append('"');
            if (!string.IsNullOrEmpty(_cfg.DoseUnit))
                sb.Append(",\"unit\":\"").Append(Escape(_cfg.DoseUnit)).Append('"');

            if (_cfg.IncludePosition)
            {
                if (p.Latitude.HasValue)
                    sb.Append(",\"lat\":").Append(p.Latitude.Value.ToString("G17", CultureInfo.InvariantCulture));
                if (p.Longitude.HasValue)
                    sb.Append(",\"lon\":").Append(p.Longitude.Value.ToString("G17", CultureInfo.InvariantCulture));
                if (p.HeadingRad.HasValue)
                    sb.Append(",\"heading\":").Append(p.HeadingRad.Value.ToString("G9", CultureInfo.InvariantCulture));
            }

            sb.Append(",\"ts\":\"").Append(DateTime.UtcNow.ToString("o")).Append('"');
            sb.Append('}');
            return sb.ToString();
        }

        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        public void Dispose()
        {
            try { Stop(); } catch { }
            try { _timer?.Dispose(); } catch { }
            try { _client?.Close(); } catch { }
        }
    }
}
