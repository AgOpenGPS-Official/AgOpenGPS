using System.Drawing;
using System.Windows.Forms;

namespace AgIO.Controls
{
    public static class TextBoxExtensions
    {
        public static void ShowKeyboard(this TextBox textBox, Form owner)
        {
            textBox.BackColor = Color.Red;
            using (var form = new FormKeyboard(textBox.Text))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    textBox.Text = form.ReturnString;
                }
            }
            textBox.BackColor = Color.AliceBlue;
        }
    }
}
