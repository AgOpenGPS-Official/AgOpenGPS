using System;
using System.IO;
using System.Windows.Forms;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormEventViewer : Form
    {
        //class variables
        string filename;

        public FormEventViewer(string _filename)
        {
            //get copy of the calling main form
            InitializeComponent();
            filename = _filename;
        }

        private void FormEventViewer_Load(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader sr = File.OpenText(filename))
                {
                    //rtbLogViewer.Text = String.Empty;
                    while (!sr.EndOfStream)
                    {
                        rtbLogViewer.AppendText(sr.ReadLine() + "\r");
                    }
                }
            }
            catch (Exception ex)
            {
                rtbLogViewer.AppendText("Catch ->  error loading logfile" + ex.ToString());
            }

            rtbLogViewer.AppendText(" **** Current Session Below ***** \r\n\r\n");

            rtbLogViewer.AppendText(Log.sbEvents.ToString());
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            rtbLogViewer.Clear();
            rtbLogViewer.HideSelection = false;

            try
            {
                using (StreamReader sr = File.OpenText(filename))
                {
                    //rtbLogViewer.Text = String.Empty;
                    while (!sr.EndOfStream)
                    {
                        rtbLogViewer.AppendText(sr.ReadLine() + "\r");
                    }
                }
            }
            catch (Exception ex)
            {
                rtbLogViewer.AppendText("Catch ->  error loading logfile" + ex.ToString());
            }

            rtbLogViewer.AppendText(" **** Current Session Below ***** \r\n\r\n");

            rtbLogViewer.AppendText(Log.sbEvents.ToString());
        }
    }
}