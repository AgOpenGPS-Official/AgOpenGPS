using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class Form_Keys : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public Form_Keys(Form callingForm)
        {
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void btnEditShortcut_Click(object sender, EventArgs e)
        {
            EditShortcut((Button)sender);
        }

        private void EditShortcut(Button btn)
        {
            lblAutosteer.Text = "Press New Shortcut Key";
            btn.Text = "...";
            tboxKey.TextChanged -= tboxKey_TextChanged;
            tboxKey.Text = "";
            tboxKey.TextChanged += tboxKey_TextChanged;
            tboxKey.Focus();
        }

        private void tboxKey_TextChanged(object sender, EventArgs e)
        {
            tboxKey.TextChanged -= tboxKey_TextChanged;
            if (tboxKey.Text.Length > 1)
            {
                tboxKey.Text = "";
                tboxKey.TextChanged += tboxKey_TextChanged;
                return;
            }

            tboxKey.Text = Regex.Replace(tboxKey.Text, "[^0-9a-zA-Z]", "");
            if (tboxKey.Text.Length > 0)
            {
                lblAutosteer.Text = "Press Button to Edit Shortcut";
                string but = tboxKey.Text.ToUpper();

                if (btnAutosteer.Text == "...") { btnAutosteer.Text = but; }
                else if (btnCycleLines.Text == "...") { btnCycleLines.Text = but; }
                else if (btnFieldMenu.Text == "...") { btnFieldMenu.Text = but; }
                else if (btnNewFlag.Text == "...") { btnNewFlag.Text = but; }
                else if (btnManualSection.Text == "...") { btnManualSection.Text = but; }
                else if (btnAutoSection.Text == "...") { btnAutoSection.Text = but; }
                else if (btnSnapToPivot.Text == "...") { btnSnapToPivot.Text = but; }
                else if (btnMoveLineLeft.Text == "...") { btnMoveLineLeft.Text = but; }
                else if (btnMoveLineRight.Text == "...") { btnMoveLineRight.Text = but; }
                else if (btnVehicleSettings.Text == "...") { btnVehicleSettings.Text = but; }
                else if (btnSteerWizard.Text == "...") { btnSteerWizard.Text = but; }
                else if (btnSection1.Text == "...") { btnSection1.Text = but; }
                else if (btnSection2.Text == "...") { btnSection2.Text = but; }
                else if (btnSection3.Text == "...") { btnSection3.Text = but; }
                else if (btnSection4.Text == "...") { btnSection4.Text = but; }
                else if (btnSection5.Text == "...") { btnSection5.Text = but; }
                else if (btnSection6.Text == "...") { btnSection6.Text = but; }
                else if (btnSection7.Text == "...") { btnSection7.Text = but; }
                else if (btnSection8.Text == "...") { btnSection8.Text = but; }
                tboxKey.Text = but;
            }
            else
            {
                mf.TimedMessageBox(2000, "Alphanumeric Only", "A to Z and 0 to 9");
            }
            tboxKey.TextChanged += tboxKey_TextChanged;
        }

        private void Form_Keys_Load(object sender, EventArgs e)
        {
            LoadButtonText();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

        }

        private void Form_Keys_FormClosing(object sender, FormClosingEventArgs e)
        {

            Properties.Settings.Default.setKey_hotkeys =
                btnAutosteer.Text + btnCycleLines.Text + btnFieldMenu.Text +
                btnNewFlag.Text + btnManualSection.Text + btnAutoSection.Text +
                btnSnapToPivot.Text + btnMoveLineLeft.Text + btnMoveLineRight.Text +
                btnVehicleSettings.Text + btnSteerWizard.Text +
                btnSection1.Text + btnSection2.Text + btnSection3.Text + btnSection4.Text +
                btnSection5.Text + btnSection6.Text + btnSection7.Text + btnSection8.Text;
            Properties.Settings.Default.Save();

            mf.hotkeys = Properties.Settings.Default.setKey_hotkeys.ToCharArray();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setKey_hotkeys = "ACFGMNPTYVW12345678";
            Properties.Settings.Default.Save();
            mf.hotkeys = Properties.Settings.Default.setKey_hotkeys.ToCharArray();
            LoadButtonText();
        }

        private void LoadButtonText()
        {
            btnAutosteer.Text = mf.hotkeys[0].ToString();
            btnCycleLines.Text = mf.hotkeys[1].ToString();
            btnFieldMenu.Text = mf.hotkeys[2].ToString();
            btnNewFlag.Text = mf.hotkeys[3].ToString();
            btnManualSection.Text = mf.hotkeys[4].ToString();
            btnAutoSection.Text = mf.hotkeys[5].ToString();
            btnSnapToPivot.Text = mf.hotkeys[6].ToString();
            btnMoveLineLeft.Text = mf.hotkeys[7].ToString();
            btnMoveLineRight.Text = mf.hotkeys[8].ToString();
            btnVehicleSettings.Text = mf.hotkeys[9].ToString();
            btnSteerWizard.Text = mf.hotkeys[10].ToString();
            btnSection1.Text = mf.hotkeys[11].ToString();
            btnSection2.Text = mf.hotkeys[12].ToString();
            btnSection3.Text = mf.hotkeys[13].ToString();
            btnSection4.Text = mf.hotkeys[14].ToString();
            btnSection5.Text = mf.hotkeys[15].ToString();
            btnSection6.Text = mf.hotkeys[16].ToString();
            btnSection7.Text = mf.hotkeys[17].ToString();
            btnSection8.Text = mf.hotkeys[18].ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((btnAutosteer.Text == "...") ||
            (btnCycleLines.Text == "...") ||
            (btnFieldMenu.Text == "...") ||
            (btnNewFlag.Text == "...") ||
            (btnManualSection.Text == "...") ||
            (btnAutoSection.Text == "...") ||
            (btnSnapToPivot.Text == "...") ||
            (btnMoveLineLeft.Text == "...") ||
            (btnMoveLineRight.Text == "...") ||
            (btnVehicleSettings.Text == "...") ||
            (btnSteerWizard.Text == "...") ||
            (btnSection1.Text == "...") ||
            (btnSection2.Text == "...") ||
            (btnSection3.Text == "...") ||
            (btnSection4.Text == "...") ||
            (btnSection5.Text == "...") ||
            (btnSection6.Text == "...") ||
            (btnSection7.Text == "...") ||
            (btnSection8.Text == "..."))
            {
                mf.TimedMessageBox(2000, "HoyKey Incomplete", "Finish Setting All, or Reset to Default");
            }

            else Close();


        }
    }
}