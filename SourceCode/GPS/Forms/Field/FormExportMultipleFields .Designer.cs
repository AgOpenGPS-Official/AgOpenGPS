
    partial class FormExportMultipleFields
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
            this.listBoxAvailable = new System.Windows.Forms.ListBox();
            this.listBoxSelected = new System.Windows.Forms.ListBox();
            this.btnAllToAvailable = new System.Windows.Forms.Button();
            this.btnOneToAvailable = new System.Windows.Forms.Button();
            this.btnAllToSelected = new System.Windows.Forms.Button();
            this.btnOneToSelected = new System.Windows.Forms.Button();
            this.lblAvailable = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExportIsoXml = new System.Windows.Forms.Button();
            this.btnExportAgShare = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxAvailable
            // 
            this.listBoxAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxAvailable.FormattingEnabled = true;
            this.listBoxAvailable.ItemHeight = 29;
            this.listBoxAvailable.Location = new System.Drawing.Point(12, 45);
            this.listBoxAvailable.Name = "listBoxAvailable";
            this.listBoxAvailable.Size = new System.Drawing.Size(848, 236);
            this.listBoxAvailable.TabIndex = 0;
            // 
            // listBoxSelected
            // 
            this.listBoxSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxSelected.FormattingEnabled = true;
            this.listBoxSelected.ItemHeight = 29;
            this.listBoxSelected.Location = new System.Drawing.Point(12, 420);
            this.listBoxSelected.Name = "listBoxSelected";
            this.listBoxSelected.Size = new System.Drawing.Size(848, 236);
            this.listBoxSelected.TabIndex = 1;
            // 
            // btnAllToAvailable
            // 
            this.btnAllToAvailable.BackColor = System.Drawing.Color.Transparent;
            this.btnAllToAvailable.FlatAppearance.BorderSize = 0;
            this.btnAllToAvailable.Image = global::AgOpenGPS.Properties.Resources.TripleUpArrows;
            this.btnAllToAvailable.Location = new System.Drawing.Point(575, 318);
            this.btnAllToAvailable.Name = "btnAllToAvailable";
            this.btnAllToAvailable.Size = new System.Drawing.Size(96, 96);
            this.btnAllToAvailable.TabIndex = 4;
            this.btnAllToAvailable.UseVisualStyleBackColor = false;
            // 
            // btnOneToAvailable
            // 
            this.btnOneToAvailable.BackColor = System.Drawing.Color.Transparent;
            this.btnOneToAvailable.FlatAppearance.BorderSize = 0;
            this.btnOneToAvailable.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnOneToAvailable.Location = new System.Drawing.Point(736, 318);
            this.btnOneToAvailable.Name = "btnOneToAvailable";
            this.btnOneToAvailable.Size = new System.Drawing.Size(96, 96);
            this.btnOneToAvailable.TabIndex = 5;
            this.btnOneToAvailable.UseVisualStyleBackColor = false;
            // 
            // btnAllToSelected
            // 
            this.btnAllToSelected.BackColor = System.Drawing.Color.Transparent;
            this.btnAllToSelected.FlatAppearance.BorderSize = 0;
            this.btnAllToSelected.Image = global::AgOpenGPS.Properties.Resources.TripleDownArrows;
            this.btnAllToSelected.Location = new System.Drawing.Point(410, 318);
            this.btnAllToSelected.Name = "btnAllToSelected";
            this.btnAllToSelected.Size = new System.Drawing.Size(96, 96);
            this.btnAllToSelected.TabIndex = 3;
            this.btnAllToSelected.UseVisualStyleBackColor = false;
            // 
            // btnOneToSelected
            // 
            this.btnOneToSelected.BackColor = System.Drawing.Color.Transparent;
            this.btnOneToSelected.FlatAppearance.BorderSize = 0;
            this.btnOneToSelected.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnOneToSelected.Location = new System.Drawing.Point(249, 318);
            this.btnOneToSelected.Name = "btnOneToSelected";
            this.btnOneToSelected.Size = new System.Drawing.Size(96, 96);
            this.btnOneToSelected.TabIndex = 2;
            this.btnOneToSelected.UseVisualStyleBackColor = false;
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoSize = true;
            this.lblAvailable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailable.Location = new System.Drawing.Point(13, 13);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(180, 25);
            this.lblAvailable.TabIndex = 6;
            this.lblAvailable.Text = "Available Fields";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 387);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Selected Fields";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(908, 620);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 64);
            this.btnClose.TabIndex = 8;
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // btnExportIsoXml
            // 
            this.btnExportIsoXml.BackColor = System.Drawing.Color.Transparent;
            this.btnExportIsoXml.FlatAppearance.BorderSize = 0;
            this.btnExportIsoXml.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportIsoXml.Image = global::AgOpenGPS.Properties.Resources.ISOXML;
            this.btnExportIsoXml.Location = new System.Drawing.Point(908, 494);
            this.btnExportIsoXml.Name = "btnExportIsoXml";
            this.btnExportIsoXml.Size = new System.Drawing.Size(64, 64);
            this.btnExportIsoXml.TabIndex = 9;
            this.btnExportIsoXml.UseVisualStyleBackColor = false;
            // 
            // btnExportAgShare
            // 
            this.btnExportAgShare.BackColor = System.Drawing.Color.Transparent;
            this.btnExportAgShare.FlatAppearance.BorderSize = 0;
            this.btnExportAgShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportAgShare.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnExportAgShare.Location = new System.Drawing.Point(908, 370);
            this.btnExportAgShare.Name = "btnExportAgShare";
            this.btnExportAgShare.Size = new System.Drawing.Size(64, 64);
            this.btnExportAgShare.TabIndex = 10;
            this.btnExportAgShare.UseVisualStyleBackColor = false;
            // 
            // FormExportMultipleFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.btnExportAgShare);
            this.Controls.Add(this.btnExportIsoXml);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblAvailable);
            this.Controls.Add(this.btnOneToAvailable);
            this.Controls.Add(this.btnAllToAvailable);
            this.Controls.Add(this.btnAllToSelected);
            this.Controls.Add(this.btnOneToSelected);
            this.Controls.Add(this.listBoxSelected);
            this.Controls.Add(this.listBoxAvailable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormExportMultipleFields";
            this.Text = "FormFieldExporter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxAvailable;
        private System.Windows.Forms.ListBox listBoxSelected;
        private System.Windows.Forms.Button btnOneToSelected;
        private System.Windows.Forms.Button btnAllToSelected;
        private System.Windows.Forms.Button btnOneToAvailable;
        private System.Windows.Forms.Button btnAllToAvailable;
        private System.Windows.Forms.Label lblAvailable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnExportIsoXml;
    private System.Windows.Forms.Button btnExportAgShare;
}
