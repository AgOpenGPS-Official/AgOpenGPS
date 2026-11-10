using System;
using System.Net;
using System.Net.Sockets;

namespace GPS_Out
{
    public class UDPComm
    {
        private readonly frmStart mf;
        private byte[] buffer = new byte[1024];
        private int cReceivePort;   // local ports must be unique for each app on same pc and each class instance
        private IPAddress cSourceIP;
        private HandleDataDelegateObj HandleDataDelegate = null;
        private Socket recvSocket;

        public UDPComm(frmStart CallingForm, int ReceivePort, string SourceIPaddress)
        {
            mf = CallingForm;
            cReceivePort = ReceivePort;
            SetSourceIP(SourceIPaddress);
        }

        // Status delegate
        private delegate void HandleDataDelegateObj(int port, byte[] msg);

        public void StartUDPServer()
        {
            try
            {
                // initialize the delegate which updates the message received
                HandleDataDelegate = HandleData;

                // initialize the receive socket
                recvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                recvSocket.Bind(new IPEndPoint(cSourceIP, cReceivePort));

                // Initialise the IPEndPoint for the client - async listner client only!
                EndPoint client = new IPEndPoint(IPAddress.Any, 0);

                // Start listening for incoming data
                recvSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref client, new AsyncCallback(ReceiveData), recvSocket);
            }
            catch (Exception e)
            {
                mf.Tls.WriteErrorLog("UDPcomm/StartUDPServer: \n" + e.Message);
            }
        }

        private void HandleData(int Port, byte[] Data)
        {
            try
            {
                if (Data.Length > 1)
                {
                    int PGN = Data[1] << 8 | Data[0];

                    switch (PGN)
                    {
                        case 33152: // AOG, 0x8180
                            int SubPGN = Data[3] << 8 | Data[2];
                            switch (SubPGN)
                            {
                                case 0xD67C: // AGIO NEMA translation
                                    mf.AGIOdata.ParseByteData(Data);
                                    break;

                                case 0x647F: // AOG roll corrected lat,lon
                                    mf.AOGdata.ParseByteData(Data);
                                    break;

                                case 0xFE7F: // AutoSteer data from AOG for simulated speed
                                    mf.AutoSteerPGN.ParseByteData(Data);
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                mf.Tls.WriteErrorLog("UDPcomm/HandleData " + ex.Message);
            }
        }

        private void ReceiveData(IAsyncResult asyncResult)
        {
            try
            {
                // Initialise the IPEndPoint for the client
                EndPoint epSender = new IPEndPoint(IPAddress.Any, 0);

                // Receive all data
                int msgLen = recvSocket.EndReceiveFrom(asyncResult, ref epSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                recvSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);

                int port = ((IPEndPoint)epSender).Port;
                // Update status through a delegate
                mf.Invoke(HandleDataDelegate, new object[] { port, localMsg });
            }
            catch (ObjectDisposedException)
            {
                // do nothing
            }
            catch (Exception ex)
            {
                //mf.Tls.ShowHelp("ReceiveData Error \n" + e.Message, "Comm", 3000, true);
                mf.Tls.WriteErrorLog("UDPcomm/ReceiveData " + ex.Message);
            }
        }

        private void SetSourceIP(string Source)
        {
            try
            {
                if (IPAddress.TryParse(Source, out IPAddress tmp))
                {
                    cSourceIP = tmp;
                }
                else
                {
                    cSourceIP = IPAddress.Any;
                }
            }
            catch (Exception ex)
            {
                mf.Tls.WriteErrorLog("UDPcomm/SetSourceEP " + ex.Message);
            }
        }
    }
}
