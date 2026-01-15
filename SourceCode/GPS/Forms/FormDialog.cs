using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;

namespace AgOpenGPS.Forms
{
    public enum DialogSeverity
    {
        Error,
        Warning,
        Info
    }

    public partial class FormDialog : Form
    {
        private static readonly Color ErrorBorderColor = Color.FromArgb(192, 0, 0);
        private static readonly Color ErrorBackgroundColor = Color.FromArgb(255, 192, 192);
        private static readonly Color WarningBorderColor = Color.FromArgb(192, 145, 0);
        private static readonly Color WarningBackgroundColor = Color.FromArgb(227, 217, 152);
        private static readonly Color InfoBorderColor = Color.FromArgb(0, 0, 192);
        private static readonly Color InfoBackgroundColor = Color.FromArgb(192, 192, 255);

        private Color _borderColor = Color.CornflowerBlue;

        private FormDialog(string title, string message, bool showCancel, DialogSeverity? severity)
        {
            InitializeComponent();

            if (severity.HasValue)
            {
                SetSeverity(severity.Value);
            }

            labelTitle.Text = title;
            labelMessage.Text = message;
            buttonCancel.Visible = showCancel;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            var borderPen = new Pen(_borderColor, 20);
            e.Graphics.DrawRectangle(borderPen, 0, 0, Width, Height);
        }

        private void SetSeverity(DialogSeverity severity)
        {
            switch (severity)
            {
                case DialogSeverity.Error:
                    _borderColor = ErrorBorderColor;
                    BackColor = ErrorBackgroundColor;
                    pictureBoxIcon.Image = Resources.Error;
                    break;
                case DialogSeverity.Warning:
                    _borderColor = WarningBorderColor;
                    BackColor = WarningBackgroundColor;
                    pictureBoxIcon.Image = Resources.Warning;
                    break;
                case DialogSeverity.Info:
                    _borderColor = InfoBorderColor;
                    BackColor = InfoBackgroundColor;
                    pictureBoxIcon.Image = Resources.Info;
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(severity), (int)severity, typeof(DialogSeverity));
            }

            pictureBoxIcon.Visible = true;
        }

        public static void Show(string title, string message, DialogSeverity? severity = null)
        {
            using (var form = new FormDialog(title, message, showCancel: false, severity))
            {
                form.ShowDialog();
            }
        }

        public static DialogResult ShowQuestion(string title, string message, DialogSeverity? severity = null)
        {
            using (var form = new FormDialog(title, message, showCancel: true, severity))
            {
                return form.ShowDialog();
            }
        }
    }
}

