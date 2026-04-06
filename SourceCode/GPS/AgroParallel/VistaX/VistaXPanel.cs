using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Windows.Forms;

namespace AgroParallel.VistaX
{
    public class VistaXPanel : UserControl
    {
        private ChromiumWebBrowser browser;
        private string _serverUrl;
        private bool _isReady = false;

        public VistaXPanel(string url)
        {
            _serverUrl = url;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            InitializeChromium();
        }

        private void InitializeChromium()
        {
            // Creamos el navegador de CefSharp
            browser = new ChromiumWebBrowser(_serverUrl);
            browser.Dock = DockStyle.Fill;

            // Configuraciones de limpieza
            var settings = new BrowserSettings
            {
                WindowlessFrameRate = 30,
                BackgroundColor = Cef.ColorSetARGB(255, 10, 10, 10)
            };
            browser.BrowserSettings = settings;

            this.Controls.Add(browser);

            // Evento cuando el navegador termina de cargar el HTML
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

            try
            {
                string json = JsonSerializer.Serialize(snap);

                // En CefSharp moderno, se accede al Frame principal para ejecutar JS
                string script = $@"if(window.updateData) {{ window.updateData('{json}'); }}";

                // CAMBIO AQUÍ:
                browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error enviando a CefSharp: " + ex.Message);
            }
        }
        // Limpieza de memoria al cerrar
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