namespace AgOpenGPS
{
    partial class FormEnterFlag
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
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.labelPoint = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnYellow = new System.Windows.Forms.Button();
            this.btnRed = new System.Windows.Forms.Button();
            this.btnGreen = new System.Windows.Forms.Button();
            this.btnImportFlags = new System.Windows.Forms.Button();
            this.btnExportFlags = new System.Windows.Forms.Button();
            this.nudLatitude = new AgOpenGPS.NudlessNumericUpDown();
            this.nudLongitude = new AgOpenGPS.NudlessNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudLatitude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongitude)).BeginInit();
            this.SuspendLayout();
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(510, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(163, 25);
            this.label9.TabIndex = 202;
            this.label9.Text = "( +180 to -180 )";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(215, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 25);
            this.label8.TabIndex = 201;
            this.label8.Text = "( +90 to -90)";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(457, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 25);
            this.label1.TabIndex = 200;
            this.label1.Text = "Longitude";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(160, 15);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(252, 25);
            this.label18.TabIndex = 199;
            this.label18.Text = "Latitude";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPoint
            // 
            this.labelPoint.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPoint.Location = new System.Drawing.Point(22, 76);
            this.labelPoint.Name = "labelPoint";
            this.labelPoint.Size = new System.Drawing.Size(125, 34);
            this.labelPoint.TabIndex = 205;
            this.labelPoint.Text = "Point A";
            this.labelPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(413, 188);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 58);
            this.btnCancel.TabIndex = 430;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnYellow
            // 
            this.btnYellow.BackColor = System.Drawing.Color.Transparent;
            this.btnYellow.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnYellow.FlatAppearance.BorderSize = 0;
            this.btnYellow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYellow.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYellow.Image = global::AgOpenGPS.Properties.Resources.FlagYel;
            this.btnYellow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYellow.Location = new System.Drawing.Point(515, 192);
            this.btnYellow.Name = "btnYellow";
            this.btnYellow.Size = new System.Drawing.Size(87, 58);
            this.btnYellow.TabIndex = 431;
            this.btnYellow.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYellow.UseVisualStyleBackColor = false;
            this.btnYellow.Click += new System.EventHandler(this.btnRed_Click);
            // 
            // btnRed
            // 
            this.btnRed.BackColor = System.Drawing.Color.Transparent;
            this.btnRed.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnRed.FlatAppearance.BorderSize = 0;
            this.btnRed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRed.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRed.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.btnRed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRed.Location = new System.Drawing.Point(672, 193);
            this.btnRed.Name = "btnRed";
            this.btnRed.Size = new System.Drawing.Size(87, 58);
            this.btnRed.TabIndex = 432;
            this.btnRed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRed.UseVisualStyleBackColor = false;
            this.btnRed.Click += new System.EventHandler(this.btnRed_Click);
            // 
            // btnGreen
            // 
            this.btnGreen.BackColor = System.Drawing.Color.Transparent;
            this.btnGreen.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnGreen.FlatAppearance.BorderSize = 0;
            this.btnGreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreen.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGreen.Image = global::AgOpenGPS.Properties.Resources.FlagGrn;
            this.btnGreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGreen.Location = new System.Drawing.Point(597, 192);
            this.btnGreen.Name = "btnGreen";
            this.btnGreen.Size = new System.Drawing.Size(87, 58);
            this.btnGreen.TabIndex = 433;
            this.btnGreen.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGreen.UseVisualStyleBackColor = false;
            this.btnGreen.Click += new System.EventHandler(this.btnRed_Click);
            // 
            // btnImportFlags
            // 
            this.btnImportFlags.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnImportFlags.Location = new System.Drawing.Point(13, 179);
            this.btnImportFlags.Margin = new System.Windows.Forms.Padding(4);
            this.btnImportFlags.Name = "btnImportFlags";
            this.btnImportFlags.Size = new System.Drawing.Size(177, 71);
            this.btnImportFlags.TabIndex = 434;
            this.btnImportFlags.Text = "Import Flags";
            this.btnImportFlags.UseVisualStyleBackColor = true;
            this.btnImportFlags.Click += new System.EventHandler(this.btnImportFlags_Click);
            // 
            // btnExportFlags
            // 
            this.btnExportFlags.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnExportFlags.Location = new System.Drawing.Point(211, 180);
            this.btnExportFlags.Margin = new System.Windows.Forms.Padding(4);
            this.btnExportFlags.Name = "btnExportFlags";
            this.btnExportFlags.Size = new System.Drawing.Size(171, 71);
            this.btnExportFlags.TabIndex = 435;
            this.btnExportFlags.Text = "Export Flags";
            this.btnExportFlags.UseVisualStyleBackColor = true;
            this.btnExportFlags.Click += new System.EventHandler(this.btnExportFlags_Click);
            // 
            // nudLatitude
            // 
            this.nudLatitude.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLatitude.DecimalPlaces = 7;
            this.nudLatitude.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLatitude.Location = new System.Drawing.Point(152, 68);
            this.nudLatitude.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.nudLatitude.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.nudLatitude.Name = "nudLatitude";
            this.nudLatitude.Size = new System.Drawing.Size(274, 52);
            this.nudLatitude.TabIndex = 198;
            this.nudLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLatitude.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLatitude.Click += new System.EventHandler(this.nudLatitude_Click);
            // 
            // nudLongitude
            // 
            this.nudLongitude.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLongitude.DecimalPlaces = 7;
            this.nudLongitude.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLongitude.Location = new System.Drawing.Point(448, 68);
            this.nudLongitude.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.nudLongitude.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.nudLongitude.Name = "nudLongitude";
            this.nudLongitude.Size = new System.Drawing.Size(298, 52);
            this.nudLongitude.TabIndex = 197;
            this.nudLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLongitude.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLongitude.Click += new System.EventHandler(this.nudLongitude_Click);
            // 
            // FormEnterFlag
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(771, 266);
            this.ControlBox = false;
            this.Controls.Add(this.btnExportFlags);
            this.Controls.Add(this.btnImportFlags);
            this.Controls.Add(this.btnGreen);
            this.Controls.Add(this.btnRed);
            this.Controls.Add(this.btnYellow);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelPoint);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.nudLatitude);
            this.Controls.Add(this.nudLongitude);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormEnterFlag";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter AB or A+";
            this.Load += new System.EventHandler(this.FormEnterAB_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudLatitude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongitude)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label18;
        private NudlessNumericUpDown nudLatitude;
        private NudlessNumericUpDown nudLongitude;
        private System.Windows.Forms.Label labelPoint;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnYellow;
        private System.Windows.Forms.Button btnRed;
        private System.Windows.Forms.Button btnGreen;
        private System.Windows.Forms.Button btnImportFlags;
        private System.Windows.Forms.Button btnExportFlags;
    }
}