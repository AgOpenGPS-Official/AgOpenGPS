using System.Drawing;
using System.Windows.Forms;

namespace TestFormLauncher
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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

        #endregion

        private Button btnFormYes;
        private Button btnFormYesWithCancel;
        private Button btnFormTimedMessage;
        private Button btnFormYesLongMessage;
        private Button btnFormTimedMessageLong;
        private Label lblTitle;
        private Label lblInstructions;
    }
}
