using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace AgOpenGPS
{
    public partial class FormWebCam : Form
    {
        private VideoCapture _capture;

        public FormWebCam()
        {
            InitializeComponent();
        }

        private void FormWebCam_Load(object sender, EventArgs e)
        {
            var videoDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            foreach (var videoDevice in videoDevices)
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
            stopButton.Enabled = _capture?.IsOpened() == true;
        }

        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            _capture = new VideoCapture(deviceComboBox.SelectedIndex, VideoCaptureAPIs.DSHOW);

            if (!_capture.IsOpened())
            {
                MessageBox.Show("Failed to open webcam.");
                return;
            }

            backgroundWorker.RunWorkerAsync();

            UpdateButtons();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
            _capture?.Release();

            UpdateButtons();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker.CancellationPending)
            {
                using (var frameMat = _capture.RetrieveMat())
                {
                    var frameBitmap = BitmapConverter.ToBitmap(frameMat);
                    backgroundWorker.ReportProgress(0, frameBitmap);
                }

                Thread.Sleep(30);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var frameBitmap = (Bitmap)e.UserState;
            pictureBox.Image?.Dispose();
            pictureBox.Image = frameBitmap;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox.Image?.Dispose();
            pictureBox.Image = null;
        }
    }
}