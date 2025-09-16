using System;
using System.Windows.Forms;
using Accord.Video.DirectShow;

namespace AgOpenGPS
{
    public partial class FormWebCam : Form
    {
        private FilterInfoCollection _videoDevices;

        public FormWebCam()
        {
            InitializeComponent();
        }

        private void FormWebCam_Load(object sender, EventArgs e)
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (var videoDevice in _videoDevices)
            {
                deviceComboBox.Items.Add(videoDevice.Name);
            }

            if (deviceComboBox.Items.Count > 0)
            {
                deviceComboBox.SelectedItem = deviceComboBox.Items[0];
            }
        }

        private void UpdateButtons()
        {
            startButton.Enabled = deviceComboBox.SelectedItem != null;
            //stopButton.Enabled = videoSourcePlayer.IsRunning;
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            var videoSource = new VideoCaptureDevice(_videoDevices[deviceComboBox.SelectedIndex].MonikerString);

            //videoSourcePlayer.VideoSource = videoSource;
            //videoSourcePlayer.Start();

            UpdateButtons();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            //videoSourcePlayer.SignalToStop();
            //videoSourcePlayer.WaitForStop();

            UpdateButtons();
        }
    }
}