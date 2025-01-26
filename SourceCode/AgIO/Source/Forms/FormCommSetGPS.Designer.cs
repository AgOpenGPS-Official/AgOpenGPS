namespace AgIO
{
    partial class FormCommSetGPS
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCloseRTCM = new System.Windows.Forms.Button();
            this.btnOpenRTCM = new System.Windows.Forms.Button();
            this.labelDifferentRtcmPort = new System.Windows.Forms.Label();
            this.cboxRtcmPort = new System.Windows.Forms.ComboBox();
            this.cboxRtcmBaud = new System.Windows.Forms.ComboBox();
            this.labelRtcmPort = new System.Windows.Forms.Label();
            this.labelRtcmBaud = new System.Windows.Forms.Label();
            this.cboxPort = new System.Windows.Forms.ComboBox();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.lblCurrentBaud = new System.Windows.Forms.Label();
            this.btnCloseSerial = new System.Windows.Forms.Button();
            this.textBoxRcv = new System.Windows.Forms.TextBox();
            this.btnOpenSerial = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cboxPort2 = new System.Windows.Forms.ComboBox();
            this.cboxBaud2 = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort2 = new System.Windows.Forms.Label();
            this.lblCurrentBaud2 = new System.Windows.Forms.Label();
            this.btnCloseSerial2 = new System.Windows.Forms.Button();
            this.textBoxRcv2 = new System.Windows.Forms.TextBox();
            this.btnOpenSerial2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboxSteerModulePort = new System.Windows.Forms.ComboBox();
            this.lblCurrentSteerModulePort = new System.Windows.Forms.Label();
            this.btnOpenSerialSteerModule = new System.Windows.Forms.Button();
            this.btnCloseSerialSteerModule = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cboxMachineModulePort = new System.Windows.Forms.ComboBox();
            this.lblCurrentMachineModulePort = new System.Windows.Forms.Label();
            this.btnOpenSerialMachineModule = new System.Windows.Forms.Button();
            this.btnCloseSerialMachineModule = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblCurrentModule3Port = new System.Windows.Forms.Label();
            this.cboxModule3Port = new System.Windows.Forms.ComboBox();
            this.btnOpenSerialModule3 = new System.Windows.Forms.Button();
            this.btnCloseSerialModule3 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblCurrentIMU = new System.Windows.Forms.Label();
            this.cboxIMU = new System.Windows.Forms.ComboBox();
            this.btnCloseIMU = new System.Windows.Forms.Button();
            this.btnOpenIMU = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblIMU = new System.Windows.Forms.Label();
            this.lblSteer = new System.Windows.Forms.Label();
            this.lblMachine = new System.Windows.Forms.Label();
            this.lblGPS = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.lblFromMU = new System.Windows.Forms.Label();
            this.lblFromSteerModule = new System.Windows.Forms.Label();
            this.lblFromMachineModule = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCloseRTCM);
            this.groupBox1.Controls.Add(this.btnOpenRTCM);
            this.groupBox1.Controls.Add(this.labelDifferentRtcmPort);
            this.groupBox1.Controls.Add(this.cboxRtcmPort);
            this.groupBox1.Controls.Add(this.cboxRtcmBaud);
            this.groupBox1.Controls.Add(this.labelRtcmPort);
            this.groupBox1.Controls.Add(this.labelRtcmBaud);
            this.groupBox1.Controls.Add(this.cboxPort);
            this.groupBox1.Controls.Add(this.cboxBaud);
            this.groupBox1.Controls.Add(this.lblCurrentPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pictureBox4);
            this.groupBox1.Controls.Add(this.lblCurrentBaud);
            this.groupBox1.Controls.Add(this.btnCloseSerial);
            this.groupBox1.Controls.Add(this.textBoxRcv);
            this.groupBox1.Controls.Add(this.btnOpenSerial);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(43, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(759, 247);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS";
            this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(154, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 29);
            this.label3.TabIndex = 97;
            this.label3.Text = "RTCM";
            // 
            // btnCloseRTCM
            // 
            this.btnCloseRTCM.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseRTCM.FlatAppearance.BorderSize = 0;
            this.btnCloseRTCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseRTCM.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseRTCM.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseRTCM.Location = new System.Drawing.Point(673, 112);
            this.btnCloseRTCM.Name = "btnCloseRTCM";
            this.btnCloseRTCM.Size = new System.Drawing.Size(56, 58);
            this.btnCloseRTCM.TabIndex = 95;
            this.btnCloseRTCM.UseVisualStyleBackColor = false;
            this.btnCloseRTCM.Click += new System.EventHandler(this.btnCloseRTCM_Click);
            // 
            // btnOpenRTCM
            // 
            this.btnOpenRTCM.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenRTCM.FlatAppearance.BorderSize = 0;
            this.btnOpenRTCM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenRTCM.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenRTCM.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenRTCM.Location = new System.Drawing.Point(572, 112);
            this.btnOpenRTCM.Name = "btnOpenRTCM";
            this.btnOpenRTCM.Size = new System.Drawing.Size(58, 58);
            this.btnOpenRTCM.TabIndex = 96;
            this.btnOpenRTCM.UseVisualStyleBackColor = false;
            this.btnOpenRTCM.Click += new System.EventHandler(this.btnOpenRTCM_Click);
            // 
            // labelDifferentRtcmPort
            // 
            this.labelDifferentRtcmPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDifferentRtcmPort.Location = new System.Drawing.Point(22, 127);
            this.labelDifferentRtcmPort.Name = "labelDifferentRtcmPort";
            this.labelDifferentRtcmPort.Size = new System.Drawing.Size(128, 43);
            this.labelDifferentRtcmPort.TabIndex = 94;
            this.labelDifferentRtcmPort.Text = "Different Port Then GPS ?";
            this.labelDifferentRtcmPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxRtcmPort
            // 
            this.cboxRtcmPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxRtcmPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRtcmPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRtcmPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxRtcmPort.FormattingEnabled = true;
            this.cboxRtcmPort.Location = new System.Drawing.Point(244, 127);
            this.cboxRtcmPort.Name = "cboxRtcmPort";
            this.cboxRtcmPort.Size = new System.Drawing.Size(124, 37);
            this.cboxRtcmPort.TabIndex = 54;
            this.cboxRtcmPort.SelectedIndexChanged += new System.EventHandler(this.cboxRtcmPort_SelectedIndexChanged);
            // 
            // cboxRtcmBaud
            // 
            this.cboxRtcmBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxRtcmBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRtcmBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRtcmBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxRtcmBaud.FormattingEnabled = true;
            this.cboxRtcmBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "128000",
            "256000"});
            this.cboxRtcmBaud.Location = new System.Drawing.Point(395, 127);
            this.cboxRtcmBaud.Name = "cboxRtcmBaud";
            this.cboxRtcmBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxRtcmBaud.TabIndex = 53;
            this.cboxRtcmBaud.SelectedIndexChanged += new System.EventHandler(this.cboxRtcmBaud_SelectedIndexChanged);
            // 
            // labelRtcmPort
            // 
            this.labelRtcmPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRtcmPort.Location = new System.Drawing.Point(244, 106);
            this.labelRtcmPort.Name = "labelRtcmPort";
            this.labelRtcmPort.Size = new System.Drawing.Size(121, 18);
            this.labelRtcmPort.TabIndex = 52;
            this.labelRtcmPort.Text = "RTCM Port";
            this.labelRtcmPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRtcmBaud
            // 
            this.labelRtcmBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRtcmBaud.Location = new System.Drawing.Point(395, 106);
            this.labelRtcmBaud.Name = "labelRtcmBaud";
            this.labelRtcmBaud.Size = new System.Drawing.Size(124, 18);
            this.labelRtcmBaud.TabIndex = 51;
            this.labelRtcmBaud.Text = "RTCM Baud";
            this.labelRtcmBaud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxPort
            // 
            this.cboxPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxPort.FormattingEnabled = true;
            this.cboxPort.Location = new System.Drawing.Point(244, 37);
            this.cboxPort.Name = "cboxPort";
            this.cboxPort.Size = new System.Drawing.Size(124, 37);
            this.cboxPort.TabIndex = 50;
            this.cboxPort.SelectedIndexChanged += new System.EventHandler(this.cboxPort_SelectedIndexChanged_1);
            // 
            // cboxBaud
            // 
            this.cboxBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud.Location = new System.Drawing.Point(395, 37);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud.TabIndex = 49;
            this.cboxBaud.SelectedIndexChanged += new System.EventHandler(this.cboxBaud_SelectedIndexChanged_1);
            // 
            // lblCurrentPort
            // 
            this.lblCurrentPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort.Location = new System.Drawing.Point(244, 16);
            this.lblCurrentPort.Name = "lblCurrentPort";
            this.lblCurrentPort.Size = new System.Drawing.Size(121, 18);
            this.lblCurrentPort.TabIndex = 47;
            this.lblCurrentPort.Text = "GPS Port";
            this.lblCurrentPort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(174, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 29);
            this.label2.TabIndex = 74;
            this.label2.Text = "GPS";
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = global::AgIO.Properties.Resources.satellite;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox4.Location = new System.Drawing.Point(12, 23);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(84, 92);
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
            // 
            // lblCurrentBaud
            // 
            this.lblCurrentBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud.Location = new System.Drawing.Point(395, 16);
            this.lblCurrentBaud.Name = "lblCurrentBaud";
            this.lblCurrentBaud.Size = new System.Drawing.Size(124, 18);
            this.lblCurrentBaud.TabIndex = 46;
            this.lblCurrentBaud.Text = "GPS Baud";
            this.lblCurrentBaud.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCloseSerial
            // 
            this.btnCloseSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerial.FlatAppearance.BorderSize = 0;
            this.btnCloseSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerial.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseSerial.Location = new System.Drawing.Point(673, 23);
            this.btnCloseSerial.Name = "btnCloseSerial";
            this.btnCloseSerial.Size = new System.Drawing.Size(56, 58);
            this.btnCloseSerial.TabIndex = 44;
            this.btnCloseSerial.UseVisualStyleBackColor = false;
            this.btnCloseSerial.Click += new System.EventHandler(this.btnCloseSerial_Click);
            // 
            // textBoxRcv
            // 
            this.textBoxRcv.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv.BackColor = System.Drawing.Color.Gainsboro;
            this.textBoxRcv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxRcv.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxRcv.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBoxRcv.Location = new System.Drawing.Point(122, 178);
            this.textBoxRcv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv.Multiline = true;
            this.textBoxRcv.Name = "textBoxRcv";
            this.textBoxRcv.ReadOnly = true;
            this.textBoxRcv.Size = new System.Drawing.Size(607, 53);
            this.textBoxRcv.TabIndex = 40;
            this.textBoxRcv.Text = "GPS";
            // 
            // btnOpenSerial
            // 
            this.btnOpenSerial.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerial.FlatAppearance.BorderSize = 0;
            this.btnOpenSerial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerial.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenSerial.Location = new System.Drawing.Point(572, 23);
            this.btnOpenSerial.Name = "btnOpenSerial";
            this.btnOpenSerial.Size = new System.Drawing.Size(58, 58);
            this.btnOpenSerial.TabIndex = 45;
            this.btnOpenSerial.UseVisualStyleBackColor = false;
            this.btnOpenSerial.Click += new System.EventHandler(this.btnOpenSerial_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cboxPort2);
            this.groupBox2.Controls.Add(this.cboxBaud2);
            this.groupBox2.Controls.Add(this.lblCurrentPort2);
            this.groupBox2.Controls.Add(this.lblCurrentBaud2);
            this.groupBox2.Controls.Add(this.btnCloseSerial2);
            this.groupBox2.Controls.Add(this.textBoxRcv2);
            this.groupBox2.Controls.Add(this.btnOpenSerial2);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(1118, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 120);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "GPS Port 2";
            // 
            // cboxPort2
            // 
            this.cboxPort2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxPort2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxPort2.FormattingEnabled = true;
            this.cboxPort2.Location = new System.Drawing.Point(10, 37);
            this.cboxPort2.Name = "cboxPort2";
            this.cboxPort2.Size = new System.Drawing.Size(169, 37);
            this.cboxPort2.TabIndex = 50;
            this.cboxPort2.SelectedIndexChanged += new System.EventHandler(this.cboxPort2_SelectedIndexChanged);
            // 
            // cboxBaud2
            // 
            this.cboxBaud2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud2.FormattingEnabled = true;
            this.cboxBaud2.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud2.Location = new System.Drawing.Point(198, 37);
            this.cboxBaud2.Name = "cboxBaud2";
            this.cboxBaud2.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud2.TabIndex = 49;
            this.cboxBaud2.SelectedIndexChanged += new System.EventHandler(this.cboxBaud2_SelectedIndexChanged);
            // 
            // lblCurrentPort2
            // 
            this.lblCurrentPort2.AutoSize = true;
            this.lblCurrentPort2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort2.Location = new System.Drawing.Point(139, 16);
            this.lblCurrentPort2.Name = "lblCurrentPort2";
            this.lblCurrentPort2.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentPort2.TabIndex = 47;
            this.lblCurrentPort2.Text = "Port";
            // 
            // lblCurrentBaud2
            // 
            this.lblCurrentBaud2.AutoSize = true;
            this.lblCurrentBaud2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud2.Location = new System.Drawing.Point(280, 16);
            this.lblCurrentBaud2.Name = "lblCurrentBaud2";
            this.lblCurrentBaud2.Size = new System.Drawing.Size(45, 18);
            this.lblCurrentBaud2.TabIndex = 46;
            this.lblCurrentBaud2.Text = "Baud";
            // 
            // btnCloseSerial2
            // 
            this.btnCloseSerial2.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerial2.FlatAppearance.BorderSize = 0;
            this.btnCloseSerial2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerial2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerial2.Location = new System.Drawing.Point(313, 37);
            this.btnCloseSerial2.Name = "btnCloseSerial2";
            this.btnCloseSerial2.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerial2.TabIndex = 44;
            this.btnCloseSerial2.UseVisualStyleBackColor = false;
            this.btnCloseSerial2.Click += new System.EventHandler(this.btnCloseSerial2_Click);
            // 
            // textBoxRcv2
            // 
            this.textBoxRcv2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv2.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxRcv2.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxRcv2.Location = new System.Drawing.Point(-355, 81);
            this.textBoxRcv2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv2.Multiline = true;
            this.textBoxRcv2.Name = "textBoxRcv2";
            this.textBoxRcv2.ReadOnly = true;
            this.textBoxRcv2.Size = new System.Drawing.Size(176, 31);
            this.textBoxRcv2.TabIndex = 40;
            // 
            // btnOpenSerial2
            // 
            this.btnOpenSerial2.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerial2.FlatAppearance.BorderSize = 0;
            this.btnOpenSerial2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerial2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerial2.Location = new System.Drawing.Point(224, 54);
            this.btnOpenSerial2.Name = "btnOpenSerial2";
            this.btnOpenSerial2.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerial2.TabIndex = 45;
            this.btnOpenSerial2.UseVisualStyleBackColor = false;
            this.btnOpenSerial2.Click += new System.EventHandler(this.btnOpenSerial2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.cboxSteerModulePort);
            this.groupBox3.Controls.Add(this.lblCurrentSteerModulePort);
            this.groupBox3.Controls.Add(this.btnOpenSerialSteerModule);
            this.groupBox3.Controls.Add(this.btnCloseSerialSteerModule);
            this.groupBox3.Controls.Add(this.pictureBox2);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(43, 410);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(526, 130);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Steer";
            this.groupBox3.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // cboxSteerModulePort
            // 
            this.cboxSteerModulePort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSteerModulePort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxSteerModulePort.FormattingEnabled = true;
            this.cboxSteerModulePort.Location = new System.Drawing.Point(173, 55);
            this.cboxSteerModulePort.Name = "cboxSteerModulePort";
            this.cboxSteerModulePort.Size = new System.Drawing.Size(124, 37);
            this.cboxSteerModulePort.TabIndex = 64;
            this.cboxSteerModulePort.SelectedIndexChanged += new System.EventHandler(this.cboxSteerModule_SelectedIndexChanged);
            // 
            // lblCurrentSteerModulePort
            // 
            this.lblCurrentSteerModulePort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentSteerModulePort.Location = new System.Drawing.Point(179, 34);
            this.lblCurrentSteerModulePort.Name = "lblCurrentSteerModulePort";
            this.lblCurrentSteerModulePort.Size = new System.Drawing.Size(112, 18);
            this.lblCurrentSteerModulePort.TabIndex = 69;
            this.lblCurrentSteerModulePort.Text = "Port";
            this.lblCurrentSteerModulePort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenSerialSteerModule
            // 
            this.btnOpenSerialSteerModule.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerialSteerModule.FlatAppearance.BorderSize = 0;
            this.btnOpenSerialSteerModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerialSteerModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialSteerModule.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenSerialSteerModule.Location = new System.Drawing.Point(324, 45);
            this.btnOpenSerialSteerModule.Name = "btnOpenSerialSteerModule";
            this.btnOpenSerialSteerModule.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerialSteerModule.TabIndex = 53;
            this.btnOpenSerialSteerModule.UseVisualStyleBackColor = false;
            this.btnOpenSerialSteerModule.Click += new System.EventHandler(this.btnOpenSerialSteerModule_Click);
            // 
            // btnCloseSerialSteerModule
            // 
            this.btnCloseSerialSteerModule.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerialSteerModule.FlatAppearance.BorderSize = 0;
            this.btnCloseSerialSteerModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerialSteerModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialSteerModule.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseSerialSteerModule.Location = new System.Drawing.Point(425, 45);
            this.btnCloseSerialSteerModule.Name = "btnCloseSerialSteerModule";
            this.btnCloseSerialSteerModule.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerialSteerModule.TabIndex = 52;
            this.btnCloseSerialSteerModule.UseVisualStyleBackColor = false;
            this.btnCloseSerialSteerModule.Click += new System.EventHandler(this.btnCloseSerialSteerModule_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AgIO.Properties.Resources.Com_AutosteerModule;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(4, 29);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(108, 88);
            this.pictureBox2.TabIndex = 70;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.Color.Transparent;
            this.groupBox4.Controls.Add(this.cboxMachineModulePort);
            this.groupBox4.Controls.Add(this.lblCurrentMachineModulePort);
            this.groupBox4.Controls.Add(this.btnOpenSerialMachineModule);
            this.groupBox4.Controls.Add(this.btnCloseSerialMachineModule);
            this.groupBox4.Controls.Add(this.pictureBox3);
            this.groupBox4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.Location = new System.Drawing.Point(43, 558);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(526, 130);
            this.groupBox4.TabIndex = 67;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Machine";
            this.groupBox4.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // cboxMachineModulePort
            // 
            this.cboxMachineModulePort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMachineModulePort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxMachineModulePort.FormattingEnabled = true;
            this.cboxMachineModulePort.Location = new System.Drawing.Point(173, 55);
            this.cboxMachineModulePort.Name = "cboxMachineModulePort";
            this.cboxMachineModulePort.Size = new System.Drawing.Size(124, 37);
            this.cboxMachineModulePort.TabIndex = 64;
            this.cboxMachineModulePort.SelectedIndexChanged += new System.EventHandler(this.cboxMachineModulePort_SelectedIndexChanged);
            // 
            // lblCurrentMachineModulePort
            // 
            this.lblCurrentMachineModulePort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentMachineModulePort.Location = new System.Drawing.Point(179, 34);
            this.lblCurrentMachineModulePort.Name = "lblCurrentMachineModulePort";
            this.lblCurrentMachineModulePort.Size = new System.Drawing.Size(112, 18);
            this.lblCurrentMachineModulePort.TabIndex = 70;
            this.lblCurrentMachineModulePort.Text = "Port";
            this.lblCurrentMachineModulePort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOpenSerialMachineModule
            // 
            this.btnOpenSerialMachineModule.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerialMachineModule.FlatAppearance.BorderSize = 0;
            this.btnOpenSerialMachineModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerialMachineModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialMachineModule.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenSerialMachineModule.Location = new System.Drawing.Point(324, 45);
            this.btnOpenSerialMachineModule.Name = "btnOpenSerialMachineModule";
            this.btnOpenSerialMachineModule.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerialMachineModule.TabIndex = 53;
            this.btnOpenSerialMachineModule.UseVisualStyleBackColor = false;
            this.btnOpenSerialMachineModule.Click += new System.EventHandler(this.btnOpenSerialMachineModule_Click);
            // 
            // btnCloseSerialMachineModule
            // 
            this.btnCloseSerialMachineModule.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerialMachineModule.FlatAppearance.BorderSize = 0;
            this.btnCloseSerialMachineModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerialMachineModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialMachineModule.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseSerialMachineModule.Location = new System.Drawing.Point(425, 45);
            this.btnCloseSerialMachineModule.Name = "btnCloseSerialMachineModule";
            this.btnCloseSerialMachineModule.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerialMachineModule.TabIndex = 52;
            this.btnCloseSerialMachineModule.UseVisualStyleBackColor = false;
            this.btnCloseSerialMachineModule.Click += new System.EventHandler(this.btnCloseSerialMachineModule_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::AgIO.Properties.Resources.Com_Module2;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(6, 29);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(108, 88);
            this.pictureBox3.TabIndex = 71;
            this.pictureBox3.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Controls.Add(this.lblCurrentModule3Port);
            this.groupBox5.Controls.Add(this.cboxModule3Port);
            this.groupBox5.Controls.Add(this.btnOpenSerialModule3);
            this.groupBox5.Controls.Add(this.btnCloseSerialModule3);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox5.Location = new System.Drawing.Point(1164, 222);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(368, 90);
            this.groupBox5.TabIndex = 68;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Module 3";
            // 
            // lblCurrentModule3Port
            // 
            this.lblCurrentModule3Port.AutoSize = true;
            this.lblCurrentModule3Port.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentModule3Port.Location = new System.Drawing.Point(115, 19);
            this.lblCurrentModule3Port.Name = "lblCurrentModule3Port";
            this.lblCurrentModule3Port.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentModule3Port.TabIndex = 71;
            this.lblCurrentModule3Port.Text = "Port";
            // 
            // cboxModule3Port
            // 
            this.cboxModule3Port.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxModule3Port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxModule3Port.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxModule3Port.FormattingEnabled = true;
            this.cboxModule3Port.Location = new System.Drawing.Point(10, 42);
            this.cboxModule3Port.Name = "cboxModule3Port";
            this.cboxModule3Port.Size = new System.Drawing.Size(124, 37);
            this.cboxModule3Port.TabIndex = 64;
            this.cboxModule3Port.SelectedIndexChanged += new System.EventHandler(this.cboxModule3Port_SelectedIndexChanged);
            // 
            // btnOpenSerialModule3
            // 
            this.btnOpenSerialModule3.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenSerialModule3.FlatAppearance.BorderSize = 0;
            this.btnOpenSerialModule3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenSerialModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialModule3.Location = new System.Drawing.Point(161, 21);
            this.btnOpenSerialModule3.Name = "btnOpenSerialModule3";
            this.btnOpenSerialModule3.Size = new System.Drawing.Size(101, 58);
            this.btnOpenSerialModule3.TabIndex = 53;
            this.btnOpenSerialModule3.UseVisualStyleBackColor = false;
            this.btnOpenSerialModule3.Click += new System.EventHandler(this.btnOpenSerialModule3_Click);
            // 
            // btnCloseSerialModule3
            // 
            this.btnCloseSerialModule3.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseSerialModule3.FlatAppearance.BorderSize = 0;
            this.btnCloseSerialModule3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseSerialModule3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialModule3.Location = new System.Drawing.Point(268, 19);
            this.btnCloseSerialModule3.Name = "btnCloseSerialModule3";
            this.btnCloseSerialModule3.Size = new System.Drawing.Size(101, 58);
            this.btnCloseSerialModule3.TabIndex = 52;
            this.btnCloseSerialModule3.UseVisualStyleBackColor = false;
            this.btnCloseSerialModule3.Click += new System.EventHandler(this.btnCloseSerialModule3_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.Transparent;
            this.groupBox6.Controls.Add(this.lblCurrentIMU);
            this.groupBox6.Controls.Add(this.cboxIMU);
            this.groupBox6.Controls.Add(this.btnCloseIMU);
            this.groupBox6.Controls.Add(this.btnOpenIMU);
            this.groupBox6.Controls.Add(this.pictureBox1);
            this.groupBox6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox6.Location = new System.Drawing.Point(43, 261);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(526, 130);
            this.groupBox6.TabIndex = 68;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "IMU";
            this.groupBox6.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
            // 
            // lblCurrentIMU
            // 
            this.lblCurrentIMU.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentIMU.Location = new System.Drawing.Point(179, 36);
            this.lblCurrentIMU.Name = "lblCurrentIMU";
            this.lblCurrentIMU.Size = new System.Drawing.Size(112, 18);
            this.lblCurrentIMU.TabIndex = 51;
            this.lblCurrentIMU.Text = "Port";
            this.lblCurrentIMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxIMU
            // 
            this.cboxIMU.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxIMU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxIMU.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxIMU.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxIMU.FormattingEnabled = true;
            this.cboxIMU.Location = new System.Drawing.Point(173, 57);
            this.cboxIMU.Name = "cboxIMU";
            this.cboxIMU.Size = new System.Drawing.Size(124, 37);
            this.cboxIMU.TabIndex = 50;
            this.cboxIMU.SelectedIndexChanged += new System.EventHandler(this.cboxIMU_SelectedIndexChanged);
            // 
            // btnCloseIMU
            // 
            this.btnCloseIMU.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseIMU.FlatAppearance.BorderSize = 0;
            this.btnCloseIMU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseIMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseIMU.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseIMU.Location = new System.Drawing.Point(425, 44);
            this.btnCloseIMU.Name = "btnCloseIMU";
            this.btnCloseIMU.Size = new System.Drawing.Size(101, 58);
            this.btnCloseIMU.TabIndex = 44;
            this.btnCloseIMU.UseVisualStyleBackColor = false;
            this.btnCloseIMU.Click += new System.EventHandler(this.btnCloseIMU_Click);
            // 
            // btnOpenIMU
            // 
            this.btnOpenIMU.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenIMU.FlatAppearance.BorderSize = 0;
            this.btnOpenIMU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenIMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenIMU.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenIMU.Location = new System.Drawing.Point(324, 44);
            this.btnOpenIMU.Name = "btnOpenIMU";
            this.btnOpenIMU.Size = new System.Drawing.Size(101, 58);
            this.btnOpenIMU.TabIndex = 45;
            this.btnOpenIMU.UseVisualStyleBackColor = false;
            this.btnOpenIMU.Click += new System.EventHandler(this.btnOpenIMU_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AgIO.Properties.Resources.Com_IMUModule;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(4, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(108, 88);
            this.pictureBox1.TabIndex = 69;
            this.pictureBox1.TabStop = false;
            // 
            // btnRescan
            // 
            this.btnRescan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRescan.BackColor = System.Drawing.Color.Transparent;
            this.btnRescan.FlatAppearance.BorderSize = 0;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRescan.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRescan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRescan.Image = global::AgIO.Properties.Resources.ScanPorts;
            this.btnRescan.Location = new System.Drawing.Point(665, 620);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(89, 63);
            this.btnRescan.TabIndex = 58;
            this.btnRescan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.btnRescan_Click);
            // 
            // btnSerialOK
            // 
            this.btnSerialOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialOK.BackColor = System.Drawing.Color.Transparent;
            this.btnSerialOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialOK.FlatAppearance.BorderSize = 0;
            this.btnSerialOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialOK.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSerialOK.Image = global::AgIO.Properties.Resources.OK64;
            this.btnSerialOK.Location = new System.Drawing.Point(826, 619);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(91, 63);
            this.btnSerialOK.TabIndex = 59;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = false;
            this.btnSerialOK.Click += new System.EventHandler(this.btnSerialOK_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Location = new System.Drawing.Point(671, 327);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 73;
            this.label1.Text = "GPS:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Location = new System.Drawing.Point(630, 456);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 76;
            this.label4.Text = "Machine:";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label5.Location = new System.Drawing.Point(659, 414);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 23);
            this.label5.TabIndex = 77;
            this.label5.Text = "Steer:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Location = new System.Drawing.Point(669, 371);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 23);
            this.label6.TabIndex = 78;
            this.label6.Text = "IMU:";
            // 
            // lblIMU
            // 
            this.lblIMU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblIMU.AutoSize = true;
            this.lblIMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIMU.Location = new System.Drawing.Point(725, 371);
            this.lblIMU.Name = "lblIMU";
            this.lblIMU.Size = new System.Drawing.Size(50, 23);
            this.lblIMU.TabIndex = 83;
            this.lblIMU.Text = "IMU";
            // 
            // lblSteer
            // 
            this.lblSteer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSteer.AutoSize = true;
            this.lblSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteer.Location = new System.Drawing.Point(725, 414);
            this.lblSteer.Name = "lblSteer";
            this.lblSteer.Size = new System.Drawing.Size(60, 23);
            this.lblSteer.TabIndex = 82;
            this.lblSteer.Text = "Steer";
            // 
            // lblMachine
            // 
            this.lblMachine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMachine.AutoSize = true;
            this.lblMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMachine.Location = new System.Drawing.Point(725, 456);
            this.lblMachine.Name = "lblMachine";
            this.lblMachine.Size = new System.Drawing.Size(60, 23);
            this.lblMachine.TabIndex = 81;
            this.lblMachine.Text = "Mach";
            // 
            // lblGPS
            // 
            this.lblGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGPS.AutoSize = true;
            this.lblGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGPS.Location = new System.Drawing.Point(725, 327);
            this.lblGPS.Name = "lblGPS";
            this.lblGPS.Size = new System.Drawing.Size(48, 23);
            this.lblGPS.TabIndex = 79;
            this.lblGPS.Text = "GPS";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromGPS.BackColor = System.Drawing.Color.Transparent;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.Color.Black;
            this.lblFromGPS.Location = new System.Drawing.Point(804, 325);
            this.lblFromGPS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(64, 27);
            this.lblFromGPS.TabIndex = 172;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMU
            // 
            this.lblFromMU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromMU.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMU.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMU.ForeColor = System.Drawing.Color.Black;
            this.lblFromMU.Location = new System.Drawing.Point(804, 368);
            this.lblFromMU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMU.Name = "lblFromMU";
            this.lblFromMU.Size = new System.Drawing.Size(64, 27);
            this.lblFromMU.TabIndex = 175;
            this.lblFromMU.Text = "---";
            this.lblFromMU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromSteerModule
            // 
            this.lblFromSteerModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromSteerModule.BackColor = System.Drawing.Color.Transparent;
            this.lblFromSteerModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromSteerModule.ForeColor = System.Drawing.Color.Black;
            this.lblFromSteerModule.Location = new System.Drawing.Point(804, 412);
            this.lblFromSteerModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromSteerModule.Name = "lblFromSteerModule";
            this.lblFromSteerModule.Size = new System.Drawing.Size(64, 27);
            this.lblFromSteerModule.TabIndex = 173;
            this.lblFromSteerModule.Text = "---";
            this.lblFromSteerModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFromMachineModule
            // 
            this.lblFromMachineModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFromMachineModule.BackColor = System.Drawing.Color.Transparent;
            this.lblFromMachineModule.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMachineModule.ForeColor = System.Drawing.Color.Black;
            this.lblFromMachineModule.Location = new System.Drawing.Point(804, 454);
            this.lblFromMachineModule.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFromMachineModule.Name = "lblFromMachineModule";
            this.lblFromMachineModule.Size = new System.Drawing.Size(64, 27);
            this.lblFromMachineModule.TabIndex = 174;
            this.lblFromMachineModule.Text = "---";
            this.lblFromMachineModule.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormCommSetGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(929, 696);
            this.ControlBox = false;
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblFromMU);
            this.Controls.Add(this.lblFromSteerModule);
            this.Controls.Add(this.lblFromMachineModule);
            this.Controls.Add(this.lblIMU);
            this.Controls.Add(this.lblSteer);
            this.Controls.Add(this.lblMachine);
            this.Controls.Add(this.lblGPS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSerialOK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormCommSetGPS";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect GPS";
            this.Load += new System.EventHandler(this.FormCommSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrentPort;
        private System.Windows.Forms.Label lblCurrentBaud;
        private System.Windows.Forms.Button btnCloseSerial;
        private System.Windows.Forms.TextBox textBoxRcv;
        private System.Windows.Forms.Button btnOpenSerial;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.ComboBox cboxPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboxPort2;
        private System.Windows.Forms.ComboBox cboxBaud2;
        private System.Windows.Forms.Label lblCurrentPort2;
        private System.Windows.Forms.Label lblCurrentBaud2;
        private System.Windows.Forms.Button btnCloseSerial2;
        private System.Windows.Forms.TextBox textBoxRcv2;
        private System.Windows.Forms.Button btnOpenSerial2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboxSteerModulePort;
        private System.Windows.Forms.Button btnOpenSerialSteerModule;
        private System.Windows.Forms.Button btnCloseSerialSteerModule;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox cboxMachineModulePort;
        private System.Windows.Forms.Button btnOpenSerialMachineModule;
        private System.Windows.Forms.Button btnCloseSerialMachineModule;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ComboBox cboxModule3Port;
        private System.Windows.Forms.Button btnOpenSerialModule3;
        private System.Windows.Forms.Button btnCloseSerialModule3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ComboBox cboxIMU;
        private System.Windows.Forms.Button btnCloseIMU;
        private System.Windows.Forms.Button btnOpenIMU;
        private System.Windows.Forms.Label lblCurrentSteerModulePort;
        private System.Windows.Forms.Label lblCurrentMachineModulePort;
        private System.Windows.Forms.Label lblCurrentModule3Port;
        private System.Windows.Forms.Label lblCurrentIMU;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblIMU;
        private System.Windows.Forms.Label lblSteer;
        private System.Windows.Forms.Label lblMachine;
        private System.Windows.Forms.Label lblGPS;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.Label lblFromMU;
        private System.Windows.Forms.Label lblFromSteerModule;
        private System.Windows.Forms.Label lblFromMachineModule;
        private System.Windows.Forms.ComboBox cboxRtcmPort;
        private System.Windows.Forms.ComboBox cboxRtcmBaud;
        private System.Windows.Forms.Label labelRtcmPort;
        private System.Windows.Forms.Label labelRtcmBaud;
        private System.Windows.Forms.Label labelDifferentRtcmPort;
        private System.Windows.Forms.Button btnCloseRTCM;
        private System.Windows.Forms.Button btnOpenRTCM;
        private System.Windows.Forms.Label label3;
    }
}