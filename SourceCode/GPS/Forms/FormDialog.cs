using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.Forms
{
    public partial class FormDialog : Form
    {
        private FormDialog(string message, string title, bool showCancel)
        {
            InitializeComponent();

            labelTitle.Text = title;
            labelMessage.Text = message;
            buttonCancel.Visible = showCancel;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var borderPen = new Pen(Color.FromArgb(192, 0, 0), 20);
            e.Graphics.DrawRectangle(borderPen, 0, 0, Width, Height);
        }

        public static DialogResult Show(string title, string message, MessageBoxButtons buttons = MessageBoxButtons.OKCancel)
        {
            bool showCancel = (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel);
            return new FormDialog(message, title, showCancel).ShowDialog();
        }
    }
}

