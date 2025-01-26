namespace AgOpenGPS
{
    partial class FormBoundaryPlayer
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
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblArea = new System.Windows.Forms.Label();
            this.lblMetersInches = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxIsRecBoundaryWhenSectionOn = new System.Windows.Forms.CheckBox();
            this.btnAntennaTool = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.btnLeftRight = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPausePlay = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnDeleteLast = new System.Windows.Forms.Button();
            this.nudOffset = new AgOpenGPS.NudlessNumericUpDown();
            this.lblPoints = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(2, 458);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 141;
            this.label1.Text = "Area:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.BackColor = System.Drawing.Color.Transparent;
            this.lblArea.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArea.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblArea.Location = new System.Drawing.Point(56, 458);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(46, 23);
            this.lblArea.TabIndex = 142;
            this.lblArea.Text = "999";
            this.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMetersInches
            // 
            this.lblMetersInches.BackColor = System.Drawing.Color.Transparent;
            this.lblMetersInches.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMetersInches.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblMetersInches.Location = new System.Drawing.Point(137, 14);
            this.lblMetersInches.Name = "lblMetersInches";
            this.lblMetersInches.Size = new System.Drawing.Size(102, 23);
            this.lblMetersInches.TabIndex = 151;
            this.lblMetersInches.Text = "cm";
            this.lblMetersInches.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(161, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 23);
            this.label3.TabIndex = 152;
            this.label3.Text = "B";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(161, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 153;
            this.label4.Text = "D";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(161, 389);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 23);
            this.label5.TabIndex = 154;
            this.label5.Text = "R";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxIsRecBoundaryWhenSectionOn
            // 
            this.cboxIsRecBoundaryWhenSectionOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsRecBoundaryWhenSectionOn.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsRecBoundaryWhenSectionOn.FlatAppearance.BorderColor = System.Drawing.Color.DarkGray;
            this.cboxIsRecBoundaryWhenSectionOn.FlatAppearance.BorderSize = 2;
            this.cboxIsRecBoundaryWhenSectionOn.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.cboxIsRecBoundaryWhenSectionOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsRecBoundaryWhenSectionOn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsRecBoundaryWhenSectionOn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsRecBoundaryWhenSectionOn.Image = global::AgOpenGPS.Properties.Resources.BoundarySectionControlOnOff;
            this.cboxIsRecBoundaryWhenSectionOn.Location = new System.Drawing.Point(155, 54);
            this.cboxIsRecBoundaryWhenSectionOn.Name = "cboxIsRecBoundaryWhenSectionOn";
            this.cboxIsRecBoundaryWhenSectionOn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsRecBoundaryWhenSectionOn.Size = new System.Drawing.Size(80, 84);
            this.cboxIsRecBoundaryWhenSectionOn.TabIndex = 468;
            this.cboxIsRecBoundaryWhenSectionOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsRecBoundaryWhenSectionOn.UseVisualStyleBackColor = false;
            this.cboxIsRecBoundaryWhenSectionOn.Click += new System.EventHandler(this.cboxIsRecBoundaryWhenSectionOn_Click);
            // 
            // btnAntennaTool
            // 
            this.btnAntennaTool.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnAntennaTool.FlatAppearance.BorderSize = 0;
            this.btnAntennaTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAntennaTool.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecordTool;
            this.btnAntennaTool.Location = new System.Drawing.Point(16, 291);
            this.btnAntennaTool.Name = "btnAntennaTool";
            this.btnAntennaTool.Size = new System.Drawing.Size(80, 64);
            this.btnAntennaTool.TabIndex = 155;
            this.btnAntennaTool.UseVisualStyleBackColor = true;
            this.btnAntennaTool.Click += new System.EventHandler(this.btnAntennaTool_Click);
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.BackColor = System.Drawing.Color.Transparent;
            this.btnAddPoint.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnAddPoint.FlatAppearance.BorderSize = 0;
            this.btnAddPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPoint.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnAddPoint.Image = global::AgOpenGPS.Properties.Resources.PointAdd;
            this.btnAddPoint.Location = new System.Drawing.Point(155, 290);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(80, 64);
            this.btnAddPoint.TabIndex = 143;
            this.btnAddPoint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddPoint.UseVisualStyleBackColor = false;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // btnLeftRight
            // 
            this.btnLeftRight.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnLeftRight.FlatAppearance.BorderSize = 0;
            this.btnLeftRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeftRight.Image = global::AgOpenGPS.Properties.Resources.BoundaryLeft;
            this.btnLeftRight.Location = new System.Drawing.Point(5, 175);
            this.btnLeftRight.Name = "btnLeftRight";
            this.btnLeftRight.Size = new System.Drawing.Size(80, 64);
            this.btnLeftRight.TabIndex = 68;
            this.btnLeftRight.UseVisualStyleBackColor = true;
            this.btnLeftRight.Click += new System.EventHandler(this.btnLeftRight_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnStop.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnStop.Location = new System.Drawing.Point(5, 395);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 78);
            this.btnStop.TabIndex = 140;
            this.btnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPausePlay
            // 
            this.btnPausePlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPausePlay.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnPausePlay.FlatAppearance.BorderSize = 0;
            this.btnPausePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPausePlay.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnPausePlay.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnPausePlay.Location = new System.Drawing.Point(155, 409);
            this.btnPausePlay.Name = "btnPausePlay";
            this.btnPausePlay.Size = new System.Drawing.Size(80, 64);
            this.btnPausePlay.TabIndex = 139;
            this.btnPausePlay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPausePlay.UseVisualStyleBackColor = false;
            this.btnPausePlay.Click += new System.EventHandler(this.btnPausePlay_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.Transparent;
            this.btnRestart.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnRestart.FlatAppearance.BorderSize = 0;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnRestart.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnRestart.Location = new System.Drawing.Point(21, 77);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(52, 52);
            this.btnRestart.TabIndex = 147;
            this.btnRestart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnDeleteLast
            // 
            this.btnDeleteLast.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteLast.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnDeleteLast.FlatAppearance.BorderSize = 0;
            this.btnDeleteLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteLast.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnDeleteLast.Image = global::AgOpenGPS.Properties.Resources.PointDelete;
            this.btnDeleteLast.Location = new System.Drawing.Point(155, 172);
            this.btnDeleteLast.Name = "btnDeleteLast";
            this.btnDeleteLast.Size = new System.Drawing.Size(80, 64);
            this.btnDeleteLast.TabIndex = 144;
            this.btnDeleteLast.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteLast.UseVisualStyleBackColor = false;
            this.btnDeleteLast.Click += new System.EventHandler(this.btnDeleteLast_Click);
            // 
            // nudOffset
            // 
            this.nudOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOffset.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOffset.Location = new System.Drawing.Point(11, 5);
            this.nudOffset.Maximum = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.ReadOnly = true;
            this.nudOffset.Size = new System.Drawing.Size(125, 46);
            this.nudOffset.TabIndex = 149;
            this.nudOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudOffset.Value = new decimal(new int[] {
            4999,
            0,
            0,
            0});
            this.nudOffset.Click += new System.EventHandler(this.nudOffset_Click);
            // 
            // lblPoints
            // 
            this.lblPoints.AutoSize = true;
            this.lblPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblPoints.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoints.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPoints.Location = new System.Drawing.Point(62, 370);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(46, 23);
            this.lblPoints.TabIndex = 469;
            this.lblPoints.Text = "999";
            this.lblPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(1, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 23);
            this.label2.TabIndex = 470;
            this.label2.Text = "Points:";
            // 
            // FormBoundaryPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(249, 483);
            this.ControlBox = false;
            this.Controls.Add(this.lblPoints);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboxIsRecBoundaryWhenSectionOn);
            this.Controls.Add(this.btnAntennaTool);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddPoint);
            this.Controls.Add(this.btnLeftRight);
            this.Controls.Add(this.btnPausePlay);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnDeleteLast);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudOffset);
            this.Controls.Add(this.lblMetersInches);
            this.Controls.Add(this.btnStop);
            this.Font = new System.Drawing.Font("Tahoma", 14F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBoundaryPlayer";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stop Record Pause Boundary";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBoundaryPlayer_FormClosing);
            this.Load += new System.EventHandler(this.FormBoundaryPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPausePlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Button btnDeleteLast;
        private System.Windows.Forms.Button btnRestart;
        private NudlessNumericUpDown nudOffset;
        private System.Windows.Forms.Button btnLeftRight;
        private System.Windows.Forms.Label lblMetersInches;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnAntennaTool;
        private System.Windows.Forms.CheckBox cboxIsRecBoundaryWhenSectionOn;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.Label label2;
    }
}