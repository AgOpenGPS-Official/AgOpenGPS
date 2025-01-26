using System;
using System.Windows.Forms;
using AgIO.Controls;

namespace AgIO
{
    public partial class FormRadioChannel : Form
    {
        private readonly FormLoop mf;

        public CRadioChannel Channel { get; set; } = new CRadioChannel();

        public FormRadioChannel(FormLoop mainForm)
        {
            mf = mainForm;
            InitializeComponent();

            // Set the icon, it is not shown on top. But it is in the taskbar
            Icon = mf.Icon;
        }

        private void FormRadioChannel_Load(object sender, EventArgs e)
        {
            tbId.Text = Channel.Id.ToString();
            tbName.Text = Channel.Name;
            tbFrequency.Text = Channel.Frequency;

            if (!string.IsNullOrEmpty(Channel.Location))
            {
                var locationArray = Channel.Location.Split(' ');

                if (locationArray.Length >= 2)
                {
                    tbLat.Text = locationArray[0];
                    tbLon.Text = locationArray[1];
                }
            }
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tbId.Text, out int channelId))
            {
                mf.TimedMessageBox(2000, "Invalid Id", $"Id '{tbId.Text}' is not a valid number");
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(tbName.Text))
            {
                mf.TimedMessageBox(2000, "Invalid Name", $"Name is not filled in");
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(tbFrequency.Text))
            {
                mf.TimedMessageBox(2000, "Invalid Frequency", $"Frequency is not filled in");
                DialogResult = DialogResult.None;
                return;
            }

            Channel.Id = channelId;
            Channel.Name = tbName.Text;
            Channel.Frequency = tbFrequency.Text;

            if (!string.IsNullOrEmpty(tbLat.Text) && !string.IsNullOrEmpty(tbLon.Text))
            {
                Channel.Location = $"{tbLat.Text} {tbLon.Text}";
            }
        }

        private void tbox_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
            }
        }
    }
}