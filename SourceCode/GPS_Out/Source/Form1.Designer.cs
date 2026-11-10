namespace GPS_Out
{
    partial class frmStart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStart));
            this.tmrGGA = new System.Windows.Forms.Timer(this.components);
            this.tmrVTG = new System.Windows.Forms.Timer(this.components);
            this.tmrMinimize = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tmrRMC = new System.Windows.Forms.Timer(this.components);
            this.tmrDisplay = new System.Windows.Forms.Timer(this.components);
            this.tmrZDA = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbAge = new System.Windows.Forms.Label();
            this.lbElev = new System.Windows.Forms.Label();
            this.lbSats = new System.Windows.Forms.Label();
            this.lbHDOP = new System.Windows.Forms.Label();
            this.lbSim = new System.Windows.Forms.Label();
            this.btnZDA = new System.Windows.Forms.Button();
            this.tbZDA = new System.Windows.Forms.TextBox();
            this.btnRMC = new System.Windows.Forms.Button();
            this.tbRMC = new System.Windows.Forms.TextBox();
            this.btnVTG = new System.Windows.Forms.Button();
            this.btnGGA = new System.Windows.Forms.Button();
            this.tbVTG = new System.Windows.Forms.TextBox();
            this.tbGGA = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PortIndicator1 = new System.Windows.Forms.Label();
            this.btnRescan = new System.Windows.Forms.Button();
            this.lbPort = new System.Windows.Forms.Label();
            this.btnConnect1 = new System.Windows.Forms.Button();
            this.cboBaud1 = new System.Windows.Forms.ComboBox();
            this.cboPort1 = new System.Windows.Forms.ComboBox();
            this.lbBaud = new System.Windows.Forms.Label();
            this.lbQuality = new System.Windows.Forms.Label();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.lbLat = new System.Windows.Forms.Label();
            this.lbLon = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cboPrecision = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ckSimulate = new System.Windows.Forms.CheckBox();
            this.rbGP = new System.Windows.Forms.RadioButton();
            this.rbGN = new System.Windows.Forms.RadioButton();
            this.ckGSA = new System.Windows.Forms.CheckBox();
            this.ckRoll = new System.Windows.Forms.CheckBox();
            this.ckAutoConnect = new System.Windows.Forms.CheckBox();
            this.ckAutoHide = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cboZDA = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cboRMC = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cboVTG = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cboGGA = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tmrGSA = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrGGA
            // 
            this.tmrGGA.Interval = 5000;
            this.tmrGGA.Tick += new System.EventHandler(this.tmrGGA_Tick);
            // 
            // tmrVTG
            // 
            this.tmrVTG.Tick += new System.EventHandler(this.tmrVTG_Tick);
            // 
            // tmrMinimize
            // 
            this.tmrMinimize.Enabled = true;
            this.tmrMinimize.Interval = 120000;
            this.tmrMinimize.Tick += new System.EventHandler(this.tmrMinimize_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // tmrRMC
            // 
            this.tmrRMC.Tick += new System.EventHandler(this.tmrRMC_Tick);
            // 
            // tmrDisplay
            // 
            this.tmrDisplay.Enabled = true;
            this.tmrDisplay.Interval = 500;
            this.tmrDisplay.Tick += new System.EventHandler(this.tmrDisplay_Tick);
            // 
            // tmrZDA
            // 
            this.tmrZDA.Tick += new System.EventHandler(this.tmrZDA_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(200, 40);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(626, 536);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 309;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.lbAge);
            this.tabPage1.Controls.Add(this.lbElev);
            this.tabPage1.Controls.Add(this.lbSats);
            this.tabPage1.Controls.Add(this.lbHDOP);
            this.tabPage1.Controls.Add(this.lbSim);
            this.tabPage1.Controls.Add(this.btnZDA);
            this.tabPage1.Controls.Add(this.tbZDA);
            this.tabPage1.Controls.Add(this.btnRMC);
            this.tabPage1.Controls.Add(this.tbRMC);
            this.tabPage1.Controls.Add(this.btnVTG);
            this.tabPage1.Controls.Add(this.btnGGA);
            this.tabPage1.Controls.Add(this.tbVTG);
            this.tabPage1.Controls.Add(this.tbGGA);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lbQuality);
            this.tabPage1.Controls.Add(this.lbSpeed);
            this.tabPage1.Controls.Add(this.lbLat);
            this.tabPage1.Controls.Add(this.lbLon);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(618, 488);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            // 
            // lbAge
            // 
            this.lbAge.AutoSize = true;
            this.lbAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAge.Location = new System.Drawing.Point(452, 218);
            this.lbAge.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbAge.Name = "lbAge";
            this.lbAge.Size = new System.Drawing.Size(126, 24);
            this.lbAge.TabIndex = 168;
            this.lbAge.Text = "1234567890.";
            this.lbAge.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbElev
            // 
            this.lbElev.AutoSize = true;
            this.lbElev.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbElev.Location = new System.Drawing.Point(452, 188);
            this.lbElev.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbElev.Name = "lbElev";
            this.lbElev.Size = new System.Drawing.Size(126, 24);
            this.lbElev.TabIndex = 167;
            this.lbElev.Text = "1234567890.";
            this.lbElev.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSats
            // 
            this.lbSats.AutoSize = true;
            this.lbSats.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSats.Location = new System.Drawing.Point(452, 158);
            this.lbSats.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbSats.Name = "lbSats";
            this.lbSats.Size = new System.Drawing.Size(126, 24);
            this.lbSats.TabIndex = 166;
            this.lbSats.Text = "1234567890.";
            this.lbSats.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbHDOP
            // 
            this.lbHDOP.AutoSize = true;
            this.lbHDOP.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHDOP.Location = new System.Drawing.Point(452, 128);
            this.lbHDOP.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbHDOP.Name = "lbHDOP";
            this.lbHDOP.Size = new System.Drawing.Size(126, 24);
            this.lbHDOP.TabIndex = 165;
            this.lbHDOP.Text = "1234567890.";
            this.lbHDOP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSim
            // 
            this.lbSim.BackColor = System.Drawing.Color.Transparent;
            this.lbSim.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSim.Location = new System.Drawing.Point(244, 216);
            this.lbSim.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbSim.Name = "lbSim";
            this.lbSim.Size = new System.Drawing.Size(102, 24);
            this.lbSim.TabIndex = 317;
            this.lbSim.Text = "Simulated";
            this.lbSim.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnZDA
            // 
            this.btnZDA.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZDA.Location = new System.Drawing.Point(6, 431);
            this.btnZDA.Name = "btnZDA";
            this.btnZDA.Size = new System.Drawing.Size(75, 47);
            this.btnZDA.TabIndex = 316;
            this.btnZDA.Text = "ZDA";
            this.btnZDA.UseVisualStyleBackColor = true;
            this.btnZDA.Click += new System.EventHandler(this.btnZDA_Click);
            // 
            // tbZDA
            // 
            this.tbZDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZDA.Location = new System.Drawing.Point(93, 428);
            this.tbZDA.Multiline = true;
            this.tbZDA.Name = "tbZDA";
            this.tbZDA.ReadOnly = true;
            this.tbZDA.Size = new System.Drawing.Size(512, 52);
            this.tbZDA.TabIndex = 315;
            // 
            // btnRMC
            // 
            this.btnRMC.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRMC.Location = new System.Drawing.Point(6, 373);
            this.btnRMC.Name = "btnRMC";
            this.btnRMC.Size = new System.Drawing.Size(75, 47);
            this.btnRMC.TabIndex = 314;
            this.btnRMC.Text = "RMC";
            this.btnRMC.UseVisualStyleBackColor = true;
            this.btnRMC.Click += new System.EventHandler(this.btnRMC_Click);
            // 
            // tbRMC
            // 
            this.tbRMC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRMC.Location = new System.Drawing.Point(93, 370);
            this.tbRMC.Multiline = true;
            this.tbRMC.Name = "tbRMC";
            this.tbRMC.ReadOnly = true;
            this.tbRMC.Size = new System.Drawing.Size(512, 52);
            this.tbRMC.TabIndex = 313;
            // 
            // btnVTG
            // 
            this.btnVTG.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVTG.Location = new System.Drawing.Point(6, 315);
            this.btnVTG.Name = "btnVTG";
            this.btnVTG.Size = new System.Drawing.Size(75, 47);
            this.btnVTG.TabIndex = 312;
            this.btnVTG.Text = "VTG";
            this.btnVTG.UseVisualStyleBackColor = true;
            this.btnVTG.Click += new System.EventHandler(this.btnVTG_Click);
            // 
            // btnGGA
            // 
            this.btnGGA.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGGA.Location = new System.Drawing.Point(6, 257);
            this.btnGGA.Name = "btnGGA";
            this.btnGGA.Size = new System.Drawing.Size(75, 47);
            this.btnGGA.TabIndex = 311;
            this.btnGGA.Text = "GGA";
            this.btnGGA.UseVisualStyleBackColor = true;
            this.btnGGA.Click += new System.EventHandler(this.btnGGA_Click);
            // 
            // tbVTG
            // 
            this.tbVTG.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVTG.Location = new System.Drawing.Point(93, 312);
            this.tbVTG.Multiline = true;
            this.tbVTG.Name = "tbVTG";
            this.tbVTG.ReadOnly = true;
            this.tbVTG.Size = new System.Drawing.Size(512, 52);
            this.tbVTG.TabIndex = 310;
            // 
            // tbGGA
            // 
            this.tbGGA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGGA.Location = new System.Drawing.Point(93, 254);
            this.tbGGA.Multiline = true;
            this.tbGGA.Name = "tbGGA";
            this.tbGGA.ReadOnly = true;
            this.tbGGA.Size = new System.Drawing.Size(512, 52);
            this.tbGGA.TabIndex = 309;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PortIndicator1);
            this.groupBox1.Controls.Add(this.btnRescan);
            this.groupBox1.Controls.Add(this.lbPort);
            this.groupBox1.Controls.Add(this.btnConnect1);
            this.groupBox1.Controls.Add(this.cboBaud1);
            this.groupBox1.Controls.Add(this.cboPort1);
            this.groupBox1.Controls.Add(this.lbBaud);
            this.groupBox1.Location = new System.Drawing.Point(8, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(597, 106);
            this.groupBox1.TabIndex = 169;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial Port";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // PortIndicator1
            // 
            this.PortIndicator1.BackColor = System.Drawing.SystemColors.Control;
            this.PortIndicator1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PortIndicator1.Image = global::GPS_Out.Properties.Resources.Off;
            this.PortIndicator1.Location = new System.Drawing.Point(453, 54);
            this.PortIndicator1.Name = "PortIndicator1";
            this.PortIndicator1.Size = new System.Drawing.Size(41, 37);
            this.PortIndicator1.TabIndex = 129;
            this.PortIndicator1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRescan
            // 
            this.btnRescan.BackColor = System.Drawing.Color.Transparent;
            this.btnRescan.FlatAppearance.BorderSize = 0;
            this.btnRescan.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightGreen;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescan.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRescan.Image = global::GPS_Out.Properties.Resources.ScanPorts;
            this.btnRescan.Location = new System.Drawing.Point(508, 24);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(72, 72);
            this.btnRescan.TabIndex = 151;
            this.btnRescan.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // lbPort
            // 
            this.lbPort.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPort.Location = new System.Drawing.Point(177, 27);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(127, 23);
            this.lbPort.TabIndex = 126;
            this.lbPort.Text = "Port";
            this.lbPort.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnConnect1
            // 
            this.btnConnect1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnConnect1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConnect1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect1.Location = new System.Drawing.Point(16, 54);
            this.btnConnect1.Name = "btnConnect1";
            this.btnConnect1.Size = new System.Drawing.Size(145, 37);
            this.btnConnect1.TabIndex = 128;
            this.btnConnect1.Text = "Connect";
            this.btnConnect1.UseVisualStyleBackColor = false;
            this.btnConnect1.Click += new System.EventHandler(this.btnConnect1_Click);
            // 
            // cboBaud1
            // 
            this.cboBaud1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboBaud1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaud1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBaud1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboBaud1.FormattingEnabled = true;
            this.cboBaud1.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboBaud1.Location = new System.Drawing.Point(323, 54);
            this.cboBaud1.Name = "cboBaud1";
            this.cboBaud1.Size = new System.Drawing.Size(127, 37);
            this.cboBaud1.TabIndex = 130;
            this.cboBaud1.SelectedIndexChanged += new System.EventHandler(this.cboBaud1_SelectedIndexChanged);
            // 
            // cboPort1
            // 
            this.cboPort1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPort1.FormattingEnabled = true;
            this.cboPort1.Location = new System.Drawing.Point(175, 54);
            this.cboPort1.Name = "cboPort1";
            this.cboPort1.Size = new System.Drawing.Size(127, 37);
            this.cboPort1.TabIndex = 127;
            this.cboPort1.SelectedIndexChanged += new System.EventHandler(this.cboPort1_SelectedIndexChanged);
            // 
            // lbBaud
            // 
            this.lbBaud.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBaud.Location = new System.Drawing.Point(327, 27);
            this.lbBaud.Name = "lbBaud";
            this.lbBaud.Size = new System.Drawing.Size(127, 23);
            this.lbBaud.TabIndex = 131;
            this.lbBaud.Text = "Baud";
            this.lbBaud.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbQuality
            // 
            this.lbQuality.BackColor = System.Drawing.Color.Transparent;
            this.lbQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbQuality.Location = new System.Drawing.Point(120, 216);
            this.lbQuality.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbQuality.Name = "lbQuality";
            this.lbQuality.Size = new System.Drawing.Size(126, 24);
            this.lbQuality.TabIndex = 164;
            this.lbQuality.Text = "RTK Fix";
            this.lbQuality.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSpeed
            // 
            this.lbSpeed.AutoSize = true;
            this.lbSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSpeed.Location = new System.Drawing.Point(119, 188);
            this.lbSpeed.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(126, 24);
            this.lbSpeed.TabIndex = 163;
            this.lbSpeed.Text = "1234567890.";
            this.lbSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbLat
            // 
            this.lbLat.AutoSize = true;
            this.lbLat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLat.Location = new System.Drawing.Point(119, 128);
            this.lbLat.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLat.Name = "lbLat";
            this.lbLat.Size = new System.Drawing.Size(126, 24);
            this.lbLat.TabIndex = 162;
            this.lbLat.Text = "1234567890.";
            this.lbLat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbLon
            // 
            this.lbLon.AutoSize = true;
            this.lbLon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLon.Location = new System.Drawing.Point(119, 158);
            this.lbLon.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLon.Name = "lbLon";
            this.lbLon.Size = new System.Drawing.Size(126, 24);
            this.lbLon.TabIndex = 161;
            this.lbLon.Text = "1234567890.";
            this.lbLon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(391, 216);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 24);
            this.label8.TabIndex = 160;
            this.label8.Text = "Age:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(350, 186);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 24);
            this.label7.TabIndex = 159;
            this.label7.Text = "Elevation:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(391, 156);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 24);
            this.label6.TabIndex = 158;
            this.label6.Text = "Sats:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(372, 126);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 24);
            this.label5.TabIndex = 157;
            this.label5.Text = "HDOP:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 216);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 24);
            this.label4.TabIndex = 156;
            this.label4.Text = "Quality:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 186);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 24);
            this.label3.TabIndex = 155;
            this.label3.Text = "Speed:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 24);
            this.label2.TabIndex = 154;
            this.label2.Text = "Latitude:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 156);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 24);
            this.label1.TabIndex = 153;
            this.label1.Text = "Longitude:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage2.Controls.Add(this.cboPrecision);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.ckSimulate);
            this.tabPage2.Controls.Add(this.rbGP);
            this.tabPage2.Controls.Add(this.rbGN);
            this.tabPage2.Controls.Add(this.ckGSA);
            this.tabPage2.Controls.Add(this.ckRoll);
            this.tabPage2.Controls.Add(this.ckAutoConnect);
            this.tabPage2.Controls.Add(this.ckAutoHide);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(618, 488);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            // 
            // cboPrecision
            // 
            this.cboPrecision.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrecision.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPrecision.FormattingEnabled = true;
            this.cboPrecision.Items.AddRange(new object[] {
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.cboPrecision.Location = new System.Drawing.Point(481, 425);
            this.cboPrecision.Name = "cboPrecision";
            this.cboPrecision.Size = new System.Drawing.Size(71, 47);
            this.cboPrecision.TabIndex = 317;
            this.cboPrecision.SelectedIndexChanged += new System.EventHandler(this.cbPrecision_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(367, 418);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 61);
            this.label12.TabIndex = 316;
            this.label12.Text = "Decimal Precision";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ckSimulate
            // 
            this.ckSimulate.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckSimulate.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ckSimulate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckSimulate.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckSimulate.Location = new System.Drawing.Point(97, 418);
            this.ckSimulate.Name = "ckSimulate";
            this.ckSimulate.Size = new System.Drawing.Size(182, 61);
            this.ckSimulate.TabIndex = 315;
            this.ckSimulate.Text = "Simulate Data";
            this.ckSimulate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckSimulate.UseVisualStyleBackColor = true;
            this.ckSimulate.CheckedChanged += new System.EventHandler(this.ckSimulate_CheckedChanged);
            // 
            // rbGP
            // 
            this.rbGP.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbGP.Checked = true;
            this.rbGP.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbGP.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbGP.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbGP.Location = new System.Drawing.Point(97, 264);
            this.rbGP.Name = "rbGP";
            this.rbGP.Size = new System.Drawing.Size(182, 61);
            this.rbGP.TabIndex = 314;
            this.rbGP.TabStop = true;
            this.rbGP.Text = "Send $GP---";
            this.rbGP.UseVisualStyleBackColor = true;
            this.rbGP.CheckedChanged += new System.EventHandler(this.rbGP_CheckedChanged);
            // 
            // rbGN
            // 
            this.rbGN.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbGN.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rbGN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbGN.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbGN.Location = new System.Drawing.Point(370, 264);
            this.rbGN.Name = "rbGN";
            this.rbGN.Size = new System.Drawing.Size(182, 61);
            this.rbGN.TabIndex = 313;
            this.rbGN.Text = "Send $GN---";
            this.rbGN.UseVisualStyleBackColor = true;
            // 
            // ckGSA
            // 
            this.ckGSA.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckGSA.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ckGSA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckGSA.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckGSA.Location = new System.Drawing.Point(370, 341);
            this.ckGSA.Name = "ckGSA";
            this.ckGSA.Size = new System.Drawing.Size(182, 61);
            this.ckGSA.TabIndex = 312;
            this.ckGSA.Text = "Send empty GSA";
            this.ckGSA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckGSA.UseVisualStyleBackColor = true;
            this.ckGSA.CheckedChanged += new System.EventHandler(this.ckGSA_CheckedChanged);
            // 
            // ckRoll
            // 
            this.ckRoll.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ckRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckRoll.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckRoll.Location = new System.Drawing.Point(97, 341);
            this.ckRoll.Name = "ckRoll";
            this.ckRoll.Size = new System.Drawing.Size(182, 61);
            this.ckRoll.TabIndex = 311;
            this.ckRoll.Text = "Roll Corrected";
            this.ckRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckRoll.UseVisualStyleBackColor = true;
            this.ckRoll.CheckedChanged += new System.EventHandler(this.ckRoll_CheckedChanged);
            // 
            // ckAutoConnect
            // 
            this.ckAutoConnect.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckAutoConnect.Checked = true;
            this.ckAutoConnect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAutoConnect.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ckAutoConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckAutoConnect.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckAutoConnect.Location = new System.Drawing.Point(97, 187);
            this.ckAutoConnect.Name = "ckAutoConnect";
            this.ckAutoConnect.Size = new System.Drawing.Size(182, 61);
            this.ckAutoConnect.TabIndex = 306;
            this.ckAutoConnect.Text = "Auto Connect";
            this.ckAutoConnect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckAutoConnect.UseVisualStyleBackColor = true;
            this.ckAutoConnect.CheckedChanged += new System.EventHandler(this.ckAutoConnect_CheckedChanged);
            // 
            // ckAutoHide
            // 
            this.ckAutoHide.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckAutoHide.Checked = true;
            this.ckAutoHide.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckAutoHide.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ckAutoHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckAutoHide.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckAutoHide.Location = new System.Drawing.Point(370, 187);
            this.ckAutoHide.Name = "ckAutoHide";
            this.ckAutoHide.Size = new System.Drawing.Size(182, 61);
            this.ckAutoHide.TabIndex = 305;
            this.ckAutoHide.Text = "Auto Hide";
            this.ckAutoHide.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckAutoHide.UseVisualStyleBackColor = true;
            this.ckAutoHide.CheckedChanged += new System.EventHandler(this.ckAutoHide_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.cboZDA);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.cboRMC);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.cboVTG);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cboGGA);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(97, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(455, 175);
            this.groupBox2.TabIndex = 155;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Transfer Rate";
            this.groupBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(392, 122);
            this.label19.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(39, 25);
            this.label19.TabIndex = 164;
            this.label19.Text = "Hz";
            // 
            // cboZDA
            // 
            this.cboZDA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboZDA.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboZDA.FormattingEnabled = true;
            this.cboZDA.Items.AddRange(new object[] {
            "0",
            "1",
            "5",
            "10"});
            this.cboZDA.Location = new System.Drawing.Point(317, 111);
            this.cboZDA.Name = "cboZDA";
            this.cboZDA.Size = new System.Drawing.Size(71, 47);
            this.cboZDA.TabIndex = 163;
            this.cboZDA.SelectedIndexChanged += new System.EventHandler(this.cboZDA_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(259, 122);
            this.label20.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(55, 25);
            this.label20.TabIndex = 162;
            this.label20.Text = "ZDA";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(392, 50);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(39, 25);
            this.label17.TabIndex = 161;
            this.label17.Text = "Hz";
            // 
            // cboRMC
            // 
            this.cboRMC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRMC.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRMC.FormattingEnabled = true;
            this.cboRMC.Items.AddRange(new object[] {
            "0",
            "1",
            "5",
            "10"});
            this.cboRMC.Location = new System.Drawing.Point(317, 39);
            this.cboRMC.Name = "cboRMC";
            this.cboRMC.Size = new System.Drawing.Size(71, 47);
            this.cboRMC.TabIndex = 160;
            this.cboRMC.SelectedIndexChanged += new System.EventHandler(this.cboRMC_SelectedIndexChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(254, 50);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(60, 25);
            this.label18.TabIndex = 159;
            this.label18.Text = "RMC";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(150, 122);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 25);
            this.label16.TabIndex = 158;
            this.label16.Text = "Hz";
            // 
            // cboVTG
            // 
            this.cboVTG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVTG.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboVTG.FormattingEnabled = true;
            this.cboVTG.Items.AddRange(new object[] {
            "0",
            "1",
            "5",
            "10"});
            this.cboVTG.Location = new System.Drawing.Point(75, 111);
            this.cboVTG.Name = "cboVTG";
            this.cboVTG.Size = new System.Drawing.Size(71, 47);
            this.cboVTG.TabIndex = 157;
            this.cboVTG.SelectedIndexChanged += new System.EventHandler(this.cboVTG_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(150, 50);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(39, 25);
            this.label11.TabIndex = 156;
            this.label11.Text = "Hz";
            // 
            // cboGGA
            // 
            this.cboGGA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGGA.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboGGA.FormattingEnabled = true;
            this.cboGGA.Items.AddRange(new object[] {
            "0",
            "1",
            "5",
            "10"});
            this.cboGGA.Location = new System.Drawing.Point(75, 39);
            this.cboGGA.Name = "cboGGA";
            this.cboGGA.Size = new System.Drawing.Size(71, 47);
            this.cboGGA.TabIndex = 155;
            this.cboGGA.SelectedIndexChanged += new System.EventHandler(this.cboGGA_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 122);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 25);
            this.label10.TabIndex = 154;
            this.label10.Text = "VTG";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 50);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 25);
            this.label9.TabIndex = 153;
            this.label9.Text = "GGA";
            // 
            // tmrGSA
            // 
            this.tmrGSA.Interval = 1000;
            this.tmrGSA.Tick += new System.EventHandler(this.tmrGSA_Tick);
            // 
            // frmStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(626, 536);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "frmStart";
            this.Text = "GPS_Out";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmStart_FormClosed);
            this.Load += new System.EventHandler(this.frmStart_Load);
            this.Resize += new System.EventHandler(this.frmStart_Resize);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrGGA;
        private System.Windows.Forms.Timer tmrVTG;
        private System.Windows.Forms.Timer tmrMinimize;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer tmrRMC;
        private System.Windows.Forms.Timer tmrDisplay;
        private System.Windows.Forms.Timer tmrZDA;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label PortIndicator1;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Button btnConnect1;
        private System.Windows.Forms.ComboBox cboBaud1;
        private System.Windows.Forms.ComboBox cboPort1;
        private System.Windows.Forms.Label lbBaud;
        private System.Windows.Forms.Label lbAge;
        private System.Windows.Forms.Label lbElev;
        private System.Windows.Forms.Label lbSats;
        private System.Windows.Forms.Label lbHDOP;
        private System.Windows.Forms.Label lbQuality;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label lbLat;
        private System.Windows.Forms.Label lbLon;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnZDA;
        private System.Windows.Forms.TextBox tbZDA;
        private System.Windows.Forms.Button btnRMC;
        private System.Windows.Forms.TextBox tbRMC;
        private System.Windows.Forms.Button btnVTG;
        private System.Windows.Forms.Button btnGGA;
        private System.Windows.Forms.TextBox tbVTG;
        private System.Windows.Forms.TextBox tbGGA;
        private System.Windows.Forms.CheckBox ckAutoConnect;
        private System.Windows.Forms.CheckBox ckAutoHide;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cboZDA;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox cboRMC;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cboVTG;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cboGGA;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton rbGN;
        private System.Windows.Forms.CheckBox ckGSA;
        private System.Windows.Forms.CheckBox ckRoll;
        private System.Windows.Forms.RadioButton rbGP;
        private System.Windows.Forms.Timer tmrGSA;
        private System.Windows.Forms.Label lbSim;
        private System.Windows.Forms.CheckBox ckSimulate;
        private System.Windows.Forms.ComboBox cboPrecision;
        private System.Windows.Forms.Label label12;
    }
}

