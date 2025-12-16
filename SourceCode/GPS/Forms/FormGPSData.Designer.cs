namespace AgOpenGPS
{
    partial class FormGPSData
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
            this.label3 = new System.Windows.Forms.Label();
            this.lblSatsTracked = new System.Windows.Forms.Label();
            this.lblLatitude = new System.Windows.Forms.Label();
            this.lblLongitude = new System.Windows.Forms.Label();
            this.lblAltitude = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblHDOP = new System.Windows.Forms.Label();
            this.lblEastingField = new System.Windows.Forms.Label();
            this.lblNorthingField = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.lblHz = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblTimeSlice = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblFrameTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbludpWatchCounts = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblAge = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblFix2FixHeading = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblIMUHeading = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFuzeHeading = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblAngularVelocity = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 18);
            this.label3.TabIndex = 9;
            this.label3.Text = "# Sats";
            // 
            // lblSatsTracked
            // 
            this.lblSatsTracked.AutoSize = true;
            this.lblSatsTracked.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSatsTracked.ForeColor = System.Drawing.Color.White;
            this.lblSatsTracked.Location = new System.Drawing.Point(57, 108);
            this.lblSatsTracked.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSatsTracked.Name = "lblSatsTracked";
            this.lblSatsTracked.Size = new System.Drawing.Size(41, 18);
            this.lblSatsTracked.TabIndex = 4;
            this.lblSatsTracked.Text = "Sats";
            this.lblSatsTracked.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLatitude
            // 
            this.lblLatitude.AutoSize = true;
            this.lblLatitude.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLatitude.ForeColor = System.Drawing.Color.White;
            this.lblLatitude.Location = new System.Drawing.Point(5, -2);
            this.lblLatitude.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLatitude.Name = "lblLatitude";
            this.lblLatitude.Size = new System.Drawing.Size(70, 18);
            this.lblLatitude.TabIndex = 12;
            this.lblLatitude.Text = "Latitude";
            // 
            // lblLongitude
            // 
            this.lblLongitude.AutoSize = true;
            this.lblLongitude.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLongitude.ForeColor = System.Drawing.Color.White;
            this.lblLongitude.Location = new System.Drawing.Point(5, 19);
            this.lblLongitude.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLongitude.Name = "lblLongitude";
            this.lblLongitude.Size = new System.Drawing.Size(119, 18);
            this.lblLongitude.TabIndex = 13;
            this.lblLongitude.Text = "-128.1234567";
            // 
            // lblAltitude
            // 
            this.lblAltitude.AutoSize = true;
            this.lblAltitude.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAltitude.ForeColor = System.Drawing.Color.White;
            this.lblAltitude.Location = new System.Drawing.Point(57, 88);
            this.lblAltitude.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAltitude.Name = "lblAltitude";
            this.lblAltitude.Size = new System.Drawing.Size(67, 18);
            this.lblAltitude.TabIndex = 14;
            this.lblAltitude.Text = "Altitude";
            this.lblAltitude.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(23, 87);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 18);
            this.label8.TabIndex = 16;
            this.label8.Text = "Elev";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(10, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 18);
            this.label9.TabIndex = 18;
            this.label9.Text = "HDOP";
            // 
            // lblHDOP
            // 
            this.lblHDOP.AutoSize = true;
            this.lblHDOP.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHDOP.ForeColor = System.Drawing.Color.White;
            this.lblHDOP.Location = new System.Drawing.Point(57, 128);
            this.lblHDOP.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHDOP.Name = "lblHDOP";
            this.lblHDOP.Size = new System.Drawing.Size(52, 18);
            this.lblHDOP.TabIndex = 17;
            this.lblHDOP.Text = "HDOP";
            this.lblHDOP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEastingField
            // 
            this.lblEastingField.AutoSize = true;
            this.lblEastingField.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEastingField.ForeColor = System.Drawing.Color.White;
            this.lblEastingField.Location = new System.Drawing.Point(34, 62);
            this.lblEastingField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEastingField.Name = "lblEastingField";
            this.lblEastingField.Size = new System.Drawing.Size(63, 18);
            this.lblEastingField.TabIndex = 477;
            this.lblEastingField.Text = "Easting";
            // 
            // lblNorthingField
            // 
            this.lblNorthingField.AutoSize = true;
            this.lblNorthingField.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNorthingField.ForeColor = System.Drawing.Color.White;
            this.lblNorthingField.Location = new System.Drawing.Point(34, 42);
            this.lblNorthingField.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblNorthingField.Name = "lblNorthingField";
            this.lblNorthingField.Size = new System.Drawing.Size(74, 18);
            this.lblNorthingField.TabIndex = 476;
            this.lblNorthingField.Text = "Northing";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(17, 62);
            this.label27.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(16, 18);
            this.label27.TabIndex = 475;
            this.label27.Text = "E";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.White;
            this.label28.Location = new System.Drawing.Point(15, 42);
            this.label28.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(18, 18);
            this.label28.TabIndex = 474;
            this.label28.Text = "N";
            // 
            // lblHz
            // 
            this.lblHz.AutoSize = true;
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHz.ForeColor = System.Drawing.Color.White;
            this.lblHz.Location = new System.Drawing.Point(57, 207);
            this.lblHz.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(46, 18);
            this.lblHz.TabIndex = 506;
            this.lblHz.Text = "msec";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(34, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 18);
            this.label4.TabIndex = 509;
            this.label4.Text = "Hz";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(2, 189);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 18);
            this.label10.TabIndex = 511;
            this.label10.Text = "Raw Hz";
            // 
            // lblTimeSlice
            // 
            this.lblTimeSlice.AutoSize = true;
            this.lblTimeSlice.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeSlice.ForeColor = System.Drawing.Color.White;
            this.lblTimeSlice.Location = new System.Drawing.Point(57, 189);
            this.lblTimeSlice.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblTimeSlice.Name = "lblTimeSlice";
            this.lblTimeSlice.Size = new System.Drawing.Size(46, 18);
            this.lblTimeSlice.TabIndex = 510;
            this.lblTimeSlice.Text = "msec";
            this.lblTimeSlice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label14
            //
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(24, 147);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(33, 18);
            this.label14.TabIndex = 524;
            this.label14.Text = "Age";
            //
            // lblAge
            //
            this.lblAge.AutoSize = true;
            this.lblAge.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.ForeColor = System.Drawing.Color.White;
            this.lblAge.Location = new System.Drawing.Point(57, 148);
            this.lblAge.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAge.Name = "lblAge";
            this.lblAge.Size = new System.Drawing.Size(36, 18);
            this.lblAge.TabIndex = 525;
            this.lblAge.Text = "0.0";
            this.lblAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // label11
            //
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(9, 171);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 18);
            this.label11.TabIndex = 513;
            this.label11.Text = "Frame";
            //
            // lblFrameTime
            //
            this.lblFrameTime.AutoSize = true;
            this.lblFrameTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrameTime.ForeColor = System.Drawing.Color.White;
            this.lblFrameTime.Location = new System.Drawing.Point(57, 171);
            this.lblFrameTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFrameTime.Name = "lblFrameTime";
            this.lblFrameTime.Size = new System.Drawing.Size(46, 18);
            this.lblFrameTime.TabIndex = 512;
            this.lblFrameTime.Text = "msec";
            this.lblFrameTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(6, 231);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 18);
            this.label2.TabIndex = 515;
            this.label2.Text = "Missed";
            // 
            // lbludpWatchCounts
            // 
            this.lbludpWatchCounts.AutoSize = true;
            this.lbludpWatchCounts.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbludpWatchCounts.ForeColor = System.Drawing.Color.White;
            this.lbludpWatchCounts.Location = new System.Drawing.Point(57, 231);
            this.lbludpWatchCounts.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbludpWatchCounts.Name = "lbludpWatchCounts";
            this.lbludpWatchCounts.Size = new System.Drawing.Size(46, 18);
            this.lbludpWatchCounts.TabIndex = 514;
            this.lbludpWatchCounts.Text = "msec";
            this.lbludpWatchCounts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(6, 259);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 18);
            this.label5.TabIndex = 519;
            this.label5.Text = "Fix2Fix";
            // 
            // lblFix2FixHeading
            // 
            this.lblFix2FixHeading.AutoSize = true;
            this.lblFix2FixHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFix2FixHeading.ForeColor = System.Drawing.Color.White;
            this.lblFix2FixHeading.Location = new System.Drawing.Point(57, 259);
            this.lblFix2FixHeading.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFix2FixHeading.Name = "lblFix2FixHeading";
            this.lblFix2FixHeading.Size = new System.Drawing.Size(53, 18);
            this.lblFix2FixHeading.TabIndex = 518;
            this.lblFix2FixHeading.Text = "359.1";
            this.lblFix2FixHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(22, 280);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 18);
            this.label13.TabIndex = 517;
            this.label13.Text = "IMU";
            // 
            // lblIMUHeading
            // 
            this.lblIMUHeading.AutoSize = true;
            this.lblIMUHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMUHeading.ForeColor = System.Drawing.Color.White;
            this.lblIMUHeading.Location = new System.Drawing.Point(57, 280);
            this.lblIMUHeading.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblIMUHeading.Name = "lblIMUHeading";
            this.lblIMUHeading.Size = new System.Drawing.Size(53, 18);
            this.lblIMUHeading.TabIndex = 516;
            this.lblIMUHeading.Text = "321.6";
            this.lblIMUHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(-2, 301);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 18);
            this.label15.TabIndex = 521;
            this.label15.Text = "Heading";
            // 
            // lblFuzeHeading
            // 
            this.lblFuzeHeading.AutoSize = true;
            this.lblFuzeHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFuzeHeading.ForeColor = System.Drawing.Color.White;
            this.lblFuzeHeading.Location = new System.Drawing.Point(57, 301);
            this.lblFuzeHeading.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFuzeHeading.Name = "lblFuzeHeading";
            this.lblFuzeHeading.Size = new System.Drawing.Size(53, 18);
            this.lblFuzeHeading.TabIndex = 520;
            this.lblFuzeHeading.Text = "344.0";
            this.lblFuzeHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(32, 327);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(26, 18);
            this.label12.TabIndex = 523;
            this.label12.Text = "AV";
            // 
            // lblAngularVelocity
            // 
            this.lblAngularVelocity.AutoSize = true;
            this.lblAngularVelocity.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAngularVelocity.ForeColor = System.Drawing.Color.White;
            this.lblAngularVelocity.Location = new System.Drawing.Point(57, 327);
            this.lblAngularVelocity.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblAngularVelocity.Name = "lblAngularVelocity";
            this.lblAngularVelocity.Size = new System.Drawing.Size(43, 18);
            this.lblAngularVelocity.TabIndex = 522;
            this.lblAngularVelocity.Text = "3.56";
            this.lblAngularVelocity.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormGPSData
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(120, 350);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblAge);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lblAngularVelocity);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lblFuzeHeading);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblFix2FixHeading);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblIMUHeading);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbludpWatchCounts);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblFrameTime);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblTimeSlice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblHz);
            this.Controls.Add(this.lblLatitude);
            this.Controls.Add(this.lblLongitude);
            this.Controls.Add(this.lblEastingField);
            this.Controls.Add(this.lblNorthingField);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblHDOP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblAltitude);
            this.Controls.Add(this.lblSatsTracked);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGPSData";
            this.ShowInTaskbar = false;
            this.Text = "System Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPSData_FormClosing);
            this.Load += new System.EventHandler(this.FormGPSData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSatsTracked;
        private System.Windows.Forms.Label lblLatitude;
        private System.Windows.Forms.Label lblLongitude;
        private System.Windows.Forms.Label lblAltitude;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblHDOP;
        private System.Windows.Forms.Label lblEastingField;
        private System.Windows.Forms.Label lblNorthingField;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lblHz;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblTimeSlice;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblFrameTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbludpWatchCounts;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFix2FixHeading;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblIMUHeading;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblFuzeHeading;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblAngularVelocity;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblAge;
    }
}