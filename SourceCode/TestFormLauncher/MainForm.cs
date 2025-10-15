using System;
using System.Drawing;
using System.Windows.Forms;
using AgLibrary.Forms;

namespace TestFormLauncher
{
    /// <summary>
    /// Test launcher for consolidated forms from commit ec2541b0.
    /// This application allows manual testing of FormYes and FormTimedMessage
    /// that were consolidated from GPS/AgIO/ModSim into AgLibrary.
    /// </summary>
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnFormYes_Click(object sender, EventArgs e)
        {
            using (var form = new FormYes("Do you want to continue?"))
            {
                var result = form.ShowDialog();
                MessageBox.Show($"User clicked: {result}", "Result");
            }
        }

        private void BtnFormYesWithCancel_Click(object sender, EventArgs e)
        {
            using (var form = new FormYes("Save changes before closing?", showCancel: true))
            {
                var result = form.ShowDialog();
                MessageBox.Show($"User clicked: {result}", "Result");
            }
        }

        private void BtnFormYesLongMessage_Click(object sender, EventArgs e)
        {
            using (var form = new FormYes(
                "This is a much longer message to test the dynamic width calculation feature. " +
                "The form should automatically resize to fit this text properly."))
            {
                var result = form.ShowDialog();
                MessageBox.Show($"User clicked: {result}", "Result");
            }
        }

        private void BtnFormTimedMessage_Click(object sender, EventArgs e)
        {
            var form = new FormTimedMessage(TimeSpan.FromSeconds(3), "Notification", "This message will close in 3 seconds");
            form.Show();
        }

        private void BtnFormTimedMessageLong_Click(object sender, EventArgs e)
        {
            var form = new FormTimedMessage(TimeSpan.FromSeconds(5), "Important Notice",
                "This is a longer message that will close automatically in 5 seconds. " +
                "The form width should adjust to fit this text.");
            form.Show();
        }
    }
}
