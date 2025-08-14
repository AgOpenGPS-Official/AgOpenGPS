namespace AgIO
{
    partial class FormNumeric
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
            this.tboxNumber = new System.Windows.Forms.TextBox();
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.keypad1 = new Keypad.NumKeypad();
            this.btnDistanceUp = new AgLibrary.Controls.RepeatButton();
            this.btnDistanceDn = new AgLibrary.Controls.RepeatButton();
            this.SuspendLayout();
            // 
            // tboxNumber
            // 
            this.tboxNumber.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxNumber.Location = new System.Drawing.Point(208, 7);
            this.tboxNumber.Name = "tboxNumber";
            this.tboxNumber.ReadOnly = true;
            this.tboxNumber.Size = new System.Drawing.Size(246, 52);
            this.tboxNumber.TabIndex = 1;
            this.tboxNumber.Text = "234.5643";
            this.tboxNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblMax
            // 
            this.lblMax.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMax.Location = new System.Drawing.Point(332, 62);
            this.lblMax.Name = "lblMax";
            this.lblMax.Size = new System.Drawing.Size(125, 36);
            this.lblMax.TabIndex = 6;
            this.lblMax.Text = "88.8";
            this.lblMax.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblMin
            // 
            this.lblMin.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMin.Location = new System.Drawing.Point(205, 62);
            this.lblMin.Name = "lblMin";
            this.lblMin.Size = new System.Drawing.Size(125, 36);
            this.lblMin.TabIndex = 7;
            this.lblMin.Text = "-22.8";
            this.lblMin.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // keypad1
            // 
            this.keypad1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.keypad1.Location = new System.Drawing.Point(0, 112);
            this.keypad1.Name = "keypad1";
            this.keypad1.Size = new System.Drawing.Size(454, 429);
            this.keypad1.TabIndex = 5;
            this.keypad1.ButtonPressed += new System.Windows.Forms.KeyPressEventHandler(this.RegisterKeypad1_ButtonPressed);
            // 
            // btnDistanceUp
            // 
            this.btnDistanceUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnDistanceUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDistanceUp.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnDistanceUp.FlatAppearance.BorderSize = 2;
            this.btnDistanceUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDistanceUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistanceUp.Image = global::AgIO.Properties.Resources.UpArrow64;
            this.btnDistanceUp.Location = new System.Drawing.Point(112, 3);
            this.btnDistanceUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDistanceUp.Name = "btnDistanceUp";
            this.btnDistanceUp.Size = new System.Drawing.Size(91, 92);
            this.btnDistanceUp.TabIndex = 148;
            this.btnDistanceUp.UseVisualStyleBackColor = false;
            this.btnDistanceUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDistanceUp_MouseDown);
            // 
            // btnDistanceDn
            // 
            this.btnDistanceDn.BackColor = System.Drawing.SystemColors.Control;
            this.btnDistanceDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDistanceDn.FlatAppearance.BorderColor = System.Drawing.Color.Blue;
            this.btnDistanceDn.FlatAppearance.BorderSize = 2;
            this.btnDistanceDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDistanceDn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistanceDn.Image = global::AgIO.Properties.Resources.DnArrow64;
            this.btnDistanceDn.Location = new System.Drawing.Point(4, 3);
            this.btnDistanceDn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDistanceDn.Name = "btnDistanceDn";
            this.btnDistanceDn.Size = new System.Drawing.Size(91, 92);
            this.btnDistanceDn.TabIndex = 147;
            this.btnDistanceDn.UseVisualStyleBackColor = false;
            this.btnDistanceDn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDistanceDn_MouseDown);
            // 
            // FormNumeric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(460, 545);
            this.ControlBox = false;
            this.Controls.Add(this.btnDistanceUp);
            this.Controls.Add(this.btnDistanceDn);
            this.Controls.Add(this.tboxNumber);
            this.Controls.Add(this.keypad1);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.lblMax);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNumeric";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter a Value";
            this.Load += new System.EventHandler(this.FormNumeric_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tboxNumber;
        private Keypad.NumKeypad keypad1;
        private System.Windows.Forms.Label lblMax;
        private System.Windows.Forms.Label lblMin;
        private AgLibrary.Controls.RepeatButton btnDistanceUp;
        private AgLibrary.Controls.RepeatButton btnDistanceDn;
    }
}