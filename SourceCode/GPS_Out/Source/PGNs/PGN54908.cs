using System;

namespace GPS_Out
{
    public class PGN54908
    {
        // data from AGIO
        // 0        0x80
        // 1        0x81
        // 2        0x7C
        // 3        0xD6
        // 4        0x33    array length - 6
        // 5-12     longitude       double
        // 13-20    latitude        double
        // 21-24    headingDual     float
        // 25-28    heading         float
        // 29-32    speed           float
        // 33-36    roll            float
        // 37-40    altitude        float
        // 41-42    satellites      ushort
        // 43       fixQuality
        // 44-45    hdopX100        ushort
        // 46-47    ageX100         ushort
        // 48-49    imuHeading      ushort
        // 50-51    imuRoll         ushort
        // 52-53    imuPitch        ushort
        // 54-55    imuYaw          ushort
        // 56       CRC

        private const byte cByteCount = 57;
        private ushort cAgeX100;
        private float cAltitude;
        private byte cFixQuality;
        private ushort cHdopX100;
        private float cHeadingDual;
        private float cImuHeading;
        private double cLatitude;
        private double cLongitude;
        private ushort cSatellites;
        private float cSpeed;
        private float cTrueHeading;
        private frmStart mf;
        private DateTime ReceiveTime;

        public PGN54908(frmStart CalledFrom)
        {
            mf = CalledFrom;
        }

        public float Age
        {
            get
            {
                if (Connected())
                {
                    return (float)(cAgeX100 / 100.0);
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 1.8F;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float Altitude
        {
            get
            {
                if (Connected())
                {
                    return cAltitude;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 732.0F;
                }
                else
                {
                    return 0;
                }
            }
        }

        public byte FixQuality
        {
            get
            {
                if (Connected())
                {
                    return cFixQuality;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 4;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float HDOP
        {
            get
            {
                if (Connected())
                {
                    return (float)(cHdopX100 / 100.0);
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 7;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float HeadingDual
        {
            get
            {
                if (Connected())
                {
                    return cHeadingDual;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 1000;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float IMUheading
        {
            get
            {
                if (Connected())
                {
                    return cImuHeading;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 1000;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Latitude
        {
            get
            {
                if (Connected())
                {
                    return cLatitude;
                }
                else
                {
                    return 0;
                }
            }
        }

        public double Longitude
        {
            get
            {
                if (Connected())
                {
                    return cLongitude;
                }
                else
                {
                    return 0;
                }
            }
        }

        public UInt16 Satellites
        {
            get
            {
                if (Connected())
                {
                    return cSatellites;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 12;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float Speed
        {
            get
            {
                float Result = 0;
                if (Connected())
                {
                    if (cSpeed < 100) Result = cSpeed;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    // simulated speed: AOG's, when it sends it, else a fixed one
                    if (mf.AutoSteerPGN.Connected())
                    {
                        Result = (float)mf.AutoSteerPGN.Speed_KMH();
                    }
                    else
                    {
                        Result = 4.8F;
                    }
                }
                return Result;
            }
        }

        public float TrueHeading
        {
            get
            {
                if (Connected())
                {
                    return cTrueHeading;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    return 1000;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool Connected()
        {
            return (DateTime.Now - ReceiveTime).TotalSeconds < 4;
        }

        public bool ParseByteData(byte[] Data)
        {
            bool Result = false;
            if (mf.Tls.GoodCRC(Data, 2))
            {
                cLongitude = BitConverter.ToDouble(Data, 5);
                cLatitude = BitConverter.ToDouble(Data, 13);
                cHeadingDual = BitConverter.ToSingle(Data, 21);
                cTrueHeading = BitConverter.ToSingle(Data, 25);
                cSpeed = BitConverter.ToSingle(Data, 29);
                cAltitude = BitConverter.ToSingle(Data, 37);
                cSatellites = BitConverter.ToUInt16(Data, 41);
                cFixQuality = Data[43];
                cHdopX100 = BitConverter.ToUInt16(Data, 44);
                cAgeX100 = BitConverter.ToUInt16(Data, 46);
                cImuHeading = (float)(BitConverter.ToUInt16(Data, 48) / 10.0);

                ReceiveTime = DateTime.Now;
                Result = true;
            }
            return Result;
        }
    }
}
