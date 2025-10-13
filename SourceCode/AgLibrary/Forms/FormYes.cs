using System.Windows.Forms;

namespace AgLibrary.Forms
{
    public partial class FormYes : Form
    {
        public FormYes(string messageStr, bool showCancel = false, string title = null)
        {
            InitializeComponent();
            lblMessage2.Text = messageStr;
            btnCancel.Visible = showCancel;

            // Show/hide title label based on whether title is provided
            if (!string.IsNullOrEmpty(title))
            {
                lblTitle.Text = title;
                lblTitle.Visible = true;
            }
            else
            {
                lblTitle.Visible = false;
            }

            AcceptButton = btnSerialOK;
            if (showCancel) CancelButton = btnCancel;
        }

        // Static Show method for compatibility with FormDialog usage
        public static DialogResult Show(string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
        {
            bool showCancel = (buttons == MessageBoxButtons.OKCancel ||
                             buttons == MessageBoxButtons.YesNo ||
                             buttons == MessageBoxButtons.YesNoCancel);

            using (var form = new FormYes(message, showCancel, title))
            {
                return form.ShowDialog();
            }
        }
    }
}