namespace AgOpenGPS
{
    partial class FormNudge
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
            this.btnSnapToPivot = new System.Windows.Forms.Button();
            this.bthOK = new System.Windows.Forms.Button();
            this.btnAdjLeft = new System.Windows.Forms.Button();
            this.btnAdjRight = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnZeroMove = new System.Windows.Forms.Button();
            this.btnHalfToolRight = new System.Windows.Forms.Button();
            this.btnHalfToolLeft = new System.Windows.Forms.Button();
            this.lblOffset = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.nudSnapDistance = new AgOpenGPS.NudlessNumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnapDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSnapToPivot
            // 
            this.btnSnapToPivot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSnapToPivot.BackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSnapToPivot.FlatAppearance.BorderSize = 0;
            this.btnSnapToPivot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapToPivot.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSnapToPivot.Image = global::AgOpenGPS.Properties.Resources.SnapToPivot;
            this.btnSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSnapToPivot.Location = new System.Drawing.Point(90, 10);
            this.btnSnapToPivot.Name = "btnSnapToPivot";
            this.btnSnapToPivot.Size = new System.Drawing.Size(69, 58);
            this.btnSnapToPivot.TabIndex = 6;
            this.btnSnapToPivot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSnapToPivot.UseVisualStyleBackColor = false;
            this.btnSnapToPivot.Click += new System.EventHandler(this.btnSnapToPivot_Click);
            // 
            // bthOK
            // 
            this.bthOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bthOK.BackColor = System.Drawing.Color.Transparent;
            this.bthOK.BackgroundImage = global::AgOpenGPS.Properties.Resources.OK64;
            this.bthOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bthOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.bthOK.FlatAppearance.BorderSize = 0;
            this.bthOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bthOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bthOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bthOK.Location = new System.Drawing.Point(58, 346);
            this.bthOK.Name = "bthOK";
            this.bthOK.Size = new System.Drawing.Size(64, 56);
            this.bthOK.TabIndex = 1;
            this.bthOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bthOK.UseVisualStyleBackColor = false;
            this.bthOK.Click += new System.EventHandler(this.bntOk_Click);
            // 
            // btnAdjLeft
            // 
            this.btnAdjLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjLeft.FlatAppearance.BorderSize = 0;
            this.btnAdjLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjLeft.Image = global::AgOpenGPS.Properties.Resources.SnapLeft;
            this.btnAdjLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjLeft.Location = new System.Drawing.Point(7, 90);
            this.btnAdjLeft.Name = "btnAdjLeft";
            this.btnAdjLeft.Size = new System.Drawing.Size(69, 58);
            this.btnAdjLeft.TabIndex = 4;
            this.btnAdjLeft.UseVisualStyleBackColor = false;
            this.btnAdjLeft.Click += new System.EventHandler(this.btnAdjLeft_Click);
            // 
            // btnAdjRight
            // 
            this.btnAdjRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjRight.FlatAppearance.BorderSize = 0;
            this.btnAdjRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjRight.Image = global::AgOpenGPS.Properties.Resources.SnapRight;
            this.btnAdjRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjRight.Location = new System.Drawing.Point(90, 90);
            this.btnAdjRight.Name = "btnAdjRight";
            this.btnAdjRight.Size = new System.Drawing.Size(69, 58);
            this.btnAdjRight.TabIndex = 5;
            this.btnAdjRight.UseVisualStyleBackColor = false;
            this.btnAdjRight.Click += new System.EventHandler(this.btnAdjRight_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnAdjLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnAdjRight, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.nudSnapDistance, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnZeroMove, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnHalfToolRight, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnHalfToolLeft, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSnapToPivot, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 38);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(150, 270);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.2963F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.66667F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.03704F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.62963F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(166, 302);
            this.tableLayoutPanel1.TabIndex = 416;
            // 
            // btnZeroMove
            // 
            this.btnZeroMove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZeroMove.BackColor = System.Drawing.Color.Transparent;
            this.btnZeroMove.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnZeroMove.FlatAppearance.BorderSize = 0;
            this.btnZeroMove.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnZeroMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnZeroMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnZeroMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroMove.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnZeroMove.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroMove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZeroMove.Location = new System.Drawing.Point(7, 10);
            this.btnZeroMove.Name = "btnZeroMove";
            this.btnZeroMove.Size = new System.Drawing.Size(69, 58);
            this.btnZeroMove.TabIndex = 534;
            this.btnZeroMove.UseVisualStyleBackColor = false;
            this.btnZeroMove.Click += new System.EventHandler(this.btnZeroMove_Click);
            // 
            // btnHalfToolRight
            // 
            this.btnHalfToolRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHalfToolRight.BackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHalfToolRight.FlatAppearance.BorderSize = 0;
            this.btnHalfToolRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHalfToolRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnHalfToolRight.Image = global::AgOpenGPS.Properties.Resources.SnapRightHalf;
            this.btnHalfToolRight.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnHalfToolRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHalfToolRight.Location = new System.Drawing.Point(90, 227);
            this.btnHalfToolRight.Name = "btnHalfToolRight";
            this.btnHalfToolRight.Size = new System.Drawing.Size(69, 58);
            this.btnHalfToolRight.TabIndex = 418;
            this.btnHalfToolRight.UseVisualStyleBackColor = false;
            this.btnHalfToolRight.Click += new System.EventHandler(this.btnHalfToolRight_Click);
            // 
            // btnHalfToolLeft
            // 
            this.btnHalfToolLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHalfToolLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnHalfToolLeft.FlatAppearance.BorderSize = 0;
            this.btnHalfToolLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHalfToolLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHalfToolLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnHalfToolLeft.Image = global::AgOpenGPS.Properties.Resources.SnapLeftHalf;
            this.btnHalfToolLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHalfToolLeft.Location = new System.Drawing.Point(7, 227);
            this.btnHalfToolLeft.Name = "btnHalfToolLeft";
            this.btnHalfToolLeft.Size = new System.Drawing.Size(69, 58);
            this.btnHalfToolLeft.TabIndex = 418;
            this.btnHalfToolLeft.UseVisualStyleBackColor = false;
            this.btnHalfToolLeft.Click += new System.EventHandler(this.btnHalfToolLeft_Click);
            // 
            // lblOffset
            // 
            this.lblOffset.AutoSize = true;
            this.lblOffset.BackColor = System.Drawing.Color.Transparent;
            this.lblOffset.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffset.ForeColor = System.Drawing.Color.Black;
            this.lblOffset.Location = new System.Drawing.Point(4, 10);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.Size = new System.Drawing.Size(74, 23);
            this.lblOffset.TabIndex = 417;
            this.lblOffset.Text = "0.35 >";
            this.lblOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // nudSnapDistance
            // 
            this.nudSnapDistance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.SetColumnSpan(this.nudSnapDistance, 2);
            this.nudSnapDistance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSnapDistance.Location = new System.Drawing.Point(51, 166);
            this.nudSnapDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSnapDistance.Name = "nudSnapDistance";
            this.nudSnapDistance.ReadOnly = true;
            this.nudSnapDistance.Size = new System.Drawing.Size(64, 36);
            this.nudSnapDistance.TabIndex = 415;
            this.nudSnapDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSnapDistance.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.nudSnapDistance.Click += new System.EventHandler(this.nudSnapDistance_Click);
            // 
            // FormNudge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(180, 410);
            this.Controls.Add(this.lblOffset);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.bthOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(180, 410);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(180, 410);
            this.Name = "FormNudge";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditTrack_FormClosing);
            this.Load += new System.EventHandler(this.FormEditTrack_Load);
            this.MouseEnter += new System.EventHandler(this.FormEditTrack_MouseEnter);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSnapDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnAdjRight;
        private System.Windows.Forms.Button btnAdjLeft;
        private System.Windows.Forms.Button bthOK;
        public System.Windows.Forms.Button btnSnapToPivot;
        private NudlessNumericUpDown nudSnapDistance;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnZeroMove;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnHalfToolLeft;
        private System.Windows.Forms.Button btnHalfToolRight;
    }
}