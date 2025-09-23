namespace AgIO.Forms
{
    partial class FormAdvancedSettings
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
            this.cboxStartMinimized = new System.Windows.Forms.CheckBox();
            this.cboxAutoRunGPS_Out = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboxStartMinimized
            // 
            this.cboxStartMinimized.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxStartMinimized.BackColor = System.Drawing.Color.Transparent;
            this.cboxStartMinimized.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxStartMinimized.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.cboxStartMinimized.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxStartMinimized.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxStartMinimized.Image = global::AgIO.Properties.Resources.MinimizeIcon;
            this.cboxStartMinimized.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxStartMinimized.Location = new System.Drawing.Point(12, 12);
            this.cboxStartMinimized.Name = "cboxStartMinimized";
            this.cboxStartMinimized.Size = new System.Drawing.Size(143, 128);
            this.cboxStartMinimized.TabIndex = 530;
            this.cboxStartMinimized.UseVisualStyleBackColor = false;
            this.cboxStartMinimized.CheckedChanged += new System.EventHandler(this.cboxStartMinimized_CheckedChanged);
            // 
            // cboxAutoRunGPS_Out
            // 
            this.cboxAutoRunGPS_Out.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxAutoRunGPS_Out.BackColor = System.Drawing.Color.Transparent;
            this.cboxAutoRunGPS_Out.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxAutoRunGPS_Out.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(180)))));
            this.cboxAutoRunGPS_Out.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxAutoRunGPS_Out.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxAutoRunGPS_Out.Image = global::AgIO.Properties.Resources.GPS_Out;
            this.cboxAutoRunGPS_Out.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxAutoRunGPS_Out.Location = new System.Drawing.Point(12, 189);
            this.cboxAutoRunGPS_Out.Name = "cboxAutoRunGPS_Out";
            this.cboxAutoRunGPS_Out.Size = new System.Drawing.Size(143, 128);
            this.cboxAutoRunGPS_Out.TabIndex = 529;
            this.cboxAutoRunGPS_Out.Text = "Auto\r\nRun";
            this.cboxAutoRunGPS_Out.UseVisualStyleBackColor = false;
            this.cboxAutoRunGPS_Out.CheckedChanged += new System.EventHandler(this.cboxAutoRunGPS_Out_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(161, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 31);
            this.label1.TabIndex = 531;
            this.label1.Text = "Start Minimized";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(161, 235);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(270, 31);
            this.label2.TabIndex = 532;
            this.label2.Text = "Auto Start GPS-Out";
            // 
            // btnClose
            // 
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::AgIO.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(367, 342);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 64);
            this.btnClose.TabIndex = 533;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormAdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 418);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboxStartMinimized);
            this.Controls.Add(this.cboxAutoRunGPS_Out);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximumSize = new System.Drawing.Size(466, 461);
            this.MinimumSize = new System.Drawing.Size(466, 461);
            this.Name = "FormAdvancedSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Advanced Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cboxAutoRunGPS_Out;
        private System.Windows.Forms.CheckBox cboxStartMinimized;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClose;
    }
}