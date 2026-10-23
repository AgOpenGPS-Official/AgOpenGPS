using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AgIO
{
    internal enum ButtonTone
    {
        Neutral,
        Positive,
        Negative,
    }

    /// <summary>
    /// A large touch button with rounded corners. Neutral by default (light with a black border),
    /// green for "Start" and red for "Stop" through <see cref="Tone"/>.
    /// </summary>
    [DefaultEvent("Click")]
    internal class RoundedButton : Control, IButtonControl
    {
        private bool isDown;
        private ButtonTone tone;
        private int cornerRadius = 14;

        public RoundedButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.Selectable, false);
            BackColor = Color.Transparent;
            Font = new Font("Tahoma", 14.25F, FontStyle.Bold);
            Size = new Size(120, 64);
        }

        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult { get; set; }

        [DefaultValue(14)]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = Math.Max(0, value); Invalidate(); }
        }

        /// <summary>Neutral, green for an action that starts something, red for one that stops it.</summary>
        [DefaultValue(ButtonTone.Neutral)]
        public ButtonTone Tone
        {
            get { return tone; }
            set { if (tone != value) { tone = value; Invalidate(); } }
        }

        public void NotifyDefault(bool value)
        {
        }

        public void PerformClick()
        {
            OnClick(EventArgs.Empty);
        }

        protected override void OnClick(EventArgs e)
        {
            Form form = FindForm();
            if (form != null && DialogResult != DialogResult.None) form.DialogResult = DialogResult;
            base.OnClick(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) { isDown = true; Invalidate(); }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isDown = false;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isDown = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color fill;
            Color text;
            if (tone == ButtonTone.Positive)
            {
                fill = isDown ? Color.FromArgb(14, 80, 54) : Color.FromArgb(21, 110, 75);
                text = Color.White;
            }
            else if (tone == ButtonTone.Negative)
            {
                fill = isDown ? Color.FromArgb(125, 18, 12) : Color.FromArgb(180, 30, 20);
                text = Color.White;
            }
            else
            {
                fill = isDown ? Color.FromArgb(205, 205, 205) : Color.FromArgb(250, 250, 250);
                text = Color.Black;
            }
            if (!Enabled)
            {
                fill = Color.FromArgb(225, 225, 225);
                text = Color.FromArgb(110, 110, 110);
            }

            var bounds = new Rectangle(1, 1, Width - 3, Height - 3);
            using (GraphicsPath path = RoundedRectangle(bounds, cornerRadius))
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(Color.Black, 2F))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            TextRenderer.DrawText(g, Text, Font, ClientRectangle, text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine
                | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
        }

        private static GraphicsPath RoundedRectangle(Rectangle r, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 0)
            {
                path.AddRectangle(r);
                return path;
            }
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
