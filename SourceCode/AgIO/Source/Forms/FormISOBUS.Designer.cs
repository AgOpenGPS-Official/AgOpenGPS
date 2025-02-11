namespace AgIO
{
    partial class FormISOBUS
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
            this.btnIsobusOK = new System.Windows.Forms.Button();
            this.btnCloseIsobus = new System.Windows.Forms.Button();
            this.btnOpenIsobus = new System.Windows.Forms.Button();
            this.cboxRadioAdapter = new System.Windows.Forms.ComboBox();
            this.lblCurrentAdapter = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutCANAdapter = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutChannel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblChannel = new System.Windows.Forms.Label();
            this.cboxRadioChannel = new System.Windows.Forms.ComboBox();
            this.flowLayoutDownloadIsobus = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDownloadIsobus = new System.Windows.Forms.Label();
            this.linkDownloadIsobus = new System.Windows.Forms.LinkLabel();
            this.textBoxRcv = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutCANAdapter.SuspendLayout();
            this.flowLayoutChannel.SuspendLayout();
            this.flowLayoutDownloadIsobus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnIsobusOK
            // 
            this.btnIsobusOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIsobusOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnIsobusOK.FlatAppearance.BorderSize = 0;
            this.btnIsobusOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIsobusOK.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIsobusOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnIsobusOK.Image = global::AgIO.Properties.Resources.OK64;
            this.btnIsobusOK.Location = new System.Drawing.Point(544, 450);
            this.btnIsobusOK.Name = "btnIsobusOK";
            this.btnIsobusOK.Size = new System.Drawing.Size(88, 64);
            this.btnIsobusOK.TabIndex = 71;
            this.btnIsobusOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnIsobusOK.UseVisualStyleBackColor = true;
            this.btnIsobusOK.Click += new System.EventHandler(this.btnIsobusOK_Click);
            // 
            // btnCloseIsobus
            // 
            this.btnCloseIsobus.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseIsobus.FlatAppearance.BorderSize = 0;
            this.btnCloseIsobus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseIsobus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseIsobus.Image = global::AgIO.Properties.Resources.USB_Disconnect;
            this.btnCloseIsobus.Location = new System.Drawing.Point(497, 3);
            this.btnCloseIsobus.Name = "btnCloseIsobus";
            this.btnCloseIsobus.Size = new System.Drawing.Size(109, 58);
            this.btnCloseIsobus.TabIndex = 177;
            this.btnCloseIsobus.UseVisualStyleBackColor = false;
            this.btnCloseIsobus.Visible = false;
            this.btnCloseIsobus.Click += new System.EventHandler(this.btnCloseIsobus_Click);
            // 
            // btnOpenIsobus
            // 
            this.btnOpenIsobus.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenIsobus.FlatAppearance.BorderSize = 0;
            this.btnOpenIsobus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenIsobus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenIsobus.Image = global::AgIO.Properties.Resources.USB_Connect;
            this.btnOpenIsobus.Location = new System.Drawing.Point(390, 3);
            this.btnOpenIsobus.Name = "btnOpenIsobus";
            this.btnOpenIsobus.Size = new System.Drawing.Size(101, 58);
            this.btnOpenIsobus.TabIndex = 178;
            this.btnOpenIsobus.UseVisualStyleBackColor = false;
            this.btnOpenIsobus.Visible = false;
            this.btnOpenIsobus.Click += new System.EventHandler(this.btnOpenIsobus_Click);
            // 
            // cboxRadioAdapter
            // 
            this.cboxRadioAdapter.CausesValidation = false;
            this.cboxRadioAdapter.DisplayMember = "0";
            this.cboxRadioAdapter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRadioAdapter.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRadioAdapter.FormattingEnabled = true;
            this.cboxRadioAdapter.Items.AddRange(new object[] {
            "PEAK-PCAN",
            "InnoMaker-USB2CAN",
            "Rusoku-TouCAN",
            "SYS-TEC-USB2CAN"});
            this.cboxRadioAdapter.Location = new System.Drawing.Point(3, 21);
            this.cboxRadioAdapter.Name = "cboxRadioAdapter";
            this.cboxRadioAdapter.Size = new System.Drawing.Size(221, 37);
            this.cboxRadioAdapter.TabIndex = 179;
            this.cboxRadioAdapter.SelectedIndexChanged += new System.EventHandler(this.cboxRadioAdapter_SelectedIndexChanged);
            // 
            // lblCurrentAdapter
            // 
            this.lblCurrentAdapter.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAdapter.Location = new System.Drawing.Point(3, 0);
            this.lblCurrentAdapter.Name = "lblCurrentAdapter";
            this.lblCurrentAdapter.Size = new System.Drawing.Size(138, 18);
            this.lblCurrentAdapter.TabIndex = 180;
            this.lblCurrentAdapter.Text = "CANbus Adapter";
            this.lblCurrentAdapter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCurrentAdapter.UseCompatibleTextRendering = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutCANAdapter);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutChannel);
            this.flowLayoutPanel1.Controls.Add(this.btnOpenIsobus);
            this.flowLayoutPanel1.Controls.Add(this.btnCloseIsobus);
            this.flowLayoutPanel1.Controls.Add(this.flowLayoutDownloadIsobus);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(613, 77);
            this.flowLayoutPanel1.TabIndex = 181;
            // 
            // flowLayoutCANAdapter
            // 
            this.flowLayoutCANAdapter.Controls.Add(this.lblCurrentAdapter);
            this.flowLayoutCANAdapter.Controls.Add(this.cboxRadioAdapter);
            this.flowLayoutCANAdapter.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutCANAdapter.Name = "flowLayoutCANAdapter";
            this.flowLayoutCANAdapter.Size = new System.Drawing.Size(242, 69);
            this.flowLayoutCANAdapter.TabIndex = 181;
            this.flowLayoutCANAdapter.Visible = false;
            // 
            // flowLayoutChannel
            // 
            this.flowLayoutChannel.Controls.Add(this.lblChannel);
            this.flowLayoutChannel.Controls.Add(this.cboxRadioChannel);
            this.flowLayoutChannel.Location = new System.Drawing.Point(251, 3);
            this.flowLayoutChannel.Name = "flowLayoutChannel";
            this.flowLayoutChannel.Size = new System.Drawing.Size(133, 69);
            this.flowLayoutChannel.TabIndex = 182;
            this.flowLayoutChannel.Visible = false;
            // 
            // lblChannel
            // 
            this.lblChannel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChannel.Location = new System.Drawing.Point(3, 0);
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(76, 18);
            this.lblChannel.TabIndex = 180;
            this.lblChannel.Text = "Channel";
            this.lblChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblChannel.UseCompatibleTextRendering = true;
            // 
            // cboxRadioChannel
            // 
            this.cboxRadioChannel.CausesValidation = false;
            this.cboxRadioChannel.DisplayMember = "0";
            this.cboxRadioChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRadioChannel.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxRadioChannel.FormattingEnabled = true;
            this.cboxRadioChannel.Items.AddRange(new object[] {
            "PCAN-USB"});
            this.cboxRadioChannel.Location = new System.Drawing.Point(3, 21);
            this.cboxRadioChannel.Name = "cboxRadioChannel";
            this.cboxRadioChannel.Size = new System.Drawing.Size(116, 37);
            this.cboxRadioChannel.TabIndex = 179;
            this.cboxRadioChannel.SelectedIndexChanged += new System.EventHandler(this.cboxRadioChannel_SelectedIndexChanged);
            // 
            // flowLayoutDownloadIsobus
            // 
            this.flowLayoutDownloadIsobus.Controls.Add(this.lblDownloadIsobus);
            this.flowLayoutDownloadIsobus.Controls.Add(this.linkDownloadIsobus);
            this.flowLayoutDownloadIsobus.Location = new System.Drawing.Point(3, 78);
            this.flowLayoutDownloadIsobus.Name = "flowLayoutDownloadIsobus";
            this.flowLayoutDownloadIsobus.Size = new System.Drawing.Size(551, 44);
            this.flowLayoutDownloadIsobus.TabIndex = 183;
            // 
            // lblDownloadIsobus
            // 
            this.lblDownloadIsobus.AutoSize = true;
            this.lblDownloadIsobus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblDownloadIsobus.Location = new System.Drawing.Point(3, 0);
            this.lblDownloadIsobus.Name = "lblDownloadIsobus";
            this.lblDownloadIsobus.Size = new System.Drawing.Size(433, 18);
            this.lblDownloadIsobus.TabIndex = 183;
            this.lblDownloadIsobus.Text = "AOG-TaskController not installed, please download from:";
            // 
            // linkDownloadIsobus
            // 
            this.linkDownloadIsobus.AutoSize = true;
            this.linkDownloadIsobus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.linkDownloadIsobus.Location = new System.Drawing.Point(3, 18);
            this.linkDownloadIsobus.Name = "linkDownloadIsobus";
            this.linkDownloadIsobus.Size = new System.Drawing.Size(390, 18);
            this.linkDownloadIsobus.TabIndex = 182;
            this.linkDownloadIsobus.TabStop = true;
            this.linkDownloadIsobus.Text = "https://github.com/GwnDaan/AOG-TaskController";
            this.linkDownloadIsobus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDownloadIsobus_LinkClicked);
            // 
            // textBoxRcv
            // 
            this.textBoxRcv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxRcv.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRcv.Location = new System.Drawing.Point(12, 97);
            this.textBoxRcv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv.MinimumSize = new System.Drawing.Size(100, 200);
            this.textBoxRcv.Multiline = true;
            this.textBoxRcv.Name = "textBoxRcv";
            this.textBoxRcv.ReadOnly = true;
            this.textBoxRcv.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRcv.Size = new System.Drawing.Size(619, 345);
            this.textBoxRcv.TabIndex = 540;
            this.textBoxRcv.Visible = false;
            this.textBoxRcv.WordWrap = false;
            // 
            // FormISOBUS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(644, 526);
            this.ControlBox = false;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.btnIsobusOK);
            this.Controls.Add(this.textBoxRcv);
            this.Name = "FormISOBUS";
            this.Text = "ISOBUS Settings";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutCANAdapter.ResumeLayout(false);
            this.flowLayoutChannel.ResumeLayout(false);
            this.flowLayoutDownloadIsobus.ResumeLayout(false);
            this.flowLayoutDownloadIsobus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIsobusOK;
        private System.Windows.Forms.Button btnCloseIsobus;
        private System.Windows.Forms.Button btnOpenIsobus;
        private System.Windows.Forms.Label lblCurrentAdapter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutCANAdapter;
        private System.Windows.Forms.LinkLabel linkDownloadIsobus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutDownloadIsobus;
        private System.Windows.Forms.Label lblDownloadIsobus;
        private System.Windows.Forms.TextBox textBoxRcv;
        private System.Windows.Forms.ComboBox cboxRadioAdapter;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutChannel;
        private System.Windows.Forms.Label lblChannel;
        private System.Windows.Forms.ComboBox cboxRadioChannel;
    }
}