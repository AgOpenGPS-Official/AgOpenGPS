// ============================================================================
// VistaXPanel.cs — Panel embebido CefSharp para AgOpenGPS
// Solo renderiza la vista /bar del servidor VistaX (Node.js)
// El servidor Node maneja MQTT, Socket.IO y toda la lógica.
//
// Layout configurable desde vistaX.json:
//   PanelHeight        → alto en píxeles (default 120)
//   PanelWidthPercent  → % del ancho del form padre (default 70)
//   PanelBottomMargin  → margen inferior en px (default 60)
// ============================================================================

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgroParallel.VistaX
{
    public class VistaXPanel : UserControl
    {
        private ChromiumWebBrowser browser;
        private string _serverUrl;
        private bool _isReady = false;
        private VistaXConfig _config;

        // Layout leído del config
        private int _panelHeight;
        private int _panelWidthPercent;
        private int _panelBottomMargin;

        public VistaXPanel(VistaXConfig config)
        {
            _config = config;
            _serverUrl = NormalizeUrl(config.ServerUrl);
            _panelHeight = config.PanelHeight;
            _panelWidthPercent = config.PanelWidthPercent;
            _panelBottomMargin = config.PanelBottomMargin;

            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            InitializeChromium();
        }

        /// <summary>
        /// Normaliza la URL del servidor:
        /// - Agrega http:// si falta
        /// - Agrega /bar si no tiene path
        /// </summary>
        private static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                url = "http://localhost:3000/bar";

            url = url.Trim();

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;

            try
            {
                var uri = new Uri(url);
                if (string.IsNullOrEmpty(uri.AbsolutePath) || uri.AbsolutePath == "/")
                    url = url.TrimEnd('/') + "/bar";
            }
            catch
            {
                if (!url.Contains("/bar"))
                    url = url.TrimEnd('/') + "/bar";
            }

            return url;
        }

        private void InitializeChromium()
        {
            browser = new ChromiumWebBrowser(_serverUrl);
            browser.Dock = DockStyle.Fill;

            var settings = new BrowserSettings
            {
                WindowlessFrameRate = 15,
                BackgroundColor = Cef.ColorSetARGB(255, 0, 0, 0)
            };
            browser.BrowserSettings = settings;

            // Popups sin bordes de Windows, tamaño configurable
            browser.LifeSpanHandler = new PopupHandler(_config);

            this.Controls.Add(browser);

            browser.FrameLoadEnd += (s, e) =>
            {
                if (e.Frame.IsMain)
                {
                    _isReady = true;
                    System.Diagnostics.Debug.WriteLine("[VistaX] CefSharp cargó: " + _serverUrl);
                }
            };

            browser.LoadError += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Error cargando " + e.FailedUrl
                    + ": " + e.ErrorText);
            };
        }

        /// <summary>
        /// Posiciona el panel según los valores de vistaX.json.
        /// Modificá PanelHeight, PanelWidthPercent y PanelBottomMargin
        /// en el JSON sin necesidad de recompilar.
        /// </summary>
        public void Reposition()
        {
            if (this.Parent == null) return;

            int parentW = this.Parent.Width;
            int parentH = this.Parent.Height;

            int panelH = _panelHeight;
            int panelW = (int)(parentW * _panelWidthPercent / 100.0);

            this.Size = new Size(panelW, panelH);
            this.Location = new Point(
                (parentW - panelW) / 2,
                parentH - panelH - _panelBottomMargin
            );
            this.BringToFront();
        }

        // =====================================================================
        // No-op — el frontend recibe datos via Socket.IO del server Node.
        // Se mantienen por compatibilidad con código existente en FormGPS.
        // =====================================================================

        public void UpdateDisplay(SeedMonitorSnapshot snap) { }
        public void FlushPending() { }

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
