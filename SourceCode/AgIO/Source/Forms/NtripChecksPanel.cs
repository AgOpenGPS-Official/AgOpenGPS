using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// "Are the corrections good?": a few lines with a green, orange or red light each, in plain words.
    /// The technical numbers are one tap away behind the Details button.
    /// </summary>
    internal sealed class NtripChecksPanel : Control
    {
        private IReadOnlyList<CheckItem> items = new CheckItem[0];
        private string signature = "";

        private readonly RoundedButton details;
        private readonly RoundedButton switchButton;
        private readonly Font headerFont = new Font("Tahoma", 14.25F, FontStyle.Bold);
        private readonly Font titleFont = new Font("Tahoma", 13.5F, FontStyle.Bold);
        private readonly Font detailFont = new Font("Tahoma", 11.25F, FontStyle.Regular);

        public event EventHandler DetailsClicked;
        public event EventHandler SwitchClicked;

        public bool ShowDetailsButton
        {
            get { return details.Visible; }
            set { details.Visible = value; }
        }

        public NtripChecksPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            switchButton = new RoundedButton
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                Text = "Switch to fallback",
                Font = new Font("Tahoma", 14.25F, FontStyle.Bold),
                Visible = false,
            };
            switchButton.Click += (s, e) =>
            {
                EventHandler handler = SwitchClicked;
                if (handler != null) handler(this, EventArgs.Empty);
            };
            Controls.Add(switchButton);

            details = new RoundedButton
            {
                Dock = DockStyle.Bottom,
                Height = 64,
                Text = "Technical details...",
                Font = new Font("Tahoma", 14.25F, FontStyle.Bold),
            };
            details.Click += (s, e) =>
            {
                EventHandler handler = DetailsClicked;
                if (handler != null) handler(this, EventArgs.Empty);
            };
            Controls.Add(details);
        }

        /// <summary>The button for switching by hand between the main service and the fallback.</summary>
        public void SetSwitch(string text, bool visible, ButtonTone tone)
        {
            switchButton.Text = text;
            switchButton.Tone = tone;
            switchButton.Visible = visible;
        }

        public void SetChecks(IReadOnlyList<CheckItem> checks)
        {
            var sb = new StringBuilder();
            foreach (CheckItem c in checks) sb.Append((int)c.Level).Append('|').Append(c.Title).Append('|').Append(c.Detail).Append(';');
            string next = sb.ToString();
            if (next == signature) return;

            signature = next;
            items = checks;
            Invalidate();
        }

        private static Color LevelColor(CheckLevel level)
        {
            switch (level)
            {
                case CheckLevel.Ok: return NtripStatusCard.LightColor(NtripLight.Ok);
                case CheckLevel.Warn: return NtripStatusCard.LightColor(NtripLight.Warn);
                default: return NtripStatusCard.LightColor(NtripLight.Bad);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            const TextFormatFlags flags = TextFormatFlags.WordBreak | TextFormatFlags.NoPadding | TextFormatFlags.Left | TextFormatFlags.Top;
            int dot = 20;
            int left = 4 + dot + 10;
            int width = Math.Max(40, Width - left - 6);

            TextRenderer.DrawText(g, "Are the corrections good?", headerFont, new Point(4, 2), Color.Black);
            int y = headerFont.Height + 14;

            foreach (CheckItem item in items)
            {
                using (var lamp = new SolidBrush(LevelColor(item.Level)))
                {
                    g.FillEllipse(lamp, 4, y + 3, dot, dot);
                }

                int titleHeight = TextRenderer.MeasureText(g, item.Title, titleFont, new Size(width, 0), flags).Height;
                TextRenderer.DrawText(g, item.Title, titleFont, new Rectangle(left, y, width, titleHeight), Color.Black, flags);
                y += titleHeight;

                if (item.Detail.Length > 0)
                {
                    int detailHeight = TextRenderer.MeasureText(g, item.Detail, detailFont, new Size(width, 0), flags).Height;
                    TextRenderer.DrawText(g, item.Detail, detailFont, new Rectangle(left, y + 1, width, detailHeight), Color.FromArgb(40, 40, 40), flags);
                    y += detailHeight + 1;
                }

                y += 16;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                headerFont.Dispose();
                titleFont.Dispose();
                detailFont.Dispose();
                details.Font.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
