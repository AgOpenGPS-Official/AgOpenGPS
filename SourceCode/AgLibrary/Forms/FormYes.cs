using System.Windows.Forms;

namespace AgLibrary.Forms
{
    public partial class FormYes : Form
    {
        public FormYes(string messageStr, bool showCancel = false)
        {
            InitializeComponent();

            lblMessage2.Text = messageStr;
            btnCancel.Visible = showCancel;

            // Dynamic width based on message length
            int messWidth = messageStr.Length;
            Width = messWidth * 15 + 180;

            this.AcceptButton = btnSerialOK;
            if (showCancel) this.CancelButton = btnCancel;
        }
    }
}