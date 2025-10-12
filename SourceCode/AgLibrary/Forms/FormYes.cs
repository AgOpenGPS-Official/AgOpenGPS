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
            AcceptButton = btnSerialOK;
            if (showCancel) CancelButton = btnCancel;
        }
    }
}