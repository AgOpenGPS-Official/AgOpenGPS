namespace AgOpenGPS
{
    partial class FormFieldData
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
            this.lblTimeRemaining = new System.Windows.Forms.Label();
            this.labelRemain = new System.Windows.Forms.Label();
            this.lblAreaRemain = new System.Windows.Forms.Label();
            this.lblWorkRate = new System.Windows.Forms.Label();
            this.labelRate = new System.Windows.Forms.Label();
            this.lblTotalArea = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.lblApplied = new System.Windows.Forms.Label();
            this.labelApplied = new System.Windows.Forms.Label();
            this.lblRemainPercent = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblActualLessOverlap = new System.Windows.Forms.Label();
            this.labelApplied2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblOverlapPercent = new System.Windows.Forms.Label();
            this.labelOverlap = new System.Windows.Forms.Label();
            this.labelActual = new System.Windows.Forms.Label();
            this.labelWorked = new System.Windows.Forms.Label();
            this.lblActualRemain = new System.Windows.Forms.Label();
            this.labelRemain2 = new System.Windows.Forms.Label();
            this.labelAreaValue = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDistance = new System.Windows.Forms.Label();
            this.labelDistanceDriven = new System.Windows.Forms.Label();
            this.labelTrip = new System.Windows.Forms.Label();
            this.btnTripReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblTimeRemaining
            // 
            this.lblTimeRemaining.AutoSize = true;
            this.lblTimeRemaining.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeRemaining.ForeColor = System.Drawing.Color.White;
            this.lblTimeRemaining.Location = new System.Drawing.Point(114, 153);
            this.lblTimeRemaining.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTimeRemaining.Name = "lblTimeRemaining";
            this.lblTimeRemaining.Size = new System.Drawing.Size(18, 23);
            this.lblTimeRemaining.TabIndex = 479;
            this.lblTimeRemaining.Text = "-";
            // 
            // labelRemain
            // 
            this.labelRemain.AutoSize = true;
            this.labelRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemain.ForeColor = System.Drawing.Color.White;
            this.labelRemain.Location = new System.Drawing.Point(12, 95);
            this.labelRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRemain.Name = "labelRemain";
            this.labelRemain.Size = new System.Drawing.Size(80, 23);
            this.labelRemain.TabIndex = 478;
            this.labelRemain.Text = "Remain:";
            // 
            // lblAreaRemain
            // 
            this.lblAreaRemain.AutoSize = true;
            this.lblAreaRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAreaRemain.ForeColor = System.Drawing.Color.White;
            this.lblAreaRemain.Location = new System.Drawing.Point(114, 95);
            this.lblAreaRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAreaRemain.Name = "lblAreaRemain";
            this.lblAreaRemain.Size = new System.Drawing.Size(18, 23);
            this.lblAreaRemain.TabIndex = 480;
            this.lblAreaRemain.Text = "-";
            // 
            // lblWorkRate
            // 
            this.lblWorkRate.AutoSize = true;
            this.lblWorkRate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWorkRate.ForeColor = System.Drawing.Color.White;
            this.lblWorkRate.Location = new System.Drawing.Point(101, 325);
            this.lblWorkRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblWorkRate.Name = "lblWorkRate";
            this.lblWorkRate.Size = new System.Drawing.Size(18, 23);
            this.lblWorkRate.TabIndex = 482;
            this.lblWorkRate.Text = "-";
            // 
            // labelRate
            // 
            this.labelRate.AutoSize = true;
            this.labelRate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRate.ForeColor = System.Drawing.Color.White;
            this.labelRate.Location = new System.Drawing.Point(10, 325);
            this.labelRate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRate.Name = "labelRate";
            this.labelRate.Size = new System.Drawing.Size(55, 23);
            this.labelRate.TabIndex = 481;
            this.labelRate.Text = "Rate:";
            // 
            // lblTotalArea
            // 
            this.lblTotalArea.AutoSize = true;
            this.lblTotalArea.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalArea.ForeColor = System.Drawing.Color.White;
            this.lblTotalArea.Location = new System.Drawing.Point(99, 0);
            this.lblTotalArea.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTotalArea.Name = "lblTotalArea";
            this.lblTotalArea.Size = new System.Drawing.Size(18, 23);
            this.lblTotalArea.TabIndex = 484;
            this.lblTotalArea.Text = "-";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(13, 0);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(58, 23);
            this.labelTotal.TabIndex = 483;
            this.labelTotal.Text = "Total:";
            // 
            // lblApplied
            // 
            this.lblApplied.AutoSize = true;
            this.lblApplied.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplied.ForeColor = System.Drawing.Color.White;
            this.lblApplied.Location = new System.Drawing.Point(114, 66);
            this.lblApplied.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblApplied.Name = "lblApplied";
            this.lblApplied.Size = new System.Drawing.Size(18, 23);
            this.lblApplied.TabIndex = 486;
            this.lblApplied.Text = "-";
            // 
            // labelApplied
            // 
            this.labelApplied.AutoSize = true;
            this.labelApplied.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplied.ForeColor = System.Drawing.Color.White;
            this.labelApplied.Location = new System.Drawing.Point(13, 66);
            this.labelApplied.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelApplied.Name = "labelApplied";
            this.labelApplied.Size = new System.Drawing.Size(79, 23);
            this.labelApplied.TabIndex = 485;
            this.labelApplied.Text = "Applied:";
            // 
            // lblRemainPercent
            // 
            this.lblRemainPercent.AutoSize = true;
            this.lblRemainPercent.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemainPercent.ForeColor = System.Drawing.Color.White;
            this.lblRemainPercent.Location = new System.Drawing.Point(114, 123);
            this.lblRemainPercent.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblRemainPercent.Name = "lblRemainPercent";
            this.lblRemainPercent.Size = new System.Drawing.Size(18, 23);
            this.lblRemainPercent.TabIndex = 487;
            this.lblRemainPercent.Text = "-";
            this.lblRemainPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 19);
            this.label1.TabIndex = 488;
            this.label1.Text = "____________________";
            // 
            // lblActualLessOverlap
            // 
            this.lblActualLessOverlap.AutoSize = true;
            this.lblActualLessOverlap.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualLessOverlap.ForeColor = System.Drawing.Color.White;
            this.lblActualLessOverlap.Location = new System.Drawing.Point(112, 229);
            this.lblActualLessOverlap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblActualLessOverlap.Name = "lblActualLessOverlap";
            this.lblActualLessOverlap.Size = new System.Drawing.Size(18, 23);
            this.lblActualLessOverlap.TabIndex = 490;
            this.lblActualLessOverlap.Text = "-";
            // 
            // labelApplied2
            // 
            this.labelApplied2.AutoSize = true;
            this.labelApplied2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplied2.ForeColor = System.Drawing.Color.White;
            this.labelApplied2.Location = new System.Drawing.Point(9, 229);
            this.labelApplied2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelApplied2.Name = "labelApplied2";
            this.labelApplied2.Size = new System.Drawing.Size(79, 23);
            this.labelApplied2.TabIndex = 489;
            this.labelApplied2.Text = "Applied:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(4, 203);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 19);
            this.label7.TabIndex = 491;
            this.label7.Text = "____________________";
            // 
            // lblOverlapPercent
            // 
            this.lblOverlapPercent.AutoSize = true;
            this.lblOverlapPercent.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverlapPercent.ForeColor = System.Drawing.Color.White;
            this.lblOverlapPercent.Location = new System.Drawing.Point(113, 287);
            this.lblOverlapPercent.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblOverlapPercent.Name = "lblOverlapPercent";
            this.lblOverlapPercent.Size = new System.Drawing.Size(18, 23);
            this.lblOverlapPercent.TabIndex = 493;
            this.lblOverlapPercent.Text = "-";
            // 
            // labelOverlap
            // 
            this.labelOverlap.AutoSize = true;
            this.labelOverlap.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOverlap.ForeColor = System.Drawing.Color.White;
            this.labelOverlap.Location = new System.Drawing.Point(9, 287);
            this.labelOverlap.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelOverlap.Name = "labelOverlap";
            this.labelOverlap.Size = new System.Drawing.Size(81, 23);
            this.labelOverlap.TabIndex = 492;
            this.labelOverlap.Text = "Overlap:";
            // 
            // labelActual
            // 
            this.labelActual.AutoSize = true;
            this.labelActual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActual.ForeColor = System.Drawing.Color.White;
            this.labelActual.Location = new System.Drawing.Point(71, 189);
            this.labelActual.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelActual.Name = "labelActual";
            this.labelActual.Size = new System.Drawing.Size(61, 23);
            this.labelActual.TabIndex = 494;
            this.labelActual.Text = "Actual";
            // 
            // labelWorked
            // 
            this.labelWorked.AutoSize = true;
            this.labelWorked.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWorked.ForeColor = System.Drawing.Color.White;
            this.labelWorked.Location = new System.Drawing.Point(71, 29);
            this.labelWorked.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelWorked.Name = "labelWorked";
            this.labelWorked.Size = new System.Drawing.Size(74, 23);
            this.labelWorked.TabIndex = 495;
            this.labelWorked.Text = "Worked";
            // 
            // lblActualRemain
            // 
            this.lblActualRemain.AutoSize = true;
            this.lblActualRemain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualRemain.ForeColor = System.Drawing.Color.White;
            this.lblActualRemain.Location = new System.Drawing.Point(113, 258);
            this.lblActualRemain.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblActualRemain.Name = "lblActualRemain";
            this.lblActualRemain.Size = new System.Drawing.Size(18, 23);
            this.lblActualRemain.TabIndex = 497;
            this.lblActualRemain.Text = "-";
            // 
            // labelRemain2
            // 
            this.labelRemain2.AutoSize = true;
            this.labelRemain2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRemain2.ForeColor = System.Drawing.Color.White;
            this.labelRemain2.Location = new System.Drawing.Point(10, 258);
            this.labelRemain2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRemain2.Name = "labelRemain2";
            this.labelRemain2.Size = new System.Drawing.Size(80, 23);
            this.labelRemain2.TabIndex = 496;
            this.labelRemain2.Text = "Remain:";
            // 
            // labelAreaValue
            // 
            this.labelAreaValue.AutoSize = true;
            this.labelAreaValue.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAreaValue.ForeColor = System.Drawing.Color.White;
            this.labelAreaValue.Location = new System.Drawing.Point(102, 382);
            this.labelAreaValue.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelAreaValue.Name = "labelAreaValue";
            this.labelAreaValue.Size = new System.Drawing.Size(17, 21);
            this.labelAreaValue.TabIndex = 499;
            this.labelAreaValue.Text = "-";
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelArea.ForeColor = System.Drawing.Color.White;
            this.labelArea.Location = new System.Drawing.Point(10, 382);
            this.labelArea.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(55, 23);
            this.labelArea.TabIndex = 498;
            this.labelArea.Text = "Area:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 364);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 19);
            this.label2.TabIndex = 501;
            this.label2.Text = "____________________";
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDistance.ForeColor = System.Drawing.Color.White;
            this.labelDistance.Location = new System.Drawing.Point(10, 407);
            this.labelDistance.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(48, 23);
            this.labelDistance.TabIndex = 502;
            this.labelDistance.Text = "Dist:";
            // 
            // labelDistanceDriven
            // 
            this.labelDistanceDriven.AutoSize = true;
            this.labelDistanceDriven.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDistanceDriven.ForeColor = System.Drawing.Color.White;
            this.labelDistanceDriven.Location = new System.Drawing.Point(102, 407);
            this.labelDistanceDriven.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelDistanceDriven.Name = "labelDistanceDriven";
            this.labelDistanceDriven.Size = new System.Drawing.Size(17, 21);
            this.labelDistanceDriven.TabIndex = 503;
            this.labelDistanceDriven.Text = "-";
            // 
            // labelTrip
            // 
            this.labelTrip.AutoSize = true;
            this.labelTrip.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTrip.ForeColor = System.Drawing.Color.White;
            this.labelTrip.Location = new System.Drawing.Point(59, 357);
            this.labelTrip.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelTrip.Name = "labelTrip";
            this.labelTrip.Size = new System.Drawing.Size(43, 23);
            this.labelTrip.TabIndex = 504;
            this.labelTrip.Text = "Trip";
            // 
            // btnTripReset
            // 
            this.btnTripReset.Image = global::AgOpenGPS.Properties.Resources.back_button;
            this.btnTripReset.Location = new System.Drawing.Point(63, 436);
            this.btnTripReset.Name = "btnTripReset";
            this.btnTripReset.Size = new System.Drawing.Size(76, 44);
            this.btnTripReset.TabIndex = 500;
            this.btnTripReset.UseVisualStyleBackColor = true;
            this.btnTripReset.Click += new System.EventHandler(this.btnTripReset_Click);
            // 
            // FormFieldData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(197, 483);
            this.Controls.Add(this.labelTrip);
            this.Controls.Add(this.labelDistanceDriven);
            this.Controls.Add(this.labelDistance);
            this.Controls.Add(this.btnTripReset);
            this.Controls.Add(this.labelAreaValue);
            this.Controls.Add(this.labelArea);
            this.Controls.Add(this.lblWorkRate);
            this.Controls.Add(this.lblActualRemain);
            this.Controls.Add(this.labelRemain2);
            this.Controls.Add(this.labelWorked);
            this.Controls.Add(this.labelActual);
            this.Controls.Add(this.lblOverlapPercent);
            this.Controls.Add(this.labelOverlap);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblActualLessOverlap);
            this.Controls.Add(this.labelApplied2);
            this.Controls.Add(this.lblRemainPercent);
            this.Controls.Add(this.lblApplied);
            this.Controls.Add(this.labelApplied);
            this.Controls.Add(this.lblTotalArea);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelRate);
            this.Controls.Add(this.lblAreaRemain);
            this.Controls.Add(this.lblTimeRemaining);
            this.Controls.Add(this.labelRemain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFieldData";
            this.ShowInTaskbar = false;
            this.Text = "System Data";
            this.Load += new System.EventHandler(this.FormFieldData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTimeRemaining;
        private System.Windows.Forms.Label labelRemain;
        private System.Windows.Forms.Label lblAreaRemain;
        private System.Windows.Forms.Label lblWorkRate;
        private System.Windows.Forms.Label labelRate;
        private System.Windows.Forms.Label lblTotalArea;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label lblApplied;
        private System.Windows.Forms.Label labelApplied;
        private System.Windows.Forms.Label lblRemainPercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblActualLessOverlap;
        private System.Windows.Forms.Label labelApplied2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblOverlapPercent;
        private System.Windows.Forms.Label labelOverlap;
        private System.Windows.Forms.Label labelActual;
        private System.Windows.Forms.Label labelWorked;
        private System.Windows.Forms.Label lblActualRemain;
        private System.Windows.Forms.Label labelRemain2;
        private System.Windows.Forms.Label labelAreaValue;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.Button btnTripReset;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Label labelDistanceDriven;
        private System.Windows.Forms.Label labelTrip;
    }
}