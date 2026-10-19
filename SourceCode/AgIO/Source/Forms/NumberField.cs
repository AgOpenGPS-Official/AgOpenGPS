using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgIO
{
    /// <summary>
    /// A number that is typed on AgIO's own number pad: a tap opens it. Looks like a button showing the value,
    /// so it is big enough for a finger.
    /// </summary>
    [DefaultEvent("ValueChanged")]
    internal class NumberField : RoundedButton
    {
        private double value;
        private int decimalPlaces;

        public event EventHandler ValueChanged;

        public NumberField()
        {
            Minimum = 0;
            Maximum = 100000;
            Font = new Font("Tahoma", 14.25F, FontStyle.Regular);
            ShowValue();
        }

        [DefaultValue(0.0)]
        public double Minimum { get; set; }

        [DefaultValue(100000.0)]
        public double Maximum { get; set; }

        [DefaultValue(0)]
        public int DecimalPlaces
        {
            get { return decimalPlaces; }
            set { decimalPlaces = Math.Max(0, value); ShowValue(); }
        }

        [DefaultValue(0.0)]
        public double Value
        {
            get { return this.value; }
            set
            {
                double v = Math.Max(Minimum, Math.Min(Maximum, value));
                if (v == this.value) { ShowValue(); return; }
                this.value = v;
                ShowValue();
                EventHandler handler = ValueChanged;
                if (handler != null) handler(this, EventArgs.Empty);
            }
        }

        /// <summary>The value as an integer, for ports and seconds.</summary>
        public int IntValue
        {
            get { return (int)Math.Round(Value); }
            set { Value = value; }
        }

        private void ShowValue()
        {
            Text = value.ToString("F" + decimalPlaces.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Form owner = FindForm();
            using (var pad = new FormNumeric(Minimum, Maximum, value))
            {
                if (pad.ShowDialog(owner) == DialogResult.OK) Value = pad.ReturnValue;
            }
        }
    }
}
