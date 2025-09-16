namespace AgOpenGPS
{
    partial class FormGrid
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
            this.oglSelf = new OpenTK.GLControl.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAlignToTrack = new System.Windows.Forms.Button();
            this.tlp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.oglSelf.APIVersion = new System.Version(3, 3, 0, 0);
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.oglSelf.IsEventDriven = true;
            this.oglSelf.Location = new System.Drawing.Point(2, 2);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.oglSelf.SharedContext = null;
            this.oglSelf.Size = new System.Drawing.Size(500, 500);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.Load += new System.EventHandler(this.oglSelf_Load);
            this.oglSelf.Paint += new System.Windows.Forms.PaintEventHandler(this.oglSelf_Paint);
            this.oglSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglSelf_MouseDown);
            this.oglSelf.Resize += new System.EventHandler(this.oglSelf_Resize);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp1.ColumnCount = 1;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp1.Controls.Add(this.btnAlignToTrack, 0, 1);
            this.tlp1.Controls.Add(this.btnCancelTouch, 0, 0);
            this.tlp1.Controls.Add(this.btnExit, 0, 3);
            this.tlp1.Controls.Add(this.btnCancel, 0, 2);
            this.tlp1.Location = new System.Drawing.Point(505, 2);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 4;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.tlp1.Size = new System.Drawing.Size(99, 495);
            this.tlp1.TabIndex = 564;
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelTouch.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelTouch.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancelTouch.FlatAppearance.BorderSize = 0;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(22, 36);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(54, 56);
            this.btnCancelTouch.TabIndex = 470;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(22, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 56);
            this.btnCancel.TabIndex = 469;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.Location = new System.Drawing.Point(22, 411);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(54, 56);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAlignToTrack
            // 
            this.btnAlignToTrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAlignToTrack.BackColor = System.Drawing.Color.Transparent;
            this.btnAlignToTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAlignToTrack.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAlignToTrack.FlatAppearance.BorderSize = 0;
            this.btnAlignToTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAlignToTrack.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAlignToTrack.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.btnAlignToTrack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAlignToTrack.Location = new System.Drawing.Point(22, 151);
            this.btnAlignToTrack.Name = "btnAlignToTrack";
            this.btnAlignToTrack.Size = new System.Drawing.Size(54, 81);
            this.btnAlignToTrack.TabIndex = 471;
            this.btnAlignToTrack.UseVisualStyleBackColor = false;
            this.btnAlignToTrack.Click += new System.EventHandler(this.btnAlignToTrack_Click);
            // 
            // FormGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(605, 501);
            this.ControlBox = false;
            this.Controls.Add(this.oglSelf);
            this.Controls.Add(this.tlp1);
            this.ForeColor = System.Drawing.Color.Black;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGrid";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "2 Points - Align Grid";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormABDraw_FormClosing);
            this.Load += new System.EventHandler(this.FormABDraw_Load);
            this.ResizeEnd += new System.EventHandler(this.FormABDraw_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.TableLayoutPanel tlp1;
        private System.Windows.Forms.Button btnAlignToTrack;
    }
}