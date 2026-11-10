using System;
using System.Globalization;

namespace GPS_Out
{
    public class PGN_GGA
    {
        //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47
        //   0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
        //        Time Lat       Lon FixSatsOP Alt
        //Where:
        //GGA Global Positioning System Fix Data
        // 123519       Fix taken at 12:35:19 UTC
        // 4807.038,N Latitude 48 deg 07.038' N
        // 01131.000,E Longitude 11 deg 31.000' E
        // 1            Fix quality: 0 = invalid
        //                           1 = GPS fix(SPS)
        //                           2 = DGPS fix
        //                           3 = PPS fix
        //                           4 = Real Time Kinematic
        //                           5 = Float RTK
        //                           6 = estimated(dead reckoning) (2.3 feature)
        //                           7 = Manual input mode
        //                           8 = Simulation mode
        // 08           Number of satellites being tracked
        // 0.9          Horizontal dilution of position
        // 545.4, M      Altitude, Meters, above mean sea level
        // 46.9, M       Height of geoid(mean sea level) above WGS84
        //                  ellipsoid
        // (empty field) time in seconds since last DGPS update
        // (empty field) DGPS station ID number
        // *47          the checksum data, always begins with*

        private string cSentence;
        private frmStart mf;

        public PGN_GGA(frmStart CalledFrom)
        {
            mf = CalledFrom;
        }

        public string Sentence
        { get { return cSentence; } }

        public string Build()
        {
            double lat;
            double lon;
            if (mf.UseAOGposition())
            {
                lat = mf.AOGdata.Latitude;
                lon = mf.AOGdata.Longitude;
            }
            else
            {
                lat = mf.AGIOdata.Latitude;
                lon = mf.AGIOdata.Longitude;
            }

            cSentence = Properties.Settings.Default.SentenceStart + "GGA";
            cSentence += "," + DateTime.UtcNow.ToString("HHmmss.ff", CultureInfo.InvariantCulture);

            string NS = ",N";
            if (lat < 0) NS = ",S";
            lat = Math.Abs(lat);
            cSentence += "," + ((int)lat).ToString("D2");
            double Mins = (double)(lat - (int)lat) * 60.0;
            cSentence += Mins.ToString(Properties.Settings.Default.SentencePrecisionFormat, CultureInfo.InvariantCulture);
            cSentence += NS;

            string EW = ",E";
            if (lon < 0) EW = ",W";
            lon = Math.Abs(lon);
            cSentence += "," + ((int)lon).ToString("D3");
            Mins = (double)(lon - (int)lon) * 60.0;
            cSentence += Mins.ToString(Properties.Settings.Default.SentencePrecisionFormat, CultureInfo.InvariantCulture);
            cSentence += EW;

            cSentence += "," + mf.AGIOdata.FixQuality.ToString();

            cSentence += "," + mf.AGIOdata.Satellites.ToString("00");

            cSentence += "," + mf.AGIOdata.HDOP.ToString("N2", CultureInfo.InvariantCulture);

            cSentence += "," + mf.AGIOdata.Altitude.ToString("N3", CultureInfo.InvariantCulture) + ",M";

            cSentence += ",0.0,M";

            cSentence += "," + mf.AGIOdata.Age.ToString("N1", CultureInfo.InvariantCulture) + ",";

            cSentence += "0000*";
            string Hex = mf.CheckSum(cSentence).ToString("X2");
            cSentence += Hex;

            return cSentence;
        }
    }
}