using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AgIO
{
    /// <summary>
    /// Two or three big buttons in one row, one of them chosen: a rounded outline, the chosen part green.
    /// The choices are one text with a bar between them, e.g. "Main service|Fallback service".
    /// </summary>
    [DefaultEvent("SelectedIndexChanged")]
    internal class SegmentedControl : Control
    {
        private string itemsText = "One|Two";
        private int selectedIndex;

        public event EventHandler SelectedIndexChanged;

        public SegmentedControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.StandardClick, true);
            BackColor = Color.Transparent;
            Font = new Font("Tahoma", 12.75F, FontStyle.Bold);
            Size = new Size(400, 56);
        }

        /// <summary>The choices, separated by a bar.</summary>
        [DefaultValue("One|Two")]
        public string ItemsText
        {
            get { return itemsText; }
            set { itemsText = value ?? ""; if (selectedIndex >= Items.Length) selectedIndex = 0; Invalidate(); }
        }

        private string[] Items
        {
            get { return itemsText.Length == 0 ? new string[0] : itemsText.Split('|'); }
        }

        [DefaultValue(0)]
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < 0 || value >= Items.Length || value == selectedIndex) return;
                selectedIndex = value;
                Invalidate();
                EventHandler handler = SelectedIndexChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            string[] items = Items;
            if (items.Length == 0 || e.Button != MouseButtons.Left || !ClientRectangle.Contains(e.Location)) return;
            SelectedIndex = Math.Min(items.Length - 1, e.X * items.Length / Math.Max(1, Width));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            string[] items = Items;
            if (items.Length == 0) return;

            int radius = 14;
            var bounds = new Rectangle(1, 1, Width - 3, Height - 3);
            float part = bounds.Width / (float)items.Length;

            using (GraphicsPath outline = Rounded(bounds, radius))
            {
                using (var white = new SolidBrush(Color.FromArgb(250, 250, 250)))
                    g.FillPath(white, outline);

                // the chosen part, clipped to the outline so the corners follow it
                g.SetClip(outline);
                var chosen = new RectangleF(bounds.X + part * selectedIndex, bounds.Y, part, bounds.Height);
                using (var green = new SolidBrush(Color.FromArgb(21, 110, 75)))
                    g.FillRectangle(green, chosen);
                g.ResetClip();

                using (var pen = new Pen(Color.Black, 2F))
                {
                    for (int i = 1; i < items.Length; i++)
                    {
                        float x = bounds.X + part * i;
                        g.DrawLine(pen, x, bounds.Y, x, bounds.Bottom);
                    }
                    g.DrawPath(pen, outline);
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                var cell = new Rectangle((int)(bounds.X + part * i), bounds.Y, (int)part, bounds.Height);
                TextRenderer.DrawText(g, items[i], Font, cell, i == selectedIndex ? Color.White : Color.Black,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
            }
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
    }
}
