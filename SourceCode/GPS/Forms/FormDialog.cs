using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS.Forms
{
    public partial class FormDialog : Form
    {
        private FormDialog(string message, string title, bool showCancel)
        {
            InitializeComponent();
            lblMessage.Text = message;
            lblTitle.Text = title;
            btnCancel.Visible = showCancel;
        }

        public static DialogResult Show(string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
        {
            bool showCancel = (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel);
            return new FormDialog(message, title, showCancel).ShowDialog();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            // Not used, but here for future compatibility
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Not used, but here for future compatibility
        }
    }
}

