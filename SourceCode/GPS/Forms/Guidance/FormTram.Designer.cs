namespace AgOpenGPS
{
    partial class FormTram
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
            this.labelSprayWidth = new System.Windows.Forms.Label();
            this.labelPasses = new System.Windows.Forms.Label();
            this.btnMode = new System.Windows.Forms.Button();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTrack = new System.Windows.Forms.Label();
            this.labelTrack = new System.Windows.Forms.Label();
            this.lblTramWidth = new System.Windows.Forms.Label();
            this.labelSeed = new System.Windows.Forms.Label();
            this.lblSeedWidth = new System.Windows.Forms.Label();
            this.labelMode = new System.Windows.Forms.Label();
            this.btnDnTrams = new System.Windows.Forms.Button();
            this.btnUpTrams = new System.Windows.Forms.Button();
            this.tbarTramAlpha = new System.Windows.Forms.TrackBar();
            this.lblAplha = new System.Windows.Forms.Label();
            this.labelAlpha = new System.Windows.Forms.Label();
            this.nudPasses = new AgOpenGPS.NudlessNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.tbarTramAlpha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPasses)).BeginInit();
            this.SuspendLayout();
            // 
            // labelSprayWidth
            // 
            this.labelSprayWidth.AutoSize = true;
            this.labelSprayWidth.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelSprayWidth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSprayWidth.ForeColor = System.Drawing.Color.Black;
            this.labelSprayWidth.Location = new System.Drawing.Point(87, 298);
            this.labelSprayWidth.Name = "labelSprayWidth";
            this.labelSprayWidth.Size = new System.Drawing.Size(49, 19);
            this.labelSprayWidth.TabIndex = 424;
            this.labelSprayWidth.Text = "Spray";
            this.labelSprayWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPasses
            // 
            this.labelPasses.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelPasses.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPasses.ForeColor = System.Drawing.Color.Black;
            this.labelPasses.Location = new System.Drawing.Point(61, 144);
            this.labelPasses.Name = "labelPasses";
            this.labelPasses.Size = new System.Drawing.Size(160, 24);
            this.labelPasses.TabIndex = 435;
            this.labelPasses.Text = "Passes";
            this.labelPasses.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnMode
            // 
            this.btnMode.BackColor = System.Drawing.Color.Transparent;
            this.btnMode.BackgroundImage = global::AgOpenGPS.Properties.Resources.TramAll;
            this.btnMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMode.FlatAppearance.BorderSize = 0;
            this.btnMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMode.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMode.ForeColor = System.Drawing.Color.White;
            this.btnMode.Location = new System.Drawing.Point(135, 19);
            this.btnMode.Name = "btnMode";
            this.btnMode.Size = new System.Drawing.Size(86, 88);
            this.btnMode.TabIndex = 460;
            this.btnMode.UseVisualStyleBackColor = false;
            this.btnMode.Click += new System.EventHandler(this.btnMode_Click);
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.ForeColor = System.Drawing.Color.White;
            this.btnSwapAB.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(1, 32);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(72, 62);
            this.btnSwapAB.TabIndex = 438;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.btnSwapAB_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(3, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 67);
            this.btnCancel.TabIndex = 421;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(256, 275);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(98, 67);
            this.btnExit.TabIndex = 234;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTrack
            // 
            this.lblTrack.AutoSize = true;
            this.lblTrack.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTrack.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrack.ForeColor = System.Drawing.Color.Black;
            this.lblTrack.Location = new System.Drawing.Point(191, 325);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(68, 23);
            this.lblTrack.TabIndex = 465;
            this.lblTrack.Text = "10 cm";
            this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTrack
            // 
            this.labelTrack.AutoSize = true;
            this.labelTrack.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelTrack.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTrack.ForeColor = System.Drawing.Color.Black;
            this.labelTrack.Location = new System.Drawing.Point(86, 328);
            this.labelTrack.Name = "labelTrack";
            this.labelTrack.Size = new System.Drawing.Size(48, 19);
            this.labelTrack.TabIndex = 464;
            this.labelTrack.Text = "Track";
            this.labelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTramWidth
            // 
            this.lblTramWidth.AutoSize = true;
            this.lblTramWidth.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTramWidth.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTramWidth.ForeColor = System.Drawing.Color.Black;
            this.lblTramWidth.Location = new System.Drawing.Point(191, 295);
            this.lblTramWidth.Name = "lblTramWidth";
            this.lblTramWidth.Size = new System.Drawing.Size(68, 23);
            this.lblTramWidth.TabIndex = 462;
            this.lblTramWidth.Text = "10 cm";
            this.lblTramWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelSeed
            // 
            this.labelSeed.AutoSize = true;
            this.labelSeed.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelSeed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSeed.ForeColor = System.Drawing.Color.Black;
            this.labelSeed.Location = new System.Drawing.Point(87, 267);
            this.labelSeed.Name = "labelSeed";
            this.labelSeed.Size = new System.Drawing.Size(43, 19);
            this.labelSeed.TabIndex = 466;
            this.labelSeed.Text = "Seed";
            this.labelSeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSeedWidth
            // 
            this.lblSeedWidth.AutoSize = true;
            this.lblSeedWidth.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSeedWidth.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeedWidth.ForeColor = System.Drawing.Color.Black;
            this.lblSeedWidth.Location = new System.Drawing.Point(191, 264);
            this.lblSeedWidth.Name = "lblSeedWidth";
            this.lblSeedWidth.Size = new System.Drawing.Size(68, 23);
            this.lblSeedWidth.TabIndex = 467;
            this.lblSeedWidth.Text = "10 cm";
            this.lblSeedWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelMode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMode.ForeColor = System.Drawing.Color.Black;
            this.labelMode.Location = new System.Drawing.Point(155, 3);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(47, 19);
            this.labelMode.TabIndex = 468;
            this.labelMode.Text = "Mode";
            this.labelMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnDnTrams
            // 
            this.btnDnTrams.BackColor = System.Drawing.Color.Transparent;
            this.btnDnTrams.FlatAppearance.BorderSize = 0;
            this.btnDnTrams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDnTrams.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDnTrams.ForeColor = System.Drawing.Color.White;
            this.btnDnTrams.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnDnTrams.Location = new System.Drawing.Point(-3, 155);
            this.btnDnTrams.Name = "btnDnTrams";
            this.btnDnTrams.Size = new System.Drawing.Size(72, 62);
            this.btnDnTrams.TabIndex = 469;
            this.btnDnTrams.UseVisualStyleBackColor = false;
            this.btnDnTrams.Click += new System.EventHandler(this.btnDnTrams_Click);
            // 
            // btnUpTrams
            // 
            this.btnUpTrams.BackColor = System.Drawing.Color.Transparent;
            this.btnUpTrams.FlatAppearance.BorderSize = 0;
            this.btnUpTrams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpTrams.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpTrams.ForeColor = System.Drawing.Color.White;
            this.btnUpTrams.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnUpTrams.Location = new System.Drawing.Point(205, 155);
            this.btnUpTrams.Name = "btnUpTrams";
            this.btnUpTrams.Size = new System.Drawing.Size(72, 62);
            this.btnUpTrams.TabIndex = 470;
            this.btnUpTrams.UseVisualStyleBackColor = false;
            this.btnUpTrams.Click += new System.EventHandler(this.btnUpTrams_Click);
            // 
            // tbarTramAlpha
            // 
            this.tbarTramAlpha.LargeChange = 10;
            this.tbarTramAlpha.Location = new System.Drawing.Point(303, 25);
            this.tbarTramAlpha.Maximum = 100;
            this.tbarTramAlpha.Minimum = 10;
            this.tbarTramAlpha.Name = "tbarTramAlpha";
            this.tbarTramAlpha.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbarTramAlpha.Size = new System.Drawing.Size(45, 216);
            this.tbarTramAlpha.SmallChange = 10;
            this.tbarTramAlpha.TabIndex = 471;
            this.tbarTramAlpha.Value = 60;
            this.tbarTramAlpha.Scroll += new System.EventHandler(this.tbarTramAlpha_Scroll);
            // 
            // lblAplha
            // 
            this.lblAplha.AutoSize = true;
            this.lblAplha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblAplha.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAplha.ForeColor = System.Drawing.Color.Black;
            this.lblAplha.Location = new System.Drawing.Point(287, 237);
            this.lblAplha.Name = "lblAplha";
            this.lblAplha.Size = new System.Drawing.Size(52, 19);
            this.lblAplha.TabIndex = 472;
            this.lblAplha.Text = "100%";
            this.lblAplha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAlpha
            // 
            this.labelAlpha.AutoSize = true;
            this.labelAlpha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelAlpha.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlpha.ForeColor = System.Drawing.Color.Black;
            this.labelAlpha.Location = new System.Drawing.Point(290, 3);
            this.labelAlpha.Name = "labelAlpha";
            this.labelAlpha.Size = new System.Drawing.Size(50, 19);
            this.labelAlpha.TabIndex = 473;
            this.labelAlpha.Text = "Alpha";
            this.labelAlpha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudPasses
            // 
            this.nudPasses.BackColor = System.Drawing.Color.White;
            this.nudPasses.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudPasses.InterceptArrowKeys = false;
            this.nudPasses.Location = new System.Drawing.Point(93, 171);
            this.nudPasses.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudPasses.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPasses.Name = "nudPasses";
            this.nudPasses.ReadOnly = true;
            this.nudPasses.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudPasses.Size = new System.Drawing.Size(92, 46);
            this.nudPasses.TabIndex = 433;
            this.nudPasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPasses.Value = new decimal(new int[] {
            777,
            0,
            0,
            0});
            this.nudPasses.Click += new System.EventHandler(this.nudPasses_Click);
            // 
            // FormTram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(353, 354);
            this.ControlBox = false;
            this.Controls.Add(this.lblAplha);
            this.Controls.Add(this.labelAlpha);
            this.Controls.Add(this.tbarTramAlpha);
            this.Controls.Add(this.btnUpTrams);
            this.Controls.Add(this.btnDnTrams);
            this.Controls.Add(this.labelMode);
            this.Controls.Add(this.labelSeed);
            this.Controls.Add(this.lblSeedWidth);
            this.Controls.Add(this.nudPasses);
            this.Controls.Add(this.lblTrack);
            this.Controls.Add(this.labelPasses);
            this.Controls.Add(this.btnSwapAB);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelTrack);
            this.Controls.Add(this.labelSprayWidth);
            this.Controls.Add(this.lblTramWidth);
            this.Controls.Add(this.btnMode);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTram";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AB Line Tramline";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTram_FormClosing);
            this.Load += new System.EventHandler(this.FormTram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbarTramAlpha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelSprayWidth;
        private NudlessNumericUpDown nudPasses;
        private System.Windows.Forms.Label labelPasses;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.Button btnMode;
        private System.Windows.Forms.Label lblTramWidth;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.Label labelTrack;
        private System.Windows.Forms.Label labelSeed;
        private System.Windows.Forms.Label lblSeedWidth;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.Button btnDnTrams;
        private System.Windows.Forms.Button btnUpTrams;
        private System.Windows.Forms.TrackBar tbarTramAlpha;
        private System.Windows.Forms.Label lblAplha;
        private System.Windows.Forms.Label labelAlpha;
    }
}