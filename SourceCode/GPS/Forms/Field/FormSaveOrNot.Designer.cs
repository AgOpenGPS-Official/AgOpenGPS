namespace AgOpenGPS
{
    partial class FormSaveOrNot
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
            this.lblQuestion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblExitCtr = new System.Windows.Forms.Label();
            this.labelExitToWindows = new System.Windows.Forms.Label();
            this.lblShutCtr = new System.Windows.Forms.Label();
            this.labelShutdownIn = new System.Windows.Forms.Label();
            this.labelShutdown = new System.Windows.Forms.Label();
            this.labelCancel = new System.Windows.Forms.Label();
            this.btnShutDown = new System.Windows.Forms.Button();
            this.labelExit = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnReturn = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblQuestion
            // 
            this.lblQuestion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.lblQuestion.Location = new System.Drawing.Point(85, 113);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(0, 25);
            this.lblQuestion.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label2.Location = new System.Drawing.Point(487, -28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 25);
            this.label2.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.BackColor = System.Drawing.Color.AliceBlue;
            this.panel1.Controls.Add(this.lblExitCtr);
            this.panel1.Controls.Add(this.labelExitToWindows);
            this.panel1.Controls.Add(this.lblShutCtr);
            this.panel1.Controls.Add(this.labelShutdownIn);
            this.panel1.Controls.Add(this.labelShutdown);
            this.panel1.Controls.Add(this.labelCancel);
            this.panel1.Controls.Add(this.btnShutDown);
            this.panel1.Controls.Add(this.labelExit);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnReturn);
            this.panel1.Location = new System.Drawing.Point(25, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 242);
            this.panel1.TabIndex = 260;
            // 
            // lblExitCtr
            // 
            this.lblExitCtr.AutoSize = true;
            this.lblExitCtr.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExitCtr.ForeColor = System.Drawing.Color.Black;
            this.lblExitCtr.Location = new System.Drawing.Point(612, 175);
            this.lblExitCtr.Name = "lblExitCtr";
            this.lblExitCtr.Size = new System.Drawing.Size(56, 58);
            this.lblExitCtr.TabIndex = 268;
            this.lblExitCtr.Text = "3";
            // 
            // labelExitToWindows
            // 
            this.labelExitToWindows.AutoSize = true;
            this.labelExitToWindows.BackColor = System.Drawing.Color.Transparent;
            this.labelExitToWindows.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExitToWindows.Location = new System.Drawing.Point(542, 145);
            this.labelExitToWindows.Name = "labelExitToWindows";
            this.labelExitToWindows.Size = new System.Drawing.Size(225, 25);
            this.labelExitToWindows.TabIndex = 269;
            this.labelExitToWindows.Text = "Exit To Windows In:";
            this.labelExitToWindows.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblShutCtr
            // 
            this.lblShutCtr.AutoSize = true;
            this.lblShutCtr.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblShutCtr.ForeColor = System.Drawing.Color.Black;
            this.lblShutCtr.Location = new System.Drawing.Point(65, 175);
            this.lblShutCtr.Name = "lblShutCtr";
            this.lblShutCtr.Size = new System.Drawing.Size(87, 58);
            this.lblShutCtr.TabIndex = 266;
            this.lblShutCtr.Text = "10";
            // 
            // labelShutdownIn
            // 
            this.labelShutdownIn.AutoSize = true;
            this.labelShutdownIn.BackColor = System.Drawing.Color.Transparent;
            this.labelShutdownIn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShutdownIn.Location = new System.Drawing.Point(31, 145);
            this.labelShutdownIn.Name = "labelShutdownIn";
            this.labelShutdownIn.Size = new System.Drawing.Size(155, 25);
            this.labelShutdownIn.TabIndex = 267;
            this.labelShutdownIn.Text = "Shutdown In:";
            // 
            // labelShutdown
            // 
            this.labelShutdown.AutoSize = true;
            this.labelShutdown.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShutdown.ForeColor = System.Drawing.Color.Black;
            this.labelShutdown.Location = new System.Drawing.Point(63, 30);
            this.labelShutdown.Name = "labelShutdown";
            this.labelShutdown.Size = new System.Drawing.Size(90, 19);
            this.labelShutdown.TabIndex = 265;
            this.labelShutdown.Text = "Shutdown";
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCancel.ForeColor = System.Drawing.Color.Black;
            this.labelCancel.Location = new System.Drawing.Point(340, 30);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(63, 19);
            this.labelCancel.TabIndex = 262;
            this.labelCancel.Text = "Cancel";
            // 
            // btnShutDown
            // 
            this.btnShutDown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutDown.BackgroundImage = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnShutDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnShutDown.FlatAppearance.BorderSize = 0;
            this.btnShutDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShutDown.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnShutDown.Location = new System.Drawing.Point(69, 52);
            this.btnShutDown.Name = "btnShutDown";
            this.btnShutDown.Size = new System.Drawing.Size(79, 79);
            this.btnShutDown.TabIndex = 263;
            this.btnShutDown.UseVisualStyleBackColor = false;
            this.btnShutDown.Click += new System.EventHandler(this.btnShutDown_Click);
            // 
            // labelExit
            // 
            this.labelExit.AutoSize = true;
            this.labelExit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExit.ForeColor = System.Drawing.Color.Black;
            this.labelExit.Location = new System.Drawing.Point(620, 30);
            this.labelExit.Name = "labelExit";
            this.labelExit.Size = new System.Drawing.Size(41, 19);
            this.labelExit.TabIndex = 260;
            this.labelExit.Text = "Exit";
            this.labelExit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Transparent;
            this.btnOk.BackgroundImage = global::AgOpenGPS.Properties.Resources.WindowClose;
            this.btnOk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnOk.Location = new System.Drawing.Point(595, 52);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 79);
            this.btnOk.TabIndex = 0;
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnReturn
            // 
            this.btnReturn.BackColor = System.Drawing.Color.Transparent;
            this.btnReturn.BackgroundImage = global::AgOpenGPS.Properties.Resources.back_button;
            this.btnReturn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnReturn.FlatAppearance.BorderSize = 0;
            this.btnReturn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReturn.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnReturn.Location = new System.Drawing.Point(321, 43);
            this.btnReturn.Name = "btnReturn";
            this.btnReturn.Size = new System.Drawing.Size(101, 96);
            this.btnReturn.TabIndex = 5;
            this.btnReturn.UseVisualStyleBackColor = false;
            this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormSaveOrNot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(825, 279);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSaveOrNot";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FormSaveOrNot_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.Button btnReturn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelCancel;
        private System.Windows.Forms.Label labelExit;
        private System.Windows.Forms.Button btnShutDown;
        private System.Windows.Forms.Label labelShutdown;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblShutCtr;
        private System.Windows.Forms.Label labelShutdownIn;
        private System.Windows.Forms.Label lblExitCtr;
        private System.Windows.Forms.Label labelExitToWindows;
    }
}