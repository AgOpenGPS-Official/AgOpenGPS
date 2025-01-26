using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using AgIO.Controls;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormRadio : Form
    {
        private readonly FormLoop mf;
        private List<CRadioChannel> _channels;
        private double _currentLat = 0;
        private double _currentLon = 0;

        //private readonly ListViewColumnSorterExt _listViewColumnSorter;

        public FormRadio(Form callingForm)
        {
            mf = callingForm as FormLoop;

            InitializeComponent();

            // Set the icon, it is not shown on top. But it is in the taskbar
            Icon = mf.Icon;
            //_listViewColumnSorter = new ListViewColumnSorterExt(lvChannels);

            _currentLat = mf.latitude;
            _currentLon = mf.longitude;

            // Load radio channels
            _channels = Properties.Settings.Default.setRadio_Channels;

            if (_channels == null)
            {
                // No channels found, create a new list
                _channels = new List<CRadioChannel>();
            }

            foreach (var channel in _channels)
            {
                AddChannelToListView(channel);
            }
        }

        private void FormRadio_Load(object sender, EventArgs e)
        {
            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }

            lblCurrentPort.Text = Properties.Settings.Default.setPort_portNameRadio;
            lblCurrentBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;
            cboxRadioPort.Text = Properties.Settings.Default.setPort_portNameRadio;
            cboxBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;
            cboxIsRadioOn.Checked = Properties.Settings.Default.setRadio_isOn;

            for (var i = 0; i < lvChannels.Items.Count; i++)
            {
                ListViewItem lvItem = lvChannels.Items[i];

                if (((CRadioChannel)lvItem.Tag).Frequency == Properties.Settings.Default.setPort_radioChannel)
                {
                    lvItem.Selected = true;
                    lvItem.Focused = true;
                    lvItem.EnsureVisible();
                    break;
                }
            }

            lvChannels.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            lvChannels.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);

            SetButtonState();
        }

        private void btnRadioOK_Click(object sender, EventArgs e)
        {
            if (cboxIsRadioOn.Checked && lvChannels.SelectedItems.Count == 0)
            {
                mf.TimedMessageBox(2000, "No channel", "Radio is set to on. But no channel is selected");
                // Cancel close
                DialogResult = DialogResult.None;
                return;
            }

            if (lvChannels.SelectedItems.Count > 0)
            {
                var selectedChannel = (CRadioChannel)lvChannels.SelectedItems[0].Tag;
                Properties.Settings.Default.setPort_radioChannel = selectedChannel.Frequency;
            }

            Properties.Settings.Default.setPort_portNameRadio = cboxRadioPort.Text;
            Properties.Settings.Default.setPort_baudRateRadio = cboxBaud.Text;
            Properties.Settings.Default.setRadio_isOn = cboxIsRadioOn.Checked;

            if (Properties.Settings.Default.setRadio_isOn && Properties.Settings.Default.setNTRIP_isOn)
            {
                mf.TimedMessageBox(2000, "NTRIP also enabled", "NTRIP is also enabled, diabling it");
                Properties.Settings.Default.setNTRIP_isOn = false;
            }

            // Save radio channels
            Properties.Settings.Default.setRadio_Channels = _channels;
            Properties.Settings.Default.Save();

            Close();
            mf.ConfigureNTRIP();
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;
            }

            // Setup and open serial port
            mf.spRadio = new SerialPort(cboxRadioPort.Text, int.Parse(cboxBaud.Text))
            {
                NewLine = "\r\n"
            };

            btnOpenSerial.Enabled = false;
            btnSetChannel.Enabled = true;
            btnCloseSerial.Enabled = true;

            try
            {
                mf.spRadio.Open();

                lblCurrentPort.Text = Properties.Settings.Default.setPort_portNameRadio;
                lblCurrentBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;

                cboxRadioPort.Enabled = false;
                cboxBaud.Enabled = false;
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(3000, "Error opening port", ex.Message);
                Log.EventWriter("Catch - > Error opening Radio port" + ex.ToString());
            }

            SetButtonState();
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;

                btnOpenSerial.Enabled = true;
                btnSetChannel.Enabled = false;
                btnCloseSerial.Enabled = false;

                cboxRadioPort.Enabled = true;
                cboxBaud.Enabled = true;
            }

            SetButtonState();
        }

        private void btnSetChannel_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen && lvChannels.SelectedItems.Count == 1)
            {
                var selectedChannel = (CRadioChannel)lvChannels.SelectedItems[0].Tag;

                // SL&F=nnn.nnnnn
                mf.spRadio.WriteLine($"SL&F={selectedChannel.Frequency}");
            }
        }

        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.WriteLine(tbCommand.Text);
                System.Threading.Thread.Sleep(0);

                int bytesToRead = mf.spRadio.BytesToRead;

                if (bytesToRead == 0)
                    return;

                // TODO: What if the radio is receiving data? It will be in the read
                byte[] buffer = new byte[bytesToRead];
                mf.spRadio.Read(buffer, 0, bytesToRead);
                tbResponse.Text = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }

        private void btnAddChannel_Click(object sender, EventArgs e)
        {
            using (var form = new FormRadioChannel(mf))
            {
                // Get max id
                var maxChannelId = 0;

                if (_channels.Count > 0)
                {
                    maxChannelId = _channels.Max(c => c.Id);
                }

                form.Channel.Id = maxChannelId + 1;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    _channels.Add(form.Channel);
                    AddChannelToListView(form.Channel);

                    // Resize
                    lvChannels.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    lvChannels.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void btnEditChannel_Click(object sender, EventArgs e)
        {
            if (lvChannels.SelectedItems.Count == 1)
            {
                using (var form = new FormRadioChannel(mf))
                {
                    var selectedChannel = (CRadioChannel)lvChannels.SelectedItems[0].Tag;

                    form.Channel.Id = selectedChannel.Id;
                    form.Channel.Name = selectedChannel.Name;
                    form.Channel.Frequency = selectedChannel.Frequency;
                    form.Channel.Location = selectedChannel.Location;

                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        // Set in channel
                        selectedChannel.Id = form.Channel.Id;
                        selectedChannel.Name = form.Channel.Name;
                        selectedChannel.Frequency = form.Channel.Frequency;
                        selectedChannel.Location = form.Channel.Location;

                        // Set in listview
                        // TODO: Use keys
                        lvChannels.SelectedItems[0].SubItems[0].Text = selectedChannel.Id.ToString();
                        lvChannels.SelectedItems[0].SubItems[1].Text = selectedChannel.Name;
                        lvChannels.SelectedItems[0].SubItems[2].Text = selectedChannel.Frequency;

                        // Resize
                        lvChannels.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                        lvChannels.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
                    }
                }
            }
        }

        private void btnDeleteChannel_Click(object sender, EventArgs e)
        {
            if (lvChannels.SelectedItems.Count > 0)
            {
                var selectedItem = lvChannels.SelectedItems[0];
                lvChannels.Items.Remove(selectedItem);
                _channels.Clear();

                foreach (ListViewItem lvItem in lvChannels.Items)
                {
                    _channels.Add((CRadioChannel)lvItem.Tag);
                }

                // Resize
                lvChannels.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                lvChannels.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
            }
        }

        private void AddChannelToListView(CRadioChannel channel)
        {
            var distance = "-";

            // calculate distance
            if (!string.IsNullOrEmpty(channel.Location) && _currentLat > 0 && _currentLon > 0)
            {
                var locationArray = channel.Location.Split(' ');

                if (locationArray.Length >= 2)
                {
                    if (double.TryParse(locationArray[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) &&
                        double.TryParse(locationArray[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
                    {
                        distance = glm.DistanceLonLat(lon, lat, _currentLon, _currentLat).ToString("N2");
                    }
                }
            }

            lvChannels.Items.Add(new ListViewItem(new[]
            {
                    channel.Id.ToString(),
                    channel.Name,
                    channel.Frequency,
                    distance
                })
            {
                Tag = channel
            });
        }

        private void SetButtonState()
        {
            // Set connect disconnect and send buttons
            var portOpen = mf.spRadio != null && mf.spRadio.IsOpen;

            btnOpenSerial.Enabled = !portOpen;
            btnCloseSerial.Enabled = portOpen;

            btnSetChannel.Enabled = portOpen;
            btnSendCommand.Enabled = portOpen;
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