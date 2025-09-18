using System.Windows.Forms;

namespace AgIO
{
    public partial class FormYes : Form
    {
        public FormYes(string messageStr, bool showCancel = false)
        {
            InitializeComponent();

            lblMessage2.Text = messageStr;
            btnCancel.Visible = showCancel;
            btnSerialOK.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            this.AcceptButton = btnSerialOK;
            if (showCancel) this.CancelButton = btnCancel;
        }
    }
}