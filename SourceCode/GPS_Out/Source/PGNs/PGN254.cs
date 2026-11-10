using System;

namespace GPS_Out.PGNs
{
    public class PGN254
    {
        // AutoSteer Data
        // 0    header Hi       128 0x80
        // 1    header Lo       129 0x81
        // 2    source          127 0x7F
        // 3    AGIO PGN        254 0xFE
        // 4    length          8
        // 5    speed Lo - kmh X 10
        // 6    speed Hi
        // 7    status
        // 8    steer angle Lo
        // 9    steer angle Hi
        // 10   -
        // 11   Relay Lo
        // 12   Relay Hi
        // 13   CRC

        private readonly frmStart mf;
        private float cSpeed;
        private int PGNlength = 14;
        private DateTime ReceiveTime;

        public PGN254(frmStart CalledFrom)
        {
            mf = CalledFrom;
        }

        public bool Connected()
        {
            return (DateTime.Now - ReceiveTime).TotalSeconds < 4;
        }

        public void ParseByteData(byte[] Data)
        {
            if (Data.Length == PGNlength)
            {
                if (mf.Tls.GoodCRC(Data, 2))
                {
                    cSpeed = (float)((Data[6] << 8 | Data[5]) / 10.0);
                    ReceiveTime = DateTime.Now;
                }
            }
        }

        public double Speed_KMH()
        {
            if (Connected())
            {
                return cSpeed;
            }
            else
            {
                return 0;
            }
        }
    }
}