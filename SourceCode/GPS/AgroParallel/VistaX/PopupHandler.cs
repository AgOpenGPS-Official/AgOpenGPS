// ============================================================================
// PopupHandler.cs — Popups sin bordes para VistaX en CefSharp
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/PopupHandler.cs
// Target: net48 (C# 7.3)
//
// Intercepta window.open() del frontend y abre un Form sin bordes
// con ChromiumWebBrowser adentro. Sin barra de título de Windows,
// tamaño configurable desde vistaX.json.
// ============================================================================

using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgroParallel.VistaX
{
    /// <summary>
    /// Intercepta popups (window.open) y los abre en Forms sin bordes.
    /// </summary>
    public class PopupHandler : ILifeSpanHandler
    {
        private readonly VistaXConfig _config;

        public PopupHandler(VistaXConfig config)
        {
            _config = config;
        }

        public bool OnBeforePopup(
            IWebBrowser chromiumWebBrowser,
            IBrowser browser,
            IFrame frame,
            string targetUrl,
            string targetFrameName,
            WindowOpenDisposition targetDisposition,
            bool userGesture,
            IPopupFeatures popupFeatures,
            IWindowInfo windowInfo,
            IBrowserSettings browserSettings,
            ref bool noJavascriptAccess,
            out IWebBrowser newBrowser)
        {
            newBrowser = null;

            // Determinar tamaño según la URL
            int w = _config.PopupDefaultWidth;
            int h = _config.PopupDefaultHeight;

            if (targetUrl != null)
            {
                string lower = targetUrl.ToLowerInvariant();

                if (lower.Contains("/config"))
                {
                    w = _config.PopupConfigWidth;
                    h = _config.PopupConfigHeight;
                }
                else if (lower.Contains("/detalle-surco") || lower.Contains("detalle"))
                {
                    w = _config.PopupDetalleWidth;
                    h = _config.PopupDetalleHeight;
                }
                else if (lower.Contains("/iniciar-lote"))
                {
                    w = _config.PopupDetalleWidth;
                    h = _config.PopupDetalleHeight;
                }
            }

            // Abrir en el hilo de UI
            var control = (Control)chromiumWebBrowser;
            var url = targetUrl;
            var popW = w;
            var popH = h;

            control.Invoke(new Action(delegate
            {
                var popup = new BorderlessPopupForm(url, popW, popH);
                popup.Show(control.FindForm());
            }));

            // Retornar true = cancelar el popup nativo de CefSharp
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser) { }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return false;
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser) { }
    }

    /// <summary>
    /// Form sin bordes con ChromiumWebBrowser.
    /// Arrastrable desde cualquier parte. Cierre con Escape o botón X.
    /// Redimensionable desde los bordes.
    /// </summary>
    public class BorderlessPopupForm : Form
    {
        private ChromiumWebBrowser _browser;
        private bool _dragging;
        private Point _dragStart;

        // Resize desde bordes
        private const int ResizeBorder = 6;
        private const int CaptionHeight = 28;

        public BorderlessPopupForm(string url, int width, int height)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(width, height);
            this.MinimumSize = new Size(300, 200);
            this.BackColor = Color.FromArgb(10, 10, 10);
            this.ShowInTaskbar = false;
            this.KeyPreview = true;

            // Barra de título custom
            var titleBar = new Panel();
            titleBar.Dock = DockStyle.Top;
            titleBar.Height = CaptionHeight;
            titleBar.BackColor = Color.FromArgb(20, 20, 20);
            titleBar.Cursor = Cursors.SizeAll;

            // Drag
            titleBar.MouseDown += delegate (object s, MouseEventArgs me)
            {
                if (me.Button == MouseButtons.Left) { _dragging = true; _dragStart = me.Location; }
            };
            titleBar.MouseMove += delegate (object s, MouseEventArgs me)
            {
                if (_dragging) this.Location = new Point(
                    this.Location.X + me.X - _dragStart.X,
                    this.Location.Y + me.Y - _dragStart.Y);
            };
            titleBar.MouseUp += delegate { _dragging = false; };

            // Título
            var lblTitle = new Label();
            lblTitle.Text = "VistaX";
            lblTitle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(0, 180, 80);
            lblTitle.Location = new Point(10, 6);
            lblTitle.AutoSize = true;
            lblTitle.BackColor = Color.Transparent;
            titleBar.Controls.Add(lblTitle);

            // Botón cerrar
            var btnClose = new Button();
            btnClose.Text = "✕";
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.BackColor = Color.FromArgb(20, 20, 20);
            btnClose.ForeColor = Color.FromArgb(100, 100, 100);
            btnClose.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnClose.Size = new Size(CaptionHeight, CaptionHeight);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Location = new Point(width - CaptionHeight, 0);
            btnClose.Cursor = Cursors.Hand;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 40, 40);
            btnClose.Click += delegate { this.Close(); };
            titleBar.Controls.Add(btnClose);

            this.Controls.Add(titleBar);

            // Browser
            _browser = new ChromiumWebBrowser(url);
            _browser.Dock = DockStyle.Fill;
            _browser.BrowserSettings = new BrowserSettings
            {
                WindowlessFrameRate = 30,
                BackgroundColor = Cef.ColorSetARGB(255, 10, 10, 10)
            };

            this.Controls.Add(_browser);
            _browser.BringToFront();

            // Escape para cerrar
            this.KeyDown += delegate (object s, KeyEventArgs ke)
            {
                if (ke.KeyCode == Keys.Escape) this.Close();
            };

            // Borde fino pintado
            this.Paint += delegate (object s, PaintEventArgs pe)
            {
                using (var pen = new Pen(Color.FromArgb(50, 50, 50)))
                    pe.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };
        }

        // Resize desde los bordes (sin barra de título nativa)
        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                var pos = this.PointToClient(new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16));
                bool left = pos.X < ResizeBorder;
                bool right = pos.X > Width - ResizeBorder;
                bool top = pos.Y < ResizeBorder;
                bool bottom = pos.Y > Height - ResizeBorder;

                if (top && left) m.Result = (IntPtr)HTTOPLEFT;
                else if (top && right) m.Result = (IntPtr)HTTOPRIGHT;
                else if (bottom && left) m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (bottom && right) m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (left) m.Result = (IntPtr)HTLEFT;
                else if (right) m.Result = (IntPtr)HTRIGHT;
                else if (top) m.Result = (IntPtr)HTTOP;
                else if (bottom) m.Result = (IntPtr)HTBOTTOM;

                return;
            }

            base.WndProc(ref m);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _browser?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
