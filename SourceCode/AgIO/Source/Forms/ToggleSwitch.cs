using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AgIO
{
    /// <summary>An on/off switch for a finger: a rounded track with a round knob, green when on.</summary>
    [DefaultEvent("CheckedChanged")]
    internal class ToggleSwitch : Control
    {
        private bool isChecked;

        public event EventHandler CheckedChanged;

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.StandardClick, true);
            BackColor = Color.Transparent;
            Size = new Size(84, 46);
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get { return isChecked; }
            set
            {
                if (isChecked == value) return;
                isChecked = value;
                Invalidate();
                EventHandler handler = CheckedChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = !Checked;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int h = Height - 3;
            var track = new Rectangle(1, 1, Width - 3, h);
            Color fill = !Enabled ? Color.FromArgb(215, 215, 215) : isChecked ? Color.FromArgb(21, 110, 75) : Color.FromArgb(150, 150, 150);

            using (var path = new GraphicsPath())
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(Color.Black, 2F))
            {
                path.AddArc(track.X, track.Y, h, h, 90, 180);
                path.AddArc(track.Right - h, track.Y, h, h, 270, 180);
                path.CloseFigure();
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            int knob = h - 10;
            int x = isChecked ? track.Right - knob - 6 : track.X + 6;
            using (var brush = new SolidBrush(Color.White))
            using (var pen = new Pen(Color.Black, 1.5F))
            {
                g.FillEllipse(brush, x, track.Y + 5, knob, knob);
                g.DrawEllipse(pen, x, track.Y + 5, knob, knob);
            }
        }
    }
}
