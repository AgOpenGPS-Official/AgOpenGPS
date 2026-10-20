using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// The summary of an NTRIP profile on the profile screen: the main service and the fallback as two
    /// clearly separated blocks, with the address in bold and the mountpoint in italics.
    /// </summary>
    internal sealed class NtripProfileSummary : Control
    {
        private static readonly Color MainColor = Color.FromArgb(21, 110, 75);
        private static readonly Color FallbackColor = Color.FromArgb(176, 96, 0);
        private static readonly Color Muted = Color.FromArgb(90, 90, 90);

        private readonly Font titleFont = new Font("Tahoma", 11.25F, FontStyle.Bold);
        private readonly Font hostFont = new Font("Tahoma", 15F, FontStyle.Bold);
        private readonly Font labelFont = new Font("Tahoma", 11.25F, FontStyle.Regular);
        private readonly Font mountFont = new Font("Tahoma", 13.5F, FontStyle.Italic | FontStyle.Bold);
        private readonly Font noteFont = new Font("Tahoma", 11.25F, FontStyle.Italic);
        private readonly Font pillFont = new Font("Tahoma", 10.5F, FontStyle.Bold);

        private NtripProfileSet profile;
        private bool inUse;
        private string message = "";

        public NtripProfileSummary()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
            Size = new Size(440, 240);
        }

        /// <summary>Shows a profile; null shows the message instead.</summary>
        public void SetProfile(NtripProfileSet set, bool isInUse, string text)
        {
            profile = set;
            inUse = isInUse;
            message = text ?? "";
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var border = new Pen(Color.Black, 2F))
                g.DrawRectangle(border, 1, 1, Width - 3, Height - 3);

            if (profile == null)
            {
                TextRenderer.DrawText(g, message, labelFont, new Rectangle(16, 16, Width - 32, Height - 32), Muted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                return;
            }

            int y = 12;
            if (inUse)
            {
                var pill = new Rectangle(16, y, 120, 26);
                using (var path = Rounded(pill, 13))
                using (var fill = new SolidBrush(MainColor))
                    g.FillPath(fill, path);
                TextRenderer.DrawText(g, "IN USE NOW", pillFont, pill, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                y += 36;
            }

            y = DrawBlock(g, y, "MAIN SERVICE", MainColor, profile.Main, null);

            using (var line = new Pen(Color.FromArgb(200, 200, 200), 1F))
                g.DrawLine(line, 16, y + 4, Width - 16, y + 4);
            y += 12;

            FallbackDefinition f = profile.Fallback;
            if (!f.Enabled)
            {
                DrawBlock(g, y, "FALLBACK", FallbackColor, null, "Off. No second service is used.");
            }
            else if (f.Type == FallbackType.SerialRadio)
            {
                string radio = "Serial radio on " + (f.RadioPort.Length > 0 ? f.RadioPort : "(no port yet)");
                DrawBlock(g, y, "FALLBACK", FallbackColor, null, radio + "\n" + SwitchNote(f));
            }
            else
            {
                DrawBlock(g, y, "FALLBACK", FallbackColor, f.Service, SwitchNote(f));
            }
        }

        private static string SwitchNote(FallbackDefinition f)
        {
            return f.AutoSwitch
                ? "Switches by itself after " + f.SwitchAfterFailures + " failed tries"
                : "You switch by hand";
        }

        private int DrawBlock(Graphics g, int y, string title, Color accent, ServiceDefinition service, string note)
        {
            int left = 28;
            int width = Width - left - 14;
            int top = y;

            TextRenderer.DrawText(g, title, titleFont, new Point(left, y), accent, TextFormatFlags.NoPadding);
            y += titleFont.Height + 2;

            if (service != null)
            {
                if (service.IsComplete)
                {
                    TextRenderer.DrawText(g, service.Host + ":" + service.Port, hostFont, new Point(left, y), Color.Black, TextFormatFlags.NoPadding);
                    y += hostFont.Height + 1;

                    if (service.Mount.Length > 0)
                    {
                        TextRenderer.DrawText(g, "Mountpoint ", labelFont, new Point(left, y + 2), Muted, TextFormatFlags.NoPadding);
                        int labelWidth = TextRenderer.MeasureText(g, "Mountpoint ", labelFont, new Size(width, 0), TextFormatFlags.NoPadding).Width;
                        TextRenderer.DrawText(g, service.Mount, mountFont, new Point(left + labelWidth, y), Color.Black, TextFormatFlags.NoPadding);
                    }
                    else
                    {
                        TextRenderer.DrawText(g, "No mountpoint chosen", noteFont, new Point(left, y + 2), Muted, TextFormatFlags.NoPadding);
                    }
                    y += mountFont.Height + 1;
                }
                else
                {
                    TextRenderer.DrawText(g, "Not filled in yet", noteFont, new Point(left, y + 2), Muted, TextFormatFlags.NoPadding);
                    y += noteFont.Height + 4;
                }
            }

            if (!string.IsNullOrEmpty(note))
            {
                foreach (string part in note.Split('\n'))
                {
                    TextRenderer.DrawText(g, part, noteFont, new Rectangle(left, y, width, noteFont.Height + 4), Muted, TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
                    y += noteFont.Height + 2;
                }
            }

            using (var bar = new SolidBrush(accent))
                g.FillRectangle(bar, 16, top + 2, 5, Math.Max(10, y - top - 4));

            return y + 2;
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var fonts = new List<Font> { titleFont, hostFont, labelFont, mountFont, noteFont, pillFont };
                foreach (Font f in fonts) f.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
