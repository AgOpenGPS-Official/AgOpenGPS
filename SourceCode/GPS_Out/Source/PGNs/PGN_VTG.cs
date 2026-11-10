using System.Globalization;

namespace GPS_Out
{
    public class PGN_VTG
    {
        // $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
        //*
        //   VTG          Track made good and ground speed
        //   054.7,T True track made good(degrees)
        //   034.4,M Magnetic track made good
        //   005.5,N Ground speed, knots
        //   010.2,K Ground speed, Kilometers per hour
        //   *48          Checksum

        private string cSentence;
        private frmStart mf;

        public PGN_VTG(frmStart CalledFrom)
        {
            mf = CalledFrom;
        }

        public string Sentence
        { get { return cSentence; } }

        public string Build()
        {
            cSentence = Properties.Settings.Default.SentenceStart + "VTG";

            cSentence += "," + mf.Heading().ToString("000.0", CultureInfo.InvariantCulture) + ",T";

            cSentence += "," + mf.Heading().ToString("000.0", CultureInfo.InvariantCulture) + ",M";

            double knots = mf.AGIOdata.Speed * 0.5399568;
            cSentence += "," + knots.ToString("000.0", CultureInfo.InvariantCulture) + ",N";

            cSentence += "," + mf.AGIOdata.Speed.ToString("000.0", CultureInfo.InvariantCulture) + ",K";

            cSentence += "," + mf.ModeIndicator(mf.AGIOdata.FixQuality) + "*";
            //cSentence += "*";
            string Hex = mf.CheckSum(cSentence).ToString("X2");
            cSentence += Hex;

            return cSentence;
        }
    }
}