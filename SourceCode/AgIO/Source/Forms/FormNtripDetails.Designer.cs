namespace AgIO
{
    partial class FormNtripDetails
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
            this.checks = new AgIO.NtripChecksPanel();
            this.right = new System.Windows.Forms.Panel();
            this.list = new System.Windows.Forms.ListView();
            this.colWhat = new System.Windows.Forms.ColumnHeader();
            this.colTimes = new System.Windows.Forms.ColumnHeader();
            this.colEvery = new System.Windows.Forms.ColumnHeader();
            this.colLast = new System.Windows.Forms.ColumnHeader();
            this.rowHeight = new System.Windows.Forms.ImageList(this.components);
            this.info = new System.Windows.Forms.Label();
            this.bar = new System.Windows.Forms.Panel();
            this.btnCopy = new AgIO.RoundedButton();
            this.btnClose = new AgIO.RoundedButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.right.SuspendLayout();
            this.bar.SuspendLayout();
            this.SuspendLayout();
            //
            // checks
            //
            this.checks.BackColor = System.Drawing.Color.Transparent;
            this.checks.Dock = System.Windows.Forms.DockStyle.Left;
            this.checks.Location = new System.Drawing.Point(0, 0);
            this.checks.Name = "checks";
            this.checks.Padding = new System.Windows.Forms.Padding(8);
            this.checks.ShowDetailsButton = false;
            this.checks.Size = new System.Drawing.Size(380, 620);
            this.checks.TabIndex = 0;
            //
            // right
            //
            this.right.Controls.Add(this.list);
            this.right.Controls.Add(this.info);
            this.right.Controls.Add(this.bar);
            this.right.Dock = System.Windows.Forms.DockStyle.Fill;
            this.right.Location = new System.Drawing.Point(380, 0);
            this.right.Name = "right";
            this.right.Padding = new System.Windows.Forms.Padding(8);
            this.right.Size = new System.Drawing.Size(620, 620);
            this.right.TabIndex = 1;
            //
            // list
            //
            this.list.BackColor = System.Drawing.Color.White;
            this.list.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWhat,
            this.colTimes,
            this.colEvery,
            this.colLast});
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FullRowSelect = true;
            this.list.GridLines = true;
            this.list.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.list.HideSelection = false;
            this.list.Location = new System.Drawing.Point(8, 100);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(544, 396);
            this.list.SmallImageList = this.rowHeight;
            this.list.TabIndex = 0;
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            //
            // colWhat
            //
            this.colWhat.Text = "What came in";
            this.colWhat.Width = 300;
            //
            // colTimes
            //
            this.colTimes.Text = "Times";
            this.colTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colTimes.Width = 70;
            //
            // colEvery
            //
            this.colEvery.Text = "How often";
            this.colEvery.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colEvery.Width = 100;
            //
            // colLast
            //
            this.colLast.Text = "Last seen";
            this.colLast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLast.Width = 90;
            //
            // rowHeight
            //
            this.rowHeight.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.rowHeight.ImageSize = new System.Drawing.Size(1, 40);
            this.rowHeight.TransparentColor = System.Drawing.Color.Transparent;
            //
            // info
            //
            this.info.Dock = System.Windows.Forms.DockStyle.Top;
            this.info.Location = new System.Drawing.Point(8, 8);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(544, 132);
            this.info.TabIndex = 1;
            this.info.Text = "info";
            //
            // bar
            //
            this.bar.Controls.Add(this.btnCopy);
            this.bar.Controls.Add(this.btnClose);
            this.bar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar.Location = new System.Drawing.Point(8, 496);
            this.bar.Name = "bar";
            this.bar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.bar.Size = new System.Drawing.Size(544, 76);
            this.bar.TabIndex = 2;
            //
            // btnCopy
            //
            this.btnCopy.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCopy.Font = new System.Drawing.Font("Tahoma", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(0, 8);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(250, 68);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.Text = "Copy for support";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            //
            // btnClose
            //
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(364, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 68);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            //
            // timer
            //
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // FormNtripDetails
            //
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1000, 620);
            this.Controls.Add(this.right);
            this.Controls.Add(this.checks);
            this.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNtripDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NTRIP details";
            this.Load += new System.EventHandler(this.FormNtripDetails_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormNtripDetails_FormClosed);
            this.right.ResumeLayout(false);
            this.bar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AgIO.NtripChecksPanel checks;
        private System.Windows.Forms.Panel right;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.ColumnHeader colWhat;
        private System.Windows.Forms.ColumnHeader colTimes;
        private System.Windows.Forms.ColumnHeader colEvery;
        private System.Windows.Forms.ColumnHeader colLast;
        private System.Windows.Forms.ImageList rowHeight;
        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Panel bar;
        private AgIO.RoundedButton btnCopy;
        private AgIO.RoundedButton btnClose;
        private System.Windows.Forms.Timer timer;
    }
}
