namespace AgIO
{
    partial class FormMountpoints
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
            this.lblMountInfo = new System.Windows.Forms.Label();
            this.lvMounts = new System.Windows.Forms.ListView();
            this.lblMountStatus = new System.Windows.Forms.Label();
            this.btnMountCancel = new AgIO.RoundedButton();
            this.btnMountSelect = new AgIO.RoundedButton();
            this.SuspendLayout();
            // 
            // lblMountInfo
            // 
            this.lblMountInfo.AutoSize = false;
            this.lblMountInfo.Font = new System.Drawing.Font("Tahoma", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMountInfo.Location = new System.Drawing.Point(20, 12);
            this.lblMountInfo.Name = "lblMountInfo";
            this.lblMountInfo.Size = new System.Drawing.Size(860, 34);
            this.lblMountInfo.TabIndex = 0;
            this.lblMountInfo.Text = "Choose the base station nearest to your field.";
            // 
            // lvMounts
            // 
            this.lvMounts.FullRowSelect = true;
            this.lvMounts.HideSelection = false;
            this.lvMounts.MultiSelect = false;
            this.lvMounts.View = System.Windows.Forms.View.Details;
            this.lvMounts.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvMounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvMounts.Location = new System.Drawing.Point(20, 52);
            this.lvMounts.Name = "lvMounts";
            this.lvMounts.Size = new System.Drawing.Size(860, 430);
            this.lvMounts.TabIndex = 1;
            this.lvMounts.DoubleClick += new System.EventHandler(this.lvMounts_DoubleClick);
            // 
            // lblMountStatus
            // 
            this.lblMountStatus.AutoSize = false;
            this.lblMountStatus.Location = new System.Drawing.Point(20, 496);
            this.lblMountStatus.Name = "lblMountStatus";
            this.lblMountStatus.Size = new System.Drawing.Size(500, 60);
            this.lblMountStatus.TabIndex = 2;
            this.lblMountStatus.Text = "Loading the list...";
            // 
            // btnMountCancel
            // 
            this.btnMountCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnMountCancel.Location = new System.Drawing.Point(560, 500);
            this.btnMountCancel.Name = "btnMountCancel";
            this.btnMountCancel.Size = new System.Drawing.Size(150, 70);
            this.btnMountCancel.TabIndex = 3;
            this.btnMountCancel.Text = "Cancel";
            // 
            // btnMountSelect
            // 
            this.btnMountSelect.Tone = AgIO.ButtonTone.Positive;
            this.btnMountSelect.Location = new System.Drawing.Point(730, 500);
            this.btnMountSelect.Name = "btnMountSelect";
            this.btnMountSelect.Size = new System.Drawing.Size(150, 70);
            this.btnMountSelect.TabIndex = 4;
            this.btnMountSelect.Text = "Choose";
            this.btnMountSelect.Click += new System.EventHandler(this.btnMountSelect_Click);
            // 
            // FormMountpoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 590);
            this.Controls.Add(this.lblMountInfo);
            this.Controls.Add(this.lvMounts);
            this.Controls.Add(this.lblMountStatus);
            this.Controls.Add(this.btnMountCancel);
            this.Controls.Add(this.btnMountSelect);
            this.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AcceptButton = this.btnMountSelect;
            this.CancelButton = this.btnMountCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Name = "FormMountpoints";
            this.Text = "Mountpoints";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMountInfo;
        private System.Windows.Forms.ListView lvMounts;
        private System.Windows.Forms.Label lblMountStatus;
        private AgIO.RoundedButton btnMountCancel;
        private AgIO.RoundedButton btnMountSelect;
    }
}
