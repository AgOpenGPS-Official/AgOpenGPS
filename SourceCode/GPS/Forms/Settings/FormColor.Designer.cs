namespace AgOpenGPS
{
    partial class FormColor
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bntOK = new System.Windows.Forms.Button();
            this.btnFrameDay = new System.Windows.Forms.Button();
            this.btnFrameNight = new System.Windows.Forms.Button();
            this.btnFieldNight = new System.Windows.Forms.Button();
            this.btnFieldDay = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnDayText = new System.Windows.Forms.Button();
            this.btnNightText = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSmoothCam = new System.Windows.Forms.Label();
            this.labelSmooth = new System.Windows.Forms.Label();
            this.hsbarSmooth = new System.Windows.Forms.HScrollBar();
            this.labelDirect = new System.Windows.Forms.Label();
            this.labelCameraBehavior = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.labelReset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            // 
            // bntOK
            // 
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.FlatAppearance.BorderSize = 0;
            this.bntOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(587, 319);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(66, 65);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // btnFrameDay
            // 
            this.btnFrameDay.BackColor = System.Drawing.Color.Transparent;
            this.btnFrameDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFrameDay.FlatAppearance.BorderSize = 0;
            this.btnFrameDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFrameDay.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFrameDay.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnFrameDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFrameDay.Location = new System.Drawing.Point(247, 12);
            this.btnFrameDay.Name = "btnFrameDay";
            this.btnFrameDay.Size = new System.Drawing.Size(59, 57);
            this.btnFrameDay.TabIndex = 4;
            this.btnFrameDay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFrameDay.UseVisualStyleBackColor = false;
            this.btnFrameDay.Click += new System.EventHandler(this.btnFrameDay_Click);
            // 
            // btnFrameNight
            // 
            this.btnFrameNight.BackColor = System.Drawing.Color.Transparent;
            this.btnFrameNight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFrameNight.FlatAppearance.BorderSize = 0;
            this.btnFrameNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFrameNight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFrameNight.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnFrameNight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFrameNight.Location = new System.Drawing.Point(247, 181);
            this.btnFrameNight.Name = "btnFrameNight";
            this.btnFrameNight.Size = new System.Drawing.Size(59, 57);
            this.btnFrameNight.TabIndex = 5;
            this.btnFrameNight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFrameNight.UseVisualStyleBackColor = false;
            this.btnFrameNight.Click += new System.EventHandler(this.btnFrameNight_Click);
            // 
            // btnFieldNight
            // 
            this.btnFieldNight.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldNight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFieldNight.FlatAppearance.BorderSize = 0;
            this.btnFieldNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldNight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFieldNight.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnFieldNight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldNight.Location = new System.Drawing.Point(164, 280);
            this.btnFieldNight.Name = "btnFieldNight";
            this.btnFieldNight.Size = new System.Drawing.Size(59, 57);
            this.btnFieldNight.TabIndex = 7;
            this.btnFieldNight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFieldNight.UseVisualStyleBackColor = false;
            this.btnFieldNight.Click += new System.EventHandler(this.btnFieldNight_Click);
            // 
            // btnFieldDay
            // 
            this.btnFieldDay.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFieldDay.FlatAppearance.BorderSize = 0;
            this.btnFieldDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldDay.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFieldDay.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnFieldDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldDay.Location = new System.Drawing.Point(164, 94);
            this.btnFieldDay.Name = "btnFieldDay";
            this.btnFieldDay.Size = new System.Drawing.Size(59, 57);
            this.btnFieldDay.TabIndex = 6;
            this.btnFieldDay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFieldDay.UseVisualStyleBackColor = false;
            this.btnFieldDay.Click += new System.EventHandler(this.btnFieldDay_Click);
            // 
            // btnSwap
            // 
            this.btnSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSwap.FlatAppearance.BorderSize = 0;
            this.btnSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwap.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSwap.Image = global::AgOpenGPS.Properties.Resources.ConD_AutoDayNight;
            this.btnSwap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSwap.Location = new System.Drawing.Point(396, 181);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(121, 65);
            this.btnSwap.TabIndex = 10;
            this.btnSwap.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // btnDayText
            // 
            this.btnDayText.BackColor = System.Drawing.Color.Transparent;
            this.btnDayText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDayText.FlatAppearance.BorderSize = 0;
            this.btnDayText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayText.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDayText.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnDayText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDayText.Location = new System.Drawing.Point(12, 38);
            this.btnDayText.Name = "btnDayText";
            this.btnDayText.Size = new System.Drawing.Size(59, 57);
            this.btnDayText.TabIndex = 11;
            this.btnDayText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnDayText.UseVisualStyleBackColor = false;
            this.btnDayText.Click += new System.EventHandler(this.btnDayText_Click);
            // 
            // btnNightText
            // 
            this.btnNightText.BackColor = System.Drawing.Color.Transparent;
            this.btnNightText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNightText.FlatAppearance.BorderSize = 0;
            this.btnNightText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNightText.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnNightText.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.btnNightText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNightText.Location = new System.Drawing.Point(12, 224);
            this.btnNightText.Name = "btnNightText";
            this.btnNightText.Size = new System.Drawing.Size(59, 57);
            this.btnNightText.TabIndex = 12;
            this.btnNightText.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnNightText.UseVisualStyleBackColor = false;
            this.btnNightText.Click += new System.EventHandler(this.btnNightText_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(80)))));
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(17, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "ABC123";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(17, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "ABC123";
            // 
            // lblSmoothCam
            // 
            this.lblSmoothCam.AutoSize = true;
            this.lblSmoothCam.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmoothCam.ForeColor = System.Drawing.Color.Black;
            this.lblSmoothCam.Location = new System.Drawing.Point(468, 80);
            this.lblSmoothCam.Name = "lblSmoothCam";
            this.lblSmoothCam.Size = new System.Drawing.Size(63, 25);
            this.lblSmoothCam.TabIndex = 465;
            this.lblSmoothCam.Text = "65%";
            // 
            // labelSmooth
            // 
            this.labelSmooth.AutoSize = true;
            this.labelSmooth.Location = new System.Drawing.Point(368, 89);
            this.labelSmooth.Name = "labelSmooth";
            this.labelSmooth.Size = new System.Drawing.Size(51, 16);
            this.labelSmooth.TabIndex = 464;
            this.labelSmooth.Text = "Smooth";
            // 
            // hsbarSmooth
            // 
            this.hsbarSmooth.LargeChange = 1;
            this.hsbarSmooth.Location = new System.Drawing.Point(366, 108);
            this.hsbarSmooth.Minimum = 2;
            this.hsbarSmooth.Name = "hsbarSmooth";
            this.hsbarSmooth.Size = new System.Drawing.Size(262, 43);
            this.hsbarSmooth.TabIndex = 463;
            this.hsbarSmooth.Value = 50;
            this.hsbarSmooth.ValueChanged += new System.EventHandler(this.hsbarSmooth_ValueChanged);
            // 
            // labelDirect
            // 
            this.labelDirect.AutoSize = true;
            this.labelDirect.Location = new System.Drawing.Point(585, 89);
            this.labelDirect.Name = "labelDirect";
            this.labelDirect.Size = new System.Drawing.Size(40, 16);
            this.labelDirect.TabIndex = 466;
            this.labelDirect.Text = "Direct";
            // 
            // labelCameraBehavior
            // 
            this.labelCameraBehavior.AutoSize = true;
            this.labelCameraBehavior.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCameraBehavior.ForeColor = System.Drawing.Color.Black;
            this.labelCameraBehavior.Location = new System.Drawing.Point(400, 38);
            this.labelCameraBehavior.Name = "labelCameraBehavior";
            this.labelCameraBehavior.Size = new System.Drawing.Size(204, 25);
            this.labelCameraBehavior.TabIndex = 467;
            this.labelCameraBehavior.Text = "Camera Behaviour";
            // 
            // btnReset
            // 
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnReset.Image = global::AgOpenGPS.Properties.Resources.ResetColors;
            this.btnReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnReset.Location = new System.Drawing.Point(396, 272);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(121, 65);
            this.btnReset.TabIndex = 468;
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // labelReset
            // 
            this.labelReset.AutoSize = true;
            this.labelReset.Location = new System.Drawing.Point(411, 340);
            this.labelReset.Name = "labelReset";
            this.labelReset.Size = new System.Drawing.Size(98, 16);
            this.labelReset.TabIndex = 469;
            this.labelReset.Text = "Reset to Default";
            // 
            // FormColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.LightGray;
            this.BackgroundImage = global::AgOpenGPS.Properties.Resources.ColorBackGnd;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(665, 396);
            this.ControlBox = false;
            this.Controls.Add(this.labelReset);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.labelCameraBehavior);
            this.Controls.Add(this.labelDirect);
            this.Controls.Add(this.lblSmoothCam);
            this.Controls.Add(this.labelSmooth);
            this.Controls.Add(this.hsbarSmooth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNightText);
            this.Controls.Add(this.btnDayText);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnFieldNight);
            this.Controls.Add(this.btnFieldDay);
            this.Controls.Add(this.btnFrameNight);
            this.Controls.Add(this.btnFrameDay);
            this.Controls.Add(this.bntOK);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormColor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Color Set";
            this.Load += new System.EventHandler(this.FormDisplaySettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnFrameDay;
        private System.Windows.Forms.Button btnFrameNight;
        private System.Windows.Forms.Button btnFieldNight;
        private System.Windows.Forms.Button btnFieldDay;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.Button btnDayText;
        private System.Windows.Forms.Button btnNightText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSmoothCam;
        private System.Windows.Forms.Label labelSmooth;
        private System.Windows.Forms.HScrollBar hsbarSmooth;
        private System.Windows.Forms.Label labelDirect;
        private System.Windows.Forms.Label labelCameraBehavior;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label labelReset;
    }
}