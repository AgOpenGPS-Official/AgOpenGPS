namespace AgOpenGPS
{
    partial class FormAllSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblVehicleName = new System.Windows.Forms.Label();
            this.lblToolName = new System.Windows.Forms.Label();
            this.lblEnvironmentName = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabVehicle = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelVehicle = new System.Windows.Forms.TableLayoutPanel();
            this.dgvVehicleL = new System.Windows.Forms.DataGridView();
            this.dgvVehicleM = new System.Windows.Forms.DataGridView();
            this.dgvVehicleR = new System.Windows.Forms.DataGridView();
            this.tabTool = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelTool = new System.Windows.Forms.TableLayoutPanel();
            this.dgvToolL = new System.Windows.Forms.DataGridView();
            this.dgvToolM = new System.Windows.Forms.DataGridView();
            this.dgvToolR = new System.Windows.Forms.DataGridView();
            this.tabEnvironment = new System.Windows.Forms.TabPage();
            this.tableLayoutPanelEnvironment = new System.Windows.Forms.TableLayoutPanel();
            this.dgvEnvironmentL = new System.Windows.Forms.DataGridView();
            this.dgvEnvironmentM = new System.Windows.Forms.DataGridView();
            this.dgvEnvironmentR = new System.Windows.Forms.DataGridView();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.dgvSystem = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.btnScreenShot = new System.Windows.Forms.Button();
            this.btnCreatePNG = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelHeader.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabVehicle.SuspendLayout();
            this.tableLayoutPanelVehicle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleR)).BeginInit();
            this.tabTool.SuspendLayout();
            this.tableLayoutPanelTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolR)).BeginInit();
            this.tabEnvironment.SuspendLayout();
            this.tableLayoutPanelEnvironment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentR)).BeginInit();
            this.tabSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystem)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.panelHeader.Controls.Add(this.lblVehicleName);
            this.panelHeader.Controls.Add(this.lblToolName);
            this.panelHeader.Controls.Add(this.lblEnvironmentName);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1188, 52);
            this.panelHeader.TabIndex = 0;
            // 
            // lblVehicleName
            // 
            this.lblVehicleName.BackColor = System.Drawing.Color.Transparent;
            this.lblVehicleName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblVehicleName.ForeColor = System.Drawing.Color.White;
            this.lblVehicleName.Location = new System.Drawing.Point(6, 6);
            this.lblVehicleName.Name = "lblVehicleName";
            this.lblVehicleName.Size = new System.Drawing.Size(380, 38);
            this.lblVehicleName.TabIndex = 0;
            this.lblVehicleName.Text = "Vehicle: -";
            this.lblVehicleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblToolName
            // 
            this.lblToolName.BackColor = System.Drawing.Color.Transparent;
            this.lblToolName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblToolName.ForeColor = System.Drawing.Color.White;
            this.lblToolName.Location = new System.Drawing.Point(400, 6);
            this.lblToolName.Name = "lblToolName";
            this.lblToolName.Size = new System.Drawing.Size(380, 38);
            this.lblToolName.TabIndex = 1;
            this.lblToolName.Text = "Tool: -";
            this.lblToolName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEnvironmentName
            // 
            this.lblEnvironmentName.BackColor = System.Drawing.Color.Transparent;
            this.lblEnvironmentName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblEnvironmentName.ForeColor = System.Drawing.Color.White;
            this.lblEnvironmentName.Location = new System.Drawing.Point(794, 6);
            this.lblEnvironmentName.Name = "lblEnvironmentName";
            this.lblEnvironmentName.Size = new System.Drawing.Size(388, 38);
            this.lblEnvironmentName.TabIndex = 2;
            this.lblEnvironmentName.Text = "Environment: -";
            this.lblEnvironmentName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabVehicle);
            this.tabControl.Controls.Add(this.tabTool);
            this.tabControl.Controls.Add(this.tabEnvironment);
            this.tabControl.Controls.Add(this.tabSystem);
            this.tabControl.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.tabControl.ItemSize = new System.Drawing.Size(220, 44);
            this.tabControl.Location = new System.Drawing.Point(0, 55);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1188, 538);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 1;
            // 
            // tabVehicle
            // 
            this.tabVehicle.Controls.Add(this.tableLayoutPanelVehicle);
            this.tabVehicle.Location = new System.Drawing.Point(4, 48);
            this.tabVehicle.Name = "tabVehicle";
            this.tabVehicle.Padding = new System.Windows.Forms.Padding(3);
            this.tabVehicle.Size = new System.Drawing.Size(1180, 486);
            this.tabVehicle.TabIndex = 0;
            this.tabVehicle.Text = "Vehicle";
            this.tabVehicle.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelVehicle
            // 
            this.tableLayoutPanelVehicle.ColumnCount = 3;
            this.tableLayoutPanelVehicle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelVehicle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelVehicle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanelVehicle.Controls.Add(this.dgvVehicleL, 0, 0);
            this.tableLayoutPanelVehicle.Controls.Add(this.dgvVehicleM, 1, 0);
            this.tableLayoutPanelVehicle.Controls.Add(this.dgvVehicleR, 2, 0);
            this.tableLayoutPanelVehicle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelVehicle.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelVehicle.Name = "tableLayoutPanelVehicle";
            this.tableLayoutPanelVehicle.RowCount = 1;
            this.tableLayoutPanelVehicle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelVehicle.Size = new System.Drawing.Size(1174, 480);
            this.tableLayoutPanelVehicle.TabIndex = 0;
            // 
            // dgvVehicleL
            // 
            this.dgvVehicleL.AllowUserToAddRows = false;
            this.dgvVehicleL.AllowUserToDeleteRows = false;
            this.dgvVehicleL.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvVehicleL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVehicleL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvVehicleL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleL.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvVehicleL.Location = new System.Drawing.Point(3, 3);
            this.dgvVehicleL.Name = "dgvVehicleL";
            this.dgvVehicleL.ReadOnly = true;
            this.dgvVehicleL.RowHeadersVisible = false;
            this.dgvVehicleL.Size = new System.Drawing.Size(385, 474);
            this.dgvVehicleL.TabIndex = 0;
            // 
            // dgvVehicleM
            // 
            this.dgvVehicleM.AllowUserToAddRows = false;
            this.dgvVehicleM.AllowUserToDeleteRows = false;
            this.dgvVehicleM.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvVehicleM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVehicleM.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvVehicleM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleM.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvVehicleM.Location = new System.Drawing.Point(394, 3);
            this.dgvVehicleM.Name = "dgvVehicleM";
            this.dgvVehicleM.ReadOnly = true;
            this.dgvVehicleM.RowHeadersVisible = false;
            this.dgvVehicleM.Size = new System.Drawing.Size(385, 474);
            this.dgvVehicleM.TabIndex = 1;
            // 
            // dgvVehicleR
            // 
            this.dgvVehicleR.AllowUserToAddRows = false;
            this.dgvVehicleR.AllowUserToDeleteRows = false;
            this.dgvVehicleR.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvVehicleR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvVehicleR.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvVehicleR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVehicleR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVehicleR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvVehicleR.Location = new System.Drawing.Point(785, 3);
            this.dgvVehicleR.Name = "dgvVehicleR";
            this.dgvVehicleR.ReadOnly = true;
            this.dgvVehicleR.RowHeadersVisible = false;
            this.dgvVehicleR.Size = new System.Drawing.Size(386, 474);
            this.dgvVehicleR.TabIndex = 2;
            // 
            // tabTool
            // 
            this.tabTool.Controls.Add(this.tableLayoutPanelTool);
            this.tabTool.Location = new System.Drawing.Point(4, 48);
            this.tabTool.Name = "tabTool";
            this.tabTool.Padding = new System.Windows.Forms.Padding(3);
            this.tabTool.Size = new System.Drawing.Size(1180, 486);
            this.tabTool.TabIndex = 1;
            this.tabTool.Text = "Tool";
            this.tabTool.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTool
            // 
            this.tableLayoutPanelTool.ColumnCount = 3;
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelTool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanelTool.Controls.Add(this.dgvToolL, 0, 0);
            this.tableLayoutPanelTool.Controls.Add(this.dgvToolM, 1, 0);
            this.tableLayoutPanelTool.Controls.Add(this.dgvToolR, 2, 0);
            this.tableLayoutPanelTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTool.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTool.Name = "tableLayoutPanelTool";
            this.tableLayoutPanelTool.RowCount = 1;
            this.tableLayoutPanelTool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTool.Size = new System.Drawing.Size(1174, 480);
            this.tableLayoutPanelTool.TabIndex = 0;
            // 
            // dgvToolL
            // 
            this.dgvToolL.AllowUserToAddRows = false;
            this.dgvToolL.AllowUserToDeleteRows = false;
            this.dgvToolL.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvToolL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvToolL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvToolL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvToolL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvToolL.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvToolL.Location = new System.Drawing.Point(3, 3);
            this.dgvToolL.Name = "dgvToolL";
            this.dgvToolL.ReadOnly = true;
            this.dgvToolL.RowHeadersVisible = false;
            this.dgvToolL.Size = new System.Drawing.Size(385, 474);
            this.dgvToolL.TabIndex = 0;
            // 
            // dgvToolM
            // 
            this.dgvToolM.AllowUserToAddRows = false;
            this.dgvToolM.AllowUserToDeleteRows = false;
            this.dgvToolM.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvToolM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvToolM.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvToolM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvToolM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvToolM.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvToolM.Location = new System.Drawing.Point(394, 3);
            this.dgvToolM.Name = "dgvToolM";
            this.dgvToolM.ReadOnly = true;
            this.dgvToolM.RowHeadersVisible = false;
            this.dgvToolM.Size = new System.Drawing.Size(385, 474);
            this.dgvToolM.TabIndex = 1;
            // 
            // dgvToolR
            // 
            this.dgvToolR.AllowUserToAddRows = false;
            this.dgvToolR.AllowUserToDeleteRows = false;
            this.dgvToolR.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvToolR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvToolR.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvToolR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvToolR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvToolR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvToolR.Location = new System.Drawing.Point(785, 3);
            this.dgvToolR.Name = "dgvToolR";
            this.dgvToolR.ReadOnly = true;
            this.dgvToolR.RowHeadersVisible = false;
            this.dgvToolR.Size = new System.Drawing.Size(386, 474);
            this.dgvToolR.TabIndex = 2;
            // 
            // tabEnvironment
            // 
            this.tabEnvironment.Controls.Add(this.tableLayoutPanelEnvironment);
            this.tabEnvironment.Location = new System.Drawing.Point(4, 48);
            this.tabEnvironment.Name = "tabEnvironment";
            this.tabEnvironment.Padding = new System.Windows.Forms.Padding(3);
            this.tabEnvironment.Size = new System.Drawing.Size(1180, 486);
            this.tabEnvironment.TabIndex = 2;
            this.tabEnvironment.Text = "Environment";
            this.tabEnvironment.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelEnvironment
            // 
            this.tableLayoutPanelEnvironment.ColumnCount = 3;
            this.tableLayoutPanelEnvironment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelEnvironment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanelEnvironment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanelEnvironment.Controls.Add(this.dgvEnvironmentL, 0, 0);
            this.tableLayoutPanelEnvironment.Controls.Add(this.dgvEnvironmentM, 1, 0);
            this.tableLayoutPanelEnvironment.Controls.Add(this.dgvEnvironmentR, 2, 0);
            this.tableLayoutPanelEnvironment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEnvironment.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelEnvironment.Name = "tableLayoutPanelEnvironment";
            this.tableLayoutPanelEnvironment.RowCount = 1;
            this.tableLayoutPanelEnvironment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEnvironment.Size = new System.Drawing.Size(1174, 480);
            this.tableLayoutPanelEnvironment.TabIndex = 0;
            // 
            // dgvEnvironmentL
            // 
            this.dgvEnvironmentL.AllowUserToAddRows = false;
            this.dgvEnvironmentL.AllowUserToDeleteRows = false;
            this.dgvEnvironmentL.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvEnvironmentL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEnvironmentL.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEnvironmentL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironmentL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnvironmentL.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvEnvironmentL.Location = new System.Drawing.Point(3, 3);
            this.dgvEnvironmentL.Name = "dgvEnvironmentL";
            this.dgvEnvironmentL.ReadOnly = true;
            this.dgvEnvironmentL.RowHeadersVisible = false;
            this.dgvEnvironmentL.Size = new System.Drawing.Size(385, 474);
            this.dgvEnvironmentL.TabIndex = 0;
            // 
            // dgvEnvironmentM
            // 
            this.dgvEnvironmentM.AllowUserToAddRows = false;
            this.dgvEnvironmentM.AllowUserToDeleteRows = false;
            this.dgvEnvironmentM.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvEnvironmentM.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEnvironmentM.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEnvironmentM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironmentM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnvironmentM.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvEnvironmentM.Location = new System.Drawing.Point(394, 3);
            this.dgvEnvironmentM.Name = "dgvEnvironmentM";
            this.dgvEnvironmentM.ReadOnly = true;
            this.dgvEnvironmentM.RowHeadersVisible = false;
            this.dgvEnvironmentM.Size = new System.Drawing.Size(385, 474);
            this.dgvEnvironmentM.TabIndex = 1;
            // 
            // dgvEnvironmentR
            // 
            this.dgvEnvironmentR.AllowUserToAddRows = false;
            this.dgvEnvironmentR.AllowUserToDeleteRows = false;
            this.dgvEnvironmentR.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvEnvironmentR.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEnvironmentR.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvEnvironmentR.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnvironmentR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEnvironmentR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvEnvironmentR.Location = new System.Drawing.Point(785, 3);
            this.dgvEnvironmentR.Name = "dgvEnvironmentR";
            this.dgvEnvironmentR.ReadOnly = true;
            this.dgvEnvironmentR.RowHeadersVisible = false;
            this.dgvEnvironmentR.Size = new System.Drawing.Size(386, 474);
            this.dgvEnvironmentR.TabIndex = 2;
            // 
            // tabSystem
            // 
            this.tabSystem.Controls.Add(this.dgvSystem);
            this.tabSystem.Location = new System.Drawing.Point(4, 48);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystem.Size = new System.Drawing.Size(1180, 486);
            this.tabSystem.TabIndex = 3;
            this.tabSystem.Text = "System / GPS";
            this.tabSystem.UseVisualStyleBackColor = true;
            // 
            // dgvSystem
            // 
            this.dgvSystem.AllowUserToAddRows = false;
            this.dgvSystem.AllowUserToDeleteRows = false;
            this.dgvSystem.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvSystem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSystem.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSystem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSystem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSystem.Font = new System.Drawing.Font("Tahoma", 10F);
            this.dgvSystem.Location = new System.Drawing.Point(3, 3);
            this.dgvSystem.Name = "dgvSystem";
            this.dgvSystem.ReadOnly = true;
            this.dgvSystem.RowHeadersVisible = false;
            this.dgvSystem.Size = new System.Drawing.Size(1174, 480);
            this.dgvSystem.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnClose.Location = new System.Drawing.Point(1112, 600);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 64);
            this.btnClose.TabIndex = 4;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExportCSV
            //
            this.btnExportCSV.BackColor = System.Drawing.Color.Transparent;
            this.btnExportCSV.FlatAppearance.BorderSize = 0;
            this.btnExportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCSV.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnExportCSV.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnExportCSV.Location = new System.Drawing.Point(6, 599);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(80, 65);
            this.btnExportCSV.TabIndex = 5;
            this.btnExportCSV.Text = "CSV";
            this.btnExportCSV.UseVisualStyleBackColor = false;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            //
            // btnScreenShot
            //
            this.btnScreenShot.FlatAppearance.BorderSize = 0;
            this.btnScreenShot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScreenShot.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnScreenShot.Image = global::AgOpenGPS.Properties.Resources.ScreenShot;
            this.btnScreenShot.Location = new System.Drawing.Point(980, 599);
            this.btnScreenShot.Name = "btnScreenShot";
            this.btnScreenShot.Size = new System.Drawing.Size(80, 65);
            this.btnScreenShot.TabIndex = 2;
            this.btnScreenShot.UseVisualStyleBackColor = true;
            this.btnScreenShot.Click += new System.EventHandler(this.btnScreenShot_Click);
            // 
            // btnCreatePNG
            // 
            this.btnCreatePNG.FlatAppearance.BorderSize = 0;
            this.btnCreatePNG.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreatePNG.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCreatePNG.Image = global::AgOpenGPS.Properties.Resources.Screen2PNG;
            this.btnCreatePNG.Location = new System.Drawing.Point(878, 599);
            this.btnCreatePNG.Name = "btnCreatePNG";
            this.btnCreatePNG.Size = new System.Drawing.Size(80, 65);
            this.btnCreatePNG.TabIndex = 3;
            this.btnCreatePNG.UseVisualStyleBackColor = true;
            this.btnCreatePNG.Click += new System.EventHandler(this.btnCreatePNG_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormAllSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1188, 671);
            this.ControlBox = false;
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExportCSV);
            this.Controls.Add(this.btnScreenShot);
            this.Controls.Add(this.btnCreatePNG);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormAllSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "All Settings";
            this.panelHeader.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabVehicle.ResumeLayout(false);
            this.tableLayoutPanelVehicle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVehicleR)).EndInit();
            this.tabTool.ResumeLayout(false);
            this.tableLayoutPanelTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvToolR)).EndInit();
            this.tabEnvironment.ResumeLayout(false);
            this.tableLayoutPanelEnvironment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnvironmentR)).EndInit();
            this.tabSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSystem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblVehicleName;
        private System.Windows.Forms.Label lblToolName;
        private System.Windows.Forms.Label lblEnvironmentName;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabVehicle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelVehicle;
        private System.Windows.Forms.DataGridView dgvVehicleL;
        private System.Windows.Forms.DataGridView dgvVehicleM;
        private System.Windows.Forms.DataGridView dgvVehicleR;
        private System.Windows.Forms.TabPage tabTool;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTool;
        private System.Windows.Forms.DataGridView dgvToolL;
        private System.Windows.Forms.DataGridView dgvToolM;
        private System.Windows.Forms.DataGridView dgvToolR;
        private System.Windows.Forms.TabPage tabEnvironment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelEnvironment;
        private System.Windows.Forms.DataGridView dgvEnvironmentL;
        private System.Windows.Forms.DataGridView dgvEnvironmentM;
        private System.Windows.Forms.DataGridView dgvEnvironmentR;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.DataGridView dgvSystem;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExportCSV;
        private System.Windows.Forms.Button btnScreenShot;
        private System.Windows.Forms.Button btnCreatePNG;
        private System.Windows.Forms.Timer timer1;
    }
}
