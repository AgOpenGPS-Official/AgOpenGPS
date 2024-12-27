using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormButtonsRightPanel : Form
    {
        //class variables
        private readonly FormGPS mf = null;
        int btnCounter = 0;
        string original;
        public FormButtonsRightPanel(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormToolPivot_Load(object sender, EventArgs e)
        {
            original = Properties.Settings.Default.setDisplay_buttonOrder;
            mf.buttonOrder?.Clear();

            //for (int i = 0; i < words.Length; i++)
            //{
            //    mf.buttonOrder.Add(int.Parse(words[i], CultureInfo.InvariantCulture));
            //}
            flpRight.Controls.Clear();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(autoSteer);
            btnAutoSteer.Enabled = false;
            mf.buttonOrder.Add(0);
            btnCounter++;
        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(youTurn);
            btnAutoYouTurn.Enabled = false;
            mf.buttonOrder.Add(1);
            btnCounter++;
        }
        private void btnSectionMasterAuto_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(autoSection);
            btnSectionMasterAuto.Enabled = false;
            mf.buttonOrder.Add(2);
            btnCounter++;
        }

        private void btnSectionMasterManual_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(manualSection);
            btnSectionMasterManual.Enabled = false;
            mf.buttonOrder.Add(3);
            btnCounter++;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_buttonOrder = "0,1,2,3,4,5,6,7";
            Properties.Settings.Default.Save();
            flpRight.Controls?.Clear();
            flpRight.Controls.Add(autoSteer);
            flpRight.Controls.Add(youTurn);
            flpRight.Controls.Add(autoSection);
            flpRight.Controls.Add(manualSection);
            flpRight.Controls.Add(skipPrev);
            flpRight.Controls.Add(skipNext);
            flpRight.Controls.Add(track);
            flpRight.Controls.Add(contour);

            mf.buttonOrder?.Clear();
            for (int i = 0; i < flpRight.Controls.Count; i++)
            {
                mf.buttonOrder.Add(i);
            }

            btnAutoSteer.Enabled = false;
            btnAutoYouTurn.Enabled = false;
            btnSectionMasterManual.Enabled = false;
            btnSectionMasterAuto.Enabled = false;
            btnCycleLinesBk.Enabled = false;
            btnCycleLines.Enabled = false;
            btnTrack.Enabled = false;
            btnContour.Enabled = false;
        }
        private void btnTrack_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(track);
            btnTrack.Enabled = false;
            mf.buttonOrder.Add(4);
            btnCounter++;
        }

        private void btnCycleLinesBk_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(skipPrev);
            btnCycleLinesBk.Enabled = false;
            mf.buttonOrder.Add(5);
            btnCounter++;
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(skipNext);
            btnCycleLines.Enabled = false;
            mf.buttonOrder.Add(6);
            btnCounter++;
        }


        private void btnContour_Click(object sender, EventArgs e)
        {
            flpRight.Controls.Add(contour);
            btnContour.Enabled = false;
            mf.buttonOrder.Add(7);
            btnCounter++;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            btnAutoSteer.Enabled = true;
            btnAutoYouTurn.Enabled = true;
            btnSectionMasterManual.Enabled = true;
            btnSectionMasterAuto.Enabled = true;
            btnCycleLinesBk.Enabled = true;
            btnCycleLines.Enabled = true;
            btnTrack.Enabled = true;
            btnContour.Enabled = true;

            btnCounter = 0;
            flpRight.Controls.Clear();
            mf.buttonOrder?.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_buttonOrder = original;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (mf.buttonOrder.Count < 2)
            {
                mf.TimedMessageBox(2000, "Button Error", "Not Enough Buttons Added");
                mf.LogEventWriter("Button Picker, Not Enough Buttons");
                return;
            }
            else
            {
                Properties.Settings.Default.setDisplay_buttonOrder = "";
                for (int i = 0; i < mf.buttonOrder.Count; i++)
                {
                    if (i < mf.buttonOrder.Count - 1)
                    {
                        Properties.Settings.Default.setDisplay_buttonOrder
                            += mf.buttonOrder[i].ToString() + ",";
                    }
                    else
                    {
                        Properties.Settings.Default.setDisplay_buttonOrder += mf.buttonOrder[i].ToString();
                    }
                }

                Properties.Settings.Default.Save();

                mf.PanelBuildRightMenu();
                mf.PanelUpdateRightAndBottom();
                Close();
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_buttonOrder = "";
            for (int i = 0; i < mf.buttonOrder.Count; i++)
            {
                if (i < mf.buttonOrder.Count - 1)
                {
                    Properties.Settings.Default.setDisplay_buttonOrder
                        += mf.buttonOrder[i].ToString() + ",";
                }
                else
                {
                    Properties.Settings.Default.setDisplay_buttonOrder += mf.buttonOrder[i].ToString();
                }
            }

            Properties.Settings.Default.Save();

            mf.PanelBuildRightMenu();
            mf.PanelUpdateRightAndBottom();
        }

        private void btnVideoHelp_Click(object sender, EventArgs e)

        {
            Process[] processName = Process.GetProcessesByName("BobsYourUncle");
            //Start application here
            string strPath = Path.Combine(Application.StartupPath, "Buttons.mp4");

            try
            {
                Process.Start(strPath);
            }
            catch
            {
                mf.TimedMessageBox(2000, "Playback Error", "Can't Find Media Player");
            }
        }
    }
}