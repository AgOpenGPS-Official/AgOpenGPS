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
    public class MainForm : Form
    {
        private Button btnFormYes;
        private Button btnFormYesWithCancel;
        private Button btnFormTimedMessage;
        private Button btnFormYesLongMessage;
        private Button btnFormTimedMessageLong;
        private Label lblTitle;
        private Label lblInstructions;

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Consolidated Forms Test Launcher";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label
            {
                Text = "Consolidated Forms Test Launcher",
                Font = new Font("Arial", 14, FontStyle.Bold),
                AutoSize = false,
                Width = 450,
                Height = 30,
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblInstructions = new Label
            {
                Text = "Click buttons below to test the consolidated forms.\n" +
                       "Commit: ec2541b0 - Consolidate duplicate forms into AgLibrary",
                AutoSize = false,
                Width = 450,
                Height = 40,
                Location = new Point(20, 60),
                TextAlign = ContentAlignment.TopCenter
            };

            btnFormYes = new Button
            {
                Text = "Launch FormYes (No Cancel)",
                Width = 200,
                Height = 40,
                Location = new Point(20, 120)
            };
            btnFormYes.Click += BtnFormYes_Click;

            btnFormYesWithCancel = new Button
            {
                Text = "Launch FormYes (With Cancel)",
                Width = 200,
                Height = 40,
                Location = new Point(260, 120)
            };
            btnFormYesWithCancel.Click += BtnFormYesWithCancel_Click;

            btnFormYesLongMessage = new Button
            {
                Text = "Launch FormYes (Long Message)",
                Width = 200,
                Height = 40,
                Location = new Point(20, 180)
            };
            btnFormYesLongMessage.Click += BtnFormYesLongMessage_Click;

            btnFormTimedMessage = new Button
            {
                Text = "Launch FormTimedMessage (3s)",
                Width = 200,
                Height = 40,
                Location = new Point(260, 180)
            };
            btnFormTimedMessage.Click += BtnFormTimedMessage_Click;

            btnFormTimedMessageLong = new Button
            {
                Text = "FormTimedMessage (Long, 5s)",
                Width = 200,
                Height = 40,
                Location = new Point(140, 240)
            };
            btnFormTimedMessageLong.Click += BtnFormTimedMessageLong_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblInstructions);
            this.Controls.Add(btnFormYes);
            this.Controls.Add(btnFormYesWithCancel);
            this.Controls.Add(btnFormYesLongMessage);
            this.Controls.Add(btnFormTimedMessage);
            this.Controls.Add(btnFormTimedMessageLong);
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
            var form = new FormTimedMessage(TimeSpan.FromMilliseconds(3000), "Notification", "This message will close in 3 seconds");
            form.Show();
        }

        private void BtnFormTimedMessageLong_Click(object sender, EventArgs e)
        {
            var form = new FormTimedMessage(TimeSpan.FromMilliseconds(5000), "Important Notice",
                "This is a longer message that will close automatically in 5 seconds. " +
                "The form width should adjust to fit this text.");
            form.Show();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
