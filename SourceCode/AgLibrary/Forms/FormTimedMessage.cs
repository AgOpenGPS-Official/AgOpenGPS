using System;
using System.Windows.Forms;

namespace AgLibrary.Forms
{
    public partial class FormTimedMessage : Form
    {
        public FormTimedMessage(int timeInMsec, string titleStr, string messageStr)
        {
            InitializeComponent();

            lblTitle.Text = titleStr;
            lblMessage2.Text = messageStr;

            timer1.Interval = timeInMsec;

            int messWidth = messageStr.Length;
            Width = messWidth * 15 + 120;
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