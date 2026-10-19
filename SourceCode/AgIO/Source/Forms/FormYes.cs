using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormYes : Form
    {
        public FormYes(string messageStr, bool showCancel = false)
        {
            InitializeComponent();

            lblMessage2.Text = messageStr;
            btnCancel.Visible = showCancel;

            // A long message gets more room: the window grows and the buttons move down with it.
            int needed = TextRenderer.MeasureText(messageStr, lblMessage2.Font, new System.Drawing.Size(lblMessage2.Width, 0), TextFormatFlags.WordBreak).Height;
            int available = btnSerialOK.Top - lblMessage2.Top - 10;
            if (needed > available)
            {
                int extra = Math.Min(needed - available, Screen.FromControl(this).WorkingArea.Height - ClientSize.Height - 80);
                if (extra > 0)
                {
                    ClientSize = new System.Drawing.Size(ClientSize.Width, ClientSize.Height + extra);
                    lblMessage2.Height += extra;
                }
            }

            this.AcceptButton = btnSerialOK;
            if (showCancel) this.CancelButton = btnCancel;
        }
    }
}