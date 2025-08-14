using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AgLibrary.Controls
{
    // Source: https://www.codeproject.com/Articles/629644/Auto-repeat-Button-in-10-Minutes
    public partial class RepeatButton : Button
    {
        private MouseEventArgs _mouseDownArgs;

        public RepeatButton()
        {
            InitializeComponent();
        }

        [DefaultValue(400)]
        [Category("Enhanced")]
        [Description("Initial delay. Time in milliseconds between button press and first repeat action.")]
        public int InitialDelay { get; set; } = 400;

        [DefaultValue(62)]
        [Category("Enhanced")]
        [Description("Repeat Interval. Repeat between each repeat action while button is hold pressed.")]
        public int RepeatInterval { get; set; } = 62;

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _mouseDownArgs = mevent;
            timerRepeater.Enabled = false;
            timerRepeater_Tick(this, EventArgs.Empty);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            timerRepeater.Enabled = false;
        }

        private void timerRepeater_Tick(object sender, EventArgs e)
        {
            base.OnMouseDown(_mouseDownArgs);

            timerRepeater.Interval = timerRepeater.Enabled ? RepeatInterval : InitialDelay;
            timerRepeater.Enabled = true;
        }
    }
}
