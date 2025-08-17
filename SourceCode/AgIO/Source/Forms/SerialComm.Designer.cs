//Please, if you use this, share the improvements

using System.IO.Ports;
using System;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormLoop
    {
        //B5,62,7F,PGN_ID,Length
        private int totalHeaderByteCount = 5;

        public static string portNameGPS = "***";
        public static int baudRateGPS = 4800;

        public static string portNameGPS2 = "***";
        public static int baudRateGPS2 = 4800;

        public static string portNameRtcm = "***";
        public static int baudRateRtcm = 4800;

        public static string portNameIMU = "***";
        public static int baudRateIMU = 38400;

        public static string portNameSteerModule = "***";
        public static int baudRateSteerModule = 38400;

        public static string portNameMachineModule = "***";
        public static int baudRateMachineModule = 38400;

        //public  static string portNameModule3 = "***";
        //public  static int baudRateModule3 = 38400;

        //used to decide to autoconnect section arduino this run
        public string recvGPSSentence = "GPS";
        public string recvGPS2Sentence = "GPS2";
        public string recvIMUSentence = "IMU";
        public string recvSteerModuleSentence = "Module 1";
        public string recvMachineModuleSentence = "Module 2";
        //public string recvModule3Sentence = "Module 3";

        public bool isGPSCommOpen = false;

        public byte checksumSent = 0;
        public byte checksumRecd = 0;

        //used to decide to autoconnect autosteer arduino this run
        public bool wasGPSConnectedLastRun = false;
        //public bool wasModule3ConnectedLastRun = false;
        public bool wasMachineModuleConnectedLastRun = false;
        public bool wasSteerModuleConnectedLastRun = false;
        public bool wasIMUConnectedLastRun = false;
        public bool wasRtcmConnectedLastRun = false;

        //serial port gps is connected to
        public SerialPort spGPS = new SerialPort(portNameGPS, baudRateGPS, Parity.None, 8, StopBits.One);

        //serial port gps2 is connected to
        public SerialPort spGPS2 = new SerialPort(portNameGPS2, baudRateGPS2, Parity.None, 8, StopBits.One);

        //serial port gps is connected to
        public SerialPort spRtcm = new SerialPort(portNameRtcm, baudRateRtcm, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spIMU = new SerialPort(portNameIMU, baudRateIMU, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spSteerModule = new SerialPort(portNameSteerModule, baudRateSteerModule, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spMachineModule = new SerialPort(portNameMachineModule, baudRateMachineModule, Parity.None, 8, StopBits.One);

        //serial port Ardiuno is connected to
        //public SerialPort spModule3 = new SerialPort(portNameModule3, baudRateModule3, Parity.None, 8, StopBits.One);

        //lists for parsing incoming bytes
        private byte[] pgnSteerModule = new byte[22];
        private byte[] pgnMachineModule = new byte[22];
        //private byte[] pgnModule3 = new byte[262];
        private byte[] pgnIMU = new byte[22];

        #region IMUSerialPort //--------------------------------------------------------------------
        private void ReceiveIMUPort(byte[] Data)
        {
            SendToLoopBackMessageAOG(Data);
            traffic.helloFromIMU = 0;
        }

        //Send machine info out to machine board
        public void SendIMUPort(byte[] items, int numItems)
        {
            //Tell Arduino to turn section on or off accordingly
            if (spIMU.IsOpen)
            {
                try
                {
                    spIMU.Write(items, 0, numItems);
                }
                catch (Exception)
                {
                    CloseIMUPort();
                }
            }
        }

        //open the Arduino serial port
        public void OpenIMUPort()
        {
            if (!spIMU.IsOpen)
            {
                spIMU.PortName = portNameIMU;
                spIMU.BaudRate = baudRateIMU;
                spIMU.DataReceived += sp_DataReceivedIMU;
                spIMU.DtrEnable = true;
                spIMU.RtsEnable = true;
            }

            try { spIMU.Open(); }
            catch (Exception ex)
            {
                Log.EventWriter("No Arduino Port, IMU Port Exc: " + ex.ToString());

                MessageBox.Show(ex.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasIMUConnected = false;
                Properties.Settings.Default.Save();
                wasIMUConnectedLastRun = false;
            }

            if (spIMU.IsOpen)
            {
                //short delay for the use of mega2560, it is working in debugmode with breakpoint
                System.Threading.Thread.Sleep(500); // 500 was not enough

                spIMU.DiscardOutBuffer();
                spIMU.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameIMU = portNameIMU;
                Properties.Settings.Default.setPort_wasIMUConnected = true;
                Properties.Settings.Default.Save();
                wasIMUConnectedLastRun = true;
                lblIMUComm.Text = portNameIMU;
            }
        }

        //close the machine port
        public void CloseIMUPort()
        {
            if (spIMU.IsOpen)
            {
                spIMU.DataReceived -= sp_DataReceivedIMU;
                try
                {
                    spIMU.Close();
                    byte[] imuClose = new byte[] { 0x80, 0x81, 0x7C, 0xD4, 2, 1, 0, 0xCC };

                    //tell AOG IMU is disconnected
                    SendToLoopBackMessageAOG(imuClose);
                }

                catch (Exception e)
                {
                    Log.EventWriter("Closing Machine Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasIMUConnected = false;
                Properties.Settings.Default.Save();

                spIMU.Dispose();
                wasIMUConnectedLastRun = false;
            }

            else
            {
                byte[] imuClose = new byte[] { 0x80, 0x81, 0x7C, 0xD4, 2, 1, 0, 0xCC };

                //tell AOG IMU is disconnected
                SendToLoopBackMessageAOG(imuClose);
                wasIMUConnectedLastRun = false;
            }

            wasIMUConnectedLastRun = false;
            lblIMUComm.Text = "---";
        }

        private void sp_DataReceivedIMU(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spIMU.IsOpen)
            {
                byte[] ByteList;
                ByteList = pgnIMU;

                try
                {
                    if (spIMU.BytesToRead > 100)
                    {
                        spIMU.DiscardInBuffer();
                        return;
                    }

                    byte a;

                    int aas = spIMU.BytesToRead;

                    for (int i = 0; i < aas; i++)
                    {
                        a = (byte)spIMU.ReadByte();

                        switch (ByteList[21])
                        {
                            case 0: //find 0x80
                                {
                                    if (a == 128) ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 1:  //find 0x81   
                                {
                                    if (a == 129) ByteList[ByteList[21]++] = a;
                                    else
                                    {
                                        if (a == 181)
                                        {
                                            ByteList[21] = 0;
                                            ByteList[ByteList[21]++] = a;
                                        }
                                        else ByteList[21] = 0;
                                    }
                                    break;
                                }

                            case 2: //Source Address (7F)
                                {
                                    if (a < 128 && a > 120)
                                        ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 3: //PGN ID
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            case 4: //Num of data bytes
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            default: //Data load and Checksum
                                {
                                    if (ByteList[21] > 4)
                                    {
                                        int length = ByteList[4] + totalHeaderByteCount;
                                        if ((ByteList[21]) < length)
                                        {
                                            ByteList[ByteList[21]++] = a;
                                            break;
                                        }
                                        else
                                        {
                                            //crc
                                            int CK_A = 0;
                                            for (int j = 2; j < length; j++)
                                            {
                                                CK_A = CK_A + ByteList[j];
                                            }

                                            //if checksum matches finish and update main thread
                                            if (a == (byte)(CK_A))
                                            {
                                                length++;
                                                ByteList[ByteList[21]++] = (byte)CK_A;
                                                BeginInvoke((MethodInvoker)(() => ReceiveIMUPort(ByteList.Take(length).ToArray())));
                                            }

                                            //clear out the current pgn
                                            ByteList[21] = 0;
                                            return;
                                        }
                                    }

                                    break;
                                }
                        }
                    }
                }
                catch
                {
                    ByteList[21] = 0;
                }
            }
        }
        #endregion ----------------------------------------------------------------

        #region SteerModuleSerialPort //--------------------------------------------------------------------
        private void ReceiveSteerModulePort(byte[] Data)
        {
            SendToLoopBackMessageAOG(Data);
            traffic.helloFromAutoSteer = 0;
        }

        //Send machine info out to machine board
        public void SendSteerModulePort(byte[] items, int numItems)
        {
            //Tell Arduino to turn section on or off accordingly
            if (spSteerModule.IsOpen)
            {
                try
                {
                    spSteerModule.Write(items, 0, numItems);
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch - > Serial Steer module disconnect: " + ex.ToString());
                    CloseSteerModulePort();
                }
            }
        }

        //open the Arduino serial port
        public void OpenSteerModulePort()
        {
            if (!spSteerModule.IsOpen)
            {
                spSteerModule.PortName = portNameSteerModule;
                spSteerModule.BaudRate = baudRateSteerModule;
                spSteerModule.DataReceived += sp_DataReceivedSteerModule;
                spSteerModule.DtrEnable = true;
                spSteerModule.RtsEnable = true;
            }

            try
            {
                spSteerModule.Open();
                //short delay for the use of mega2560, it is working in debugmode with breakpoint
                System.Threading.Thread.Sleep(1000); // 500 was not enough

            }
            catch (Exception e)
            {
                Log.EventWriter("Opening Machine Port" + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasSteerModuleConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spSteerModule.IsOpen)
            {
                spSteerModule.DiscardOutBuffer();
                spSteerModule.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameSteer = portNameSteerModule;
                Properties.Settings.Default.setPort_wasSteerModuleConnected = true;
                Properties.Settings.Default.Save();

                wasSteerModuleConnectedLastRun = true;
                lblMod1Comm.Text = portNameSteerModule;
            }
        }

        //close the machine port
        public void CloseSteerModulePort()
        {
            if (spSteerModule.IsOpen)
            {
                spSteerModule.DataReceived -= sp_DataReceivedSteerModule;
                try { spSteerModule.Close(); }
                catch (Exception e)
                {
                    Log.EventWriter("Closing Machine Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasSteerModuleConnected = false;
                Properties.Settings.Default.Save();

                spSteerModule.Dispose();
            }

            wasSteerModuleConnectedLastRun = false;
            lblMod1Comm.Text = "---";
        }

        private void sp_DataReceivedSteerModule(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spSteerModule.IsOpen)
            {
                byte[] ByteList;
                ByteList = pgnSteerModule;

                try
                {
                    if (spSteerModule.BytesToRead > 100)
                    {
                        spSteerModule.DiscardInBuffer();
                        return;
                    }

                    byte a;

                    int aas = spSteerModule.BytesToRead;

                    for (int i = 0; i < aas; i++)
                    {
                        //traffic.cntrIMUIn++;

                        a = (byte)spSteerModule.ReadByte();

                        switch (ByteList[21])
                        {
                            case 0: //find 0x80
                                {
                                    if (a == 128) ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 1:  //find 0x81   
                                {
                                    if (a == 129) ByteList[ByteList[21]++] = a;
                                    else
                                    {
                                        if (a == 181)
                                        {
                                            ByteList[21] = 0;
                                            ByteList[ByteList[21]++] = a;
                                        }
                                        else ByteList[21] = 0;
                                    }
                                    break;
                                }

                            case 2: //Source Address (7F)
                                {
                                    if (a < 128 && a > 120)
                                        ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 3: //PGN ID
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            case 4: //Num of data bytes
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            default: //Data load and Checksum
                                {
                                    if (ByteList[21] > 4)
                                    {
                                        int length = ByteList[4] + totalHeaderByteCount;
                                        if ((ByteList[21]) < length)
                                        {
                                            ByteList[ByteList[21]++] = a;
                                            break;
                                        }
                                        else
                                        {
                                            //crc
                                            int CK_A = 0;
                                            for (int j = 2; j < length; j++)
                                            {
                                                CK_A = CK_A + ByteList[j];
                                            }

                                            //if checksum matches finish and update main thread
                                            if (a == (byte)(CK_A))
                                            {
                                                length++;
                                                ByteList[ByteList[21]++] = (byte)CK_A;
                                                BeginInvoke((MethodInvoker)(() => ReceiveSteerModulePort(ByteList.Take(length).ToArray())));
                                            }

                                            //clear out the current pgn
                                            ByteList[21] = 0;
                                            return;
                                        }
                                    }

                                    break;
                                }
                        }
                    }
                }
                catch (Exception)
                {
                    ByteList[21] = 0;
                }
            }
        }
        #endregion ----------------------------------------------------------------

        #region MachineModuleSerialPort // Machine Port ------------------------------------------------

        private void ReceiveMachineModulePort(byte[] Data)
        {
            try
            {
                SendToLoopBackMessageAOG(Data);
                traffic.helloFromMachine = 0;
            }
            catch (Exception e)
            {
                Log.EventWriter("Machine Module Send Exc: " + e.ToString());
            }
        }

        //Send machine info out to machine board
        public void SendMachineModulePort(byte[] items, int numItems)
        {
            if (spMachineModule.IsOpen)
            {
                try
                {
                    spMachineModule.Write(items, 0, numItems);
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch - > Serial Machine module disconnect: " + ex.ToString());
                    CloseMachineModulePort();
                }
            }
        }

        //open the Arduino serial port
        public void OpenMachineModulePort()
        {
            if (!spMachineModule.IsOpen)
            {
                spMachineModule.PortName = portNameMachineModule;
                spMachineModule.BaudRate = baudRateMachineModule;
                spMachineModule.DataReceived += sp_DataReceivedMachineModule;
                spMachineModule.DtrEnable = true;
                spMachineModule.RtsEnable = true;
            }

            try
            {
                spMachineModule.Open();
                //short delay for the use of mega2560, it is working in debugmode with breakpoint
                System.Threading.Thread.Sleep(1000); // 500 was not enough

            }
            catch (Exception e)
            {
                Log.EventWriter("Opening Machine Port: " + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasMachineModuleConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spMachineModule.IsOpen)
            {
                spMachineModule.DiscardOutBuffer();
                spMachineModule.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameMachine = portNameMachineModule;
                Properties.Settings.Default.setPort_wasMachineModuleConnected = true;
                Properties.Settings.Default.Save();

                wasMachineModuleConnectedLastRun = true;
                lblMod2Comm.Text = portNameMachineModule;
            }
        }

        //close the machine port
        public void CloseMachineModulePort()
        {
            if (spMachineModule.IsOpen)
            {
                spMachineModule.DataReceived -= sp_DataReceivedMachineModule;
                try { spMachineModule.Close(); }
                catch (Exception e)
                {
                    Log.EventWriter("Closing Machine Serial Port: " + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasMachineModuleConnected = false;
                Properties.Settings.Default.Save();

                spMachineModule.Dispose();
            }

            wasMachineModuleConnectedLastRun = false;
            lblMod2Comm.Text = "---";
        }

        private void sp_DataReceivedMachineModule(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spMachineModule.IsOpen)
            {
                byte[] ByteList;
                ByteList = pgnMachineModule;

                try
                {
                    if (spMachineModule.BytesToRead > 100)
                    {
                        spMachineModule.DiscardInBuffer();
                        return;
                    }

                    byte a;

                    int aas = spMachineModule.BytesToRead;

                    for (int i = 0; i < aas; i++)
                    {
                        //traffic.cntrIMUIn++;

                        a = (byte)spMachineModule.ReadByte();

                        switch (ByteList[21])
                        {
                            case 0: //find 0x80
                                {
                                    if (a == 128) ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 1:  //find 0x81   
                                {
                                    if (a == 129) ByteList[ByteList[21]++] = a;
                                    else
                                    {
                                        if (a == 181)
                                        {
                                            ByteList[21] = 0;
                                            ByteList[ByteList[21]++] = a;
                                        }
                                        else ByteList[21] = 0;
                                    }
                                    break;
                                }

                            case 2: //Source Address (7F)
                                {
                                    if (a < 128 && a > 120)
                                        ByteList[ByteList[21]++] = a;
                                    else ByteList[21] = 0;
                                    break;
                                }

                            case 3: //PGN ID
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            case 4: //Num of data bytes
                                {
                                    ByteList[ByteList[21]++] = a;
                                    break;
                                }

                            default: //Data load and Checksum
                                {
                                    if (ByteList[21] > 4)
                                    {
                                        int length = ByteList[4] + totalHeaderByteCount;
                                        if ((ByteList[21]) < length)
                                        {
                                            ByteList[ByteList[21]++] = a;
                                            break;
                                        }
                                        else
                                        {
                                            //crc
                                            int CK_A = 0;
                                            for (int j = 2; j < length; j++)
                                            {
                                                CK_A = CK_A + ByteList[j];
                                            }

                                            //if checksum matches finish and update main thread
                                            if (a == (byte)(CK_A))
                                            {
                                                ByteList[ByteList[21]++] = (byte)CK_A;
                                                length++;
                                                BeginInvoke((MethodInvoker)(() => ReceiveMachineModulePort(ByteList.Take(length).ToArray())));
                                            }

                                            //clear out the current pgn
                                            ByteList[21] = 0;
                                            return;
                                        }
                                    }

                                    break;
                                }
                        }
                    }
                }
                catch (Exception)
                {
                    ByteList[21] = 0;
                }
            }
        }
        #endregion --------------------------------------------------------------------

        #region Module3SerialPort // --------------------------------------------------------------------
        //private void ReceiveModule3Port(byte[] Data)
        //{
        //    try
        //    {
        //        SendToLoopBackMessageAOG(Data);
        //        if (isPluginUsed) SendToLoopBackMessageVR(Data);
        //        //traffic.cntrModule3Out += Data.Length;
        //    }
        //    catch { }
        //}

        //public void SendModule3Port(byte[] items, int numItems)
        //{
        //    if (spModule3.IsOpen)
        //    {
        //        try
        //        {
        //            spModule3.Write(items, 0, numItems);
        //            //traffic.cntrModule3In += items.Length;
        //        }
        //        catch (Exception)
        //        {
        //            CloseModule3Port();
        //        }
        //    }
        //}

        ////open the Arduino serial port
        //public void OpenModule3Port()
        //{
        //    if (!spModule3.IsOpen)
        //    {
        //        spModule3.PortName = portNameModule3;
        //        spModule3.BaudRate = baudRateModule3;
        //        spModule3.DataReceived += sp_DataReceiveModule3;
        //        spModule3.DtrEnable = true;
        //        spModule3.RtsEnable = true;
        //    }

        //    try
        //    {
        //        spModule3.Open();
        //        //short delay for the use of mega2560, it is working in debugmode with breakpoint
        //        System.Threading.Thread.Sleep(1000); // 500 was not enough

        //    }
        //    catch (Exception e)
        //    {
        //        //WriteErrorLog("Opening Steer Port" + e.ToString());

        //        MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No AutoSteer Port Active");

        //        Properties.Settings.Default.setPort_wasModule3Connected = false;
        //        Properties.Settings.Default.Save();
        //    }

        //    if (spModule3.IsOpen)
        //    {
        //        spModule3.DiscardOutBuffer();
        //        spModule3.DiscardInBuffer();

        //        //update port status label

        //        Properties.Settings.Default.setPort_portNameTool = portNameModule3;
        //        Properties.Settings.Default.setPort_wasModule3Connected = true;
        //        Properties.Settings.Default.Save();

        //        wasModule3ConnectedLastRun = true;
        //    }
        //}

        //public void CloseModule3Port()
        //{
        //    if (spModule3.IsOpen)
        //    {
        //        spModule3.DataReceived -= sp_DataReceiveModule3;
        //        try { spModule3.Close(); }
        //        catch (Exception e)
        //        {
        //            //WriteErrorLog("Closing steer Port" + e.ToString());
        //            MessageBox.Show(e.Message, "Connection already terminated??");
        //        }

        //        Properties.Settings.Default.setPort_wasModule3Connected = false;
        //        Properties.Settings.Default.Save();

        //        spModule3.Dispose();
        //    }

        //    wasModule3ConnectedLastRun = false;

        //}

        ////called by the module delegate every time a chunk is rec'd
        //private void sp_DataReceiveModule3(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    if (spModule3.IsOpen)
        //    {
        //        byte[] ByteList;
        //        ByteList = pgnModule3;

        //        try
        //        {
        //            if (spModule3.BytesToRead > 100)
        //            {
        //                spModule3.DiscardInBuffer();
        //                return;
        //            }

        //            byte a;

        //            int aas = spModule3.BytesToRead;

        //            for (int i = 0; i < aas; i++)
        //            {
        //                //traffic.cntrIMUIn++;

        //                a = (byte)spModule3.ReadByte();

        //                switch (ByteList[21])
        //                {
        //                    case 0: //find 0x80
        //                        {
        //                            if (a == 128) ByteList[ByteList[21]++] = a;
        //                            else ByteList[21] = 0;
        //                            break;
        //                        }

        //                    case 1:  //find 0x81   
        //                        {
        //                            if (a == 129) ByteList[ByteList[21]++] = a;
        //                            else
        //                            {
        //                                if (a == 181)
        //                                {
        //                                    ByteList[21] = 0;
        //                                    ByteList[ByteList[21]++] = a;
        //                                }
        //                                else ByteList[21] = 0;
        //                            }
        //                            break;
        //                        }

        //                    case 2: //Source Address (7F)
        //                        {
        //                            if (a < 128 && a > 120)
        //                                ByteList[ByteList[21]++] = a;
        //                            else ByteList[21] = 0;
        //                            break;
        //                        }

        //                    case 3: //PGN ID
        //                        {
        //                            ByteList[ByteList[21]++] = a;
        //                            break;
        //                        }

        //                    case 4: //Num of data bytes
        //                        {
        //                            ByteList[ByteList[21]++] = a;
        //                            break;
        //                        }

        //                    default: //Data load and Checksum
        //                        {
        //                            if (ByteList[21] > 4)
        //                            {
        //                                int length = ByteList[4] + totalHeaderByteCount;
        //                                if ((ByteList[21]) < length)
        //                                {
        //                                    ByteList[ByteList[21]++] = a;
        //                                    break;
        //                                }
        //                                else
        //                                {
        //                                    //crc
        //                                    int CK_A = 0;
        //                                    for (int j = 2; j < length; j++)
        //                                    {
        //                                        CK_A = CK_A + ByteList[j];
        //                                    }

        //                                    //if checksum matches finish and update main thread
        //                                    if (a == (byte)(CK_A))
        //                                    {
        //                                        ByteList[ByteList[21]++] = (byte)CK_A;
        //                                        BeginInvoke((MethodInvoker)(() => ReceiveModule3Port(ByteList.Take(length).ToArray())));
        //                                    }

        //                                    //clear out the current pgn
        //                                    ByteList[21] = 0;
        //                                    return;
        //                                }
        //                            }

        //                            break;
        //                        }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            ByteList[21] = 0;
        //        }
        //    }
        //}
        #endregion

        #region GPS SerialPort --------------------------------------------------------------------------

        public void SendGPSPort(byte[] data)
        {
            try
            {
                if (spRtcm.IsOpen)
                {
                    spRtcm.Write(data, 0, data.Length);
                }

                else if (spGPS.IsOpen)
                {
                    spGPS.Write(data, 0, data.Length);
                }
            }
            catch (Exception e)
            {
                Log.EventWriter("Opening RTCM Port: " + e.ToString());
            }
        }

        public void OpenGPSPort()
        {

            if (spGPS.IsOpen)
            {
                //close it first
                CloseGPSPort();
            }


            if (!spGPS.IsOpen)
            {
                spGPS.PortName = portNameGPS;
                spGPS.BaudRate = baudRateGPS;
                spGPS.DataReceived += sp_DataReceivedGPS;
                spGPS.WriteTimeout = 1000;
            }

            try { spGPS.Open(); }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Serial GPS Open Fail: " + ex.ToString());
            }

            if (spGPS.IsOpen)
            {
                //discard any stuff in the buffers
                spGPS.DiscardOutBuffer();
                spGPS.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS = portNameGPS;
                Properties.Settings.Default.setPort_baudRateGPS = baudRateGPS;
                Properties.Settings.Default.setPort_wasGPSConnected = true;
                Properties.Settings.Default.Save();
                lblGPS1Comm.Text = portNameGPS;
                wasGPSConnectedLastRun = true;
            }
        }
        public void CloseGPSPort()
        {
            //if (sp.IsOpen)
            {
                //spGPS.DataReceived -= sp_DataReceivedGPS;
                try { spGPS.Close(); }
                catch (Exception e)
                {
                    Log.EventWriter("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS.Text = " * * " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;
                //stripOnlineGPS.Value = 1;
                spGPS.Dispose();
            }
            lblGPS1Comm.Text = "---";
            wasGPSConnectedLastRun = false;

        }

        //called by the GPS delegate every time a chunk is rec'd
        private void ReceiveGPSPort(string sentence)
        {
            rawBuffer += sentence;
            ParseNMEA(ref rawBuffer);

            //SendToLoopBackMessageAOG(sentence);
            traffic.cntrGPSOut += sentence.Length;
            if (isGPSCommOpen) recvGPSSentence = sentence;
        }

        //serial port receive in its own thread
        private void sp_DataReceivedGPS(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS.IsOpen)
            {
                try
                {
                    string sentence = spGPS.ReadExisting();
                    BeginInvoke((MethodInvoker)(() => ReceiveGPSPort(sentence)));
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion SerialPortGPS

        #region GPS2 SerialPort //--------------------------------------------------------------------------

        //called by the GPS2 delegate every time a chunk is rec'd
        private void ReceiveGPS2Port(string sentence)
        {
            //dead end
            //traffic.cntrGPS2Out += sentence.Length;
            recvGPS2Sentence = sentence;
        }
        public void SendGPS2Port(byte[] data)
        {
            try
            {
                if (spGPS2.IsOpen)
                {
                    spGPS2.Write(data, 0, data.Length);
                }
            }
            catch (Exception)
            {
            }

        }
        public void OpenGPS2Port()
        {
            //close it first
            CloseGPS2Port();

            //if (spGPS2.IsOpen)
            //{
            //    //simulatorOnToolStripMenuItem.Checked = false;
            //    //panelSim.Visible = false;
            //    //timerSim.Enabled = false;

            //    //Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            //    //Settings.Default.Save();
            //}


            if (!spGPS2.IsOpen)
            {
                spGPS2.PortName = portNameGPS2;
                spGPS2.BaudRate = baudRateGPS2;
                spGPS2.DataReceived += sp_DataReceivedGPS2;
                spGPS2.WriteTimeout = 1000;
            }

            try { spGPS2.Open(); }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Serial GPS Open Fail: " + ex.ToString());
            }

            if (spGPS2.IsOpen)
            {
                //discard any stuff in the buffers
                spGPS2.DiscardOutBuffer();
                spGPS2.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS2 = portNameGPS2;
                Properties.Settings.Default.setPort_baudRateGPS2 = baudRateGPS2;
                Properties.Settings.Default.Save();
            }
        }
        public void CloseGPS2Port()
        {
            //if (sp.IsOpen)
            {
                spGPS2.DataReceived -= sp_DataReceivedGPS2;
                try { spGPS2.Close(); }
                catch (Exception e)
                {
                    Log.EventWriter("Closing GPS2 Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS2.Text = " * * " + baudRateGPS2.ToString();
                //stripPortGPS2.ForeColor = Color.ForestGreen;
                //stripOnlineGPS2.Value = 1;
                spGPS2.Dispose();
            }
        }

        //serial port receive in its own thread
        private void sp_DataReceivedGPS2(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS2.IsOpen)
            {
                try
                {
                    string sentence = spGPS2.ReadLine();
                    BeginInvoke((MethodInvoker)(() => ReceiveGPS2Port(sentence)));
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion //--------------------------------------------------------

        public void OpenRtcmPort()
        {
            if (spRtcm.IsOpen)
            {
                //close it first
                CloseRtcmPort();
            }

            if (!spRtcm.IsOpen)
            {
                spRtcm.PortName = portNameRtcm;
                spRtcm.BaudRate = baudRateRtcm;
                spRtcm.WriteTimeout = 1000;
            }

            try { spRtcm.Open(); }
            catch (Exception ex)
            {
                Log.EventWriter("Catch - > Serial RTCM Open Fail: " + ex.ToString());
            }

            if (spRtcm.IsOpen)
            {
                //discard any stuff in the buffers
                spRtcm.DiscardOutBuffer();
                spRtcm.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameRtcm = portNameRtcm;
                Properties.Settings.Default.setPort_baudRateRtcm = baudRateRtcm;
                Properties.Settings.Default.setPort_wasRtcmConnected = true;
                Properties.Settings.Default.Save();
                //lblRtcmComm.Text = portNameRtcm;
                wasRtcmConnectedLastRun = true;
            }
        }

        public void CloseRtcmPort()
        {
            {
                try { spRtcm.Close(); }
                catch (Exception e)
                {
                    Log.EventWriter("Closing RTCM Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }
            }

            wasRtcmConnectedLastRun = false;
        }
    }//end class
}//end namespace