using System;
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

        //counts seconds for the radio / serial pass-through connect and watchdog
        private int ntripCounter = 10;

        //the settings window stores the address it resolved here
        public string broadCasterIP;

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
        private readonly StringBuilder sbNtripMessages = new StringBuilder();

        //every second: the NTRIP client status on the labels, radio / serial pass-through connect logic
        private void DoNTRIPSecondRoutine()
        {
            if (isNTRIP_RequiredOn)
            {
                UpdateNtripFromHost();
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

            if (isRadio_RequiredOn)
            {
                lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

                //Bypass if sleeping
                if (focusSkipCounter != 0)
                {
                    //update byte counter and up counter
                    if (ntripCounter > 59) btnStartStopNtrip.Text = (ntripCounter >> 6) + " Min";
                    else if (ntripCounter < 60 && ntripCounter > 25) btnStartStopNtrip.Text = ntripCounter + " Secs";
                    else btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 25)) + " secs";

                    if (isNTRIP_Connecting) lblWatch.Text = "Authourizing";
                    else if (NTRIP_Watchdog > 10) lblWatch.Text = "Waiting";
                    else lblWatch.Text = "Listening Radio";
                }
            }
            else if (isSerialPass_RequiredOn)
            {
                lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

                //update byte counter and up counter
                if (ntripCounter > 59) btnStartStopNtrip.Text = (ntripCounter >> 6) + " Min";
                else if (ntripCounter < 60 && ntripCounter > 22) btnStartStopNtrip.Text = ntripCounter + " Secs";
                else btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 22)) + " secs";
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

            NtripStatus st = host.Status;

            //bytes from the caster since the last look (the client counts per session)
            long received = st.BytesReceived;
            tripBytes += (uint)(received >= lastNtripBytesReceived ? received - lastNtripBytesReceived : received);
            lastNtripBytesReceived = received;

            isNTRIP_Connected = st.State == NtripState.Streaming || st.State == NtripState.Connected;
            lblNTRIPBytes.Text = ((tripBytes >> 10)).ToString("###,###,### kb");

            //Bypass if sleeping
            if (focusSkipCounter == 0) return;

            lblWatch.Text = NtripStateText(st.State);
            btnStartStopNtrip.Text = st.State == NtripState.Streaming ? "Online" : NtripStateText(st.State);

            NtripProfile profile = host.Profile;
            lblNTRIP_IP.Text = profile == null ? "--" : profile.Host;
            lblMount.Text = profile == null ? "--" : profile.Mount;
            lblToGPS.Text = bytesPerSecond < 1 ? "---" : bytesOut.ToString();

            if (isViewAdvanced) UpdateNtripPanel(host, st);
        }

        private static string NtripStateText(NtripState state)
        {
            switch (state)
            {
                case NtripState.Streaming: return "Listening NTRIP";
                case NtripState.Connected: return "Connected";
                case NtripState.Connecting: return "Connecting";
                case NtripState.Waiting: return "Waiting";
                default: return "Stopped";
            }
        }

        //the fly-out: what the caster sends, from the client's own statistics
        private void UpdateNtripPanel(NtripHost host, NtripStatus st)
        {
            RtcmStreamSnapshot rtcm = st.Rtcm;
            if (rtcm == null || rtcm.Types.Count == 0)
            {
                lblMessages.Text = "Reading...";
                lblMessagesFound.Text = "-";
                lblStationID.Text = "-";
            }
            else
            {
                sbNtripMessages.Clear();
                int shown = 0;
                foreach (RtcmTypeStat t in rtcm.Types)
                {
                    if (shown++ >= 12) break;
                    sbNtripMessages.AppendLine(t.Type + " - " + t.Count);
                }
                lblMessages.Text = sbNtripMessages.ToString();
                lblMessagesFound.Text = rtcm.Types.Count.ToString();
                lblStationID.Text = rtcm.StationId >= 0 ? rtcm.StationId.ToString() : "-";
            }
            lblCount.Text = host.QueuedBytes.ToString();
        }

        public void ConfigureNTRIP()
        {
            lblWatch.Text = "Wait GPS";
            lblMessages.Text = "Reading...";
            lblNTRIP_IP.Text = "";
            lblMount.Text = "";

            StopNtripHost();

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
            btnStartStopNtrip.Visible = anySource;
            lblWatch.Visible = anySource;
            lblNTRIPBytes.Visible = anySource;
            lblToGPS.Visible = anySource;
            lblMount.Visible = anySource;
            lblNTRIP_IP.Visible = anySource;

            btnStartStopNtrip.Text = "Off";

            if (isNTRIP_RequiredOn) StartNtripHost();
        }

        private void StartNtripHost()
        {
            if (ntripHost == null)
            {
                ntripHost = new NtripHost(this);
                ntripHost.Problem += OnNtripProblem;
            }

            lastNtripBytesReceived = 0;
            ntripHost.Start();
            Log.EventWriter("NTRIP - Client started");
        }

        private void StopNtripHost()
        {
            if (ntripHost != null) ntripHost.Stop();
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
                    TimedMessageBox(2500, title, message);
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
                    lblWatch.Text = "RTCM Serial";

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

            lblToGPS.Text = data.Length.ToString();

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

        //new settings were saved: reconnect with them
        private void SettingsShutDownNTRIP()
        {
            if (isNTRIP_RequiredOn)
            {
                StartNtripHost();
            }

            if (spRadio != null && spRadio.IsOpen)
            {
                spRadio.Close();
                spRadio.Dispose();
                spRadio = null;
                ReconnectRequest();
            }
        }
    }
}
