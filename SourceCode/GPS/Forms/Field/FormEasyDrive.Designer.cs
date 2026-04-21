namespace AgOpenGPS
{
    partial class FormEasyDrive
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblPivot = new System.Windows.Forms.Label();
            this.nudWidth = new AgOpenGPS.NudlessNumericUpDown();
            this.nudPivotDistance = new AgOpenGPS.NudlessNumericUpDown();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUnitWidth = new System.Windows.Forms.Label();
            this.lblUnitPivot = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPivotDistance)).BeginInit();
            this.SuspendLayout();
            //
            // lblInfo
            //
            this.lblInfo.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Italic);
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblInfo.Location = new System.Drawing.Point(20, 15);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(360, 80);
            this.lblInfo.Text = "info";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // lblWidth
            //
            this.lblWidth.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblWidth.Location = new System.Drawing.Point(20, 105);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(360, 40);
            this.lblWidth.Text = "Working Width";
            this.lblWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudWidth
            //
            this.nudWidth.BackColor = System.Drawing.Color.AliceBlue;
            this.nudWidth.DecimalPlaces = 1;
            this.nudWidth.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Bold);
            this.nudWidth.InterceptArrowKeys = false;
            this.nudWidth.Location = new System.Drawing.Point(20, 150);
            this.nudWidth.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudWidth.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.ReadOnly = true;
            this.nudWidth.Size = new System.Drawing.Size(280, 54);
            this.nudWidth.TabIndex = 0;
            this.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudWidth.Value = new decimal(new int[] { 60, 0, 0, 65536 });
            this.nudWidth.Click += new System.EventHandler(this.nudWidth_Click);
            //
            // lblUnitWidth
            //
            this.lblUnitWidth.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblUnitWidth.Location = new System.Drawing.Point(310, 150);
            this.lblUnitWidth.Name = "lblUnitWidth";
            this.lblUnitWidth.Size = new System.Drawing.Size(70, 54);
            this.lblUnitWidth.Text = "m";
            this.lblUnitWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblPivot
            //
            this.lblPivot.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblPivot.Location = new System.Drawing.Point(20, 220);
            this.lblPivot.Name = "lblPivot";
            this.lblPivot.Size = new System.Drawing.Size(360, 40);
            this.lblPivot.Text = "Hitch Length";
            this.lblPivot.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // nudPivotDistance
            //
            this.nudPivotDistance.BackColor = System.Drawing.Color.AliceBlue;
            this.nudPivotDistance.DecimalPlaces = 2;
            this.nudPivotDistance.Font = new System.Drawing.Font("Tahoma", 28F, System.Drawing.FontStyle.Bold);
            this.nudPivotDistance.InterceptArrowKeys = false;
            this.nudPivotDistance.Location = new System.Drawing.Point(20, 265);
            this.nudPivotDistance.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudPivotDistance.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            this.nudPivotDistance.Name = "nudPivotDistance";
            this.nudPivotDistance.ReadOnly = true;
            this.nudPivotDistance.Size = new System.Drawing.Size(280, 54);
            this.nudPivotDistance.TabIndex = 1;
            this.nudPivotDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPivotDistance.Value = new decimal(new int[] { 100, 0, 0, 131072 });
            this.nudPivotDistance.Click += new System.EventHandler(this.nudPivotDistance_Click);
            //
            // lblUnitPivot
            //
            this.lblUnitPivot.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblUnitPivot.Location = new System.Drawing.Point(310, 265);
            this.lblUnitPivot.Name = "lblUnitPivot";
            this.lblUnitPivot.Size = new System.Drawing.Size(70, 54);
            this.lblUnitPivot.Text = "m";
            this.lblUnitPivot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnStart
            //
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(201)))));
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnStart.FlatAppearance.BorderSize = 2;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnStart.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnStart.Location = new System.Drawing.Point(20, 350);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(170, 80);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Next";
            this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            //
            // btnCancel
            //
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(205)))), ((int)(((byte)(210)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnCancel.FlatAppearance.BorderSize = 2;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(210, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(170, 80);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // FormEasyDrive
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.ControlBox = false;
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.lblUnitWidth);
            this.Controls.Add(this.lblPivot);
            this.Controls.Add(this.nudPivotDistance);
            this.Controls.Add(this.lblUnitPivot);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormEasyDrive";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Easy Drive";
            this.Load += new System.EventHandler(this.FormEasyDrive_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPivotDistance)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblPivot;
        private AgOpenGPS.NudlessNumericUpDown nudWidth;
        private AgOpenGPS.NudlessNumericUpDown nudPivotDistance;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblUnitWidth;
        private System.Windows.Forms.Label lblUnitPivot;
    }
}
