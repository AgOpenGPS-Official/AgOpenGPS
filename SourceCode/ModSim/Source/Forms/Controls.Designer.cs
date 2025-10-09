using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Forms;
using AgLibrary.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;

namespace ModSim
{
    public partial class FormSim
    {
        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setGPS_SimLatitude = (double)nudLat.Value;
            Properties.Settings.Default.setGPS_SimLongitude = (double)nudLon.Value;
            Properties.Settings.Default.Save();
            latitude = Properties.Settings.Default.setGPS_SimLatitude;
            longitude = Properties.Settings.Default.setGPS_SimLongitude;
        }

        private void lblWAS_Click(object sender, EventArgs e)
        {
            tbarSteerAngleWAS.Value = 0;
            steerAngle = 0;
            lblWAS.Text = "Steer: 0.0°";
        }
        private void lblKmh_Click(object sender, EventArgs e)
        {
            tbarSpeed.Value = 0;
            lblKmh.Text = "Kmh: 0.0";
            mSec.Text = "M/Sec: 0.0";
        }

        private void tbarSteerAngleWAS_Scroll(object sender, EventArgs e)
        {
            steerAngleActual = tbarSteerAngleWAS.Value * 0.01;
            lblWAS.Text = "Steer: " + (steerAngleActual).ToString("N2") + "°";
        }

        private void tbarSpeed_Scroll(object sender, EventArgs e)
        {
            if (tbarSpeed.Value < 0) lblKmh.BackColor = Color.Salmon;
            else lblKmh.BackColor = Color.LightGreen;

            lblKmh.Text = "Kmh: " + (tbarSpeed.Value * 0.1).ToString("N1");
            mSec.Text = "M/Sec: " + (tbarSpeed.Value * 0.027777777777).ToString("N1");
        }

        private void tbarRoll_Scroll(object sender, EventArgs e)
        {
            roll = (double)tbarRoll.Value * 0.1;
            rollIMU = (int)(roll * 10);
            lblRoll.Text = "Roll: " + (roll).ToString("N2") + "°";
        }

        private void lblRoll_Click(object sender, EventArgs e)
        {
            roll = 0;
            rollIMU = 0;
            lblRoll.Text = "Roll: 0°";
            tbarRoll.Value = 0;
        }

        private void btnSteerButtonRemote_Click(object sender, EventArgs e)
        {
            if (steerSwitch > 0) steerSwitch = 0;
            else steerSwitch = 1;
            btnSteerButtonRemote.BackColor = Color.Green;
        }

        private void cboxSteerSwitchRemote_Click(object sender, EventArgs e)
        {
            if (cboxSteerSwitchRemote.Checked) steerSwitch = 0;
            else steerSwitch = 1;
        }

        private void cboxWorkSwitch_Click(object sender, EventArgs e)
        {
            if (cboxWorkSwitch.Checked) workSwitch = 0;
            else workSwitch = 1;
        }

        public void TimedMessageBox(int timeout, string title, string message)
        {
            var form = new FormTimedMessage(timeout, title, message);
            form.Show();
        }

        public void YesMessageBox(string s1)
        {
            var form = new FormYes(s1);
            form.ShowDialog(this);
        }

        #region GPS Simulator

        private string TimeNow = "";

        //Our two new nmea strings
        private readonly StringBuilder sbOGI = new StringBuilder();
        private readonly StringBuilder sbNDA = new StringBuilder();

        private readonly StringBuilder sbHDT = new StringBuilder();
        private readonly StringBuilder sbRMC = new StringBuilder();

        private readonly StringBuilder sbGGA = new StringBuilder();
        private readonly StringBuilder sbVTG = new StringBuilder();
        private readonly StringBuilder sbAVR = new StringBuilder();
        private readonly StringBuilder sbKSXT = new StringBuilder();

        //The entire string to send out
        private readonly StringBuilder sbSendText = new StringBuilder();

        //GPS related properties
        private readonly int fixQuality = 8, sats = 12;

        private readonly double HDOP = 0.9;
        public double altitude = 300;
        private char EW = 'W';
        private char NS = 'N';

        public double latitude, longitude;

        private double latDeg, latMinu, longDeg, longMinu, latNMEA, longNMEA;
        public double speed = 0.6, headingTrue, stepDistance = 0.05, steerAngle;
        private double degrees, roll = 0;

        private int rollIMU = 0, headingIMU = 0;//, pitchIMU = 0;

        private const double ToRadians = 0.01745329251994329576923690768489, ToDegrees = 57.295779513082325225835265587528;

        //The checksum of an NMEA line
        private string sumStr = "";

        private void simTimer_Tick(object sender, EventArgs e)
        {
            stepDistance = tbarSpeed.Value * 0.027777777777 * (0.1);

            if (guidanceStatus == 0)
                steerAngle = tbarSteerAngleWAS.Value * 0.01;
            else
            {
                steerAngle = steerAngleSetPoint;
                tbarSteerAngleWAS.Value = (int)(steerAngleSetPoint);
                steerAngleActual = steerAngle;
                lblWAS.Text = "Steer: " + (steerAngleActual).ToString("N2") + "°";
            }

            double temp = (stepDistance * Math.Tan(steerAngle * 0.02) / 2.5);
            headingTrue += temp;

            if (headingTrue > (2.0 * Math.PI)) headingTrue -= (2.0 * Math.PI);
            if (headingTrue < 0) headingTrue += (2.0 * Math.PI);

            degrees = ToDegrees * headingTrue;

            headingIMU = (int)(degrees * 10);

            lblHeading.Text = (headingTrue * 57.29577951308).ToString("N2") + '°';

            CalculateNewPostionFromBearingDistance(ToRadians * latitude, ToRadians * longitude, headingTrue, stepDistance / 1000.0);

            lblCurrentLon.Text = longitude.ToString("N7");
            lblCurrentLat.Text = latitude.ToString("N7");

            //calc the speed
            speed = Math.Round(1.944 * stepDistance * 1.0 / (0.1), 1);

            TimeNow = DateTime.UtcNow.ToString("HHmmss.fff,", CultureInfo.InvariantCulture);

            if (cboxVTG.Checked)
            {
                BuildVTG();
                sbSendText.Append(sbVTG.ToString());
                SendUDPMessage(sbVTG.ToString());
            }
            if (cboxAVR.Checked)
            {
                BuildAVR();
                sbSendText.Append(sbAVR.ToString());
                SendUDPMessage(sbAVR.ToString());
            }
            if (cboxHDT.Checked)
            {
                BuildHDT();
                sbSendText.Append(sbHDT.ToString());
                SendUDPMessage(sbHDT.ToString());
            }
            if (cboxGGA.Checked)
            {
                BuildGGA();
                sbSendText.Append(sbGGA.ToString());
                SendUDPMessage(sbGGA.ToString());
            }
            if (cboxRMC.Checked)
            {
                BuildRMC();
                sbSendText.Append(sbRMC.ToString());
                SendUDPMessage(sbRMC.ToString());
            }
            if (cboxOGI.Checked)
            {
                BuildOGI();
                sbSendText.Append(sbOGI.ToString());
                SendUDPMessage(sbOGI.ToString());
            }

            if (cboxNDA.Checked)
            {
                BuildNDA();
                sbSendText.Append(sbNDA.ToString());
                SendUDPMessage(sbNDA.ToString());
            }

            if (cboxKSXT.Checked)
            {
                BuildKSXT();
                sbSendText.Append(sbKSXT.ToString());
                SendUDPMessage(sbKSXT.ToString());
            }

            sbSendText.Clear();
        }

        public void CalculateNewPostionFromBearingDistance(double lat, double lng, double bearing, double distance)
        {
            double R = distance / 6371.0; // Earth Radius in Km

            double lat2 = Math.Asin((Math.Sin(lat) * Math.Cos(R)) + (Math.Cos(lat) * Math.Sin(R) * Math.Cos(bearing)));
            double lon2 = lng + Math.Atan2(Math.Sin(bearing) * Math.Sin(R) * Math.Cos(lat), Math.Cos(R) - (Math.Sin(lat) * Math.Sin(lat2)));

            latitude = ToDegrees * lat2;
            longitude = ToDegrees * lon2;

            //convert to DMS from Degrees
            latMinu = latitude;
            longMinu = longitude;

            latDeg = (int)latitude;
            longDeg = (int)longitude;

            latMinu -= latDeg;
            longMinu -= longDeg;

            latMinu = Math.Round(latMinu * 60.0, 7);
            longMinu = Math.Round(longMinu * 60.0, 7);

            latDeg *= 100.0;
            longDeg *= 100.0;

            latNMEA = latMinu + latDeg;
            longNMEA = longMinu + longDeg;

            if (latitude >= 0) NS = 'N';
            else NS = 'S';
            if (longitude >= 0) EW = 'E';
            else EW = 'W';
        }

        //calculate the NMEA checksum to stuff at the end
        public void CalculateChecksum(string Sentence)
        {
            int sum = 0, inx;
            char[] sentence_chars = Sentence.ToCharArray();
            char tmp;
            // All character xor:ed results in the trailing hex checksum
            // The checksum calc starts after '$' and ends before '*'
            for (inx = 1; ; inx++)
            {
                tmp = sentence_chars[inx];
                // Indicates end of data and start of checksum
                if (tmp == '*')
                    break;
                sum ^= tmp;    // Build checksum
            }
            // Calculated checksum converted to a 2 digit hex string
            sumStr = String.Format("{0:X2}", sum);
        }

        private void BuildGGA()
        {
            sbGGA.Clear();
            sbGGA.Append("$GPGGA,");
            sbGGA.Append(TimeNow);
            sbGGA.Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbGGA.Append(Math.Abs(longNMEA).ToString("00000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(EW).Append(',');
            sbGGA.Append(fixQuality.ToString(CultureInfo.InvariantCulture)).Append(',').Append(sats.ToString(CultureInfo.InvariantCulture)).Append(',').Append(HDOP.ToString(CultureInfo.InvariantCulture)).Append(',').Append("1000");
            sbGGA.Append(",M,46.9,M,37.1,,*");

            CalculateChecksum(sbGGA.ToString());
            sbGGA.Append(sumStr);
            sbGGA.Append("\r\n");
        }

        private void BuildVTG()
        {
            sbVTG.Clear();
            sbVTG.Append("$GPVTG,");
            sbVTG.Append(degrees.ToString("N5", CultureInfo.InvariantCulture));
            sbVTG.Append(",T,034.4,M,");
            sbVTG.Append(speed.ToString(CultureInfo.InvariantCulture));
            sbVTG.Append(",N,");
            sbVTG.Append((speed * 1.852).ToString(CultureInfo.InvariantCulture));
            sbVTG.Append(",K*");

            CalculateChecksum(sbVTG.ToString());
            sbVTG.Append(sumStr);
            sbVTG.Append("\r\n");
        }

        private void BuildHDT()
        {
            sbHDT.Clear();
            sbHDT.Append("$GNHDT,");
            sbHDT.Append(degrees.ToString("N5", CultureInfo.InvariantCulture));
            sbHDT.Append(",T*");

            CalculateChecksum(sbHDT.ToString());
            sbHDT.Append(sumStr);
            sbHDT.Append("\r\n");
        }

        private void BuildAVR()
        {
            sbAVR.Clear();
            sbAVR.Append("$PTNL,AVR,");
            sbAVR.Append(TimeNow);
            sbAVR.Append(degrees.ToString("N5", CultureInfo.InvariantCulture)); //field 2

            sbAVR.Append(",Yaw,-2.1,Tilt,"); //field 3,4,5

            sbAVR.Append(roll.ToString() + ",Roll,"); //field 6,7

            sbAVR.Append("444.232,3,1.2,17*"); //field 8 thru 12

            CalculateChecksum(sbAVR.ToString());
            sbAVR.Append(sumStr);
            sbAVR.Append("\r\n");
        }

        private void BuildOGI()
        {

            sbOGI.Clear();
            sbOGI.Append("$PAOGI,");

            sbOGI.Append(TimeNow);
            sbOGI.Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbOGI.Append(Math.Abs(longNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(EW).Append(',');

            sbOGI.Append(fixQuality.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(sats.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(HDOP.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append("1000,3.2,")                                                                    //10
                .Append(speed.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(degrees.ToString("N5", CultureInfo.InvariantCulture)).Append(',')
                .Append(roll.ToString(CultureInfo.InvariantCulture)).Append(",0.12,359.9,T*");

            CalculateChecksum(sbOGI.ToString());
            sbOGI.Append(sumStr);
            sbOGI.Append("\r\n");
        }

        private void BuildNDA()
        {

            sbNDA.Clear();
            sbNDA.Append("$PANDA,");

            sbNDA.Append(TimeNow);
            sbNDA.Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',');
            sbNDA.Append(Math.Abs(longNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(EW).Append(',');

            sbNDA.Append(fixQuality.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(sats.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(HDOP.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append("1000,3.2,")                                                                    //10
                .Append(speed.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append((headingIMU).ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append((rollIMU).ToString(CultureInfo.InvariantCulture)).Append(",32,298").Append("*");

            CalculateChecksum(sbNDA.ToString());
            sbNDA.Append(sumStr);
            sbNDA.Append("\r\n");
        }

        private void BuildKSXT()
        {
            sbKSXT.Clear();
            sbKSXT.Append("$KSXT,"); //1

            sbKSXT.Append(TimeNow);
            sbKSXT.Append(Math.Abs(longitude).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',');
            sbKSXT.Append(Math.Abs(latitude).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',');

            sbKSXT.Append(altitude.ToString(CultureInfo.InvariantCulture)).Append(',') //altitude
                .Append(degrees.ToString("N5", CultureInfo.InvariantCulture)) //true heading
                .Append(",22,35,") // Pitch, SpeedAngle
                .Append(speed.ToString(CultureInfo.InvariantCulture)).Append(',')
                .Append(roll.ToString(CultureInfo.InvariantCulture)).Append(",3,3,13,-1075,-98,-8,,,,37,13,,")
                .Append("*3FCF0C9B");

            //sbKSXT.Append(sumStr);
            sbKSXT.Append("\r\n");
        }

        /*
                $KSXT $KSXT Log header

                2 20191219093115.00 YYYYMMDDhhmmss.ss Satellite time in format of yyyymmddhhmmss.ss,
                                    e.g. 2016040106284180 means
                                    2016(year)4(month)1(day)06(hour)28(mins)41.80(secs)

                3 112.87713062 x1 Longitude(°)

                4 28.23315515 x2 Latitude(°)

                5 65.5618 x3 Height (m)

                6 0.00 x4 Yaw, the angle between the line connecting two
                antennas and True North (primary antenna
                positioning and secondary antenna heading) (0°
                360°)

                7 0.00 x5 Pitch (-90° 90°)

                8 336.65 x6 Speed angle, the angle between vehicle traveling
                direction and True North (0° 360°)

                9 0.010 x7 Speed in vehicle traveling direction (km/h)

                10 x8 Roll (-90° 90°)

                11 3 x9 Positioning status: 0-invalid solution; 1-single point solution; 
                                            2-RTK floating point; 3-RTK fixed point

                12 0 x10 Heading status: 0-invalid solution; 1-single point solution; 
                                        2-RTK floating point; 3-RTK fixed point

                13 0 x11 Number of satellites used in heading

                14 23 x12 Number of satellites used in positioning (primary antenna)

                15 -1075.146 x13 East position under geographic coordinates with
                                the base station as the origin (m) (empty if none)

                16 -98.462 x14 North position under geographic coordinates with
                the base station as the origin (m) (empty if none)

                17 -8.618 x15 Up position under geographic coordinates with
                the base station as the origin (m) (empty if none)

                18 -0.004 x16 East speed under geographic coordinates (km/h)
                (empty if none)

                19 0.009 x17 North speed under geographic coordinates (km/h)
                (empty if none)

                20 0.004 x18 Up speed under geographic coordinates (km/h)
                (empty if none)

                21 1.0 x19 Age of differential

                22 30 x20 Number of satellites tracked in base station

                23 x23 Reserved

                23 Parity 3FCF0C9B XOR check sum (Hex string, check from the
                beginning of the frame)


                        */

        private void BuildRMC()
        {
            sbRMC.Clear();
            sbRMC.Append("$GPRMC,")
            .Append(TimeNow).Append("A,")
            .Append(Math.Abs(latNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(NS).Append(',')
            .Append(Math.Abs(longNMEA).ToString("0000.0000000", CultureInfo.InvariantCulture)).Append(',').Append(EW).Append(',')
            .Append(speed.ToString(CultureInfo.InvariantCulture)).Append(',')
            .Append(degrees.ToString("N5", CultureInfo.InvariantCulture))
            .Append(",230394,359.9*");

            CalculateChecksum(sbRMC.ToString());
            sbRMC.Append(sumStr);
            sbRMC.Append("\r\n");
        }

        #endregion
    }
}


