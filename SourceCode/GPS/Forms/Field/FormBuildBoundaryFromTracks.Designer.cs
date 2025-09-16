using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS.Forms.Field
{
    partial class FormBuildBoundaryFromTracks
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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (var pen = new Pen(Color.DarkGray, 15))
            {
                var rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                e.Graphics.DrawRectangle(pen, rect);
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControlPreview = new OpenTK.GLControl.GLControl();
            this.flpTrackList = new System.Windows.Forms.FlowLayoutPanel();
            this.lblList = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnShrinkB = new System.Windows.Forms.Button();
            this.btnShrinkA = new System.Windows.Forms.Button();
            this.btnExtendForward = new System.Windows.Forms.Button();
            this.btnExtendBackward = new System.Windows.Forms.Button();
            this.btnSelectNext = new System.Windows.Forms.Button();
            this.btnSelectPrevious = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnBuildBoundary = new System.Windows.Forms.Button();
            this.btnResetPreview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // glControlPreview
            // 
            this.glControlPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControlPreview.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControlPreview.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControlPreview.BackColor = System.Drawing.Color.Black;
            this.glControlPreview.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControlPreview.IsEventDriven = true;
            this.glControlPreview.Location = new System.Drawing.Point(8, 8);
            this.glControlPreview.Name = "glControlPreview";
            this.glControlPreview.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControlPreview.SharedContext = null;
            this.glControlPreview.Size = new System.Drawing.Size(565, 715);
            this.glControlPreview.TabIndex = 0;
            this.glControlPreview.Load += new System.EventHandler(this.glControlPreview_Load);
            this.glControlPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.glControlPreview_Paint);
            this.glControlPreview.Resize += new System.EventHandler(this.glControlPreview_Resize);
            // 
            // flpTrackList
            // 
            this.flpTrackList.AutoScroll = true;
            this.flpTrackList.BackColor = System.Drawing.Color.Transparent;
            this.flpTrackList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flpTrackList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTrackList.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flpTrackList.Location = new System.Drawing.Point(588, 38);
            this.flpTrackList.Name = "flpTrackList";
            this.flpTrackList.Size = new System.Drawing.Size(399, 370);
            this.flpTrackList.TabIndex = 12;
            this.flpTrackList.WrapContents = false;
            // 
            // lblList
            // 
            this.lblList.AutoSize = true;
            this.lblList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblList.Location = new System.Drawing.Point(583, 10);
            this.lblList.Name = "lblList";
            this.lblList.Size = new System.Drawing.Size(273, 25);
            this.lblList.TabIndex = 13;
            this.lblList.Text = "(De)Select Tracks to use";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnSave.Location = new System.Drawing.Point(800, 623);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 95);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnShrinkB
            // 
            this.btnShrinkB.BackColor = System.Drawing.Color.Transparent;
            this.btnShrinkB.FlatAppearance.BorderSize = 0;
            this.btnShrinkB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShrinkB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShrinkB.Image = global::AgOpenGPS.Properties.Resources.APlusMinusB;
            this.btnShrinkB.Location = new System.Drawing.Point(798, 511);
            this.btnShrinkB.Name = "btnShrinkB";
            this.btnShrinkB.Size = new System.Drawing.Size(95, 95);
            this.btnShrinkB.TabIndex = 10;
            this.btnShrinkB.Text = "Shrink B";
            this.btnShrinkB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShrinkB.UseVisualStyleBackColor = false;
            this.btnShrinkB.Click += new System.EventHandler(this.btnShrinkB_Click);
            // 
            // btnShrinkA
            // 
            this.btnShrinkA.BackColor = System.Drawing.Color.Transparent;
            this.btnShrinkA.FlatAppearance.BorderSize = 0;
            this.btnShrinkA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShrinkA.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShrinkA.Image = global::AgOpenGPS.Properties.Resources.APlusMinusA;
            this.btnShrinkA.Location = new System.Drawing.Point(686, 511);
            this.btnShrinkA.Name = "btnShrinkA";
            this.btnShrinkA.Size = new System.Drawing.Size(95, 95);
            this.btnShrinkA.TabIndex = 9;
            this.btnShrinkA.Text = "Shrink A";
            this.btnShrinkA.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnShrinkA.UseVisualStyleBackColor = false;
            this.btnShrinkA.Click += new System.EventHandler(this.btnShrinkA_Click);
            // 
            // btnExtendForward
            // 
            this.btnExtendForward.BackColor = System.Drawing.Color.Transparent;
            this.btnExtendForward.FlatAppearance.BorderSize = 0;
            this.btnExtendForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtendForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtendForward.Image = global::AgOpenGPS.Properties.Resources.APlusPlusB;
            this.btnExtendForward.Location = new System.Drawing.Point(899, 511);
            this.btnExtendForward.Name = "btnExtendForward";
            this.btnExtendForward.Size = new System.Drawing.Size(95, 95);
            this.btnExtendForward.TabIndex = 8;
            this.btnExtendForward.Text = "Extend B";
            this.btnExtendForward.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExtendForward.UseVisualStyleBackColor = false;
            this.btnExtendForward.Click += new System.EventHandler(this.btnExtendForward_Click);
            // 
            // btnExtendBackward
            // 
            this.btnExtendBackward.BackColor = System.Drawing.Color.Transparent;
            this.btnExtendBackward.FlatAppearance.BorderSize = 0;
            this.btnExtendBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtendBackward.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExtendBackward.Image = global::AgOpenGPS.Properties.Resources.APlusPlusA;
            this.btnExtendBackward.Location = new System.Drawing.Point(576, 511);
            this.btnExtendBackward.Name = "btnExtendBackward";
            this.btnExtendBackward.Size = new System.Drawing.Size(95, 95);
            this.btnExtendBackward.TabIndex = 7;
            this.btnExtendBackward.Text = "Extend A";
            this.btnExtendBackward.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExtendBackward.UseVisualStyleBackColor = false;
            this.btnExtendBackward.Click += new System.EventHandler(this.btnExtendBackward_Click);
            // 
            // btnSelectNext
            // 
            this.btnSelectNext.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectNext.FlatAppearance.BorderSize = 0;
            this.btnSelectNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectNext.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnSelectNext.Location = new System.Drawing.Point(798, 409);
            this.btnSelectNext.Name = "btnSelectNext";
            this.btnSelectNext.Size = new System.Drawing.Size(95, 95);
            this.btnSelectNext.TabIndex = 6;
            this.btnSelectNext.Text = "Next";
            this.btnSelectNext.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectNext.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectNext.UseVisualStyleBackColor = false;
            this.btnSelectNext.Click += new System.EventHandler(this.btnSelectNext_Click);
            // 
            // btnSelectPrevious
            // 
            this.btnSelectPrevious.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectPrevious.FlatAppearance.BorderSize = 0;
            this.btnSelectPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectPrevious.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnSelectPrevious.Location = new System.Drawing.Point(686, 409);
            this.btnSelectPrevious.Name = "btnSelectPrevious";
            this.btnSelectPrevious.Size = new System.Drawing.Size(95, 95);
            this.btnSelectPrevious.TabIndex = 5;
            this.btnSelectPrevious.Text = "Previous";
            this.btnSelectPrevious.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectPrevious.UseVisualStyleBackColor = false;
            this.btnSelectPrevious.Click += new System.EventHandler(this.btnSelectPrevious_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(901, 623);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 95);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnBuildBoundary
            // 
            this.btnBuildBoundary.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildBoundary.FlatAppearance.BorderSize = 0;
            this.btnBuildBoundary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildBoundary.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuildBoundary.Image = global::AgOpenGPS.Properties.Resources.Boundary;
            this.btnBuildBoundary.Location = new System.Drawing.Point(688, 624);
            this.btnBuildBoundary.Name = "btnBuildBoundary";
            this.btnBuildBoundary.Size = new System.Drawing.Size(95, 95);
            this.btnBuildBoundary.TabIndex = 3;
            this.btnBuildBoundary.Text = "Build";
            this.btnBuildBoundary.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBuildBoundary.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnBuildBoundary.UseVisualStyleBackColor = false;
            this.btnBuildBoundary.Click += new System.EventHandler(this.btnBuildBoundary_Click);
            // 
            // btnResetPreview
            // 
            this.btnResetPreview.BackColor = System.Drawing.Color.Transparent;
            this.btnResetPreview.FlatAppearance.BorderSize = 0;
            this.btnResetPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetPreview.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetPreview.Image = global::AgOpenGPS.Properties.Resources.Reset_Default;
            this.btnResetPreview.Location = new System.Drawing.Point(578, 623);
            this.btnResetPreview.Name = "btnResetPreview";
            this.btnResetPreview.Size = new System.Drawing.Size(95, 95);
            this.btnResetPreview.TabIndex = 2;
            this.btnResetPreview.Text = "Reset";
            this.btnResetPreview.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnResetPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnResetPreview.UseVisualStyleBackColor = false;
            this.btnResetPreview.Click += new System.EventHandler(this.btnResetPreview_Click);
            // 
            // FormBuildBoundaryFromTracks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.ControlBox = false;
            this.Controls.Add(this.lblList);
            this.Controls.Add(this.flpTrackList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnShrinkB);
            this.Controls.Add(this.btnShrinkA);
            this.Controls.Add(this.btnExtendForward);
            this.Controls.Add(this.btnExtendBackward);
            this.Controls.Add(this.btnSelectNext);
            this.Controls.Add(this.btnSelectPrevious);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBuildBoundary);
            this.Controls.Add(this.btnResetPreview);
            this.Controls.Add(this.glControlPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormBuildBoundaryFromTracks";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Build Boundary From Tracks";
            this.Load += new System.EventHandler(this.FormBuildBoundaryFromTracks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenTK.GLControl.GLControl glControlPreview;
        private System.Windows.Forms.Button btnResetPreview;
        private System.Windows.Forms.Button btnBuildBoundary;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSelectPrevious;
        private System.Windows.Forms.Button btnSelectNext;
        private System.Windows.Forms.Button btnExtendBackward;
        private System.Windows.Forms.Button btnExtendForward;
        private System.Windows.Forms.Button btnShrinkA;
        private System.Windows.Forms.Button btnShrinkB;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.FlowLayoutPanel flpTrackList;
        private System.Windows.Forms.Label lblList;
    }
}