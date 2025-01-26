using System.Drawing;
using System.Windows.Forms;

namespace AgIO.Controls
{
    public static class NumericUpDownExtensions
    {
        public static void ShowKeypad(this NumericUpDown numericUpDown, Form owner)
        {
            numericUpDown.BackColor = Color.Red;
            using (var form = new FormNumeric((double)numericUpDown.Minimum, (double)numericUpDown.Maximum, (double)numericUpDown.Value))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    numericUpDown.Value = (decimal)form.ReturnValue;
                }
            }
            numericUpDown.BackColor = Color.AliceBlue;
        }
    }
}
