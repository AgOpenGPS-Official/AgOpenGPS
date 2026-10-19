using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// The NTRIP status in the main window: a coloured light, one sentence a farmer understands,
    /// where the corrections come from, and a big Start/Stop button for a finger.
    /// </summary>
    internal sealed class NtripStatusCard : Control
    {
        private string headline = "NTRIP is off";
        private string subline = "";
        private string received = "";
        private NtripLight light = NtripLight.Off;

        private readonly Button button;
        private readonly Font smallFont = new Font("Tahoma", 11.25F, FontStyle.Regular);

        public event EventHandler ButtonClicked;

        public NtripStatusCard()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Font = new Font("Tahoma", 13.5F, FontStyle.Bold);

            button = new Button
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                Text = "Start",
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Tahoma", 16.5F, FontStyle.Bold),
                BackColor = Color.WhiteSmoke,
                UseVisualStyleBackColor = false,
                TabStop = false,
            };
            button.FlatAppearance.BorderColor = Color.Black;
            button.Click += (s, e) =>
            {
                EventHandler handler = ButtonClicked;
                if (handler != null) handler(this, EventArgs.Empty);
            };
            Controls.Add(button);
        }

        public void SetStatus(string headlineText, NtripLight statusLight, string sublineText, string receivedText, string buttonText)
        {
            if (headlineText == headline && statusLight == light && sublineText == subline
                && receivedText == received && buttonText == button.Text)
            {
                return;
            }

            headline = headlineText;
            light = statusLight;
            subline = sublineText;
            received = receivedText;
            button.Text = buttonText;
            Invalidate();
        }

        internal static Color LightColor(NtripLight l)
        {
            switch (l)
            {
                case NtripLight.Ok: return Color.FromArgb(21, 110, 75);
                case NtripLight.Warn: return Color.FromArgb(176, 92, 0);
                case NtripLight.Bad: return Color.FromArgb(180, 30, 20);
                case NtripLight.Info: return Color.FromArgb(27, 95, 168);
                default: return Color.FromArgb(105, 105, 105);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            int pad = 8;
            int dot = 22;
            var box = new Rectangle(0, 0, Width - 1, Math.Max(10, Height - button.Height - 6));

            using (var fill = new SolidBrush(Color.White))
            {
                g.FillRectangle(fill, box);
            }
            using (var border = new Pen(Color.FromArgb(90, 90, 90)))
            {
                g.DrawRectangle(border, box);
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            using (var lamp = new SolidBrush(LightColor(light)))
            {
                g.FillEllipse(lamp, pad, pad + 2, dot, dot);
            }

            const TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis
                | TextFormatFlags.NoPadding | TextFormatFlags.Left | TextFormatFlags.Top;

            int textLeft = pad * 2 + dot;
            int textWidth = box.Width - textLeft - pad;
            int headlineHeight = TextRenderer.MeasureText(g, headline, Font, new Size(textWidth, 0), flags).Height;
            headlineHeight = Math.Min(headlineHeight, 3 * Font.Height);

            TextRenderer.DrawText(g, headline, Font, new Rectangle(textLeft, pad, textWidth, headlineHeight), Color.Black, flags);

            int y = pad + Math.Max(headlineHeight, dot + 4) + 6;
            int remaining = Math.Max(0, box.Bottom - y - 4);
            if (subline.Length > 0)
            {
                int h = Math.Min(remaining, 2 * smallFont.Height);
                TextRenderer.DrawText(g, subline, smallFont, new Rectangle(pad, y, box.Width - 2 * pad, h), Color.FromArgb(30, 30, 30), flags);
                y += h + 2;
            }
            if (received.Length > 0 && y + smallFont.Height <= box.Bottom)
            {
                TextRenderer.DrawText(g, received, smallFont, new Rectangle(pad, y, box.Width - 2 * pad, smallFont.Height), Color.FromArgb(30, 30, 30), flags);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                smallFont.Dispose();
                button.Font.Dispose();
                Font.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
