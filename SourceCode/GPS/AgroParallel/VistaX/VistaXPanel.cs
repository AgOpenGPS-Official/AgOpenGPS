using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;

namespace AgroParallel.VistaX
{
    public class VistaXPanel : UserControl
    {
        private ChromiumWebBrowser browser;
        private string _serverUrl;
        private bool _isReady = false;

        // Throttle: máximo ~4-5 updates/segundo
        private const int MinUpdateIntervalMs = 220;
        private DateTime _lastUpdate = DateTime.MinValue;
        private SeedMonitorSnapshot _pendingSnap;

        // Reusable buffers to reduce GC pressure
        private byte[] _jsonBuffer;
        private readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public VistaXPanel(string url)
        {
            _serverUrl = url;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            InitializeChromium();
        }

        private void InitializeChromium()
        {
            browser = new ChromiumWebBrowser(_serverUrl);
            browser.Dock = DockStyle.Fill;

            var settings = new BrowserSettings
            {
                WindowlessFrameRate = 15,
                BackgroundColor = Cef.ColorSetARGB(255, 10, 10, 10)
            };
            browser.BrowserSettings = settings;

            this.Controls.Add(browser);

            browser.FrameLoadEnd += (s, e) => {
                if (e.Frame.IsMain)
                {
                    _isReady = true;
                }
            };
        }

        public void Reposition()
        {
            if (this.Parent == null) return;
            this.Size = new Size((int)(this.Parent.Width * 0.95), 180);
            this.Location = new Point((this.Parent.Width - this.Width) / 2, this.Parent.Height - this.Height - 110);
            this.BringToFront();
        }

        public void UpdateDisplay(SeedMonitorSnapshot snap)
        {
            if (!_isReady || snap == null || browser == null) return;

            // Throttle: drop frames si se llama más de ~4-5 veces/segundo
            var now = DateTime.UtcNow;
            if ((now - _lastUpdate).TotalMilliseconds < MinUpdateIntervalMs)
            {
                _pendingSnap = snap;
                return;
            }

            SendToChromium(snap);
            _lastUpdate = now;
            _pendingSnap = null;
        }

        /// <summary>
        /// Envía un snapshot pendiente si fue dropeado por throttle.
        /// Llamar desde un timer externo si se quiere flush periódico.
        /// </summary>
        public void FlushPending()
        {
            var pending = _pendingSnap;
            if (pending == null) return;
            if ((DateTime.UtcNow - _lastUpdate).TotalMilliseconds < MinUpdateIntervalMs) return;

            _pendingSnap = null;
            SendToChromium(pending);
            _lastUpdate = DateTime.UtcNow;
        }

        private void SendToChromium(SeedMonitorSnapshot snap)
        {
            try
            {
                // Serializar a byte[] para evitar string intermedio grande
                _jsonBuffer = JsonSerializer.SerializeToUtf8Bytes(snap, _jsonOpts);
                string b64 = Convert.ToBase64String(_jsonBuffer);
                // El script es pequeño; el payload pesado está en b64 que CefSharp maneja internamente
                browser.GetMainFrame().ExecuteJavaScriptAsync(
                    "if(window.updateData){window.updateData(atob('" + b64 + "'));}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error enviando a CefSharp: " + ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                browser?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}