using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTimedMessage : Form
    {
        //class variables
        //private FormGPS mf = null;

        public FormTimedMessage(int timeInMsec, string titleString, string messageString)
        {
            InitializeComponent();

            // Set text first
            lblMessage.Text = titleString;
            lblMessage2.Text = messageString;

            // Force labels to measure their content
            lblMessage.AutoSize = true;
            lblMessage2.AutoSize = true;

            // Calculate required size based on actual label sizes
            int maxLabelWidth = Math.Max(lblMessage.PreferredWidth, lblMessage2.PreferredWidth);
            int totalLabelHeight = lblMessage.PreferredHeight + lblMessage2.PreferredHeight + lblMessage2.Top;

            // Add padding for panel and form borders
            int requiredWidth = maxLabelWidth + 60;  // Extra space for padding
            int requiredHeight = totalLabelHeight + 30;  // Extra space for padding

            // Set form size
            this.ClientSize = new System.Drawing.Size(requiredWidth, requiredHeight);

            timer1.Interval = timeInMsec;
            this.Left = 20;
            this.Top = 20;
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