namespace AgOpenGPS
{
    partial class FormShiftPos
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
            this.nudNorth = new AgOpenGPS.NudlessNumericUpDown();
            this.nudEast = new AgOpenGPS.NudlessNumericUpDown();
            this.label27 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSouth = new AgLibrary.Controls.RepeatButton();
            this.btnNorth = new AgLibrary.Controls.RepeatButton();
            this.btnEast = new AgLibrary.Controls.RepeatButton();
            this.btnWest = new AgLibrary.Controls.RepeatButton();
            this.bntOK = new System.Windows.Forms.Button();
            this.btnZero = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkOffsetsOn = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudNorth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEast)).BeginInit();
            this.SuspendLayout();
            // 
            // nudNorth
            // 
            this.nudNorth.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold);
            this.nudNorth.Location = new System.Drawing.Point(61, 256);
            this.nudNorth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudNorth.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudNorth.Name = "nudNorth";
            this.nudNorth.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudNorth.Size = new System.Drawing.Size(185, 65);
            this.nudNorth.TabIndex = 154;
            this.nudNorth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNorth.ThousandsSeparator = true;
            this.nudNorth.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudNorth.Value = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudNorth.Click += new System.EventHandler(this.nudNorth_Click);
            // 
            // nudEast
            // 
            this.nudEast.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold);
            this.nudEast.Location = new System.Drawing.Point(387, 256);
            this.nudEast.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudEast.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.nudEast.Name = "nudEast";
            this.nudEast.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudEast.Size = new System.Drawing.Size(185, 65);
            this.nudEast.TabIndex = 155;
            this.nudEast.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudEast.ThousandsSeparator = true;
            this.nudEast.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.nudEast.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudEast.Click += new System.EventHandler(this.nudEast_Click);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label27.Location = new System.Drawing.Point(92, 148);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(115, 25);
            this.label27.TabIndex = 156;
            this.label27.Text = "North (+)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(429, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 25);
            this.label2.TabIndex = 158;
            this.label2.Text = "West (-)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(431, 410);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 25);
            this.label3.TabIndex = 159;
            this.label3.Text = "East (+)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(91, 410);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 25);
            this.label4.TabIndex = 160;
            this.label4.Text = "South (-)";
            // 
            // btnSouth
            // 
            this.btnSouth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSouth.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnSouth.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnSouth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSouth.Location = new System.Drawing.Point(103, 335);
            this.btnSouth.Name = "btnSouth";
            this.btnSouth.Size = new System.Drawing.Size(72, 72);
            this.btnSouth.TabIndex = 192;
            this.btnSouth.UseVisualStyleBackColor = true;
            this.btnSouth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSouth_MouseDown);
            // 
            // btnNorth
            // 
            this.btnNorth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNorth.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnNorth.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnNorth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNorth.Location = new System.Drawing.Point(103, 174);
            this.btnNorth.Name = "btnNorth";
            this.btnNorth.Size = new System.Drawing.Size(72, 72);
            this.btnNorth.TabIndex = 193;
            this.btnNorth.UseVisualStyleBackColor = true;
            this.btnNorth.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnNorth_MouseDown);
            // 
            // btnEast
            // 
            this.btnEast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEast.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnEast.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnEast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEast.Location = new System.Drawing.Point(438, 335);
            this.btnEast.Name = "btnEast";
            this.btnEast.Size = new System.Drawing.Size(72, 72);
            this.btnEast.TabIndex = 194;
            this.btnEast.UseVisualStyleBackColor = true;
            this.btnEast.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnEast_MouseDown);
            // 
            // btnWest
            // 
            this.btnWest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWest.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnWest.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnWest.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnWest.Location = new System.Drawing.Point(438, 174);
            this.btnWest.Name = "btnWest";
            this.btnWest.Size = new System.Drawing.Size(72, 72);
            this.btnWest.TabIndex = 195;
            this.btnWest.UseVisualStyleBackColor = true;
            this.btnWest.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnWest_MouseDown);
            // 
            // bntOK
            // 
            this.bntOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(502, 596);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(144, 66);
            this.bntOK.TabIndex = 196;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // btnZero
            // 
            this.btnZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZero.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZero.Location = new System.Drawing.Point(25, 596);
            this.btnZero.Name = "btnZero";
            this.btnZero.Size = new System.Drawing.Size(93, 66);
            this.btnZero.TabIndex = 227;
            this.btnZero.Text = "> 0 <";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(249, 279);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 33);
            this.label1.TabIndex = 228;
            this.label1.Text = "CM";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(574, 279);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 33);
            this.label5.TabIndex = 229;
            this.label5.Text = "CM";
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(70, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(518, 114);
            this.label6.TabIndex = 230;
            this.label6.Text = "This offset distance is added to Lat and Lon and saved in FIELD.KML file when clo" +
    "sing the field.";
            // 
            // chkOffsetsOn
            // 
            this.chkOffsetsOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkOffsetsOn.BackColor = System.Drawing.Color.AliceBlue;
            this.chkOffsetsOn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkOffsetsOn.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkOffsetsOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkOffsetsOn.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOffsetsOn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkOffsetsOn.Location = new System.Drawing.Point(93, 468);
            this.chkOffsetsOn.Name = "chkOffsetsOn";
            this.chkOffsetsOn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkOffsetsOn.Size = new System.Drawing.Size(109, 78);
            this.chkOffsetsOn.TabIndex = 491;
            this.chkOffsetsOn.Text = "Off";
            this.chkOffsetsOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkOffsetsOn.UseVisualStyleBackColor = false;
            this.chkOffsetsOn.Click += new System.EventHandler(this.chkOffsetsOn_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(208, 475);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(338, 71);
            this.label7.TabIndex = 492;
            this.label7.Text = "Keep offset even after field is closed?";
            // 
            // FormShiftPos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 674);
            this.ControlBox = false;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.chkOffsetsOn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.bntOK);
            this.Controls.Add(this.btnEast);
            this.Controls.Add(this.btnWest);
            this.Controls.Add(this.btnSouth);
            this.Controls.Add(this.btnNorth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.nudEast);
            this.Controls.Add(this.nudNorth);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShiftPos";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Shift GPS Position (cm)";
            this.Load += new System.EventHandler(this.FormShiftPos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudNorth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NudlessNumericUpDown nudNorth;
        private NudlessNumericUpDown nudEast;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private AgLibrary.Controls.RepeatButton btnSouth;
        private AgLibrary.Controls.RepeatButton btnNorth;
        private AgLibrary.Controls.RepeatButton btnEast;
        private AgLibrary.Controls.RepeatButton btnWest;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkOffsetsOn;
        private System.Windows.Forms.Label label7;
    }
}