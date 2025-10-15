using System;
using System.Windows.Forms;

namespace AgLibrary.Forms
{
    public partial class FormTimedMessage : Form
    {
        public FormTimedMessage(TimeSpan duration, string title, string message)
        {
            InitializeComponent();
            lblTitle.Text = title;
            lblMessage.Text = message;
            timer1.Interval = (int)duration.TotalMilliseconds;
        }

        private void Timer1Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer1.Dispose();
            Dispose();
            Close();
        }
    }
}