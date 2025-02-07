namespace AgOpenGPS
{
    partial class FormQuickAB
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
            this.lblCurveExists = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelName = new System.Windows.Forms.Panel();
            this.labelAddName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panelCurve = new System.Windows.Forms.Panel();
            this.labelCurve = new System.Windows.Forms.Label();
            this.btnRefSideCurve = new System.Windows.Forms.Button();
            this.btnCancel_Curve = new System.Windows.Forms.Button();
            this.btnACurve = new System.Windows.Forms.Button();
            this.btnBCurve = new System.Windows.Forms.Button();
            this.btnPausePlay = new System.Windows.Forms.Button();
            this.panelChoose = new System.Windows.Forms.Panel();
            this.btnCancelChoose = new System.Windows.Forms.Button();
            this.btnzAPlus = new System.Windows.Forms.Button();
            this.btnzABLine = new System.Windows.Forms.Button();
            this.btnzABCurve = new System.Windows.Forms.Button();
            this.panelABLine = new System.Windows.Forms.Panel();
            this.btnRefSideAB = new System.Windows.Forms.Button();
            this.labelABLine = new System.Windows.Forms.Label();
            this.btnCancel_ABLine = new System.Windows.Forms.Button();
            this.btnEnter_AB = new System.Windows.Forms.Button();
            this.btnALine = new System.Windows.Forms.Button();
            this.btnBLine = new System.Windows.Forms.Button();
            this.panelAPlus = new System.Windows.Forms.Panel();
            this.btnRefSideAPlus = new System.Windows.Forms.Button();
            this.labelAPlus = new System.Windows.Forms.Label();
            this.btnCancel_APlus = new System.Windows.Forms.Button();
            this.btnEnter_APlus = new System.Windows.Forms.Button();
            this.nudHeading = new AgOpenGPS.NudlessNumericUpDown();
            this.btnAPlus = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelName.SuspendLayout();
            this.panelCurve.SuspendLayout();
            this.panelChoose.SuspendLayout();
            this.panelABLine.SuspendLayout();
            this.panelAPlus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCurveExists
            // 
            this.lblCurveExists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurveExists.AutoSize = true;
            this.lblCurveExists.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveExists.ForeColor = System.Drawing.Color.Black;
            this.lblCurveExists.Location = new System.Drawing.Point(111, 195);
            this.lblCurveExists.Name = "lblCurveExists";
            this.lblCurveExists.Size = new System.Drawing.Size(91, 23);
            this.lblCurveExists.TabIndex = 87;
            this.lblCurveExists.Text = "> OFF <";
            this.lblCurveExists.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.Color.Black;
            this.labelStatus.Location = new System.Drawing.Point(36, 195);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(84, 23);
            this.labelStatus.TabIndex = 148;
            this.labelStatus.Text = "Status: ";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelName
            // 
            this.panelName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelName.Controls.Add(this.labelAddName);
            this.panelName.Controls.Add(this.label1);
            this.panelName.Controls.Add(this.btnAddTime);
            this.panelName.Controls.Add(this.btnAdd);
            this.panelName.Controls.Add(this.textBox1);
            this.panelName.Location = new System.Drawing.Point(8, 336);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(241, 310);
            this.panelName.TabIndex = 434;
            // 
            // labelAddName
            // 
            this.labelAddName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAddName.AutoSize = true;
            this.labelAddName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAddName.ForeColor = System.Drawing.Color.Black;
            this.labelAddName.Location = new System.Drawing.Point(16, 14);
            this.labelAddName.Name = "labelAddName";
            this.labelAddName.Size = new System.Drawing.Size(66, 16);
            this.labelAddName.TabIndex = 450;
            this.labelAddName.Text = "Add Name";
            this.labelAddName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(48, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 29);
            this.label1.TabIndex = 424;
            this.label1.Text = "+";
            // 
            // btnAddTime
            // 
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTime.Image = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.btnAddTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddTime.Location = new System.Drawing.Point(87, 133);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(68, 69);
            this.btnAddTime.TabIndex = 150;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(141, 233);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(88, 70);
            this.btnAdd.TabIndex = 150;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightCyan;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.CausesValidation = false;
            this.textBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(17, 37);
            this.textBox1.Margin = new System.Windows.Forms.Padding(0);
            this.textBox1.MaxLength = 100;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(204, 77);
            this.textBox1.TabIndex = 145;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox1.Click += new System.EventHandler(this.textBox_Click);
            // 
            // panelCurve
            // 
            this.panelCurve.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelCurve.Controls.Add(this.labelCurve);
            this.panelCurve.Controls.Add(this.btnRefSideCurve);
            this.panelCurve.Controls.Add(this.btnCancel_Curve);
            this.panelCurve.Controls.Add(this.btnACurve);
            this.panelCurve.Controls.Add(this.btnBCurve);
            this.panelCurve.Controls.Add(this.btnPausePlay);
            this.panelCurve.Controls.Add(this.lblCurveExists);
            this.panelCurve.Controls.Add(this.labelStatus);
            this.panelCurve.Location = new System.Drawing.Point(255, 336);
            this.panelCurve.Name = "panelCurve";
            this.panelCurve.Size = new System.Drawing.Size(241, 310);
            this.panelCurve.TabIndex = 436;
            // 
            // labelCurve
            // 
            this.labelCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurve.AutoSize = true;
            this.labelCurve.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurve.ForeColor = System.Drawing.Color.Black;
            this.labelCurve.Location = new System.Drawing.Point(4, 4);
            this.labelCurve.Name = "labelCurve";
            this.labelCurve.Size = new System.Drawing.Size(40, 16);
            this.labelCurve.TabIndex = 449;
            this.labelCurve.Text = "Curve";
            this.labelCurve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRefSideCurve
            // 
            this.btnRefSideCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefSideCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnRefSideCurve.FlatAppearance.BorderSize = 0;
            this.btnRefSideCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefSideCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRefSideCurve.Image = global::AgOpenGPS.Properties.Resources.BoundaryRight;
            this.btnRefSideCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefSideCurve.Location = new System.Drawing.Point(88, 2);
            this.btnRefSideCurve.Name = "btnRefSideCurve";
            this.btnRefSideCurve.Size = new System.Drawing.Size(70, 60);
            this.btnRefSideCurve.TabIndex = 426;
            this.btnRefSideCurve.UseVisualStyleBackColor = false;
            this.btnRefSideCurve.Click += new System.EventHandler(this.btnRefSideCurve_Click);
            // 
            // btnCancel_Curve
            // 
            this.btnCancel_Curve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel_Curve.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel_Curve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel_Curve.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_Curve.FlatAppearance.BorderSize = 0;
            this.btnCancel_Curve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel_Curve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel_Curve.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel_Curve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel_Curve.Location = new System.Drawing.Point(7, 233);
            this.btnCancel_Curve.Name = "btnCancel_Curve";
            this.btnCancel_Curve.Size = new System.Drawing.Size(88, 70);
            this.btnCancel_Curve.TabIndex = 423;
            this.btnCancel_Curve.UseVisualStyleBackColor = false;
            this.btnCancel_Curve.Click += new System.EventHandler(this.btnCancelCurve_Click);
            // 
            // btnACurve
            // 
            this.btnACurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnACurve.BackColor = System.Drawing.Color.Transparent;
            this.btnACurve.FlatAppearance.BorderSize = 0;
            this.btnACurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnACurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnACurve.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.btnACurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnACurve.Location = new System.Drawing.Point(7, 97);
            this.btnACurve.Name = "btnACurve";
            this.btnACurve.Size = new System.Drawing.Size(88, 70);
            this.btnACurve.TabIndex = 63;
            this.btnACurve.UseVisualStyleBackColor = false;
            this.btnACurve.Click += new System.EventHandler(this.btnACurve_Click);
            // 
            // btnBCurve
            // 
            this.btnBCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnBCurve.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnBCurve.FlatAppearance.BorderSize = 0;
            this.btnBCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBCurve.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnBCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBCurve.Location = new System.Drawing.Point(144, 233);
            this.btnBCurve.Name = "btnBCurve";
            this.btnBCurve.Size = new System.Drawing.Size(88, 70);
            this.btnBCurve.TabIndex = 64;
            this.btnBCurve.UseVisualStyleBackColor = false;
            this.btnBCurve.Click += new System.EventHandler(this.btnBCurve_Click);
            // 
            // btnPausePlay
            // 
            this.btnPausePlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPausePlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPausePlay.Enabled = false;
            this.btnPausePlay.FlatAppearance.BorderSize = 0;
            this.btnPausePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPausePlay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausePlay.Image = global::AgOpenGPS.Properties.Resources.boundaryPause;
            this.btnPausePlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPausePlay.Location = new System.Drawing.Point(144, 97);
            this.btnPausePlay.Name = "btnPausePlay";
            this.btnPausePlay.Size = new System.Drawing.Size(88, 70);
            this.btnPausePlay.TabIndex = 140;
            this.btnPausePlay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPausePlay.UseVisualStyleBackColor = false;
            this.btnPausePlay.Click += new System.EventHandler(this.btnPausePlayCurve_Click);
            // 
            // panelChoose
            // 
            this.panelChoose.BackColor = System.Drawing.Color.Gainsboro;
            this.panelChoose.Controls.Add(this.btnCancelChoose);
            this.panelChoose.Controls.Add(this.btnzAPlus);
            this.panelChoose.Controls.Add(this.btnzABLine);
            this.panelChoose.Controls.Add(this.btnzABCurve);
            this.panelChoose.Location = new System.Drawing.Point(65, 5);
            this.panelChoose.Name = "panelChoose";
            this.panelChoose.Size = new System.Drawing.Size(241, 310);
            this.panelChoose.TabIndex = 441;
            // 
            // btnCancelChoose
            // 
            this.btnCancelChoose.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelChoose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelChoose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelChoose.FlatAppearance.BorderSize = 0;
            this.btnCancelChoose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelChoose.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelChoose.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancelChoose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelChoose.Location = new System.Drawing.Point(12, 223);
            this.btnCancelChoose.Name = "btnCancelChoose";
            this.btnCancelChoose.Size = new System.Drawing.Size(65, 70);
            this.btnCancelChoose.TabIndex = 423;
            this.btnCancelChoose.UseVisualStyleBackColor = false;
            this.btnCancelChoose.Click += new System.EventHandler(this.btnCancelCurve_Click);
            // 
            // btnzAPlus
            // 
            this.btnzAPlus.BackColor = System.Drawing.Color.White;
            this.btnzAPlus.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnzAPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnzAPlus.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnzAPlus.Image = global::AgOpenGPS.Properties.Resources.ABTrackA_;
            this.btnzAPlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnzAPlus.Location = new System.Drawing.Point(9, 29);
            this.btnzAPlus.Name = "btnzAPlus";
            this.btnzAPlus.Size = new System.Drawing.Size(96, 98);
            this.btnzAPlus.TabIndex = 444;
            this.btnzAPlus.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnzAPlus.UseVisualStyleBackColor = false;
            this.btnzAPlus.Click += new System.EventHandler(this.btnzAPlus_Click);
            // 
            // btnzABLine
            // 
            this.btnzABLine.BackColor = System.Drawing.Color.White;
            this.btnzABLine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnzABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnzABLine.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnzABLine.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.btnzABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnzABLine.Location = new System.Drawing.Point(138, 29);
            this.btnzABLine.Name = "btnzABLine";
            this.btnzABLine.Size = new System.Drawing.Size(96, 98);
            this.btnzABLine.TabIndex = 444;
            this.btnzABLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnzABLine.UseVisualStyleBackColor = false;
            this.btnzABLine.Click += new System.EventHandler(this.btnzABLine_Click);
            // 
            // btnzABCurve
            // 
            this.btnzABCurve.BackColor = System.Drawing.Color.White;
            this.btnzABCurve.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnzABCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnzABCurve.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnzABCurve.Image = global::AgOpenGPS.Properties.Resources.ABTrackCurve;
            this.btnzABCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnzABCurve.Location = new System.Drawing.Point(138, 195);
            this.btnzABCurve.Name = "btnzABCurve";
            this.btnzABCurve.Size = new System.Drawing.Size(96, 98);
            this.btnzABCurve.TabIndex = 443;
            this.btnzABCurve.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnzABCurve.UseVisualStyleBackColor = false;
            this.btnzABCurve.Click += new System.EventHandler(this.btnzABCurve_Click);
            // 
            // panelABLine
            // 
            this.panelABLine.BackColor = System.Drawing.Color.Transparent;
            this.panelABLine.Controls.Add(this.btnRefSideAB);
            this.panelABLine.Controls.Add(this.labelABLine);
            this.panelABLine.Controls.Add(this.btnCancel_ABLine);
            this.panelABLine.Controls.Add(this.btnEnter_AB);
            this.panelABLine.Controls.Add(this.btnALine);
            this.panelABLine.Controls.Add(this.btnBLine);
            this.panelABLine.Location = new System.Drawing.Point(503, 336);
            this.panelABLine.Name = "panelABLine";
            this.panelABLine.Size = new System.Drawing.Size(241, 310);
            this.panelABLine.TabIndex = 442;
            // 
            // btnRefSideAB
            // 
            this.btnRefSideAB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefSideAB.BackColor = System.Drawing.Color.Transparent;
            this.btnRefSideAB.FlatAppearance.BorderSize = 0;
            this.btnRefSideAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefSideAB.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRefSideAB.Image = global::AgOpenGPS.Properties.Resources.BoundaryRight;
            this.btnRefSideAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefSideAB.Location = new System.Drawing.Point(90, 3);
            this.btnRefSideAB.Name = "btnRefSideAB";
            this.btnRefSideAB.Size = new System.Drawing.Size(70, 60);
            this.btnRefSideAB.TabIndex = 427;
            this.btnRefSideAB.UseVisualStyleBackColor = false;
            this.btnRefSideAB.Click += new System.EventHandler(this.btnRefSideAB_Click);
            // 
            // labelABLine
            // 
            this.labelABLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelABLine.AutoSize = true;
            this.labelABLine.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelABLine.ForeColor = System.Drawing.Color.Black;
            this.labelABLine.Location = new System.Drawing.Point(5, 4);
            this.labelABLine.Name = "labelABLine";
            this.labelABLine.Size = new System.Drawing.Size(49, 16);
            this.labelABLine.TabIndex = 447;
            this.labelABLine.Text = "AB Line";
            this.labelABLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel_ABLine
            // 
            this.btnCancel_ABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel_ABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel_ABLine.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_ABLine.FlatAppearance.BorderSize = 0;
            this.btnCancel_ABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel_ABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel_ABLine.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel_ABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel_ABLine.Location = new System.Drawing.Point(8, 233);
            this.btnCancel_ABLine.Name = "btnCancel_ABLine";
            this.btnCancel_ABLine.Size = new System.Drawing.Size(88, 70);
            this.btnCancel_ABLine.TabIndex = 428;
            this.btnCancel_ABLine.UseVisualStyleBackColor = false;
            this.btnCancel_ABLine.Click += new System.EventHandler(this.btnCancelCurve_Click);
            // 
            // btnEnter_AB
            // 
            this.btnEnter_AB.BackColor = System.Drawing.Color.Transparent;
            this.btnEnter_AB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEnter_AB.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnEnter_AB.FlatAppearance.BorderSize = 0;
            this.btnEnter_AB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnter_AB.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnEnter_AB.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnEnter_AB.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnter_AB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEnter_AB.Location = new System.Drawing.Point(142, 233);
            this.btnEnter_AB.Name = "btnEnter_AB";
            this.btnEnter_AB.Size = new System.Drawing.Size(88, 70);
            this.btnEnter_AB.TabIndex = 427;
            this.btnEnter_AB.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnEnter_AB.UseVisualStyleBackColor = false;
            this.btnEnter_AB.Click += new System.EventHandler(this.btnEnter_AB_Click);
            // 
            // btnALine
            // 
            this.btnALine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnALine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnALine.FlatAppearance.BorderSize = 0;
            this.btnALine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnALine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnALine.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.btnALine.Location = new System.Drawing.Point(13, 103);
            this.btnALine.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnALine.Name = "btnALine";
            this.btnALine.Size = new System.Drawing.Size(80, 78);
            this.btnALine.TabIndex = 57;
            this.btnALine.UseVisualStyleBackColor = true;
            this.btnALine.Click += new System.EventHandler(this.btnALine_Click);
            // 
            // btnBLine
            // 
            this.btnBLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBLine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBLine.FlatAppearance.BorderSize = 0;
            this.btnBLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBLine.Image = global::AgOpenGPS.Properties.Resources.LetterBBlue;
            this.btnBLine.Location = new System.Drawing.Point(156, 103);
            this.btnBLine.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnBLine.Name = "btnBLine";
            this.btnBLine.Size = new System.Drawing.Size(80, 78);
            this.btnBLine.TabIndex = 58;
            this.btnBLine.UseVisualStyleBackColor = true;
            this.btnBLine.Click += new System.EventHandler(this.btnBLine_Click);
            // 
            // panelAPlus
            // 
            this.panelAPlus.BackColor = System.Drawing.Color.Transparent;
            this.panelAPlus.Controls.Add(this.btnRefSideAPlus);
            this.panelAPlus.Controls.Add(this.labelAPlus);
            this.panelAPlus.Controls.Add(this.btnCancel_APlus);
            this.panelAPlus.Controls.Add(this.btnEnter_APlus);
            this.panelAPlus.Controls.Add(this.nudHeading);
            this.panelAPlus.Controls.Add(this.btnAPlus);
            this.panelAPlus.Location = new System.Drawing.Point(351, 5);
            this.panelAPlus.Name = "panelAPlus";
            this.panelAPlus.Size = new System.Drawing.Size(241, 310);
            this.panelAPlus.TabIndex = 448;
            // 
            // btnRefSideAPlus
            // 
            this.btnRefSideAPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefSideAPlus.BackColor = System.Drawing.Color.Transparent;
            this.btnRefSideAPlus.FlatAppearance.BorderSize = 0;
            this.btnRefSideAPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefSideAPlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRefSideAPlus.Image = global::AgOpenGPS.Properties.Resources.BoundaryRight;
            this.btnRefSideAPlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefSideAPlus.Location = new System.Drawing.Point(26, 62);
            this.btnRefSideAPlus.Name = "btnRefSideAPlus";
            this.btnRefSideAPlus.Size = new System.Drawing.Size(70, 60);
            this.btnRefSideAPlus.TabIndex = 448;
            this.btnRefSideAPlus.UseVisualStyleBackColor = false;
            this.btnRefSideAPlus.Click += new System.EventHandler(this.btnRefSideAPlus_Click);
            // 
            // labelAPlus
            // 
            this.labelAPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAPlus.AutoSize = true;
            this.labelAPlus.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAPlus.ForeColor = System.Drawing.Color.Black;
            this.labelAPlus.Location = new System.Drawing.Point(6, 4);
            this.labelAPlus.Name = "labelAPlus";
            this.labelAPlus.Size = new System.Drawing.Size(23, 16);
            this.labelAPlus.TabIndex = 447;
            this.labelAPlus.Text = "A+";
            this.labelAPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel_APlus
            // 
            this.btnCancel_APlus.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel_APlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel_APlus.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel_APlus.FlatAppearance.BorderSize = 0;
            this.btnCancel_APlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel_APlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel_APlus.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel_APlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel_APlus.Location = new System.Drawing.Point(8, 238);
            this.btnCancel_APlus.Name = "btnCancel_APlus";
            this.btnCancel_APlus.Size = new System.Drawing.Size(88, 70);
            this.btnCancel_APlus.TabIndex = 428;
            this.btnCancel_APlus.UseVisualStyleBackColor = false;
            this.btnCancel_APlus.Click += new System.EventHandler(this.btnCancelCurve_Click);
            // 
            // btnEnter_APlus
            // 
            this.btnEnter_APlus.BackColor = System.Drawing.Color.Transparent;
            this.btnEnter_APlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEnter_APlus.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnEnter_APlus.FlatAppearance.BorderSize = 0;
            this.btnEnter_APlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnter_APlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnEnter_APlus.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnEnter_APlus.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEnter_APlus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEnter_APlus.Location = new System.Drawing.Point(142, 238);
            this.btnEnter_APlus.Name = "btnEnter_APlus";
            this.btnEnter_APlus.Size = new System.Drawing.Size(88, 70);
            this.btnEnter_APlus.TabIndex = 427;
            this.btnEnter_APlus.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnEnter_APlus.UseVisualStyleBackColor = false;
            this.btnEnter_APlus.Click += new System.EventHandler(this.btnEnter_APlus_Click);
            // 
            // nudHeading
            // 
            this.nudHeading.BackColor = System.Drawing.Color.LightCyan;
            this.nudHeading.DecimalPlaces = 4;
            this.nudHeading.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHeading.InterceptArrowKeys = false;
            this.nudHeading.Location = new System.Drawing.Point(25, 174);
            this.nudHeading.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudHeading.Name = "nudHeading";
            this.nudHeading.ReadOnly = true;
            this.nudHeading.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudHeading.Size = new System.Drawing.Size(195, 46);
            this.nudHeading.TabIndex = 413;
            this.nudHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudHeading.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudHeading.Click += new System.EventHandler(this.nudHeading_Click);
            // 
            // btnAPlus
            // 
            this.btnAPlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAPlus.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAPlus.FlatAppearance.BorderSize = 0;
            this.btnAPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAPlus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAPlus.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.btnAPlus.Location = new System.Drawing.Point(140, 53);
            this.btnAPlus.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnAPlus.Name = "btnAPlus";
            this.btnAPlus.Size = new System.Drawing.Size(80, 78);
            this.btnAPlus.TabIndex = 57;
            this.btnAPlus.UseVisualStyleBackColor = true;
            this.btnAPlus.Click += new System.EventHandler(this.btnAPlus_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormQuickAB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(763, 654);
            this.ControlBox = false;
            this.Controls.Add(this.panelAPlus);
            this.Controls.Add(this.panelABLine);
            this.Controls.Add(this.panelChoose);
            this.Controls.Add(this.panelCurve);
            this.Controls.Add(this.panelName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQuickAB";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tracks";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormQuickAB_FormClosing);
            this.Load += new System.EventHandler(this.FormQuickAB_Load);
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelCurve.ResumeLayout(false);
            this.panelCurve.PerformLayout();
            this.panelChoose.ResumeLayout(false);
            this.panelABLine.ResumeLayout(false);
            this.panelABLine.PerformLayout();
            this.panelAPlus.ResumeLayout(false);
            this.panelAPlus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBCurve;
        private System.Windows.Forms.Button btnACurve;
        private System.Windows.Forms.Label lblCurveExists;
        private System.Windows.Forms.Button btnPausePlay;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button btnCancel_Curve;
        private System.Windows.Forms.Panel panelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panelCurve;
        private System.Windows.Forms.Panel panelChoose;
        private System.Windows.Forms.Button btnCancelChoose;
        private System.Windows.Forms.Button btnzABLine;
        private System.Windows.Forms.Button btnzABCurve;
        private System.Windows.Forms.Button btnzAPlus;
        private System.Windows.Forms.Label labelAddName;
        private System.Windows.Forms.Panel panelABLine;
        private System.Windows.Forms.Label labelABLine;
        private System.Windows.Forms.Button btnCancel_ABLine;
        private System.Windows.Forms.Button btnEnter_AB;
        private System.Windows.Forms.Button btnALine;
        private System.Windows.Forms.Button btnBLine;
        private NudlessNumericUpDown nudHeading;
        private System.Windows.Forms.Panel panelAPlus;
        private System.Windows.Forms.Label labelAPlus;
        private System.Windows.Forms.Button btnCancel_APlus;
        private System.Windows.Forms.Button btnEnter_APlus;
        private System.Windows.Forms.Button btnAPlus;
        private System.Windows.Forms.Button btnRefSideCurve;
        private System.Windows.Forms.Button btnRefSideAB;
        private System.Windows.Forms.Button btnRefSideAPlus;
        private System.Windows.Forms.Label labelCurve;
        private System.Windows.Forms.Timer timer1;
    }
}
