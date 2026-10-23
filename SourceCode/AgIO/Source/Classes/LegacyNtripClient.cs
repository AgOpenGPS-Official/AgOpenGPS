using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// The NTRIP client as it was before the rewrite (develop at edd7fae, after #1228), kept as a fallback for
    /// users who run into a problem with the new client in the field. Only the internet path: radio and serial
    /// pass-through are handled by FormLoop for both clients.
    ///
    /// The logic is the old one on purpose, including its timing (first attempt after about 10 s, a fixed
    /// retry about 6 s later). Only the plumbing changed: it is a class instead of a part of FormLoop, it takes
    /// its settings from the NTRIP profile in use (main service), and it reports a status for the new status
    /// card instead of writing to the old labels. Everything runs on the UI thread, as before.
    /// </summary>
    internal sealed class LegacyNtripClient : IDisposable
    {
        private readonly FormLoop mf;

        //for the NTRIP CLient counting
        private int ntripCounter = 10;

        private Socket clientSocket;                      // Server connection
        private readonly byte[] casterRecBuffer = new byte[2800];    // Recieved data buffer

        //Send GGA back timer
        private Timer tmr;

        //meters the RTCM out to the GPS in packets, as the old client did
        private readonly Timer ntripMeterTimer = new Timer { Interval = 50 };

        private string broadCasterIP;
        private string casterHost;
        private int broadCasterPort;
        private string mount;
        private string username;
        private string password;
        private bool isHttp10;
        private bool isRawTcp;
        private bool isGgaManual;
        private double manualLat;
        private double manualLon;

        private int sendGGAInterval;

        private int NTRIP_Watchdog = 100;

        private bool isRequiredOn;
        public bool IsConnected { get; private set; }
        private bool isStarting;
        private bool isConnecting;

        //true once the HTTP request has been sent to the caster, reset on every reconnect
        private bool isAuthSent;

        //counts real RTCM bytes received since the caster accepted the connection - a caster
        //can send "200 OK" and still reject the mountpoint by closing before any real data flows
        private uint bytesSinceConnected;

        //NTRIP metering
        private readonly Queue<byte> rawTrip = new Queue<byte>();

        /// <summary>Bytes received from the caster since the start of this client.</summary>
        public uint BytesReceived { get; private set; }

        //bytes sent on to the GPS since FormLoop last asked, for the label under the card
        private int bytesToGps;

        /// <summary>Bytes sent on to the GPS since the last call.</summary>
        public int TakeBytesToGps()
        {
            int n = bytesToGps;
            bytesToGps = 0;
            return n;
        }

        public string Host => casterHost ?? "";
        public string Mount => mount ?? "";

        public LegacyNtripClient(FormLoop form)
        {
            mf = form;
            ntripMeterTimer.Tick += NtripMeterTimer_Tick;
        }

        /// <summary>Reads the settings and starts connecting on the next second ticks. False when there is no caster.</summary>
        public bool Start()
        {
            Stop();

            var s = Properties.Settings.Default;
            NtripProfileSet set = NtripProfiles.LoadActive();
            ServiceDefinition main = set?.Main;
            if (main == null || !main.IsComplete) return false;

            casterHost = main.Host.Trim();
            broadCasterPort = main.Port;
            mount = main.Mount;
            username = main.User;
            password = main.Password;
            isHttp10 = main.Version == NtripVersion.V1;
            isRawTcp = main.RawTcp;
            sendGGAInterval = s.setNTRIP_sendGGAInterval;
            isGgaManual = s.setNTRIP_isGGAManual;
            manualLat = s.setNTRIP_manualLat;
            manualLon = s.setNTRIP_manualLon;

            //an address typed as an IP needs no lookup; else start from the last known one, as before
            broadCasterIP = IPAddress.TryParse(casterHost, out _) ? casterHost : s.setNTRIP_casterIP;
            lastCasterResolveTime = DateTime.MinValue;
            ResolveCasterIPAsync();

            mf.SetNtripEndpoint(s.setNTRIP_sendToUDPPort);

            ntripCounter = 10;
            NTRIP_Watchdog = 100;
            BytesReceived = 0;
            isRequiredOn = true;
            Log.EventWriter("NTRIP - Old client started: " + mount + "@" + casterHost + ":" + broadCasterPort);
            return true;
        }

        public void Stop()
        {
            if (!isRequiredOn) return;
            isRequiredOn = false;
            CloseSocket();
            ReconnectRequest();
            ntripMeterTimer.Enabled = false;
            rawTrip.Clear();
            Log.EventWriter("NTRIP - Old client stopped");
        }

        public bool IsRunning => isRequiredOn;

        public void Dispose()
        {
            Stop();
            ntripMeterTimer.Dispose();
            tmr?.Dispose();
        }

        /// <summary>The status card text for the current state.</summary>
        public void GetStatus(out string headline, out NtripLight light)
        {
            if (!isRequiredOn) { headline = "NTRIP Off"; light = NtripLight.Off; return; }

            if (isConnecting) { headline = "Authorizing"; light = NtripLight.Info; return; }

            if (IsConnected)
            {
                if (NTRIP_Watchdog > 10) { headline = "Waiting"; light = NtripLight.Warn; }
                else { headline = "Listening NTRIP"; light = NtripLight.Ok; }
                return;
            }

            headline = ntripCounter < 20 ? "Connecting in " + (21 - ntripCounter) + " s" : "Connecting";
            light = NtripLight.Info;
        }

        //set up connection to Caster - called once a second from FormLoop
        public void DoSecondRoutine()
        {
            if (!isRequiredOn) return;

            IncrementNTRIPWatchDog();

            //Have we NTRIP connection
            if (!IsConnected && !isConnecting)
            {
                if (!isStarting && ntripCounter > 20)
                {
                    StartNTRIP();
                }
            }

            if (isConnecting)
            {
                if (ntripCounter > 29)
                {
                    Log.EventWriter("NTRIP - Not connecting to caster (generic connecting-phase timeout)");
                    mf.TimedMessageBox(1500, "Connection Problem", "Not Connecting To Caster");
                    ReconnectRequest();
                }
                if (clientSocket != null && clientSocket.Connected)
                {
                    SendAuthorization();
                }
            }
        }

        //once we have a known caster IP, only re-resolve this often
        private static readonly TimeSpan casterResolveInterval = TimeSpan.FromSeconds(30);
        private DateTime lastCasterResolveTime = DateTime.MinValue;
        private bool isCasterResolving;

        // Resolve the caster hostname/URL to an IPv4 address on a worker thread. With no internet
        // a DNS lookup can hang for several seconds, and this UI thread also relays all module UDP
        // traffic - blocking it here stops autosteer. Never call Dns.GetHostAddresses on the UI thread.
        private void ResolveCasterIPAsync()
        {
            if (isCasterResolving) return;
            if (IPAddress.TryParse(casterHost, out _)) return;

            if (!string.IsNullOrEmpty(broadCasterIP) && DateTime.UtcNow - lastCasterResolveTime < casterResolveInterval)
            {
                return;
            }

            isCasterResolving = true;
            lastCasterResolveTime = DateTime.UtcNow;
            string casterURL = casterHost;

            Task.Run(() =>
            {
                string resolvedIP = null;
                string error = null;

                try
                {
                    foreach (IPAddress address in Dns.GetHostAddresses(casterURL))
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            resolvedIP = address.ToString().Trim();
                            break;
                        }
                    }
                    if (resolvedIP == null) error = "no IPv4 address found";
                }
                catch (Exception ex)
                {
                    error = ex.ToString();
                }

                try
                {
                    mf.BeginInvoke((MethodInvoker)(() => OnCasterResolved(casterURL, resolvedIP, error)));
                }
                catch (InvalidOperationException)
                {
                    //form already closed
                }
            });
        }

        //back on the UI thread with the result of ResolveCasterIPAsync
        private void OnCasterResolved(string casterURL, string resolvedIP, string error)
        {
            isCasterResolving = false;

            //caster changed meanwhile - resolve the new one on the next attempt
            if (casterURL != casterHost)
            {
                lastCasterResolveTime = DateTime.MinValue;
                return;
            }

            if (resolvedIP == null)
            {
                Log.EventWriter("NTRIP - Cannot resolve caster URL: " + casterURL + " -> " + error);
                mf.TimedMessageBox(2500, "NTRIP Connection Failed", "Cannot resolve caster URL: " + casterURL);
                return;
            }

            bool hadNoIP = string.IsNullOrEmpty(broadCasterIP);

            if (resolvedIP != broadCasterIP)
            {
                broadCasterIP = resolvedIP;
                Properties.Settings.Default.setNTRIP_casterIP = broadCasterIP;
                Properties.Settings.Default.Save();
                Log.EventWriter("NTRIP - Caster IP resolved to: " + broadCasterIP);
            }

            //was waiting on this lookup to have anything to connect to - start on the next second tick
            if (hadNoIP && isRequiredOn && !IsConnected && !isConnecting && ntripCounter < 20)
            {
                ntripCounter = 20;
            }
        }

        private void StartNTRIP()
        {
            // Re-resolve the caster hostname in case its IP changed (e.g. dynamic DNS).
            // Runs in the background - this attempt uses the last known IP.
            ResolveCasterIPAsync();

            if (string.IsNullOrEmpty(broadCasterIP))
            {
                //no known IP yet - nothing to connect to, try again once the lookup is done
                ReconnectRequest();
                return;
            }

            //if we had a timer already, kill it
            tmr?.Dispose();
            tmr = null;

            //create new timer at fast rate to start
            if (sendGGAInterval > 0)
            {
                tmr = new Timer { Interval = 5000 };
                tmr.Tick += NTRIPtick;
            }

            try
            {
                // Always release the previous socket. After resume from standby the old
                // connection is usually half-open (Connected == false) and was leaked here.
                CloseSocket();

                // Create the socket object
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    // Connect to server non-Blocking method
                    Blocking = false
                };
                clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(broadCasterIP), broadCasterPort), new AsyncCallback(OnConnect), clientSocket);

                Log.EventWriter("NTRIP - Connecting to IP: " + broadCasterIP + ":" + broadCasterPort
                    + " To Port: " + Properties.Settings.Default.setNTRIP_sendToUDPPort + " Mount: " + mount);
            }
            catch (Exception ex)
            {
                ReconnectRequest();
                Log.EventWriter("Catch - > NTRIP Reconnect Request: " + ex.ToString());
                return;
            }

            isConnecting = true;
        }

        private void CloseSocket()
        {
            Socket oldSocket = clientSocket;
            if (oldSocket == null) return;
            clientSocket = null;
            try
            {
                if (oldSocket.Connected) oldSocket.Shutdown(SocketShutdown.Both);
            }
            catch { /* already dead */ }
            finally
            {
                try { oldSocket.Close(); } catch { /* already disposed */ }
            }
        }

        private void ReconnectRequest()
        {
            ntripCounter = 15;
            IsConnected = false;
            isStarting = false;
            isConnecting = false;
            isAuthSent = false;
            bytesSinceConnected = 0;

            //if we had a timer already, kill it
            tmr?.Dispose();
            tmr = null;
        }

        private void IncrementNTRIPWatchDog()
        {
            //increment once every second
            ntripCounter++;

            //Thinks is connected but not receiving anything
            if (NTRIP_Watchdog++ > 30 && IsConnected)
            {
                Log.EventWriter("NTRIP - No data from caster for 30 s, reconnecting");
                ReconnectRequest();
            }

            //Once all connected set the timer GGA to NTRIP Settings
            if (sendGGAInterval > 0 && ntripCounter == 40 && tmr != null) tmr.Interval = sendGGAInterval * 1000;
        }

        private void SendAuthorization()
        {
            //already sent, now waiting on the caster's response - see OnAddMessage
            if (isAuthSent) return;

            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                ReconnectRequest();
                return;
            }

            // Read the message from settings and send it
            try
            {
                if (!isRawTcp)
                {
                    //encode user and password
                    string auth = ToBase64(username + ":" + password);

                    string htt = isHttp10 ? "1.0" : "1.1";

                    //Build authorization string
                    string str = "GET /" + mount + " HTTP/" + htt + "\r\n";
                    str += "User-Agent: NTRIP AgOpenGPSClient/6.4\r\n";
                    str += "Authorization: Basic " + auth + "\r\n"; //This line can be removed if no authorization is needed
                    str += "Accept: */*\r\nConnection: close\r\n";
                    str += "\r\n";

                    // Convert to byte array and send.
                    byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                    clientSocket.Send(byteDateLine, byteDateLine.Length, 0);

                    //enable to periodically send GGA sentence to server.
                    if (sendGGAInterval > 0 && tmr != null) tmr.Enabled = true;

                    //request sent - wait for the caster's HTTP response before declaring connected (see OnAddMessage)
                    isAuthSent = true;
                    isStarting = false;
                }
                else
                {
                    //raw TCP passthrough, no HTTP handshake to validate
                    IsConnected = true;
                    isStarting = false;
                    isConnecting = false;
                }
            }
            catch (Exception ex)
            {
                ReconnectRequest();
                Log.EventWriter("Catch - > NTRIP Send Authourization: " + ex.ToString());
            }
        }

        private void OnAddMessage(byte[] data)
        {
            //the first reply after sending the request is the caster's HTTP response - validate it
            //before treating anything as RTCM data
            if (isAuthSent && !IsConnected && !isRawTcp)
            {
                string response = Encoding.ASCII.GetString(data);
                int headerEnd = response.IndexOf("\r\n\r\n", StringComparison.Ordinal);
                string headerText = headerEnd >= 0 ? response.Substring(0, headerEnd) : response;

                if (headerText.Contains("200"))
                {
                    IsConnected = true;
                    isConnecting = false;
                    bytesSinceConnected = 0;
                    //the header itself is legitimate traffic - some casters (e.g. a "nearest
                    //mountpoint" alias) need a moment after this to resolve a real station and
                    //start streaming, so don't let the watchdog fire in that gap
                    NTRIP_Watchdog = 0;
                    Log.EventWriter("NTRIP - Caster accepted connection");

                    //some casters send the first RTCM data in the same read as the "200 OK"
                    //header - keep it instead of discarding it below
                    int bodyStart = headerEnd >= 0 ? headerEnd + 4 : data.Length;
                    if (bodyStart >= data.Length) return;

                    byte[] body = new byte[data.Length - bodyStart];
                    Array.Copy(data, bodyStart, body, 0, body.Length);
                    data = body;
                    //fall through to process the leftover RTCM bytes below
                }
                else
                {
                    string firstLine = response.Split('\r')[0];
                    Log.EventWriter("NTRIP - Caster rejected connection: " + firstLine);
                    mf.TimedMessageBox(2500, "NTRIP Connection Rejected", firstLine);
                    ReconnectRequest();
                    return;
                }
            }

            //update gui with stats
            BytesReceived += (uint)data.Length;

            //only count genuine RTCM3 frames (sync byte 0xD3) - a caster can still send extra
            //protocol/error text after its initial "200 OK" before closing the connection
            if (data.Length > 0 && data[0] == 0xD3) bytesSinceConnected += (uint)data.Length;

            //reset watchdog since we have updated data
            NTRIP_Watchdog = 0;

            //move the ntrip stream to queue
            for (int i = 0; i < data.Length; i++)
            {
                rawTrip.Enqueue(data[i]);
            }

            ntripMeterTimer.Enabled = true;
        }

        private void NtripMeterTimer_Tick(object sender, EventArgs e)
        {
            //we really should get here, but have to check
            if (rawTrip.Count == 0) return;

            //how many bytes in the Queue
            int cnt = rawTrip.Count;

            //how many sends have occured
            mf.traffic.cntrGPSIn++;

            //128 bytes chunks max
            if (cnt > mf.packetSizeNTRIP) cnt = mf.packetSizeNTRIP;

            //new data array to send
            byte[] trip = new byte[cnt];

            mf.traffic.cntrGPSInBytes += cnt;

            //dequeue into the array
            for (int i = 0; i < cnt; i++) trip[i] = rawTrip.Dequeue();

            //send it
            if (mf.isSendToSerial) mf.SendGPSPort(trip);
            if (mf.isSendToUDP) mf.SendNtripDatagram(trip);

            bytesToGps += cnt;

            //Are we done?
            if (rawTrip.Count == 0)
            {
                ntripMeterTimer.Enabled = false;
            }

            //Can't keep up as internet dumped a shit load so clear
            if (rawTrip.Count > 10000) rawTrip.Clear();
        }

        private void SendGGA()
        {
            //timer may have brought us here so return if not connected
            if (!IsConnected)
                return;
            // Check we are connected
            if (clientSocket == null || !clientSocket.Connected)
            {
                ReconnectRequest();
                return;
            }

            // Read the message from the text box and send it
            try
            {
                string str = BuildGGA();
                byte[] byteDateLine = Encoding.ASCII.GetBytes(str.ToCharArray());
                clientSocket.Send(byteDateLine, byteDateLine.Length, 0);
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Send GGA" + ex.ToString());

                ReconnectRequest();
            }
        }

        private void NTRIPtick(object o, EventArgs e)
        {
            SendGGA();
        }

        private void OnConnect(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            // Check if we were sucessfull
            try
            {
                //a stale callback from a socket that has since been replaced by a newer connection attempt
                if (sock != clientSocket) return;

                sock.EndConnect(ar);

                if (sock.Connected)
                    sock.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), sock);
            }
            catch (Exception ex)
            {
                //only react if this is still the current connection attempt
                bool isCurrent = (sock == clientSocket);
                if (isCurrent)
                {
                    try { sock.Close(); } catch { /* already closed/disposed */ }
                }

                //marshal logging to the UI thread too - Log.sbEvents is not thread-safe
                string exDetails = ex.ToString();
                string message = ex.Message;
                Post(() =>
                {
                    Log.EventWriter("Catch -> NTRIP OnConnect: " + exDetails);

                    //re-check here - a newer attempt may have replaced clientSocket since the callback ran
                    if (isCurrent && sock == clientSocket)
                    {
                        mf.TimedMessageBox(2500, "NTRIP Connection Failed", message);
                        ReconnectRequest();
                    }
                });
            }
        }

        private void OnRecievedData(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            // Check if we got any data
            try
            {
                //a stale callback from a socket that has since been replaced by a newer connection attempt
                if (sock != clientSocket) return;

                int nBytesRec = sock.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    byte[] localMsg = new byte[nBytesRec];
                    Array.Copy(casterRecBuffer, localMsg, nBytesRec);

                    Post(() =>
                    {
                        //drop data from a socket a newer connection attempt has replaced meanwhile
                        if (sock == clientSocket) OnAddMessage(localMsg);
                    });
                    sock.BeginReceive(casterRecBuffer, 0, casterRecBuffer.Length, SocketFlags.None, new AsyncCallback(OnRecievedData), sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead.
                    // Some casters reject a bad mountpoint/credentials this way - by closing
                    // the connection without ever sending an HTTP response.
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();

                    //evaluate state lazily on the UI thread - it may already have been resolved
                    Post(() =>
                    {
                        //the previous socket's close must not tear down a newer attempt
                        if (sock != clientSocket) return;

                        //this attempt was already handled by another event
                        if (!isConnecting && !IsConnected) return;

                        //a caster can send "200 OK" and still reject the mountpoint by closing again
                        //right away, before any real RTCM data ever flows - treat that as a rejection too
                        bool wasAwaitingResponse = isAuthSent && (!IsConnected || bytesSinceConnected == 0);

                        if (wasAwaitingResponse)
                        {
                            Log.EventWriter("NTRIP - Caster closed connection without sending any RTCM data (check mountpoint/credentials)");
                            mf.TimedMessageBox(2500, "NTRIP Connection Rejected", "Caster closed the connection without sending any data (check mountpoint/credentials)");
                        }
                        else
                        {
                            Log.EventWriter("NTRIP - Connection closed by caster (was connected: " + IsConnected + ", auth sent: " + isAuthSent + ")");
                        }
                        ReconnectRequest();
                    });
                }
            }
            catch (Exception ex)
            {
                //closing the previous socket ends its pending receive this way - expected
                if (ex is ObjectDisposedException) return;

                string exDetails = ex.ToString();
                Post(() =>
                {
                    if (sock != clientSocket) return;
                    Log.EventWriter("Catch -> NTRIP OnRecievedData: " + exDetails);
                });
            }
        }

        //run on the UI thread, where all the state of this client lives
        private void Post(Action action)
        {
            try
            {
                if (!mf.IsDisposed && mf.IsHandleCreated) mf.BeginInvoke((MethodInvoker)(() => action()));
            }
            catch (InvalidOperationException)
            {
                //form closing
            }
        }

        private static string ToBase64(string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return Convert.ToBase64String(byteArray, 0, byteArray.Length);
        }

        //calculate the NMEA checksum to stuff at the end
        private static string CalculateChecksum(string sentence)
        {
            int sum = 0;

            // The checksum calc starts after '$' and ends before '*'
            for (int inx = 1; inx < sentence.Length; inx++)
            {
                char tmp = sentence[inx];
                if (tmp == '*') break;
                sum ^= tmp;    // Build checksum
            }

            // Calculated checksum converted to a 2 digit hex string
            return string.Format("{0:X2}", sum);
        }

        private string BuildGGA()
        {
            double latitude = isGgaManual ? manualLat : mf.latitude;
            double longitude = isGgaManual ? manualLon : mf.longitude;

            //convert to DMS from Degrees
            double latMinu = latitude;
            double longMinu = longitude;

            double latDeg = (int)latitude;
            double longDeg = (int)longitude;

            latMinu -= latDeg;
            longMinu -= longDeg;

            latMinu = Math.Round(latMinu * 60.0, 7);
            longMinu = Math.Round(longMinu * 60.0, 7);

            latDeg *= 100.0;
            longDeg *= 100.0;

            double latNMEA = latMinu + latDeg;
            double longNMEA = longMinu + longDeg;

            char NS = latitude >= 0 ? 'N' : 'S';
            char EW = longitude >= 0 ? 'E' : 'W';

            var sbGGA = new StringBuilder();
            sbGGA.Append("$GPGGA,");
            sbGGA.Append(DateTime.Now.ToString("HHmmss.00,", CultureInfo.InvariantCulture));
            sbGGA.Append(Math.Abs(latNMEA).ToString("0000.000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbGGA.Append(Math.Abs(longNMEA).ToString("00000.000", CultureInfo.InvariantCulture)).Append(',').Append(EW);
            sbGGA.Append(',').Append(mf.fixQualityData.ToString(CultureInfo.InvariantCulture)).Append(',');
            sbGGA.Append(mf.satellitesData.ToString(CultureInfo.InvariantCulture)).Append(',');

            if (mf.hdopData > 0) sbGGA.Append(mf.hdopData.ToString("0.##", CultureInfo.InvariantCulture)).Append(',');
            else sbGGA.Append("1,");

            sbGGA.Append(mf.altitudeData.ToString("0.###", CultureInfo.InvariantCulture)).Append(',');
            sbGGA.Append("M,");
            sbGGA.Append("46.4,M,");  //udulation
            sbGGA.Append(mf.ageData.ToString("0.#", CultureInfo.InvariantCulture)).Append(','); //age
            sbGGA.Append("0*");

            sbGGA.Append(CalculateChecksum(sbGGA.ToString()));
            sbGGA.Append("\r\n");
            return sbGGA.ToString();
        }
    }
}
