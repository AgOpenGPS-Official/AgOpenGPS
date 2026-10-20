namespace AgIO
{
    partial class FormNtripSettings
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
            this.tglUse = new AgIO.ToggleSwitch();
            this.lblUse = new System.Windows.Forms.Label();
            this.lblProfile = new System.Windows.Forms.Label();
            this.lblProfileName = new System.Windows.Forms.Label();
            this.segPage = new AgIO.SegmentedControl();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblHost = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.numPort = new AgIO.NumberField();
            this.lblMount = new System.Windows.Forms.Label();
            this.txtMount = new System.Windows.Forms.TextBox();
            this.btnMounts = new AgIO.RoundedButton();
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnShowPassword = new AgIO.RoundedButton();
            this.lblVersion = new System.Windows.Forms.Label();
            this.segVersion = new AgIO.SegmentedControl();
            this.lblRaw = new System.Windows.Forms.Label();
            this.tglRaw = new AgIO.ToggleSwitch();
            this.pnlFallback = new System.Windows.Forms.Panel();
            this.tglFallback = new AgIO.ToggleSwitch();
            this.lblFallbackOn = new System.Windows.Forms.Label();
            this.segFbType = new AgIO.SegmentedControl();
            this.segAuto = new AgIO.SegmentedControl();
            this.lblAfter = new System.Windows.Forms.Label();
            this.numAfter = new AgIO.NumberField();
            this.lblAfter2 = new System.Windows.Forms.Label();
            this.pnlFbService = new System.Windows.Forms.Panel();
            this.lblFbHost = new System.Windows.Forms.Label();
            this.txtFbHost = new System.Windows.Forms.TextBox();
            this.lblFbPort = new System.Windows.Forms.Label();
            this.numFbPort = new AgIO.NumberField();
            this.lblFbMount = new System.Windows.Forms.Label();
            this.txtFbMount = new System.Windows.Forms.TextBox();
            this.lblFbUser = new System.Windows.Forms.Label();
            this.txtFbUser = new System.Windows.Forms.TextBox();
            this.lblFbPassword = new System.Windows.Forms.Label();
            this.txtFbPassword = new System.Windows.Forms.TextBox();
            this.btnFbMounts = new AgIO.RoundedButton();
            this.pnlFbRadio = new System.Windows.Forms.Panel();
            this.lblRadioPort = new System.Windows.Forms.Label();
            this.cboRadioPort = new System.Windows.Forms.ComboBox();
            this.lblRadioBaud = new System.Windows.Forms.Label();
            this.cboRadioBaud = new System.Windows.Forms.ComboBox();
            this.pnlOutput = new System.Windows.Forms.Panel();
            this.lblPos = new System.Windows.Forms.Label();
            this.segPosition = new AgIO.SegmentedControl();
            this.lblLat = new System.Windows.Forms.Label();
            this.numLat = new AgIO.NumberField();
            this.lblLon = new System.Windows.Forms.Label();
            this.numLon = new AgIO.NumberField();
            this.lblAlt = new System.Windows.Forms.Label();
            this.numAlt = new AgIO.NumberField();
            this.lblInterval = new System.Windows.Forms.Label();
            this.numInterval = new AgIO.NumberField();
            this.lblOutput = new System.Windows.Forms.Label();
            this.segOutput = new AgIO.SegmentedControl();
            this.lblNetPort = new System.Windows.Forms.Label();
            this.numNetPort = new AgIO.NumberField();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnCheck = new AgIO.RoundedButton();
            this.btnCancel = new AgIO.RoundedButton();
            this.btnSave = new AgIO.RoundedButton();
            this.pnlFallback.SuspendLayout();
            this.pnlFbRadio.SuspendLayout();
            this.pnlFbService.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // tglUse
            // 
            this.tglUse.Location = new System.Drawing.Point(20, 12);
            this.tglUse.Name = "tglUse";
            this.tglUse.Size = new System.Drawing.Size(84, 46);
            this.tglUse.TabIndex = 0;
            this.tglUse.CheckedChanged += new System.EventHandler(this.tglUse_CheckedChanged);
            // 
            // lblUse
            // 
            this.lblUse.AutoSize = false;
            this.lblUse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUse.Location = new System.Drawing.Point(116, 20);
            this.lblUse.Name = "lblUse";
            this.lblUse.Size = new System.Drawing.Size(300, 30);
            this.lblUse.TabIndex = 1;
            this.lblUse.Text = "Use NTRIP corrections";
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = false;
            this.lblProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProfile.Location = new System.Drawing.Point(440, 20);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(110, 30);
            this.lblProfile.TabIndex = 2;
            this.lblProfile.Text = "Profile";
            // 
            // lblProfileName
            // 
            this.lblProfileName.AutoSize = false;
            this.lblProfileName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProfileName.Location = new System.Drawing.Point(560, 20);
            this.lblProfileName.Name = "lblProfileName";
            this.lblProfileName.Size = new System.Drawing.Size(420, 30);
            this.lblProfileName.TabIndex = 3;
            this.lblProfileName.Text = "Default";
            // 
            // segPage
            // 
            this.segPage.ItemsText = "Main service|Fallback|Output and position";
            this.segPage.Location = new System.Drawing.Point(20, 76);
            this.segPage.Name = "segPage";
            this.segPage.Size = new System.Drawing.Size(960, 52);
            this.segPage.TabIndex = 4;
            this.segPage.SelectedIndexChanged += new System.EventHandler(this.segPage_SelectedIndexChanged);
            // 
            // pnlMain
            // 
            this.pnlMain.Location = new System.Drawing.Point(20, 142);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(960, 410);
            this.pnlMain.TabIndex = 5;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = false;
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHost.Location = new System.Drawing.Point(0, 8);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(220, 30);
            this.lblHost.TabIndex = 6;
            this.lblHost.Text = "Address";
            // 
            // txtHost
            // 
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost.ReadOnly = true;
            this.txtHost.BackColor = System.Drawing.Color.White;
            this.txtHost.Location = new System.Drawing.Point(230, 8);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(720, 31);
            this.txtHost.TabIndex = 7;
            this.txtHost.Click += new System.EventHandler(this.txtHost_Click);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = false;
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPort.Location = new System.Drawing.Point(0, 64);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(220, 30);
            this.lblPort.TabIndex = 8;
            this.lblPort.Text = "Port";
            // 
            // numPort
            // 
            this.numPort.Minimum = 1;
            this.numPort.Maximum = 65535;
            this.numPort.Location = new System.Drawing.Point(230, 56);
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(160, 48);
            this.numPort.TabIndex = 9;
            // 
            // lblMount
            // 
            this.lblMount.AutoSize = false;
            this.lblMount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMount.Location = new System.Drawing.Point(0, 120);
            this.lblMount.Name = "lblMount";
            this.lblMount.Size = new System.Drawing.Size(220, 30);
            this.lblMount.TabIndex = 10;
            this.lblMount.Text = "Mountpoint";
            // 
            // txtMount
            // 
            this.txtMount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMount.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMount.ReadOnly = true;
            this.txtMount.BackColor = System.Drawing.Color.White;
            this.txtMount.Location = new System.Drawing.Point(230, 120);
            this.txtMount.Name = "txtMount";
            this.txtMount.Size = new System.Drawing.Size(510, 31);
            this.txtMount.TabIndex = 11;
            this.txtMount.Click += new System.EventHandler(this.txtMount_Click);
            // 
            // btnMounts
            // 
            this.btnMounts.Location = new System.Drawing.Point(760, 108);
            this.btnMounts.Name = "btnMounts";
            this.btnMounts.Size = new System.Drawing.Size(200, 56);
            this.btnMounts.TabIndex = 12;
            this.btnMounts.Text = "Choose from list...";
            this.btnMounts.Click += new System.EventHandler(this.btnMounts_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = false;
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUser.Location = new System.Drawing.Point(0, 184);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(220, 30);
            this.lblUser.TabIndex = 13;
            this.lblUser.Text = "User name";
            // 
            // txtUser
            // 
            this.txtUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUser.ReadOnly = true;
            this.txtUser.BackColor = System.Drawing.Color.White;
            this.txtUser.Location = new System.Drawing.Point(230, 184);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(720, 31);
            this.txtUser.TabIndex = 14;
            this.txtUser.Click += new System.EventHandler(this.txtUser_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = false;
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPassword.Location = new System.Drawing.Point(0, 240);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(220, 30);
            this.lblPassword.TabIndex = 15;
            this.lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ReadOnly = true;
            this.txtPassword.BackColor = System.Drawing.Color.White;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Location = new System.Drawing.Point(230, 240);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(510, 31);
            this.txtPassword.TabIndex = 16;
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            // 
            // btnShowPassword
            // 
            this.btnShowPassword.Location = new System.Drawing.Point(760, 228);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(200, 56);
            this.btnShowPassword.TabIndex = 17;
            this.btnShowPassword.Text = "Show";
            this.btnShowPassword.Click += new System.EventHandler(this.btnShowPassword_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = false;
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVersion.Location = new System.Drawing.Point(0, 304);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(220, 30);
            this.lblVersion.TabIndex = 18;
            this.lblVersion.Text = "NTRIP version";
            // 
            // segVersion
            // 
            this.segVersion.ItemsText = "Automatic|1.0|2.0";
            this.segVersion.Location = new System.Drawing.Point(230, 294);
            this.segVersion.Name = "segVersion";
            this.segVersion.Size = new System.Drawing.Size(510, 52);
            this.segVersion.TabIndex = 19;
            // 
            // lblRaw
            // 
            this.lblRaw.AutoSize = false;
            this.lblRaw.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRaw.Location = new System.Drawing.Point(0, 362);
            this.lblRaw.Name = "lblRaw";
            this.lblRaw.Size = new System.Drawing.Size(220, 30);
            this.lblRaw.TabIndex = 20;
            this.lblRaw.Text = "Raw TCP stream";
            // 
            // tglRaw
            // 
            this.tglRaw.Location = new System.Drawing.Point(230, 354);
            this.tglRaw.Name = "tglRaw";
            this.tglRaw.Size = new System.Drawing.Size(84, 46);
            this.tglRaw.TabIndex = 21;
            // 
            // pnlFallback
            // 
            this.pnlFallback.Location = new System.Drawing.Point(20, 142);
            this.pnlFallback.Name = "pnlFallback";
            this.pnlFallback.Size = new System.Drawing.Size(960, 410);
            this.pnlFallback.TabIndex = 22;
            // 
            // tglFallback
            // 
            this.tglFallback.Location = new System.Drawing.Point(0, 4);
            this.tglFallback.Name = "tglFallback";
            this.tglFallback.Size = new System.Drawing.Size(84, 46);
            this.tglFallback.TabIndex = 23;
            this.tglFallback.CheckedChanged += new System.EventHandler(this.tglFallback_CheckedChanged);
            // 
            // lblFallbackOn
            // 
            this.lblFallbackOn.AutoSize = false;
            this.lblFallbackOn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFallbackOn.Location = new System.Drawing.Point(96, 12);
            this.lblFallbackOn.Name = "lblFallbackOn";
            this.lblFallbackOn.Size = new System.Drawing.Size(600, 30);
            this.lblFallbackOn.TabIndex = 24;
            this.lblFallbackOn.Text = "Use a fallback when the main service fails";
            // 
            // segFbType
            // 
            this.segFbType.ItemsText = "Another NTRIP service|Serial radio";
            this.segFbType.Location = new System.Drawing.Point(0, 62);
            this.segFbType.Name = "segFbType";
            this.segFbType.Size = new System.Drawing.Size(960, 52);
            this.segFbType.TabIndex = 25;
            this.segFbType.SelectedIndexChanged += new System.EventHandler(this.segFbType_SelectedIndexChanged);
            // 
            // segAuto
            // 
            this.segAuto.ItemsText = "Switch by itself|I switch myself";
            this.segAuto.Location = new System.Drawing.Point(0, 124);
            this.segAuto.Name = "segAuto";
            this.segAuto.Size = new System.Drawing.Size(440, 52);
            this.segAuto.TabIndex = 26;
            // 
            // lblAfter
            // 
            this.lblAfter.AutoSize = false;
            this.lblAfter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAfter.Location = new System.Drawing.Point(460, 132);
            this.lblAfter.Name = "lblAfter";
            this.lblAfter.Size = new System.Drawing.Size(150, 30);
            this.lblAfter.TabIndex = 27;
            this.lblAfter.Text = "Switch after";
            // 
            // numAfter
            // 
            this.numAfter.Minimum = 1;
            this.numAfter.Maximum = 20;
            this.numAfter.Location = new System.Drawing.Point(620, 124);
            this.numAfter.Name = "numAfter";
            this.numAfter.Size = new System.Drawing.Size(120, 48);
            this.numAfter.TabIndex = 28;
            // 
            // lblAfter2
            // 
            this.lblAfter2.AutoSize = false;
            this.lblAfter2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAfter2.Location = new System.Drawing.Point(750, 132);
            this.lblAfter2.Name = "lblAfter2";
            this.lblAfter2.Size = new System.Drawing.Size(200, 30);
            this.lblAfter2.TabIndex = 29;
            this.lblAfter2.Text = "failed tries";
            // 
            // pnlFbService
            // 
            this.pnlFbService.Location = new System.Drawing.Point(0, 196);
            this.pnlFbService.Name = "pnlFbService";
            this.pnlFbService.Size = new System.Drawing.Size(960, 210);
            this.pnlFbService.TabIndex = 30;
            // 
            // lblFbHost
            // 
            this.lblFbHost.AutoSize = false;
            this.lblFbHost.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFbHost.Location = new System.Drawing.Point(0, 8);
            this.lblFbHost.Name = "lblFbHost";
            this.lblFbHost.Size = new System.Drawing.Size(200, 30);
            this.lblFbHost.TabIndex = 31;
            this.lblFbHost.Text = "Address";
            // 
            // txtFbHost
            // 
            this.txtFbHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFbHost.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFbHost.ReadOnly = true;
            this.txtFbHost.BackColor = System.Drawing.Color.White;
            this.txtFbHost.Location = new System.Drawing.Point(210, 8);
            this.txtFbHost.Name = "txtFbHost";
            this.txtFbHost.Size = new System.Drawing.Size(740, 31);
            this.txtFbHost.TabIndex = 32;
            this.txtFbHost.Click += new System.EventHandler(this.txtFbHost_Click);
            // 
            // lblFbPort
            // 
            this.lblFbPort.AutoSize = false;
            this.lblFbPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFbPort.Location = new System.Drawing.Point(0, 64);
            this.lblFbPort.Name = "lblFbPort";
            this.lblFbPort.Size = new System.Drawing.Size(200, 30);
            this.lblFbPort.TabIndex = 33;
            this.lblFbPort.Text = "Port";
            // 
            // numFbPort
            // 
            this.numFbPort.Minimum = 1;
            this.numFbPort.Maximum = 65535;
            this.numFbPort.Location = new System.Drawing.Point(210, 56);
            this.numFbPort.Name = "numFbPort";
            this.numFbPort.Size = new System.Drawing.Size(160, 48);
            this.numFbPort.TabIndex = 34;
            // 
            // lblFbMount
            // 
            this.lblFbMount.AutoSize = false;
            this.lblFbMount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFbMount.Location = new System.Drawing.Point(400, 64);
            this.lblFbMount.Name = "lblFbMount";
            this.lblFbMount.Size = new System.Drawing.Size(140, 30);
            this.lblFbMount.TabIndex = 35;
            this.lblFbMount.Text = "Mountpoint";
            // 
            // txtFbMount
            // 
            this.txtFbMount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFbMount.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFbMount.ReadOnly = true;
            this.txtFbMount.BackColor = System.Drawing.Color.White;
            this.txtFbMount.Location = new System.Drawing.Point(550, 64);
            this.txtFbMount.Name = "txtFbMount";
            this.txtFbMount.Size = new System.Drawing.Size(400, 31);
            this.txtFbMount.TabIndex = 36;
            this.txtFbMount.Click += new System.EventHandler(this.txtFbMount_Click);
            // 
            // lblFbUser
            // 
            this.lblFbUser.AutoSize = false;
            this.lblFbUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFbUser.Location = new System.Drawing.Point(0, 128);
            this.lblFbUser.Name = "lblFbUser";
            this.lblFbUser.Size = new System.Drawing.Size(200, 30);
            this.lblFbUser.TabIndex = 37;
            this.lblFbUser.Text = "User name";
            // 
            // txtFbUser
            // 
            this.txtFbUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFbUser.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFbUser.ReadOnly = true;
            this.txtFbUser.BackColor = System.Drawing.Color.White;
            this.txtFbUser.Location = new System.Drawing.Point(210, 128);
            this.txtFbUser.Name = "txtFbUser";
            this.txtFbUser.Size = new System.Drawing.Size(330, 31);
            this.txtFbUser.TabIndex = 38;
            this.txtFbUser.Click += new System.EventHandler(this.txtFbUser_Click);
            // 
            // lblFbPassword
            // 
            this.lblFbPassword.AutoSize = false;
            this.lblFbPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFbPassword.Location = new System.Drawing.Point(560, 128);
            this.lblFbPassword.Name = "lblFbPassword";
            this.lblFbPassword.Size = new System.Drawing.Size(120, 30);
            this.lblFbPassword.TabIndex = 39;
            this.lblFbPassword.Text = "Password";
            // 
            // txtFbPassword
            // 
            this.txtFbPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFbPassword.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFbPassword.ReadOnly = true;
            this.txtFbPassword.BackColor = System.Drawing.Color.White;
            this.txtFbPassword.UseSystemPasswordChar = true;
            this.txtFbPassword.Location = new System.Drawing.Point(690, 128);
            this.txtFbPassword.Name = "txtFbPassword";
            this.txtFbPassword.Size = new System.Drawing.Size(260, 31);
            this.txtFbPassword.TabIndex = 40;
            this.txtFbPassword.Click += new System.EventHandler(this.txtFbPassword_Click);
            // 
            // btnFbMounts
            // 
            this.btnFbMounts.Location = new System.Drawing.Point(710, 168);
            this.btnFbMounts.Name = "btnFbMounts";
            this.btnFbMounts.Size = new System.Drawing.Size(240, 40);
            this.btnFbMounts.TabIndex = 41;
            this.btnFbMounts.Text = "Choose from list...";
            this.btnFbMounts.Click += new System.EventHandler(this.btnFbMounts_Click);
            // 
            // pnlFbRadio
            // 
            this.pnlFbRadio.Location = new System.Drawing.Point(0, 196);
            this.pnlFbRadio.Name = "pnlFbRadio";
            this.pnlFbRadio.Size = new System.Drawing.Size(960, 210);
            this.pnlFbRadio.TabIndex = 42;
            // 
            // lblRadioPort
            // 
            this.lblRadioPort.AutoSize = false;
            this.lblRadioPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRadioPort.Location = new System.Drawing.Point(0, 12);
            this.lblRadioPort.Name = "lblRadioPort";
            this.lblRadioPort.Size = new System.Drawing.Size(200, 30);
            this.lblRadioPort.TabIndex = 43;
            this.lblRadioPort.Text = "Radio on port";
            // 
            // cboRadioPort
            // 
            this.cboRadioPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRadioPort.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRadioPort.FormattingEnabled = true;
            this.cboRadioPort.Location = new System.Drawing.Point(210, 8);
            this.cboRadioPort.Name = "cboRadioPort";
            this.cboRadioPort.Size = new System.Drawing.Size(260, 38);
            this.cboRadioPort.TabIndex = 44;
            // 
            // lblRadioBaud
            // 
            this.lblRadioBaud.AutoSize = false;
            this.lblRadioBaud.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblRadioBaud.Location = new System.Drawing.Point(0, 76);
            this.lblRadioBaud.Name = "lblRadioBaud";
            this.lblRadioBaud.Size = new System.Drawing.Size(200, 30);
            this.lblRadioBaud.TabIndex = 45;
            this.lblRadioBaud.Text = "Speed (baud)";
            // 
            // cboRadioBaud
            // 
            this.cboRadioBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRadioBaud.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboRadioBaud.FormattingEnabled = true;
            this.cboRadioBaud.Items.AddRange(new object[] { "4800", "9600", "19200", "38400", "57600", "115200" });
            this.cboRadioBaud.Location = new System.Drawing.Point(210, 72);
            this.cboRadioBaud.Name = "cboRadioBaud";
            this.cboRadioBaud.Size = new System.Drawing.Size(260, 38);
            this.cboRadioBaud.TabIndex = 46;
            // 
            // pnlOutput
            // 
            this.pnlOutput.Location = new System.Drawing.Point(20, 142);
            this.pnlOutput.Name = "pnlOutput";
            this.pnlOutput.Size = new System.Drawing.Size(960, 418);
            this.pnlOutput.TabIndex = 47;
            // 
            // lblPos
            // 
            this.lblPos.AutoSize = false;
            this.lblPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPos.Location = new System.Drawing.Point(0, 14);
            this.lblPos.Name = "lblPos";
            this.lblPos.Size = new System.Drawing.Size(220, 30);
            this.lblPos.TabIndex = 48;
            this.lblPos.Text = "Position sent";
            // 
            // segPosition
            // 
            this.segPosition.ItemsText = "My GPS position|A fixed position";
            this.segPosition.Location = new System.Drawing.Point(230, 4);
            this.segPosition.Name = "segPosition";
            this.segPosition.Size = new System.Drawing.Size(720, 52);
            this.segPosition.TabIndex = 49;
            this.segPosition.SelectedIndexChanged += new System.EventHandler(this.segPosition_SelectedIndexChanged);
            // 
            // lblLat
            // 
            this.lblLat.AutoSize = false;
            this.lblLat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLat.Location = new System.Drawing.Point(0, 72);
            this.lblLat.Name = "lblLat";
            this.lblLat.Size = new System.Drawing.Size(220, 30);
            this.lblLat.TabIndex = 50;
            this.lblLat.Text = "Latitude";
            // 
            // numLat
            // 
            this.numLat.Minimum = -90;
            this.numLat.Maximum = 90;
            this.numLat.DecimalPlaces = 8;
            this.numLat.Location = new System.Drawing.Point(230, 64);
            this.numLat.Name = "numLat";
            this.numLat.Size = new System.Drawing.Size(260, 48);
            this.numLat.TabIndex = 51;
            // 
            // lblLon
            // 
            this.lblLon.AutoSize = false;
            this.lblLon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLon.Location = new System.Drawing.Point(0, 130);
            this.lblLon.Name = "lblLon";
            this.lblLon.Size = new System.Drawing.Size(220, 30);
            this.lblLon.TabIndex = 52;
            this.lblLon.Text = "Longitude";
            // 
            // numLon
            // 
            this.numLon.Minimum = -180;
            this.numLon.Maximum = 180;
            this.numLon.DecimalPlaces = 8;
            this.numLon.Location = new System.Drawing.Point(230, 122);
            this.numLon.Name = "numLon";
            this.numLon.Size = new System.Drawing.Size(260, 48);
            this.numLon.TabIndex = 53;
            // 
            // lblAlt
            // 
            this.lblAlt.AutoSize = false;
            this.lblAlt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAlt.Location = new System.Drawing.Point(0, 188);
            this.lblAlt.Name = "lblAlt";
            this.lblAlt.Size = new System.Drawing.Size(220, 30);
            this.lblAlt.TabIndex = 54;
            this.lblAlt.Text = "Altitude (m)";
            // 
            // numAlt
            // 
            this.numAlt.Minimum = -500;
            this.numAlt.Maximum = 9000;
            this.numAlt.DecimalPlaces = 1;
            this.numAlt.Location = new System.Drawing.Point(230, 180);
            this.numAlt.Name = "numAlt";
            this.numAlt.Size = new System.Drawing.Size(260, 48);
            this.numAlt.TabIndex = 55;
            // 
            // lblInterval
            // 
            this.lblInterval.AutoSize = false;
            this.lblInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblInterval.Location = new System.Drawing.Point(0, 246);
            this.lblInterval.Name = "lblInterval";
            this.lblInterval.Size = new System.Drawing.Size(220, 30);
            this.lblInterval.TabIndex = 56;
            this.lblInterval.Text = "Send every (s)";
            // 
            // numInterval
            // 
            this.numInterval.Minimum = 0;
            this.numInterval.Maximum = 120;
            this.numInterval.Location = new System.Drawing.Point(230, 238);
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(260, 48);
            this.numInterval.TabIndex = 57;
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = false;
            this.lblOutput.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblOutput.Location = new System.Drawing.Point(0, 314);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(220, 30);
            this.lblOutput.TabIndex = 58;
            this.lblOutput.Text = "Corrections go to";
            // 
            // segOutput
            // 
            this.segOutput.ItemsText = "My GPS Receiver (Serial)|My AIO Board (Network)";
            this.segOutput.Location = new System.Drawing.Point(230, 304);
            this.segOutput.Name = "segOutput";
            this.segOutput.Size = new System.Drawing.Size(720, 52);
            this.segOutput.TabIndex = 59;
            this.segOutput.SelectedIndexChanged += new System.EventHandler(this.segOutput_SelectedIndexChanged);
            // 
            // lblNetPort
            // 
            this.lblNetPort.AutoSize = false;
            this.lblNetPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblNetPort.Location = new System.Drawing.Point(0, 372);
            this.lblNetPort.Name = "lblNetPort";
            this.lblNetPort.Size = new System.Drawing.Size(220, 30);
            this.lblNetPort.TabIndex = 60;
            this.lblNetPort.Text = "Network port";
            // 
            // numNetPort
            // 
            this.numNetPort.Minimum = 1;
            this.numNetPort.Maximum = 65535;
            this.numNetPort.Location = new System.Drawing.Point(230, 364);
            this.numNetPort.Name = "numNetPort";
            this.numNetPort.Size = new System.Drawing.Size(260, 48);
            this.numNetPort.TabIndex = 61;
            // 
            // txtLog
            // 
            this.txtLog.Multiline = true;
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.BackColor = System.Drawing.Color.White;
            this.txtLog.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(20, 562);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(600, 128);
            this.txtLog.TabIndex = 62;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(640, 562);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(340, 56);
            this.btnCheck.TabIndex = 63;
            this.btnCheck.Text = "Check connection";
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(640, 628);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 62);
            this.btnCancel.TabIndex = 64;
            this.btnCancel.Text = "Cancel";
            // 
            // btnSave
            // 
            this.btnSave.Tone = AgIO.ButtonTone.Positive;
            this.btnSave.Location = new System.Drawing.Point(820, 628);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 62);
            this.btnSave.TabIndex = 65;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.pnlFallback.Controls.Add(this.tglFallback);
            this.pnlFallback.Controls.Add(this.lblFallbackOn);
            this.pnlFallback.Controls.Add(this.segFbType);
            this.pnlFallback.Controls.Add(this.segAuto);
            this.pnlFallback.Controls.Add(this.lblAfter);
            this.pnlFallback.Controls.Add(this.numAfter);
            this.pnlFallback.Controls.Add(this.lblAfter2);
            this.pnlFallback.Controls.Add(this.pnlFbService);
            this.pnlFallback.Controls.Add(this.pnlFbRadio);
            this.pnlFbRadio.Controls.Add(this.lblRadioPort);
            this.pnlFbRadio.Controls.Add(this.cboRadioPort);
            this.pnlFbRadio.Controls.Add(this.lblRadioBaud);
            this.pnlFbRadio.Controls.Add(this.cboRadioBaud);
            this.pnlFbService.Controls.Add(this.lblFbHost);
            this.pnlFbService.Controls.Add(this.txtFbHost);
            this.pnlFbService.Controls.Add(this.lblFbPort);
            this.pnlFbService.Controls.Add(this.numFbPort);
            this.pnlFbService.Controls.Add(this.lblFbMount);
            this.pnlFbService.Controls.Add(this.txtFbMount);
            this.pnlFbService.Controls.Add(this.lblFbUser);
            this.pnlFbService.Controls.Add(this.txtFbUser);
            this.pnlFbService.Controls.Add(this.lblFbPassword);
            this.pnlFbService.Controls.Add(this.txtFbPassword);
            this.pnlFbService.Controls.Add(this.btnFbMounts);
            this.pnlMain.Controls.Add(this.lblHost);
            this.pnlMain.Controls.Add(this.txtHost);
            this.pnlMain.Controls.Add(this.lblPort);
            this.pnlMain.Controls.Add(this.numPort);
            this.pnlMain.Controls.Add(this.lblMount);
            this.pnlMain.Controls.Add(this.txtMount);
            this.pnlMain.Controls.Add(this.btnMounts);
            this.pnlMain.Controls.Add(this.lblUser);
            this.pnlMain.Controls.Add(this.txtUser);
            this.pnlMain.Controls.Add(this.lblPassword);
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Controls.Add(this.btnShowPassword);
            this.pnlMain.Controls.Add(this.lblVersion);
            this.pnlMain.Controls.Add(this.segVersion);
            this.pnlMain.Controls.Add(this.lblRaw);
            this.pnlMain.Controls.Add(this.tglRaw);
            this.pnlOutput.Controls.Add(this.lblPos);
            this.pnlOutput.Controls.Add(this.segPosition);
            this.pnlOutput.Controls.Add(this.lblLat);
            this.pnlOutput.Controls.Add(this.numLat);
            this.pnlOutput.Controls.Add(this.lblLon);
            this.pnlOutput.Controls.Add(this.numLon);
            this.pnlOutput.Controls.Add(this.lblAlt);
            this.pnlOutput.Controls.Add(this.numAlt);
            this.pnlOutput.Controls.Add(this.lblInterval);
            this.pnlOutput.Controls.Add(this.numInterval);
            this.pnlOutput.Controls.Add(this.lblOutput);
            this.pnlOutput.Controls.Add(this.segOutput);
            this.pnlOutput.Controls.Add(this.lblNetPort);
            this.pnlOutput.Controls.Add(this.numNetPort);
            // 
            // FormNtripSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 706);
            this.Controls.Add(this.tglUse);
            this.Controls.Add(this.lblUse);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.lblProfileName);
            this.Controls.Add(this.segPage);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFallback);
            this.Controls.Add(this.pnlOutput);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Name = "FormNtripSettings";
            this.Text = "NTRIP settings";
            this.pnlFallback.ResumeLayout(false);
            this.pnlFbRadio.ResumeLayout(false);
            this.pnlFbService.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlOutput.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AgIO.ToggleSwitch tglUse;
        private System.Windows.Forms.Label lblUse;
        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.Label lblProfileName;
        private AgIO.SegmentedControl segPage;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblPort;
        private AgIO.NumberField numPort;
        private System.Windows.Forms.Label lblMount;
        private System.Windows.Forms.TextBox txtMount;
        private AgIO.RoundedButton btnMounts;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private AgIO.RoundedButton btnShowPassword;
        private System.Windows.Forms.Label lblVersion;
        private AgIO.SegmentedControl segVersion;
        private System.Windows.Forms.Label lblRaw;
        private AgIO.ToggleSwitch tglRaw;
        private System.Windows.Forms.Panel pnlFallback;
        private AgIO.ToggleSwitch tglFallback;
        private System.Windows.Forms.Label lblFallbackOn;
        private AgIO.SegmentedControl segFbType;
        private AgIO.SegmentedControl segAuto;
        private System.Windows.Forms.Label lblAfter;
        private AgIO.NumberField numAfter;
        private System.Windows.Forms.Label lblAfter2;
        private System.Windows.Forms.Panel pnlFbService;
        private System.Windows.Forms.Label lblFbHost;
        private System.Windows.Forms.TextBox txtFbHost;
        private System.Windows.Forms.Label lblFbPort;
        private AgIO.NumberField numFbPort;
        private System.Windows.Forms.Label lblFbMount;
        private System.Windows.Forms.TextBox txtFbMount;
        private System.Windows.Forms.Label lblFbUser;
        private System.Windows.Forms.TextBox txtFbUser;
        private System.Windows.Forms.Label lblFbPassword;
        private System.Windows.Forms.TextBox txtFbPassword;
        private AgIO.RoundedButton btnFbMounts;
        private System.Windows.Forms.Panel pnlFbRadio;
        private System.Windows.Forms.Label lblRadioPort;
        private System.Windows.Forms.ComboBox cboRadioPort;
        private System.Windows.Forms.Label lblRadioBaud;
        private System.Windows.Forms.ComboBox cboRadioBaud;
        private System.Windows.Forms.Panel pnlOutput;
        private System.Windows.Forms.Label lblPos;
        private AgIO.SegmentedControl segPosition;
        private System.Windows.Forms.Label lblLat;
        private AgIO.NumberField numLat;
        private System.Windows.Forms.Label lblLon;
        private AgIO.NumberField numLon;
        private System.Windows.Forms.Label lblAlt;
        private AgIO.NumberField numAlt;
        private System.Windows.Forms.Label lblInterval;
        private AgIO.NumberField numInterval;
        private System.Windows.Forms.Label lblOutput;
        private AgIO.SegmentedControl segOutput;
        private System.Windows.Forms.Label lblNetPort;
        private AgIO.NumberField numNetPort;
        private System.Windows.Forms.TextBox txtLog;
        private AgIO.RoundedButton btnCheck;
        private AgIO.RoundedButton btnCancel;
        private AgIO.RoundedButton btnSave;
    }
}
