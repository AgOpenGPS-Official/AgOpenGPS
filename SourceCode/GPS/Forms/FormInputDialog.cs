using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgOpenGPS.Classes;
using AgOpenGPS.Controls;

namespace AgOpenGPS.Forms
{
    public partial class FormInputDialog : Form
    {
        private static readonly Color BorderColor = Color.CornflowerBlue;

        private readonly FormGPS _formGPS;

        private FormInputDialog(string title, string prompt, FormGPS formGPS)
        {
            InitializeComponent();

            _formGPS = formGPS;
            labelTitle.Text = title;
            labelPrompt.Text = prompt;

            textBoxInput.Click += (s, e) =>
            {
                if (_formGPS?.isKeyboardOn == true)
                    textBoxInput.ShowKeyboard(this);
            };

            textBoxInput.TextChanged += (s, e) =>
            {
                int pos = textBoxInput.SelectionStart;
                textBoxInput.Text = Regex.Replace(textBoxInput.Text, glm.fileRegex, "");
                textBoxInput.SelectionStart = pos;
            };
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            using (var pen = new Pen(BorderColor, 20))
                e.Graphics.DrawRectangle(pen, 0, 0, Width, Height);
        }

        // Geeft de ingevoerde naam terug, of null bij Cancel / lege invoer
        public static string ShowInput(string title, string prompt, FormGPS formGPS)
        {
            using (var form = new FormInputDialog(title, prompt, formGPS))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string name = form.textBoxInput.Text.Trim();
                    if (!string.IsNullOrEmpty(name))
                        return name;
                }
                return null;
            }
        }
    }
}
