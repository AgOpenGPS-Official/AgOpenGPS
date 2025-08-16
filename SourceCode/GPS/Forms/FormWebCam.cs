using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace AgOpenGPS
{
    public partial class FormWebCam : Form
    {
        private readonly VideoCapture _capture = new VideoCapture();

        public FormWebCam()
        {
            InitializeComponent();
        }

        private void FormWebCam_Load(object sender, EventArgs e)
        {
            webCamBackgroundWorker.RunWorkerAsync();
        }

        private void FormWebCam_FormClosing(object sender, FormClosingEventArgs e)
        {
            webCamBackgroundWorker.CancelAsync();
            _capture.Dispose();
        }

        private void webCamBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _capture.Open(0);

            if (!_capture.IsOpened())
            {
                Close();
                return;
            }

            while (!webCamBackgroundWorker.CancellationPending)
            {
                using (var frameMat = _capture.RetrieveMat())
                {
                    var bitmap = BitmapConverter.ToBitmap(frameMat);
                    webCamBackgroundWorker.ReportProgress(0, bitmap);
                }
                Thread.Sleep(100);
            }
        }

        private void webCamBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var bitmap = (Bitmap)e.UserState;
            webCamPictureBox.Image?.Dispose();
            webCamPictureBox.Image = bitmap;
        }
    }
}