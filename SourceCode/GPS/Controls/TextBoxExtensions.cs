using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.Controls
{
    public static class TextBoxExtensions
    {
        public static void ShowKeyboard(this TextBox textBox, Form owner)
        {
            var color = textBox.BackColor;
            textBox.BackColor = Color.Red;
            using (FormKeyboard form = new FormKeyboard(textBox.Text))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    textBox.Text = form.ReturnString;
                }
            }
            textBox.BackColor = color;
        }
    }
}
