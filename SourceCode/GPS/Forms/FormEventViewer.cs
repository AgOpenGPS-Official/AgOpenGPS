using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEventViewer : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormEventViewer(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormEventViewer_Load(object sender, EventArgs e)
        {
            rtbAutoSteerStopEvents.HideSelection = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rtbAutoSteerStopEvents.TextLength != mf.SystemEventsLogger.RawLogs.Length)
            {
                rtbAutoSteerStopEvents.Clear();
                rtbAutoSteerStopEvents.AppendText(mf.SystemEventsLogger.RawLogs);
            }
        }
    }
}