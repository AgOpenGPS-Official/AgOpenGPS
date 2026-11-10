using System;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// All NTRIP settings in one window: the main service, the fallback, where the corrections go and which
    /// position is sent. The services belong to an NTRIP profile (its own file); position and output belong
    /// to the AgIO settings file.
    /// </summary>
    public partial class FormNtripSettings : Form
    {
        private readonly FormLoop mf;
        private NtripProfileSet work;
        private CancellationTokenSource checkCts;

        private readonly NtripProfileSet initial;

        /// <param name="profile">The profile to edit; null for the one in use.</param>
        internal FormNtripSettings(FormLoop callingForm, NtripProfileSet profile = null)
        {
            mf = callingForm;
            initial = profile;
            InitializeComponent();
            tglRaw.CheckedChanged += (s, e) => UpdateEnabled();
            tglFbRaw.CheckedChanged += (s, e) => UpdateEnabled();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (string port in SerialPort.GetPortNames()) { cboRadioPort.Items.Add(port); cboMainRadioPort.Items.Add(port); }

            work = (initial ?? NtripProfiles.LoadActive()).Clone();
            if (string.IsNullOrEmpty(work.Name)) work.Name = "";
            ToUi();
            ShowPage();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            CancelCheck();
            base.OnFormClosed(e);
        }

        // ---- settings <-> controls

        private void ToUi()
        {
            var s = Properties.Settings.Default;

            tglUse.Checked = s.setNTRIP_isOn;
            lblProfileName.Text = work.Name.Length > 0 ? work.Name : "(not saved yet)";

            txtHost.Text = work.Main.Host;
            numPort.IntValue = work.Main.Port;
            txtMount.Text = work.Main.Mount;
            txtUser.Text = work.Main.User;
            txtPassword.Text = work.Main.Password;
            segVersion.SelectedIndex = (int)work.Main.Version;
            tglRaw.Checked = work.Main.RawTcp;
            tglLegacy.Checked = s.setNTRIP_useLegacyClient;
            segMainType.SelectedIndex = work.MainIsRadio ? 1 : 0;
            if (work.MainRadioPort.Length > 0 && !cboMainRadioPort.Items.Contains(work.MainRadioPort)) cboMainRadioPort.Items.Add(work.MainRadioPort);
            cboMainRadioPort.SelectedItem = work.MainRadioPort.Length > 0 ? work.MainRadioPort : null;
            cboMainRadioBaud.SelectedItem = work.MainRadioBaud.ToString();
            if (cboMainRadioBaud.SelectedIndex < 0) cboMainRadioBaud.SelectedItem = "9600";

            FallbackDefinition f = work.Fallback;
            tglFallback.Checked = f.Enabled;
            segFbType.SelectedIndex = f.Type == FallbackType.SerialRadio ? 1 : 0;
            segAuto.SelectedIndex = f.AutoSwitch ? 0 : 1;
            numAfter.IntValue = f.SwitchAfterFailures;
            txtFbHost.Text = f.Service.Host;
            numFbPort.IntValue = f.Service.Port;
            txtFbMount.Text = f.Service.Mount;
            txtFbUser.Text = f.Service.User;
            txtFbPassword.Text = f.Service.Password;
            tglFbRaw.Checked = f.Service.RawTcp;
            if (f.RadioPort.Length > 0 && !cboRadioPort.Items.Contains(f.RadioPort)) cboRadioPort.Items.Add(f.RadioPort);
            cboRadioPort.SelectedItem = f.RadioPort.Length > 0 ? f.RadioPort : null;
            cboRadioBaud.SelectedItem = f.RadioBaud.ToString();
            if (cboRadioBaud.SelectedIndex < 0) cboRadioBaud.SelectedItem = "9600";

            //the Fixed Position belongs to the profile; one without its own starts as the GPS position, with the
            //numbers last used as a starting point
            FixedPositionDefinition position = work.FixedPosition;
            segPosition.SelectedIndex = position != null && position.Enabled ? 1 : 0;
            numLat.Value = position != null ? position.Latitude : s.setNTRIP_manualLat;
            numLon.Value = position != null ? position.Longitude : s.setNTRIP_manualLon;
            numAlt.Value = position != null ? position.Altitude : s.setNTRIP_manualAltitude;
            numInterval.IntValue = s.setNTRIP_sendGGAInterval;
            // Serial, network or both (the AgIO default is both). Both must stay a choice: showing it as "network" would drop
            // the serial output of a GPS receiver the moment the settings are saved.
            segOutput.SelectedIndex = s.setNTRIP_sendToSerial && s.setNTRIP_sendToUDP ? 2 : s.setNTRIP_sendToSerial ? 0 : 1;
            numNetPort.IntValue = s.setNTRIP_sendToUDPPort;

            UpdateEnabled();
        }

        private void FromUi()
        {
            work.Main.Host = txtHost.Text.Trim();
            work.Main.Port = numPort.IntValue;
            work.Main.Mount = txtMount.Text.Trim();
            work.Main.User = txtUser.Text.Trim();
            work.Main.Password = txtPassword.Text;
            work.Main.Version = (NtripVersion)segVersion.SelectedIndex;
            work.Main.RawTcp = tglRaw.Checked;
            work.MainType = segMainType.SelectedIndex == 1 ? FallbackType.SerialRadio : FallbackType.NtripService;
            work.MainRadioPort = cboMainRadioPort.SelectedItem == null ? "" : cboMainRadioPort.SelectedItem.ToString();
            int mainBaud;
            work.MainRadioBaud = int.TryParse(cboMainRadioBaud.SelectedItem as string, out mainBaud) ? mainBaud : 9600;

            FallbackDefinition f = work.Fallback;
            f.Enabled = tglFallback.Checked;
            f.Type = segFbType.SelectedIndex == 1 ? FallbackType.SerialRadio : FallbackType.NtripService;
            f.AutoSwitch = segAuto.SelectedIndex == 0;
            f.SwitchAfterFailures = numAfter.IntValue;
            f.Service.Host = txtFbHost.Text.Trim();
            f.Service.Port = numFbPort.IntValue;
            f.Service.Mount = txtFbMount.Text.Trim();
            f.Service.User = txtFbUser.Text.Trim();
            f.Service.Password = txtFbPassword.Text;
            f.Service.RawTcp = tglFbRaw.Checked;
            f.RadioPort = cboRadioPort.SelectedItem == null ? "" : cboRadioPort.SelectedItem.ToString();
            int baud;
            f.RadioBaud = int.TryParse(cboRadioBaud.SelectedItem as string, out baud) ? baud : 9600;

            work.FixedPosition = new FixedPositionDefinition
            {
                Enabled = segPosition.SelectedIndex == 1,
                Latitude = numLat.Value,
                Longitude = numLon.Value,
                Altitude = numAlt.Value,
            };
        }

        private void UpdateEnabled()
        {
            bool fixedPosition = segPosition.SelectedIndex == 1;
            numLat.Enabled = numLon.Enabled = numAlt.Enabled = fixedPosition;
            numNetPort.Enabled = segOutput.SelectedIndex >= 1;   // network or both

            // The service fields give way to the radio's when the main source is a serial radio.
            bool mainRadio = segMainType.SelectedIndex == 1;
            pnlMainRadio.Visible = mainRadio;
            foreach (Control c in new Control[] { lblHost, txtHost, lblPort, numPort, lblMount, txtMount, btnMounts, lblUser, txtUser,
                lblPassword, txtPassword, btnShowPassword, lblVersion, segVersion, lblRaw, tglRaw, lblLegacy, tglLegacy })
                c.Visible = !mainRadio;

            // A raw TCP stream has no NTRIP request: no mountpoint and no login (the values are kept, in case it is switched off again).
            bool mainRaw = tglRaw.Checked;
            foreach (Control c in new Control[] { lblMount, txtMount, btnMounts, lblUser, txtUser, lblPassword, txtPassword, btnShowPassword })
                c.Enabled = !mainRaw;
            bool fallbackRaw = tglFbRaw.Checked;
            foreach (Control c in new Control[] { lblFbMount, txtFbMount, btnFbMounts, lblFbUser, txtFbUser, lblFbPassword, txtFbPassword })
                c.Enabled = !fallbackRaw;

            bool fallbackOn = tglFallback.Checked;
            segFbType.Enabled = segAuto.Enabled = numAfter.Enabled = lblAfter.Enabled = lblAfter2.Enabled = pnlFbService.Enabled = pnlFbRadio.Enabled = fallbackOn;
            pnlFbService.Visible = segFbType.SelectedIndex == 0;
            pnlFbRadio.Visible = segFbType.SelectedIndex == 1;
        }

        private void ShowPage()
        {
            pnlMain.Visible = segPage.SelectedIndex == 0;
            pnlFallback.Visible = segPage.SelectedIndex == 1;
            pnlOutput.Visible = segPage.SelectedIndex == 2;
            btnCheck.Visible = txtLog.Visible = segPage.SelectedIndex != 2;
            UpdateEnabled();
        }

        // ---- events

        private void segPage_SelectedIndexChanged(object sender, EventArgs e) { ShowPage(); }
        private void tglUse_CheckedChanged(object sender, EventArgs e) { }
        private void tglFallback_CheckedChanged(object sender, EventArgs e) { UpdateEnabled(); }
        private void segMainType_SelectedIndexChanged(object sender, EventArgs e) { UpdateEnabled(); }
        private void segFbType_SelectedIndexChanged(object sender, EventArgs e) { UpdateEnabled(); }
        private void segPosition_SelectedIndexChanged(object sender, EventArgs e) { UpdateEnabled(); }
        private void segOutput_SelectedIndexChanged(object sender, EventArgs e) { UpdateEnabled(); }

        private void AskText(TextBox box)
        {
            if (!mf.isKeyboardOn) return;
            using (var keyboard = new FormKeyboard(box.Text))
            {
                if (keyboard.ShowDialog(this) == DialogResult.OK) box.Text = keyboard.ReturnString;
            }
        }

        private void txtHost_Click(object sender, EventArgs e) { AskText(txtHost); }
        private void txtMount_Click(object sender, EventArgs e) { AskText(txtMount); }
        private void txtUser_Click(object sender, EventArgs e) { AskText(txtUser); }
        private void txtPassword_Click(object sender, EventArgs e) { AskText(txtPassword); }
        private void txtFbHost_Click(object sender, EventArgs e) { AskText(txtFbHost); }
        private void txtFbMount_Click(object sender, EventArgs e) { AskText(txtFbMount); }
        private void txtFbUser_Click(object sender, EventArgs e) { AskText(txtFbUser); }
        private void txtFbPassword_Click(object sender, EventArgs e) { AskText(txtFbPassword); }

        private void btnShowPassword_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            btnShowPassword.Text = txtPassword.UseSystemPasswordChar ? "Show" : "Hide";
        }

        private void btnMounts_Click(object sender, EventArgs e) { ChooseMount(work.Main, txtHost, numPort, txtMount, txtUser, txtPassword); }
        private void btnFbMounts_Click(object sender, EventArgs e) { ChooseMount(work.Fallback.Service, txtFbHost, numFbPort, txtFbMount, txtFbUser, txtFbPassword); }

        private void ChooseMount(ServiceDefinition service, TextBox host, NumberField port, TextBox mount, TextBox user, TextBox password)
        {
            var probe = new ServiceDefinition
            {
                Host = host.Text.Trim(),
                Port = port.IntValue,
                User = user.Text.Trim(),
                Password = password.Text,
                Version = service.Version,
            };
            if (!probe.IsComplete)
            {
                mf.TimedMessageBox(2500, "No address", "Fill in the address first.");
                return;
            }

            double lat = segPosition.SelectedIndex == 1 ? numLat.Value : mf.latitude;
            double lon = segPosition.SelectedIndex == 1 ? numLon.Value : mf.longitude;
            using (var form = new FormMountpoints(probe.ToProfile(10), lat, lon))
            {
                if (form.ShowDialog(this) == DialogResult.OK && form.SelectedMount.Length > 0) mount.Text = form.SelectedMount;
            }
        }

        // ---- check the connection

        private void CancelCheck()
        {
            CancellationTokenSource c = checkCts;
            checkCts = null;
            if (c != null) { c.Cancel(); }
        }

        private void Log(string line)
        {
            if (IsDisposed || !IsHandleCreated) return;
            try
            {
                BeginInvoke((MethodInvoker)(() => txtLog.AppendText(line + "\r\n")));
            }
            catch (InvalidOperationException) { /* closing */ }
        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            FromUi();
            CancelCheck();
            txtLog.Clear();

            var cts = new CancellationTokenSource();
            checkCts = cts;
            btnCheck.Enabled = false;
            try
            {
                ConnectionCheckResult result;
                bool fallbackPage = segPage.SelectedIndex == 1;
                bool radioCheck = fallbackPage ? work.Fallback.Type == FallbackType.SerialRadio : work.MainIsRadio;
                if (radioCheck)
                {
                    string port = fallbackPage ? work.Fallback.RadioPort : work.MainRadioPort;
                    int baud = fallbackPage ? work.Fallback.RadioBaud : work.MainRadioBaud;
                    if (port.Length == 0) { Log("Choose the port of the radio first."); return; }
                    result = await ConnectionCheck.RunRadioAsync(port, () => new SerialRadioStream(port, baud), Log, cts.Token);
                }
                else
                {
                    ServiceDefinition service = segPage.SelectedIndex == 1 ? work.Fallback.Service : work.Main;
                    NtripProfile profile = service.ToProfile(numInterval.IntValue);
                    if (profile == null) { Log("Fill in the address first."); return; }

                    double lat = segPosition.SelectedIndex == 1 ? numLat.Value : mf.latitude;
                    double lon = segPosition.SelectedIndex == 1 ? numLon.Value : mf.longitude;
                    double alt = segPosition.SelectedIndex == 1 ? numAlt.Value : 0;
                    Func<string> gga = () => GgaBuilder.FromManual(DateTime.UtcNow, lat, lon, alt);
                    result = await ConnectionCheck.RunAsync(profile, gga, Log, cts.Token);
                }

                txtLog.AppendText("\r\n" + result.Summary);
            }
            catch (OperationCanceledException)
            {
                // window closed
            }
            finally
            {
                if (!IsDisposed) btnCheck.Enabled = true;
            }
        }

        // ---- save

        private void btnSave_Click(object sender, EventArgs e)
        {
            FromUi();
            CancelCheck();

            if (tglUse.Checked && !work.MainIsComplete)
            {
                mf.TimedMessageBox(3000, work.MainIsRadio ? "No port" : "No address",
                    work.MainIsRadio ? "Choose the port of the main radio first." : "Fill in the address of the main service first.");
                segPage.SelectedIndex = 0;
                return;
            }

            if (work.Fallback.Enabled)
            {
                bool radio = work.Fallback.Type == FallbackType.SerialRadio;
                bool missing = radio ? work.Fallback.RadioPort.Length == 0 : !work.Fallback.Service.IsComplete;
                if (missing)
                {
                    segPage.SelectedIndex = 1;
                    mf.YesMessageBox(radio ? "Choose the radio port to use Fallback" : "Fill in Caster information to use Fallback");
                    return;
                }
            }
            if (work.Name.Length == 0) work.Name = NtripProfiles.Store.UniqueName("Default");
            try
            {
                NtripProfiles.SaveAndUse(work);
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(4000, "Could not save", ex.Message);
                return;
            }

            var s = Properties.Settings.Default;
            s.setNTRIP_isOn = tglUse.Checked;
            //the Fixed Position was saved with the profile; the numbers stay as the start for a profile without one
            s.setNTRIP_isGGAManual = false;
            s.setNTRIP_manualLat = numLat.Value;
            s.setNTRIP_manualLon = numLon.Value;
            s.setNTRIP_manualAltitude = numAlt.Value;
            s.setNTRIP_sendGGAInterval = numInterval.IntValue;
            s.setNTRIP_sendToSerial = segOutput.SelectedIndex == 0 || segOutput.SelectedIndex == 2;
            s.setNTRIP_sendToUDP = segOutput.SelectedIndex >= 1;
            s.setNTRIP_sendToUDPPort = numNetPort.IntValue;
            s.setNTRIP_useLegacyClient = tglLegacy.Checked;

            //NTRIP, a standalone radio and serial pass-through are alternatives: using NTRIP switches the others off,
            //also when they were only stopped with the status card (the setting stays on then)
            if (s.setNTRIP_isOn)
            {
                s.setRadio_isOn = false;
                s.setPass_isOn = false;
            }

            //the old client sends by these flags, which are otherwise only read at start-up
            mf.isSendToSerial = s.setNTRIP_sendToSerial;
            mf.isSendToUDP = s.setNTRIP_sendToUDP;
            s.Save();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
