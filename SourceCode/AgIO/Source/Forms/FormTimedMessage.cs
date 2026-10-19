using System;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormTimedMessage : Form
    {
        public FormTimedMessage(int timeInMsec, string titleStr, string messageStr)
        {
            InitializeComponent();

            lblTitle.Text = titleStr;
            lblMessage2.Text = messageStr;

            timer1.Interval = timeInMsec;

            // Long messages wrap below each other and the window grows with them.
            int maxWidth = Math.Min(760, Screen.FromControl(this).WorkingArea.Width - 80);
            lblMessage2.MaximumSize = new System.Drawing.Size(maxWidth - 100, 0);
            lblTitle.MaximumSize = new System.Drawing.Size(maxWidth - 40, 0);

            Width = Math.Max(300, Math.Min(maxWidth, lblMessage2.PreferredWidth + 120));
            int needed = lblMessage2.Top + lblMessage2.PreferredHeight + 30;
            if (needed > ClientSize.Height) ClientSize = new System.Drawing.Size(ClientSize.Width, needed);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Dispose();
            Dispose();
            Close();
        }
    }
}