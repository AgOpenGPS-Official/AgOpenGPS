// ============================================================================
// VistaXNativePanel.cs - Panel VistaX renderizado 100% en WinForms/GDI+
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/VistaXNativePanel.cs
// Target: net48 (C# 7.3)
//
// Reemplaza el VistaXPanel basado en CefSharp. Consume SeedMonitorSnapshot
// del SeedMonitor (MQTT nativo) y dibuja header con KPIs + tubos por surco
// agrupados por tren + footer de estado.
//
// Sin CefSharp, sin Node.js, sin Socket.IO. Solo GDI+ sobre un UserControl.
// ============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace AgroParallel.VistaX
{
    public class VistaXNativePanel : UserControl
    {
        private static readonly Color CBgDark = Color.FromArgb(10, 10, 10);
        private static readonly Color CBgHeader = Color.FromArgb(18, 18, 22);
        private static readonly Color CText = Color.FromArgb(235, 235, 235);
        private static readonly Color CTextDim = Color.FromArgb(140, 140, 140);
        private static readonly Color CAccent = Color.FromArgb(0, 230, 118);
        private static readonly Color CRed = Color.FromArgb(255, 23, 68);
        private static readonly Color CYellow = Color.FromArgb(255, 210, 30);
        private static readonly Color CBlue = Color.FromArgb(64, 164, 255);
        private static readonly Color CPink = Color.FromArgb(255, 96, 180);
        private static readonly Color CBorder = Color.FromArgb(40, 40, 48);

        private const int HeaderHeight = 40;
        private const int FooterHeight = 20;
        private const int AlarmBannerHeight = 16;
        private const int TrainLabelWidth = 110;

        private readonly VistaXConfig _config;
        private readonly int _panelHeight;
        private readonly int _panelWidthPercent;
        private readonly int _panelBottomMargin;

        private volatile SeedMonitorSnapshot _snap;
        private int _lastInvalidateTick;

        public VistaXNativePanel(VistaXConfig config)
        {
            _config = config ?? throw new ArgumentNullException("config");
            _panelHeight = config.PanelHeight > 0 ? config.PanelHeight : 150;
            _panelWidthPercent = config.PanelWidthPercent > 0 ? config.PanelWidthPercent : 60;
            _panelBottomMargin = config.PanelBottomMargin >= 0 ? config.PanelBottomMargin : 200;

            DoubleBuffered = true;
            BackColor = CBgDark;
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
        }

        // Llamado por SeedMonitor.SnapshotUpdated. El evento viene desde un Timer
        // en hilo background, por eso hay que marshaller a UI antes de tocar state.
        public void SetSnapshot(SeedMonitorSnapshot snap)
        {
            if (InvokeRequired)
            {
                try { BeginInvoke(new Action<SeedMonitorSnapshot>(SetSnapshot), snap); }
                catch { /* handle dispuesto: ignorar */ }
                return;
            }

            _snap = snap;

            // Throttle a ~60fps. El monitor reporta a 500ms asi que en la
            // practica no se activa, pero protege si alguien sube la frecuencia.
            int now = Environment.TickCount;
            if (now - _lastInvalidateTick < 16) return;
            _lastInvalidateTick = now;
            Invalidate();
        }

        // Compatibilidad con VistaXPanel de CefSharp — el caller sigue
        // llamando estos metodos. SetSnapshot es el camino real.
        public void UpdateDisplay(SeedMonitorSnapshot snap) { SetSnapshot(snap); }
        public void FlushPending() { }

        public void Reposition()
        {
            if (Parent == null) return;

            int parentW = Parent.Width;
            int parentH = Parent.Height;

            int panelH = _panelHeight;
            int panelW = (int)(parentW * _panelWidthPercent / 100.0);

            Size = new Size(panelW, panelH);
            Location = new Point(
                (parentW - panelW) / 2,
                parentH - panelH - _panelBottomMargin);
            BringToFront();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(CBgDark);

            using (var borderPen = new Pen(CBorder))
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);

            var snap = _snap;
            if (snap == null || snap.Trenes == null || snap.Trenes.Length == 0)
            {
                DrawWaitingState(g, snap);
                return;
            }

            DrawHeader(g, snap);

            int bodyTop = HeaderHeight + (snap.HasAlarm ? AlarmBannerHeight : 0);
            int bodyBottom = Height - FooterHeight;
            if (snap.HasAlarm) DrawAlarmBanner(g, snap);

            DrawTrains(g, snap, bodyTop, bodyBottom);
            DrawFooter(g, snap);
        }

        private void DrawWaitingState(Graphics g, SeedMonitorSnapshot snap)
        {
            // Header minimalista para ver si al menos hay conexion.
            using (var hbg = new SolidBrush(CBgHeader))
                g.FillRectangle(hbg, 0, 0, Width, HeaderHeight);

            using (var fTitle = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(CAccent))
                g.DrawString("VistaX", fTitle, titleBrush, 12, 10);

            bool connected = snap != null && snap.IsConnected;
            DrawConnectionLed(g, connected);

            using (var f = new Font("Segoe UI", 10f))
            using (var dim = new SolidBrush(CTextDim))
            {
                string msg = connected
                    ? "Conectado — esperando telemetria de sensores..."
                    : "Esperando conexion con broker MQTT...";
                var sz = g.MeasureString(msg, f);
                g.DrawString(msg, f, dim,
                    (Width - sz.Width) / 2f,
                    HeaderHeight + (Height - HeaderHeight - FooterHeight - sz.Height) / 2f);
            }
        }

        private void DrawHeader(Graphics g, SeedMonitorSnapshot snap)
        {
            using (var hbg = new SolidBrush(CBgHeader))
                g.FillRectangle(hbg, 0, 0, Width, HeaderHeight);

            using (var fTitle = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(CAccent))
                g.DrawString("VistaX", fTitle, titleBrush, 12, 10);

            using (var fSub = new Font("Segoe UI", 8.5f))
            using (var dimBrush = new SolidBrush(CTextDim))
            {
                string impl = string.IsNullOrEmpty(snap.NombreImplemento)
                    ? ""
                    : snap.NombreImplemento;
                g.DrawString(impl, fSub, dimBrush, 70, 14);
            }

            // KPIs alineados a la derecha, antes del LED de conexion.
            int kpiRight = Width - 30;
            using (var fLbl = new Font("Segoe UI", 7.5f))
            using (var fVal = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var lblBrush = new SolidBrush(CTextDim))
            using (var valBrush = new SolidBrush(CText))
            using (var redBrush = new SolidBrush(CRed))
            {
                int totalOnline = snap.SurcosActivos;
                int totalConfig = (snap.Surcos != null) ? snap.Surcos.Length : 0;
                string online = totalConfig > 0
                    ? totalOnline + "/" + totalConfig
                    : totalOnline.ToString(CultureInfo.InvariantCulture);

                var kpis = new[]
                {
                    Tuple.Create("VEL", snap.Velocidad.ToString("F1", CultureInfo.InvariantCulture), false),
                    Tuple.Create("S/M", snap.SpmPromedio.ToString("F1", CultureInfo.InvariantCulture), false),
                    Tuple.Create("FALLAS", snap.FallasActivas.ToString(CultureInfo.InvariantCulture),
                        snap.FallasActivas > 0),
                    Tuple.Create("ONLINE", online, false)
                };

                for (int i = kpis.Length - 1; i >= 0; i--)
                {
                    var k = kpis[i];
                    var valSz = g.MeasureString(k.Item2, fVal);
                    var lblSz = g.MeasureString(k.Item1, fLbl);
                    float boxW = Math.Max(valSz.Width, lblSz.Width) + 4;
                    float boxX = kpiRight - boxW;

                    g.DrawString(k.Item1, fLbl, lblBrush, boxX, 4);
                    g.DrawString(k.Item2, fVal, k.Item3 ? redBrush : valBrush, boxX, 17);

                    kpiRight = (int)boxX - 12;
                    if (kpiRight < 160) break;
                }
            }

            DrawConnectionLed(g, snap.IsConnected);
        }

        private void DrawConnectionLed(Graphics g, bool connected)
        {
            int r = 6;
            int cx = Width - 14;
            int cy = 14;
            using (var ledBrush = new SolidBrush(connected ? CAccent : CRed))
                g.FillEllipse(ledBrush, cx - r, cy - r, r * 2, r * 2);
            using (var rim = new Pen(Color.FromArgb(30, 30, 36)))
                g.DrawEllipse(rim, cx - r, cy - r, r * 2, r * 2);
        }

        private void DrawAlarmBanner(Graphics g, SeedMonitorSnapshot snap)
        {
            using (var bg = new SolidBrush(Color.FromArgb(90, 0, 0)))
                g.FillRectangle(bg, 0, HeaderHeight, Width, AlarmBannerHeight);
            using (var fAlarm = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var textBrush = new SolidBrush(CRed))
            {
                string msg = snap.AlarmMessage ?? "ALARMA";
                g.DrawString(msg, fAlarm, textBrush, 10, HeaderHeight + 1);
            }
        }

        private void DrawTrains(Graphics g, SeedMonitorSnapshot snap, int bodyTop, int bodyBottom)
        {
            int bodyH = bodyBottom - bodyTop;
            if (bodyH <= 0) return;

            // Dos trenes arriba/abajo. Si hay solo uno usa el body completo.
            int n = snap.Trenes.Length;
            int perTrainH = n > 0 ? bodyH / n : bodyH;

            for (int i = 0; i < n; i++)
            {
                var tren = snap.Trenes[i];
                if (tren == null) continue;
                int y0 = bodyTop + i * perTrainH;
                int y1 = y0 + perTrainH;
                DrawTrain(g, tren, y0, y1);
            }
        }

        private void DrawTrain(Graphics g, TrenLayout tren, int y0, int y1)
        {
            using (var fLbl = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var lblBrush = new SolidBrush(CTextDim))
            {
                string name = tren.Tren == 1 ? "DELANTERO" : tren.Tren == 2 ? "TRASERO"
                    : "TREN " + tren.Tren.ToString(CultureInfo.InvariantCulture);
                g.DrawString(name, fLbl, lblBrush, 8, y0 + 4);
                g.DrawString("(" + tren.Count + ")", fLbl, lblBrush, 8, y0 + 18);
            }

            if (tren.Surcos == null || tren.Surcos.Length == 0) return;

            int drawAreaLeft = TrainLabelWidth;
            int drawAreaRight = Width - 8;
            int drawAreaW = drawAreaRight - drawAreaLeft;
            if (drawAreaW < 32) return;

            int tubeH = (y1 - y0) - 10;
            if (tubeH < 8) return;
            int ledR = 3;

            // Reajustar SensorWidthPx / SpacingPx si el layout calculado no
            // cabe en el area real disponible (el snapshot asumia ContainerWidthPx=1200).
            int sensorW = tren.SensorWidthPx > 0 ? tren.SensorWidthPx : 20;
            int spacing = tren.SpacingPx;
            int total = tren.Surcos.Length * sensorW
                + Math.Max(0, tren.Surcos.Length - 1) * spacing;
            if (total > drawAreaW)
            {
                int available = drawAreaW - Math.Max(0, tren.Surcos.Length - 1) * 2;
                sensorW = Math.Max(6, available / Math.Max(1, tren.Surcos.Length));
                spacing = 2;
                total = tren.Surcos.Length * sensorW + (tren.Surcos.Length - 1) * spacing;
            }
            int offsetX = drawAreaLeft + (drawAreaW - total) / 2;

            int tubeTop = y0 + 8;

            for (int i = 0; i < tren.Surcos.Length; i++)
            {
                var s = tren.Surcos[i];
                int x = offsetX + i * (sensorW + spacing);
                DrawTube(g, x, tubeTop, sensorW, tubeH, s);
                DrawLed(g, x + sensorW / 2, tubeTop - ledR - 2, ledR, s.State);
            }
        }

        private void DrawTube(Graphics g, int x, int y, int w, int h, SurcoState s)
        {
            var rect = new Rectangle(x, y, w, h);

            switch (s.State)
            {
                case RowState.Failure:
                    using (var b = new SolidBrush(Color.FromArgb(200, 32, 40)))
                        g.FillRectangle(b, rect);
                    break;
                case RowState.NoData:
                    using (var b = new SolidBrush(Color.FromArgb(45, 45, 55)))
                        g.FillRectangle(b, rect);
                    break;
                default:
                    // Gradiente azul (tope) -> rosa (base) modulado por SPM.
                    using (var brush = new LinearGradientBrush(
                        new Point(x, y), new Point(x, y + h), CBlue, CPink))
                    {
                        g.FillRectangle(brush, rect);
                    }
                    break;
            }

            using (var border = new Pen(CBorder))
                g.DrawRectangle(border, rect);
        }

        private void DrawLed(Graphics g, int cx, int cy, int r, RowState state)
        {
            Color c;
            switch (state)
            {
                case RowState.Ok: c = CAccent; break;
                case RowState.Failure: c = CRed; break;
                case RowState.LowRate: c = CYellow; break;
                case RowState.HighRate: c = CYellow; break;
                default: c = Color.FromArgb(80, 80, 80); break;
            }
            using (var b = new SolidBrush(c))
                g.FillEllipse(b, cx - r, cy - r, r * 2, r * 2);
        }

        private void DrawFooter(Graphics g, SeedMonitorSnapshot snap)
        {
            using (var fg = new SolidBrush(CBgHeader))
                g.FillRectangle(fg, 0, Height - FooterHeight, Width, FooterHeight);

            string msg = snap.MonitoreoActivo
                ? "SISTEMA VISTAX OPERATIVO"
                : "EN ESPERA DE INICIO (" + snap.MetodoInicio + ")";

            using (var f = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var brush = new SolidBrush(snap.MonitoreoActivo ? CAccent : CTextDim))
            {
                var sz = g.MeasureString(msg, f);
                g.DrawString(msg, f, brush,
                    (Width - sz.Width) / 2f,
                    Height - FooterHeight + (FooterHeight - sz.Height) / 2f);
            }
        }
    }
}
