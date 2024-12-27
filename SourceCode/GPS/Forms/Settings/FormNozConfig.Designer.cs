namespace AgOpenGPS
{
    partial class FormNozConfig
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cboxBypass = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSprayAcres = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tboxUnitsPerArea = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tboxUnitsApplied = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboxSectionValve3Wire = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFlow = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabUnits = new System.Windows.Forms.TabPage();
            this.tabGain = new System.Windows.Forms.TabPage();
            this.unoChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblUnitsActual = new System.Windows.Forms.Label();
            this.lblUnitsSet = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.nudSlowPWM = new AgOpenGPS.NudlessNumericUpDown();
            this.nudFastPWM = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSwitchAtFlowError = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayKp = new AgOpenGPS.NudlessNumericUpDown();
            this.nudDeadbandError = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayPressureCal = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayFlowCal = new AgOpenGPS.NudlessNumericUpDown();
            this.nudMaxHz = new AgOpenGPS.NudlessNumericUpDown();
            this.nudMinHz = new AgOpenGPS.NudlessNumericUpDown();
            this.tabControl1.SuspendLayout();
            this.tabFlow.SuspendLayout();
            this.tabUnits.SuspendLayout();
            this.tabGain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.unoChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlowPWM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFastPWM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwitchAtFlowError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayKp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadbandError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayPressureCal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayFlowCal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxHz)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinHz)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 333;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(202, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(166, 29);
            this.label9.TabIndex = 586;
            this.label9.Text = "P Gain";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(18, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 29);
            this.label8.TabIndex = 585;
            this.label8.Text = "Pressure Cal";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 29);
            this.label1.TabIndex = 580;
            this.label1.Text = "Cal Factor x10";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 29);
            this.label3.TabIndex = 592;
            this.label3.Text = "Fast PWM %";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(10, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 29);
            this.label4.TabIndex = 594;
            this.label4.Text = "Slow PWM %";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(204, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 29);
            this.label6.TabIndex = 597;
            this.label6.Text = "Deadband %";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(10, 247);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(166, 29);
            this.label11.TabIndex = 599;
            this.label11.Text = "Fast > Slow %";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxBypass
            // 
            this.cboxBypass.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxBypass.BackColor = System.Drawing.Color.White;
            this.cboxBypass.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxBypass.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxBypass.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxBypass.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxBypass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxBypass.Location = new System.Drawing.Point(220, 17);
            this.cboxBypass.Name = "cboxBypass";
            this.cboxBypass.Size = new System.Drawing.Size(137, 46);
            this.cboxBypass.TabIndex = 601;
            this.cboxBypass.Text = "Normal";
            this.cboxBypass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxBypass.UseVisualStyleBackColor = false;
            this.cboxBypass.Click += new System.EventHandler(this.cboxBypass_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(220, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 30);
            this.label2.TabIndex = 602;
            this.label2.Text = "Section Mode";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSprayAcres
            // 
            this.btnSprayAcres.BackColor = System.Drawing.Color.Transparent;
            this.btnSprayAcres.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSprayAcres.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSprayAcres.FlatAppearance.BorderSize = 0;
            this.btnSprayAcres.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSprayAcres.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSprayAcres.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSprayAcres.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSprayAcres.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSprayAcres.ForeColor = System.Drawing.Color.White;
            this.btnSprayAcres.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSprayAcres.Location = new System.Drawing.Point(235, 195);
            this.btnSprayAcres.Name = "btnSprayAcres";
            this.btnSprayAcres.Size = new System.Drawing.Size(104, 44);
            this.btnSprayAcres.TabIndex = 614;
            this.btnSprayAcres.Text = "-127";
            this.btnSprayAcres.UseVisualStyleBackColor = false;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(239, 242);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 19);
            this.label12.TabIndex = 613;
            this.label12.Text = "PWM";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(220, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 29);
            this.label5.TabIndex = 616;
            this.label5.Text = "Metering Units";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tboxUnitsPerArea
            // 
            this.tboxUnitsPerArea.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxUnitsPerArea.Location = new System.Drawing.Point(233, 200);
            this.tboxUnitsPerArea.MaxLength = 5;
            this.tboxUnitsPerArea.Name = "tboxUnitsPerArea";
            this.tboxUnitsPerArea.Size = new System.Drawing.Size(111, 46);
            this.tboxUnitsPerArea.TabIndex = 617;
            this.tboxUnitsPerArea.Text = "L/Ha";
            this.tboxUnitsPerArea.TextChanged += new System.EventHandler(this.tboxUnitsPerArea_TextChanged);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(220, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 29);
            this.label7.TabIndex = 618;
            this.label7.Text = "Rate Units";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tboxUnitsApplied
            // 
            this.tboxUnitsApplied.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxUnitsApplied.Location = new System.Drawing.Point(220, 114);
            this.tboxUnitsApplied.MaxLength = 10;
            this.tboxUnitsApplied.Name = "tboxUnitsApplied";
            this.tboxUnitsApplied.Size = new System.Drawing.Size(137, 46);
            this.tboxUnitsApplied.TabIndex = 619;
            this.tboxUnitsApplied.Text = "Gallons";
            this.tboxUnitsApplied.TextChanged += new System.EventHandler(this.tboxUnitsApplied_TextChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(21, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 29);
            this.label10.TabIndex = 621;
            this.label10.Text = "Section Style";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxSectionValve3Wire
            // 
            this.cboxSectionValve3Wire.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxSectionValve3Wire.BackColor = System.Drawing.Color.White;
            this.cboxSectionValve3Wire.Checked = true;
            this.cboxSectionValve3Wire.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxSectionValve3Wire.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxSectionValve3Wire.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxSectionValve3Wire.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxSectionValve3Wire.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSectionValve3Wire.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxSectionValve3Wire.Location = new System.Drawing.Point(17, 18);
            this.cboxSectionValve3Wire.Name = "cboxSectionValve3Wire";
            this.cboxSectionValve3Wire.Size = new System.Drawing.Size(137, 46);
            this.cboxSectionValve3Wire.TabIndex = 620;
            this.cboxSectionValve3Wire.Text = "3 Wire ";
            this.cboxSectionValve3Wire.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxSectionValve3Wire.UseVisualStyleBackColor = false;
            this.cboxSectionValve3Wire.Click += new System.EventHandler(this.cboxSectionValve3Wire_Click);
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(211, 63);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(166, 29);
            this.label13.TabIndex = 623;
            this.label13.Text = "Max Hz";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(8, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(166, 29);
            this.label14.TabIndex = 625;
            this.label14.Text = "Min Hz";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabFlow);
            this.tabControl1.Controls.Add(this.tabUnits);
            this.tabControl1.Controls.Add(this.tabGain);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(126, 48);
            this.tabControl1.Location = new System.Drawing.Point(0, 88);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(392, 348);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 626;
            // 
            // tabFlow
            // 
            this.tabFlow.BackColor = System.Drawing.Color.Black;
            this.tabFlow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabFlow.Controls.Add(this.label20);
            this.tabFlow.Controls.Add(this.label18);
            this.tabFlow.Controls.Add(this.nudSlowPWM);
            this.tabFlow.Controls.Add(this.label3);
            this.tabFlow.Controls.Add(this.label4);
            this.tabFlow.Controls.Add(this.label11);
            this.tabFlow.Controls.Add(this.btnSprayAcres);
            this.tabFlow.Controls.Add(this.label12);
            this.tabFlow.Controls.Add(this.nudFastPWM);
            this.tabFlow.Controls.Add(this.nudSwitchAtFlowError);
            this.tabFlow.Controls.Add(this.nudSprayKp);
            this.tabFlow.Controls.Add(this.nudDeadbandError);
            this.tabFlow.Controls.Add(this.label9);
            this.tabFlow.Controls.Add(this.label6);
            this.tabFlow.ForeColor = System.Drawing.Color.Black;
            this.tabFlow.ImageIndex = 3;
            this.tabFlow.Location = new System.Drawing.Point(4, 52);
            this.tabFlow.Name = "tabFlow";
            this.tabFlow.Size = new System.Drawing.Size(384, 292);
            this.tabFlow.TabIndex = 16;
            this.tabFlow.Text = "Gain";
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label20.Location = new System.Drawing.Point(579, 228);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(204, 30);
            this.label20.TabIndex = 302;
            this.label20.Text = "Look Ahead Min";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label18.Location = new System.Drawing.Point(561, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(204, 30);
            this.label18.TabIndex = 300;
            this.label18.Text = "Look Ahead";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabUnits
            // 
            this.tabUnits.BackColor = System.Drawing.Color.Black;
            this.tabUnits.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabUnits.Controls.Add(this.tboxUnitsPerArea);
            this.tabUnits.Controls.Add(this.cboxBypass);
            this.tabUnits.Controls.Add(this.cboxSectionValve3Wire);
            this.tabUnits.Controls.Add(this.tboxUnitsApplied);
            this.tabUnits.Controls.Add(this.label5);
            this.tabUnits.Controls.Add(this.label7);
            this.tabUnits.Controls.Add(this.label10);
            this.tabUnits.Controls.Add(this.label8);
            this.tabUnits.Controls.Add(this.label2);
            this.tabUnits.Controls.Add(this.label1);
            this.tabUnits.Controls.Add(this.nudSprayPressureCal);
            this.tabUnits.Controls.Add(this.nudSprayFlowCal);
            this.tabUnits.ImageIndex = 2;
            this.tabUnits.Location = new System.Drawing.Point(4, 52);
            this.tabUnits.Name = "tabUnits";
            this.tabUnits.Size = new System.Drawing.Size(384, 292);
            this.tabUnits.TabIndex = 15;
            this.tabUnits.Text = "Cal";
            // 
            // tabGain
            // 
            this.tabGain.AutoScroll = true;
            this.tabGain.BackColor = System.Drawing.Color.Black;
            this.tabGain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabGain.Controls.Add(this.nudMaxHz);
            this.tabGain.Controls.Add(this.nudMinHz);
            this.tabGain.Controls.Add(this.label13);
            this.tabGain.Controls.Add(this.label14);
            this.tabGain.ImageIndex = 1;
            this.tabGain.Location = new System.Drawing.Point(4, 52);
            this.tabGain.Name = "tabGain";
            this.tabGain.Size = new System.Drawing.Size(384, 292);
            this.tabGain.TabIndex = 13;
            this.tabGain.Text = "Limits";
            // 
            // unoChart
            // 
            this.unoChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unoChart.AntiAliasing = System.Windows.Forms.DataVisualization.Charting.AntiAliasingStyles.None;
            this.unoChart.BackColor = System.Drawing.Color.Black;
            chartArea1.AxisX.LabelAutoFitMaxFontSize = 8;
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.DimGray;
            chartArea1.AxisY.LineWidth = 2;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.DimGray;
            chartArea1.BackColor = System.Drawing.Color.Black;
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.unoChart.ChartAreas.Add(chartArea1);
            this.unoChart.Location = new System.Drawing.Point(-35, 9);
            this.unoChart.Margin = new System.Windows.Forms.Padding(0);
            this.unoChart.Name = "unoChart";
            this.unoChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.BackSecondaryColor = System.Drawing.Color.White;
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series1.Color = System.Drawing.Color.LightSalmon;
            series1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            series1.Legend = "Legend1";
            series1.MarkerBorderWidth = 2;
            series1.Name = "S";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series2.Color = System.Drawing.Color.Lime;
            series2.IsVisibleInLegend = false;
            series2.Legend = "Legend1";
            series2.Name = "PWM";
            this.unoChart.Series.Add(series1);
            this.unoChart.Series.Add(series2);
            this.unoChart.Size = new System.Drawing.Size(431, 97);
            this.unoChart.TabIndex = 627;
            this.unoChart.TextAntiAliasingQuality = System.Windows.Forms.DataVisualization.Charting.TextAntiAliasingQuality.SystemDefault;
            // 
            // lblUnitsActual
            // 
            this.lblUnitsActual.AutoSize = true;
            this.lblUnitsActual.BackColor = System.Drawing.Color.Transparent;
            this.lblUnitsActual.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitsActual.ForeColor = System.Drawing.Color.Lime;
            this.lblUnitsActual.Location = new System.Drawing.Point(43, -1);
            this.lblUnitsActual.Name = "lblUnitsActual";
            this.lblUnitsActual.Size = new System.Drawing.Size(54, 22);
            this.lblUnitsActual.TabIndex = 615;
            this.lblUnitsActual.Text = "2700";
            this.lblUnitsActual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnitsSet
            // 
            this.lblUnitsSet.AutoSize = true;
            this.lblUnitsSet.BackColor = System.Drawing.Color.Transparent;
            this.lblUnitsSet.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitsSet.ForeColor = System.Drawing.Color.LightSalmon;
            this.lblUnitsSet.Location = new System.Drawing.Point(159, -1);
            this.lblUnitsSet.Name = "lblUnitsSet";
            this.lblUnitsSet.Size = new System.Drawing.Size(54, 22);
            this.lblUnitsSet.TabIndex = 628;
            this.lblUnitsSet.Text = "0300";
            this.lblUnitsSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.LightSalmon;
            this.label15.Location = new System.Drawing.Point(120, -1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 22);
            this.label15.TabIndex = 629;
            this.label15.Text = "Set:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Lime;
            this.label16.Location = new System.Drawing.Point(1, -1);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 22);
            this.label16.TabIndex = 630;
            this.label16.Text = "Act:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudSlowPWM
            // 
            this.nudSlowPWM.BackColor = System.Drawing.Color.White;
            this.nudSlowPWM.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSlowPWM.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudSlowPWM.Location = new System.Drawing.Point(16, 108);
            this.nudSlowPWM.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSlowPWM.Name = "nudSlowPWM";
            this.nudSlowPWM.ReadOnly = true;
            this.nudSlowPWM.Size = new System.Drawing.Size(155, 46);
            this.nudSlowPWM.TabIndex = 595;
            this.nudSlowPWM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSlowPWM.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.nudSlowPWM.Click += new System.EventHandler(this.nudSlowPWM_Click);
            // 
            // nudFastPWM
            // 
            this.nudFastPWM.BackColor = System.Drawing.Color.White;
            this.nudFastPWM.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudFastPWM.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudFastPWM.Location = new System.Drawing.Point(16, 22);
            this.nudFastPWM.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudFastPWM.Name = "nudFastPWM";
            this.nudFastPWM.ReadOnly = true;
            this.nudFastPWM.Size = new System.Drawing.Size(155, 46);
            this.nudFastPWM.TabIndex = 593;
            this.nudFastPWM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudFastPWM.Value = new decimal(new int[] {
            65,
            0,
            0,
            0});
            this.nudFastPWM.Click += new System.EventHandler(this.nudFastPWM_Click);
            // 
            // nudSwitchAtFlowError
            // 
            this.nudSwitchAtFlowError.BackColor = System.Drawing.Color.White;
            this.nudSwitchAtFlowError.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSwitchAtFlowError.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudSwitchAtFlowError.Location = new System.Drawing.Point(16, 205);
            this.nudSwitchAtFlowError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSwitchAtFlowError.Name = "nudSwitchAtFlowError";
            this.nudSwitchAtFlowError.ReadOnly = true;
            this.nudSwitchAtFlowError.Size = new System.Drawing.Size(155, 46);
            this.nudSwitchAtFlowError.TabIndex = 600;
            this.nudSwitchAtFlowError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSwitchAtFlowError.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudSwitchAtFlowError.Click += new System.EventHandler(this.nudSwitchAtFlowError_Click);
            // 
            // nudSprayKp
            // 
            this.nudSprayKp.BackColor = System.Drawing.Color.White;
            this.nudSprayKp.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayKp.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudSprayKp.Location = new System.Drawing.Point(208, 107);
            this.nudSprayKp.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSprayKp.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSprayKp.Name = "nudSprayKp";
            this.nudSprayKp.ReadOnly = true;
            this.nudSprayKp.Size = new System.Drawing.Size(155, 46);
            this.nudSprayKp.TabIndex = 587;
            this.nudSprayKp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayKp.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudSprayKp.Click += new System.EventHandler(this.nudSprayKpClick);
            // 
            // nudDeadbandError
            // 
            this.nudDeadbandError.BackColor = System.Drawing.Color.White;
            this.nudDeadbandError.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDeadbandError.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudDeadbandError.Location = new System.Drawing.Point(210, 18);
            this.nudDeadbandError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeadbandError.Name = "nudDeadbandError";
            this.nudDeadbandError.ReadOnly = true;
            this.nudDeadbandError.Size = new System.Drawing.Size(155, 46);
            this.nudDeadbandError.TabIndex = 598;
            this.nudDeadbandError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDeadbandError.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudDeadbandError.Click += new System.EventHandler(this.nudDeadbandError_Click);
            // 
            // nudSprayPressureCal
            // 
            this.nudSprayPressureCal.BackColor = System.Drawing.Color.White;
            this.nudSprayPressureCal.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayPressureCal.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudSprayPressureCal.Location = new System.Drawing.Point(17, 201);
            this.nudSprayPressureCal.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSprayPressureCal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSprayPressureCal.Name = "nudSprayPressureCal";
            this.nudSprayPressureCal.ReadOnly = true;
            this.nudSprayPressureCal.Size = new System.Drawing.Size(137, 46);
            this.nudSprayPressureCal.TabIndex = 584;
            this.nudSprayPressureCal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayPressureCal.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudSprayPressureCal.Click += new System.EventHandler(this.nudSprayPressureCal_Click);
            // 
            // nudSprayFlowCal
            // 
            this.nudSprayFlowCal.BackColor = System.Drawing.Color.White;
            this.nudSprayFlowCal.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayFlowCal.Location = new System.Drawing.Point(17, 111);
            this.nudSprayFlowCal.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudSprayFlowCal.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudSprayFlowCal.Name = "nudSprayFlowCal";
            this.nudSprayFlowCal.ReadOnly = true;
            this.nudSprayFlowCal.Size = new System.Drawing.Size(137, 46);
            this.nudSprayFlowCal.TabIndex = 579;
            this.nudSprayFlowCal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayFlowCal.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSprayFlowCal.Click += new System.EventHandler(this.nudSprayFlowCal_Click);
            // 
            // nudMaxHz
            // 
            this.nudMaxHz.BackColor = System.Drawing.Color.White;
            this.nudMaxHz.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxHz.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudMaxHz.Location = new System.Drawing.Point(217, 21);
            this.nudMaxHz.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudMaxHz.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaxHz.Name = "nudMaxHz";
            this.nudMaxHz.ReadOnly = true;
            this.nudMaxHz.Size = new System.Drawing.Size(155, 46);
            this.nudMaxHz.TabIndex = 622;
            this.nudMaxHz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMaxHz.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // nudMinHz
            // 
            this.nudMinHz.BackColor = System.Drawing.Color.White;
            this.nudMinHz.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinHz.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudMinHz.Location = new System.Drawing.Point(14, 21);
            this.nudMinHz.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudMinHz.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMinHz.Name = "nudMinHz";
            this.nudMinHz.ReadOnly = true;
            this.nudMinHz.Size = new System.Drawing.Size(155, 46);
            this.nudMinHz.TabIndex = 624;
            this.nudMinHz.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMinHz.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // FormNozConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(45)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(392, 436);
            this.Controls.Add(this.lblUnitsSet);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lblUnitsActual);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.unoChart);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(408, 768);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(408, 449);
            this.Name = "FormNozConfig";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Controller Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNozConfig_FormClosing);
            this.Load += new System.EventHandler(this.FormDisplaySettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabFlow.ResumeLayout(false);
            this.tabUnits.ResumeLayout(false);
            this.tabUnits.PerformLayout();
            this.tabGain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.unoChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlowPWM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFastPWM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSwitchAtFlowError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayKp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadbandError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayPressureCal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayFlowCal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxHz)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinHz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private NudlessNumericUpDown nudSprayKp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private NudlessNumericUpDown nudSprayPressureCal;
        private System.Windows.Forms.Label label1;
        private NudlessNumericUpDown nudSprayFlowCal;
        private NudlessNumericUpDown nudFastPWM;
        private System.Windows.Forms.Label label3;
        private NudlessNumericUpDown nudSlowPWM;
        private System.Windows.Forms.Label label4;
        private NudlessNumericUpDown nudDeadbandError;
        private System.Windows.Forms.Label label6;
        private NudlessNumericUpDown nudSwitchAtFlowError;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cboxBypass;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button btnSprayAcres;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tboxUnitsPerArea;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tboxUnitsApplied;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cboxSectionValve3Wire;
        private NudlessNumericUpDown nudMaxHz;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private NudlessNumericUpDown nudMinHz;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFlow;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tabUnits;
        private System.Windows.Forms.TabPage tabGain;
        private System.Windows.Forms.DataVisualization.Charting.Chart unoChart;
        private System.Windows.Forms.Label lblUnitsActual;
        private System.Windows.Forms.Label lblUnitsSet;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
    }
}