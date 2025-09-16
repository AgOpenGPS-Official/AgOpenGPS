namespace AgOpenGPS
{
    partial class FormWebCam
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
            deviceComboBox = new System.Windows.Forms.ComboBox();
            stopButton = new System.Windows.Forms.Button();
            startButton = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // deviceComboBox
            // 
            deviceComboBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            deviceComboBox.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            deviceComboBox.FormattingEnabled = true;
            deviceComboBox.Location = new System.Drawing.Point(13, 234);
            deviceComboBox.Name = "deviceComboBox";
            deviceComboBox.Size = new System.Drawing.Size(196, 27);
            deviceComboBox.TabIndex = 11;
            deviceComboBox.SelectedIndexChanged += deviceComboBox_SelectedIndexChanged;
            // 
            // stopButton
            // 
            stopButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            stopButton.BackgroundImage = Properties.Resources.Stop;
            stopButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            stopButton.Enabled = false;
            stopButton.FlatAppearance.BorderSize = 0;
            stopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            stopButton.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            stopButton.Location = new System.Drawing.Point(319, 229);
            stopButton.Margin = new System.Windows.Forms.Padding(2);
            stopButton.Name = "stopButton";
            stopButton.Size = new System.Drawing.Size(75, 37);
            stopButton.TabIndex = 13;
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // startButton
            // 
            startButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            startButton.BackgroundImage = Properties.Resources.Play;
            startButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            startButton.Enabled = false;
            startButton.FlatAppearance.BorderSize = 0;
            startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            startButton.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            startButton.Location = new System.Drawing.Point(226, 229);
            startButton.Margin = new System.Windows.Forms.Padding(2);
            startButton.Name = "startButton";
            startButton.Size = new System.Drawing.Size(75, 37);
            startButton.TabIndex = 12;
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // FormWebCam
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(398, 268);
            Controls.Add(deviceComboBox);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            Name = "FormWebCam";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "WebCam";
            Load += FormWebCam_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button startButton;
    }
}