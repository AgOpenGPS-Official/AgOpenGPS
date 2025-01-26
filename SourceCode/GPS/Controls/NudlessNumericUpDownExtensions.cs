using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS.Controls
{
    public static class NudlessNumericUpDownExtensions
    {
        public static bool ShowKeypad(this NudlessNumericUpDown nudlessNumericUpDown, Form owner)
        {
            var color = nudlessNumericUpDown.BackColor;
            nudlessNumericUpDown.BackColor = Color.Red;
            nudlessNumericUpDown.Value = Math.Round(nudlessNumericUpDown.Value, nudlessNumericUpDown.DecimalPlaces);

            using (FormNumeric form = new FormNumeric((double)nudlessNumericUpDown.Minimum, (double)nudlessNumericUpDown.Maximum, (double)nudlessNumericUpDown.Value))
            {
                DialogResult result = form.ShowDialog(owner);
                if (result == DialogResult.OK)
                {
                    nudlessNumericUpDown.Value = (decimal)form.ReturnValue;
                    nudlessNumericUpDown.BackColor = color;
                    return true;
                }
                else if (result == DialogResult.Cancel)
                {
                    nudlessNumericUpDown.BackColor = color;
                }
                return false;
            }
        }
    }
}
