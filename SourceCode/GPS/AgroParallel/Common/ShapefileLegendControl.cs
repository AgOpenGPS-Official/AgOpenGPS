// ============================================================================
// ShapefileLegendControl.cs - Leyenda visual del gradiente verde→amarillo→rojo
// Ubicación: SourceCode/GPS/AgroParallel/Common/ShapefileLegendControl.cs
// Target: net48 (C# 7.3)
//
// Control WinForms que se superpone sobre el area del mapa (oglMain) y muestra:
//   - Titulo con el nombre del campo DBF activo
//   - Barra vertical de gradiente (mismos colores que ShapefileLayer)
//   - Etiquetas con min/max
//
// Se actualiza desde FormGPS llamando SetLegend(...) / Clear(). La visibilidad
// del control se controla desde afuera (Visible = true/false).
// ============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace AgroParallel.Common
{
    public class ShapefileLegendControl : UserControl
    {
        private string _fieldName;
        private double _min;
        private double _max;
        private double _current;
        private bool _hasCurrent;

        // Estado UDP (paso 11).
        private bool _udpActive;
        private int _udpPackets;
        private DateTime? _udpLastSend;

        // Mismos colores que ShapefileLayer.ApplyColorByField (sin alpha:
        // el alpha del layer pinta el mapa; en la leyenda usamos opaco).
        private static readonly Color CLow = Color.FromArgb(0, 200, 0);
        private static readonly Color CMid = Color.FromArgb(255, 220, 0);
        private static readonly Color CHigh = Color.FromArgb(220, 40, 0);
        private static readonly Color CBg = Color.FromArgb(220, 20, 20, 20);
        private static readonly Color CBorder = Color.FromArgb(80, 80, 80);
        private static readonly Color CText = Color.FromArgb(235, 235, 235);
        private static readonly Color CTextDim = Color.FromArgb(170, 170, 170);

        public ShapefileLegendControl()
        {
            DoubleBuffered = true;
            Size = new Size(130, 240);
            BackColor = Color.Black;
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint, true);
        }

        public void SetLegend(string fieldName, double min, double max)
        {
            _fieldName = fieldName;
            _min = min;
            _max = max;
            Invalidate();
        }

        public void Clear()
        {
            _fieldName = null;
            _hasCurrent = false;
            Invalidate();
        }

        public void SetCurrent(double value, bool inside)
        {
            if (_hasCurrent == inside && _hasCurrent && _current == value) return;
            _current = value;
            _hasCurrent = inside;
            Invalidate();
        }

        public void SetUdpStatus(bool active, int packets, DateTime? lastSendUtc)
        {
            if (_udpActive == active && _udpPackets == packets && _udpLastSend == lastSendUtc)
                return;
            _udpActive = active;
            _udpPackets = packets;
            _udpLastSend = lastSendUtc;
            Invalidate();
        }

        public bool HasData { get { return !string.IsNullOrEmpty(_fieldName); } }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(CBg);

            using (var borderPen = new Pen(CBorder))
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);

            if (!HasData)
            {
                using (var brush = new SolidBrush(CTextDim))
                using (var f = new Font("Segoe UI", 8f))
                    g.DrawString("Sin estilo", f, brush, 8, 8);
                return;
            }

            // Titulo (nombre del campo).
            using (var titleBrush = new SolidBrush(CText))
            using (var fTitle = new Font("Segoe UI", 9f, FontStyle.Bold))
            {
                string name = _fieldName.Length > 14 ? _fieldName.Substring(0, 13) + "…" : _fieldName;
                g.DrawString(name, fTitle, titleBrush, 8, 6);
            }

            // Barra de gradiente vertical (max arriba, min abajo).
            var barRect = new Rectangle(12, 32, 22, 150);
            using (var brush = new LinearGradientBrush(
                new Point(0, barRect.Top),
                new Point(0, barRect.Bottom),
                CHigh, CLow))
            {
                var blend = new ColorBlend(3);
                blend.Colors = new[] { CHigh, CMid, CLow };
                blend.Positions = new[] { 0f, 0.5f, 1f };
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, barRect);
            }
            using (var barPen = new Pen(CBorder))
                g.DrawRectangle(barPen, barRect);

            // Etiquetas min/max.
            using (var valBrush = new SolidBrush(CText))
            using (var fVal = new Font("Segoe UI", 8.5f, FontStyle.Bold))
            {
                string maxStr = _max.ToString("G4", CultureInfo.InvariantCulture);
                string minStr = _min.ToString("G4", CultureInfo.InvariantCulture);
                g.DrawString(maxStr, fVal, valBrush, 40, barRect.Top - 2);
                g.DrawString(minStr, fVal, valBrush, 40, barRect.Bottom - 14);
            }

            // Etiqueta media (50%).
            using (var midBrush = new SolidBrush(CTextDim))
            using (var fMid = new Font("Segoe UI", 7.5f))
            {
                double mid = (_min + _max) / 2.0;
                string midStr = mid.ToString("G4", CultureInfo.InvariantCulture);
                g.DrawString(midStr, fMid, midBrush, 40, barRect.Top + (barRect.Height / 2) - 7);
            }

            // Marcador del valor actual (triangulo a la izquierda de la barra).
            if (_hasCurrent && _max > _min)
            {
                double t = (_current - _min) / (_max - _min);
                if (t < 0) t = 0;
                else if (t > 1) t = 1;
                int y = barRect.Bottom - (int)(t * barRect.Height);
                var tri = new[] {
                    new Point(barRect.Left - 2, y),
                    new Point(barRect.Left - 10, y - 5),
                    new Point(barRect.Left - 10, y + 5)
                };
                using (var mkBrush = new SolidBrush(Color.White))
                    g.FillPolygon(mkBrush, tri);
            }

            // Readout actual al pie.
            using (var fLbl = new Font("Segoe UI", 8f))
            using (var fVal = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var dimBrush = new SolidBrush(CTextDim))
            using (var textBrush = new SolidBrush(CText))
            {
                g.DrawString("Dosis actual", fLbl, dimBrush, 8, Height - 42);
                string val = _hasCurrent
                    ? _current.ToString("G5", CultureInfo.InvariantCulture)
                    : "fuera";
                g.DrawString(val, fVal, textBrush, 8, Height - 26);
            }

            // UDP: LED + contador de paquetes (paso 11).
            DrawUdpStatus(g);
        }

        private void DrawUdpStatus(Graphics g)
        {
            // LED en la esquina superior derecha (6px + halo).
            var ledCenter = new Point(Width - 16, 12);
            Color ledColor;
            if (!_udpActive)
            {
                ledColor = Color.FromArgb(80, 80, 80); // apagado
            }
            else if (_udpLastSend.HasValue
                && (DateTime.UtcNow - _udpLastSend.Value).TotalMilliseconds < 500)
            {
                ledColor = Color.FromArgb(0, 220, 80);  // activo y enviando
            }
            else
            {
                ledColor = Color.FromArgb(180, 180, 60); // activo pero ultimo send > 500ms
            }

            using (var ledBrush = new SolidBrush(ledColor))
                g.FillEllipse(ledBrush, ledCenter.X - 5, ledCenter.Y - 5, 10, 10);
            using (var rim = new Pen(Color.FromArgb(50, 50, 50)))
                g.DrawEllipse(rim, ledCenter.X - 5, ledCenter.Y - 5, 10, 10);

            // Contador debajo del LED.
            using (var fCnt = new Font("Segoe UI", 7.5f))
            using (var dim = new SolidBrush(CTextDim))
            {
                string txt = _udpActive
                    ? "UDP " + _udpPackets
                    : "UDP off";
                var sz = g.MeasureString(txt, fCnt);
                g.DrawString(txt, fCnt, dim, Width - sz.Width - 6, 22);
            }
        }
    }
}
