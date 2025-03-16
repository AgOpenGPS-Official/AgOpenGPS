namespace AgOpenGPS
{
    partial class FormKeyboard
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
            this.keyboardString = new System.Windows.Forms.TextBox();
            this.keyboard1 = new Keypad.Keyboard();
            this.btnCharLeft = new System.Windows.Forms.Button();
            this.btnCharRight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // keyboardString
            // 
            this.keyboardString.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyboardString.Location = new System.Drawing.Point(11, 4);
            this.keyboardString.Name = "keyboardString";
            this.keyboardString.Size = new System.Drawing.Size(751, 46);
            this.keyboardString.TabIndex = 0;
            // 
            // keyboard1
            // 
            this.keyboard1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyboard1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.keyboard1.Location = new System.Drawing.Point(5, 55);
            this.keyboard1.Margin = new System.Windows.Forms.Padding(4);
            this.keyboard1.Name = "keyboard1";
            this.keyboard1.Size = new System.Drawing.Size(929, 395);
            this.keyboard1.TabIndex = 2;
            this.keyboard1.ButtonPressed += new System.Windows.Forms.KeyPressEventHandler(this.RegisterKeyboard1_ButtonPressed);
            // 
            // btnCharLeft
            // 
            this.btnCharLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCharLeft.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCharLeft.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCharLeft.FlatAppearance.BorderSize = 0;
            this.btnCharLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Turquoise;
            this.btnCharLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharLeft.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnCharLeft.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnCharLeft.Location = new System.Drawing.Point(777, 4);
            this.btnCharLeft.Name = "btnCharLeft";
            this.btnCharLeft.Size = new System.Drawing.Size(73, 45);
            this.btnCharLeft.TabIndex = 457;
            this.btnCharLeft.UseVisualStyleBackColor = false;
            this.btnCharLeft.Click += new System.EventHandler(this.btnCharLeft_Click);
            // 
            // btnCharRight
            // 
            this.btnCharRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCharRight.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCharRight.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnCharRight.FlatAppearance.BorderSize = 0;
            this.btnCharRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Turquoise;
            this.btnCharRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharRight.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnCharRight.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnCharRight.Location = new System.Drawing.Point(860, 4);
            this.btnCharRight.Name = "btnCharRight";
            this.btnCharRight.Size = new System.Drawing.Size(73, 45);
            this.btnCharRight.TabIndex = 458;
            this.btnCharRight.UseVisualStyleBackColor = false;
            this.btnCharRight.Click += new System.EventHandler(this.btnCharRight_Click);
            // 
            // FormKeyboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(942, 454);
            this.ControlBox = false;
            this.Controls.Add(this.btnCharRight);
            this.Controls.Add(this.btnCharLeft);
            this.Controls.Add(this.keyboard1);
            this.Controls.Add(this.keyboardString);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Location = new System.Drawing.Point(250, 250);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormKeyboard";
            this.ShowInTaskbar = false;
            this.Text = "Keyboard";
            this.Load += new System.EventHandler(this.FormKeyboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox keyboardString;
        private Keypad.Keyboard keyboard1;
        private System.Windows.Forms.Button btnCharLeft;
        private System.Windows.Forms.Button btnCharRight;
    }
}