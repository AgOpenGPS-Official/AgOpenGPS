using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using AgLibrary.Logging;
using AgLibrary.Ntrip;

namespace AgIO
{
    public partial class FormLoop
    {
        // NTRIP over the internet runs in NtripHost (AgLibrary.Ntrip) on its own threads: connecting,
        // retrying, framing and pacing the corrections never touch this form. This file only starts
        // and stops it, feeds it the position once a second and shows its status.
        // Serial radio and serial pass-through still use the flags and counters below.
        internal NtripHost ntripHost;

        // The client from before the rewrite, for users who need a way out in the field
        // (Settings -> NTRIP -> "Use the old NTRIP client"). Internet NTRIP only, no fallback.
        internal LegacyNtripClient legacyNtrip;

        // The legacy client reads the main service of the profile and cannot open a radio, so a profile with a serial radio as
        // main source always uses the new client, whatever the (global) preference for the old one says.
        private static bool UseLegacyNtrip
        {
            get
            {
                if (!Properties.Settings.Default.setNTRIP_useLegacyClient) return false;
                NtripProfileSet active = NtripProfiles.LoadActive();
                return active == null || !active.MainIsRadio;
            }
        }

        //counts seconds for the radio / serial pass-through connect and watchdog
        private int ntripCounter = 10;

        //the settings window stores the address it resolved here

        public uint tripBytes = 0;
        private int toUDP_Port = 0;
        private int NTRIP_Watchdog = 100;

        //NTRIP over the internet is switched on
        public bool isNTRIP_RequiredOn = false;

        //radio / serial pass-through is connected (for NTRIP see ntripHost.Status)
        public bool isNTRIP_Connected = false;
        public bool isNTRIP_Starting = false;
        public bool isNTRIP_Connecting = false;

        public bool isRadio_RequiredOn = false;
        public bool isSerialPass_RequiredOn = false;
        internal SerialPort spRadio = new SerialPort("Radio", 9600, Parity.None, 8, StopBits.One);

        private long lastNtripBytesReceived;

        //every second: the NTRIP client status on the labels, radio / serial pass-through connect logic
        private void DoNTRIPSecondRoutine()
        {
            if (isNTRIP_RequiredOn)
            {
                if (legacyNtrip != null && legacyNtrip.IsRunning) UpdateNtripFromLegacy();
                else UpdateNtripFromHost();
            }

            //count up the clock only if everything is alive
            if (isRadio_RequiredOn || isSerialPass_RequiredOn)
            {
                IncrementNTRIPWatchDog();

                if (!isNTRIP_Connected && !isNTRIP_Connecting && !isNTRIP_Starting)
                {
                    StartNTRIP();
                }
            }

            //radio and serial pass-through show on the same status card as NTRIP
            if ((isRadio_RequiredOn || isSerialPass_RequiredOn) && isUiActive)
            {
                RefreshNtripCard();
            }
        }

        //position in, status out - runs on the UI thread, once a second
        private void UpdateNtripFromHost()
        {
            NtripHost host = ntripHost;
            if (host == null) return;

            host.UpdatePosition(latitude, longitude, fixQualityData, satellitesData, hdopData, altitudeData, ageData);

            int datagrams;
            int bytesOut;
            double bytesPerSecond;
            host.TakeTraffic(out datagrams, out bytesOut, out bytesPerSecond);
            traffic.cntrGPSIn += datagrams;
            traffic.cntrGPSInBytes += bytesOut;

            //one snapshot of the client per cycle: every look at the status copies it, so the card gets this one too
            ManagerStatus snapshot = host.Manager;
            NtripStatus st = snapshot != null ? snapshot.Status : new NtripStatus();

            //bytes from the caster since the last look (the client counts per session)
            long received = st.BytesReceived;
            tripBytes += (uint)(received >= lastNtripBytesReceived ? received - lastNtripBytesReceived : received);
            lastNtripBytesReceived = received;

            isNTRIP_Connected = st.State == NtripState.Streaming || st.State == NtripState.Connected;

            //nobody looks at the labels: minimized, or another window is in use
            if (!isUiActive) return;

            lblToGPS.Text = bytesPerSecond < 1 ? "---" : bytesOut.ToString();

            RefreshNtripCard(snapshot);
        }

        //the old client: its one-second routine, then the same counters and card as the new one
        private void UpdateNtripFromLegacy()
        {
            LegacyNtripClient legacy = legacyNtrip;
            legacy.DoSecondRoutine();

            uint received = legacy.BytesReceived;
            tripBytes += received >= lastNtripBytesReceived ? (uint)(received - lastNtripBytesReceived) : received;
            lastNtripBytesReceived = received;

            isNTRIP_Connected = legacy.IsConnected;

            int bytesOut = legacy.TakeBytesToGps();

            //nobody looks at the labels: minimized, or another window is in use
            if (!isUiActive) return;

            lblToGPS.Text = bytesOut == 0 ? "---" : bytesOut.ToString();

            RefreshNtripCard();
        }

        //the status card for the old client
        private void RefreshLegacyCard()
        {
            legacyNtrip.GetStatus(out string headline, out NtripLight light);
            string received = (tripBytes >> 10).ToString("###,###,##0") + " kb received";
            ntripCard.Alert = false;
            ntripCard.SetStatus(headline + " (old client)", light, legacyNtrip.Host + "\n" + legacyNtrip.Mount, received, "Stop");
        }

        //the lines of the "are the corrections good?" list, also used by the details and the support report.
        //The caster is given the fixed position when NTRIP is set to Fixed Position, so the distance to the base is measured from there.
        internal System.Collections.Generic.IReadOnlyList<CheckItem> BuildNtripChecks(NtripStatus st)
        {
            NtripHost host = ntripHost;
            bool fixedPosition = host != null && host.UsesFixedPosition;
            double fromLat = fixedPosition ? host.FixedLatitude : latitude;
            double fromLon = fixedPosition ? host.FixedLongitude : longitude;
            return CorrectionChecks.Build(st, fixQualityData, ageData, fromLat, fromLon, fixedPosition);
        }

        //the status card and the "are the corrections good?" list in the main window;
        //the once-a-second update hands in its snapshot of the client, other callers get a fresh one
        private void RefreshNtripCard(ManagerStatus snapshot = null)
        {
            if (ntripCard == null || !ntripCard.Visible) return;

            if (isRadio_RequiredOn || isSerialPass_RequiredOn)
            {
                RefreshRadioCard();
                return;
            }

            if (isNTRIP_RequiredOn && legacyNtrip != null && legacyNtrip.IsRunning)
            {
                RefreshLegacyCard();
                return;
            }

            if (!isNTRIP_RequiredOn || ntripHost == null)
            {
                var s = Properties.Settings.Default;
                string off = s.setNTRIP_isOn ? "NTRIP Off" : s.setRadio_isOn ? "Radio Off" : "Serial pass Off";
                ntripCard.Alert = false;
                ntripCard.SetStatus(off, NtripLight.Off, "", "", "Start");
                if (ntripChecks.Visible)
                {
                    ntripChecks.SetChecks(CorrectionChecks.Build(new NtripStatus(), fixQualityData, ageData));
                    // stopped: nothing to switch (the host keeps its manager, but must not be asked to switch)
                    ntripChecks.SetSwitch("", false, ButtonTone.Neutral);
                }
                return;
            }

            ManagerStatus ms = snapshot ?? ntripHost.Manager;
            NtripStatus st = ms != null ? ms.Status : new NtripStatus();
            NtripProfile profile = ntripHost.Profile;
            string source = profile == null ? (ntripHost.MainRadio == null ? "" : "Radio\n" + ntripHost.MainRadio) : profile.Host + "\n" + profile.Mount;
            if (ms != null && ms.Active == SourceRole.Fallback) source = ms.Using.Replace("Fallback: ", "Fallback:\n");
            string received = (tripBytes >> 10).ToString("###,###,##0") + " kb received";
            bool running = st.State == NtripState.Streaming || st.State == NtripState.Connecting
                || st.State == NtripState.Connected || st.State == NtripState.Waiting;

            ntripCard.Alert = ms != null && ms.Active == SourceRole.Fallback;
            ntripCard.SetStatus(st.Headline, st.Light, source, received, running ? "Stop" : "Start");
            if (ntripChecks.Visible)
            {
                ntripChecks.SetChecks(BuildNtripChecks(st));
                if (ms == null || !ms.FallbackConfigured) ntripChecks.SetSwitch("", false, ButtonTone.Neutral);
                else if (ms.Active == SourceRole.Main) ntripChecks.SetSwitch("Switch to fallback", true, ButtonTone.Neutral);
                else if (ms.MainPaused) ntripChecks.SetSwitch("Try the main service again", true, ButtonTone.Neutral);
                else ntripChecks.SetSwitch("Switch back to main", true, ms.MainBack ? ButtonTone.Positive : ButtonTone.Neutral);
            }
        }

        //the status card for a serial radio or serial pass-through (no internet involved)
        private void RefreshRadioCard()
        {
            string name = isRadio_RequiredOn ? "Radio" : "Serial pass";
            string port = Properties.Settings.Default.setPort_portNameRadio;
            string received = (tripBytes >> 10).ToString("###,###,##0") + " kb received";

            if (!isNTRIP_Connected)
                ntripCard.SetStatus("Opening " + name + "...", NtripLight.Info, port, received, "Stop");
            else if (isRadio_RequiredOn && NTRIP_Watchdog > 10)
                ntripCard.SetStatus(name + " waiting for data", NtripLight.Warn, port, received, "Stop");
            else
                ntripCard.SetStatus(name + " Active", NtripLight.Ok, port, received, "Stop");

            if (ntripChecks.Visible) ntripChecks.SetChecks(new CheckItem[0]);
        }

        private void ntripCard_ButtonClicked(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setNTRIP_isOn || Properties.Settings.Default.setRadio_isOn)
            {
                // The client gave up (wrong login or mountpoint): the button starts it again
                if (isNTRIP_RequiredOn && ntripHost != null && ntripHost.Status.State == AgLibrary.Ntrip.NtripState.GaveUp)
                {
                    StartNtripHost();
                    RefreshNtripCard();
                    return;
                }

                if (isNTRIP_RequiredOn || isRadio_RequiredOn)
                {
                    ShutDownNTRIP();
                    isNTRIP_RequiredOn = false;
                    isRadio_RequiredOn = false;
                }
                else
                {
                    isNTRIP_RequiredOn = Properties.Settings.Default.setNTRIP_isOn;
                    isRadio_RequiredOn = Properties.Settings.Default.setRadio_isOn;
                    if (isNTRIP_RequiredOn) StartNtripHost();
                }

                RefreshNtripCard();
            }
            else
            {
                TimedMessageBox(2000, "Turn on NTRIP", "NTRIP Client Not Set Up");
            }
        }

        private void ntripChecks_SwitchClicked(object sender, EventArgs e)
        {
            NtripHost host = ntripHost;
            if (host == null || !host.IsRunning) return;
            ManagerStatus ms = host.Manager;
            if (ms == null) return;

            if (ms.Active == SourceRole.Main)
            {
                AskNtrip("Switch to the fallback?",
                    "Your GPS may lose RTK for a moment, and your position can shift a little if the two base stations use different coordinates.",
                    host.SwitchToFallback);
            }
            else if (ms.MainPaused)
            {
                host.TryMainAgain();
            }
            else
            {
                AskNtrip("Switch back to the main service?", "Your GPS may lose RTK for a moment.", host.SwitchBackToMain);
            }
        }

        private bool ntripQuestionOpen;

        //Advanced settings: all NTRIP error messages and notices can be turned off; the log still gets them
        internal static bool NtripMessagesOn => Properties.Settings.Default.setNTRIP_showErrors;

        //an error message of the old NTRIP client, which obeys the same setting
        public void NtripErrorBox(int timeout, string title, string message)
        {
            Log.EventWriter("NTRIP - " + title + ": " + message);
            if (NtripMessagesOn) TimedMessageBox(timeout, title, message);
        }

        //a question with Yes and Cancel; a second question while one is open is skipped
        private void AskNtrip(string title, string message, Action onYes, bool showCancel = true)
        {
            if (ntripQuestionOpen) return;

            // One FormYes at a time: a question or notice always takes the place of an open outage alert.
            CloseNtripAlert();

            ntripQuestionOpen = true;
            try
            {
                using (var dialog = new FormYes(title + "\r\n\r\n" + message, showCancel))
                {
                    // No owner: a dialog owned by a minimized AgIO stays hidden. It is topmost, so it shows over AgOpenGPS.
                    if (dialog.ShowDialog() == DialogResult.OK) onYes();
                }
            }
            finally
            {
                ntripQuestionOpen = false;
            }
        }

        //notices about switching between the main service and the fallback
        private void OnNtripNotice(NoticeKind kind, string title, string message)
        {
            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    Log.EventWriter("NTRIP - " + title + ": " + message);
                    if (!NtripMessagesOn) return;

                    // The switching notice is the better story: it replaces an open outage alert.
                    CloseNtripAlert();

                    if (kind == NoticeKind.MainBack) AskNtrip(title, message, () => ntripHost.SwitchBackToMain());
                    else if (kind == NoticeKind.MainFailing) AskNtrip(title, message, () => ntripHost.SwitchToFallback());
                    else AskNtrip(title, message, () => { }, false);
                }));
            }
            catch (InvalidOperationException)
            {
                //closing
            }
        }

        private void ntripChecks_DetailsClicked(object sender, EventArgs e)
        {
            ShowNtripDetails();
        }

        private void ShowNtripDetails()
        {
            using (var form = new FormNtripDetails(this))
            {
                form.ShowDialog(this);
            }
        }

        public void ConfigureNTRIP()
        {

            StopNtripHost();

            //an older settings file keeps the service in loose settings: make an NTRIP profile of it once
            NtripProfiles.MigrateLooseSettings();

            //start NTRIP if required
            isNTRIP_RequiredOn = Properties.Settings.Default.setNTRIP_isOn;
            isRadio_RequiredOn = Properties.Settings.Default.setRadio_isOn;
            isSerialPass_RequiredOn = Properties.Settings.Default.setPass_isOn;

            if (isRadio_RequiredOn || isSerialPass_RequiredOn)
            {
                // Immediatly connect radio
                ntripCounter = 20;
            }

            bool anySource = isNTRIP_RequiredOn || isRadio_RequiredOn || isSerialPass_RequiredOn;
            bool radioOrPass = isRadio_RequiredOn || isSerialPass_RequiredOn;

            //one status card for NTRIP, radio and serial pass-through
            ntripCard.Visible = Properties.Settings.Default.setNTRIP_isOn || radioOrPass;
            //the checks and the switch belong to the new client; the list is part of the main window
            ntripChecks.Visible = isNTRIP_RequiredOn && !UseLegacyNtrip;
            lblToGPS.Visible = anySource;

            if (isNTRIP_RequiredOn) StartNtripHost();
            RefreshNtripCard();
        }

        private void StartNtripHost()
        {
            if (UseLegacyNtrip)
            {
                StartLegacyNtrip();
                return;
            }

            if (ntripHost == null)
            {
                ntripHost = new NtripHost(this);
                ntripHost.Problem += OnNtripProblem;
                ntripHost.Notice += OnNtripNotice;
                ntripHost.Alert += OnNtripAlert;
                ntripHost.AlertCleared += OnNtripAlertCleared;
                ntripHost.Recovered += OnNtripRecovered;
            }

            lastNtripBytesReceived = 0;
            ntripHost.Start();
            Log.EventWriter("NTRIP - Client started");
        }

        private void StopNtripHost()
        {
            if (ntripHost != null) ntripHost.Stop();
            if (legacyNtrip != null) legacyNtrip.Stop();
        }

        private void StartLegacyNtrip()
        {
            if (ntripHost != null) ntripHost.Stop();
            if (legacyNtrip == null) legacyNtrip = new LegacyNtripClient(this);

            lastNtripBytesReceived = 0;
            if (!legacyNtrip.Start())
            {
                isNTRIP_RequiredOn = false;
                TimedMessageBox(3000, "NTRIP", "Fill in the address of the main service first.");
            }
        }

        //one notice per kind of problem; the client raises it on a worker thread
        private void OnNtripProblem(string title, string message)
        {
            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    Log.EventWriter("NTRIP - " + title + ": " + message);
                    if (NtripMessagesOn) TimedMessageBox(8000, title, message);
                }));
            }
            catch (InvalidOperationException)
            {
                //closing
            }
        }

        private FormYes ntripAlertForm;
        private bool ntripAlertClosedByCode;

        //closes an open alert from code; this does not count as the user dismissing it
        private void CloseNtripAlert()
        {
            FormYes open = ntripAlertForm;
            if (open == null || open.IsDisposed) return;
            ntripAlertClosedByCode = true;
            open.Close();
        }

        //the corrections have been missing for a minute: a topmost message that comes back after every failed retry
        private void OnNtripAlert(string title, string message)
        {
            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    Log.EventWriter("NTRIP - " + title + ": " + message.Replace("\r\n", " "));
                    if (!NtripMessagesOn)
                    {
                        if (ntripHost != null) ntripHost.AlertClosed(false);
                        return;
                    }

                    // A question or switching notice is open: never stack a second FormYes on it.
                    // The alert comes back after the next failed retry.
                    if (ntripQuestionOpen || ntripAlertForm != null)
                    {
                        if (ntripHost != null) ntripHost.AlertClosed(false);
                        return;
                    }

                    var dialog = new FormYes(title + "\r\n\r\n" + message);
                    ntripAlertForm = dialog;
                    ntripAlertClosedByCode = false;
                    try
                    {
                        // No owner, so it also shows when AgIO is minimized; the form itself is topmost.
                        dialog.ShowDialog();
                    }
                    finally
                    {
                        ntripAlertForm = null;
                        dialog.Dispose();
                        // Only a click by the user counts; the program closing it (switch, recovery) does not.
                        if (ntripHost != null) ntripHost.AlertClosed(!ntripAlertClosedByCode);
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                //closing
            }
        }

        //the corrections are back: the alert has done its job
        private void OnNtripAlertCleared()
        {
            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    CloseNtripAlert();
                }));
            }
            catch (InvalidOperationException)
            {
                //closing
            }
        }

        //the corrections are back after an outage the user was told about: say so, also when the alert was clicked away
        private void OnNtripRecovered(string title, string message)
        {
            if (IsDisposed || !IsHandleCreated) return;

            try
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    Log.EventWriter("NTRIP - " + title + ": " + message);
                    CloseNtripAlert();
                    if (NtripMessagesOn) AskNtrip(title, message, () => { }, false);
                }));
            }
            catch (InvalidOperationException)
            {
                //closing
            }
        }

        //radio / serial pass-through only
        public void StartNTRIP()
        {
            if (isRadio_RequiredOn)
            {
                SetNtripEndpoint(Properties.Settings.Default.setNTRIP_sendToUDPPort);

                if (!string.IsNullOrEmpty(Properties.Settings.Default.setPort_portNameRadio))
                {
                    // Disconnect when already connected
                    if (spRadio != null)
                    {
                        spRadio.Close();
                        spRadio.Dispose();
                    }

                    // Setup and open serial port
                    spRadio = new SerialPort(Properties.Settings.Default.setPort_portNameRadio);
                    spRadio.BaudRate = int.Parse(Properties.Settings.Default.setPort_baudRateRadio);
                    spRadio.DataReceived += NtripPort_DataReceived;
                    isNTRIP_Connecting = false;
                    isNTRIP_Connected = true;

                    try
                    {
                        spRadio.Open();
                    }
                    catch (Exception ex)
                    {
                        isNTRIP_Connecting = false;
                        isNTRIP_Connected = false;
                        isRadio_RequiredOn = false;
                        Log.EventWriter("Catch - > Error connecting to radio" + ex.ToString());

                        TimedMessageBox(2000, "Error connecting to radio", $"{ex.Message}");
                    }
                }
            }
            else if (isSerialPass_RequiredOn)
            {
                toUDP_Port = Properties.Settings.Default.setNTRIP_sendToUDPPort; //send rtcm to which udp port
                SetNtripEndpoint(toUDP_Port);

                if (!string.IsNullOrEmpty(Properties.Settings.Default.setPort_portNameRadio))
                {
                    // Disconnect when already connected
                    if (spRadio != null)
                    {
                        spRadio.Close();
                        spRadio.Dispose();
                    }

                    // Setup and open serial port
                    spRadio = new SerialPort(Properties.Settings.Default.setPort_portNameRadio);
                    spRadio.BaudRate = int.Parse(Properties.Settings.Default.setPort_baudRateRadio);
                    spRadio.DataReceived += NtripPort_DataReceived;
                    isNTRIP_Connecting = false;
                    isNTRIP_Connected = true;

                    try
                    {
                        spRadio.Open();
                    }
                    catch (Exception ex)
                    {
                        isNTRIP_Connecting = false;
                        isNTRIP_Connected = false;
                        isSerialPass_RequiredOn = false;
                        Log.EventWriter("Catch - > Serial Pass Radio: " + ex.ToString());

                        TimedMessageBox(2000, "Error connecting to Serial Pass", $"{ex.Message}");
                    }
                }
            }
        }

        private void ReconnectRequest()
        {
            ntripCounter = 15;
            isNTRIP_Connected = false;
            isNTRIP_Starting = false;
            isNTRIP_Connecting = false;
        }

        private void IncrementNTRIPWatchDog()
        {
            //increment once every second
            ntripCounter++;

            //Thinks is connected but not receiving anything
            if (NTRIP_Watchdog++ > 30 && isNTRIP_Connected)
            {
                Log.EventWriter("NTRIP - No data from radio / serial for 30 s, reconnecting");
                ReconnectRequest();
            }
        }

        //data from the radio or serial pass-through port; the internet client does not come through here
        public void OnAddMessage(byte[] data)
        {
            //update gui with stats
            tripBytes += (uint)data.Length;

            //reset watchdog since we have updated data
            NTRIP_Watchdog = 0;

            if (isUiActive) lblToGPS.Text = data.Length.ToString();

            //send it
            SendNTRIP(data);
        }

        public void SendNTRIP(byte[] data)
        {
            //serial send out GPS port
            if (isSendToSerial)
            {
                SendGPSPort(data);
            }

            //send out UDP Port
            if (isSendToUDP)
            {
                SendUDPMessage(data, epNtrip);
            }
        }

        private void NtripPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Check if we got any data
            try
            {
                SerialPort comport = (SerialPort)sender;
                if (comport.BytesToRead < 32)
                    return;

                int nBytesRec = comport.BytesToRead;

                if (nBytesRec > 0)
                {
                    byte[] localMsg = new byte[nBytesRec];
                    comport.Read(localMsg, 0, nBytesRec);

                    BeginInvoke((MethodInvoker)(() => OnAddMessage(localMsg)));
                }
            }
            catch (Exception)
            {
                //MessageBox.Show( this, ex.Message, "Unusual error druing Recieve!" );
            }
        }

        private void ShutDownNTRIP()
        {
            if (isNTRIP_RequiredOn)
            {
                StopNtripHost();

                //Also stop the requests now
                isNTRIP_RequiredOn = false;
            }
            else if (spRadio != null)
            {
                spRadio.Close();
                spRadio.Dispose();
                spRadio = null;

                ReconnectRequest();

                //Also stop the requests now
                isRadio_RequiredOn = false;
            }
        }

    }
}
