using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public class CISOBUS
    {
        private readonly FormGPS mf;

        private DateTimeOffset timestamp;

        private bool sectionControlEnabled;
        private bool[] actualSectionStates;

        private int lastGuidanceLineDeviation;
        private DateTimeOffset guidanceLineDeviationTime;
        private int lastActualSpeed;
        private DateTimeOffset actualSpeedTime;
        private int lastTotalDistance;
        private DateTimeOffset totalDistanceTime;


        public CISOBUS(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public bool IsSectionOn(int section)
        {
            if (section < actualSectionStates.Length)
            {
                return actualSectionStates[section];
            }
            return false;
        }

        public void RequestSectionControlEnabled(bool enabled)
        {
            // Send the request
            int data = (byte)(enabled ? 0x01 : 0x00); // Section control enabled request
            SendCanbusMessage(BuildCanFrameIdentifier(), BuildPDdata(160, data, 3));
        }

        public void SendCanbusMessage(int id, byte[] data)
        {
            int length = data.Length;
            byte[] idBytes = BitConverter.GetBytes(id);
            byte[] message = new byte[length + 11];
            message[0] = 0x80; // standard AIO header
            message[1] = 0x81; // PGN header
            message[2] = 0x7F; // SRC address
            message[3] = 0xF2; // PGN
            message[4] = (byte)(length + 5); // Length
            message[5] = 0; //a reserved byte to be able to add some future AOG ecosystem command
            message[6] = idBytes[3];
            message[7] = idBytes[2];
            message[8] = idBytes[1];
            message[9] = idBytes[0];
            for (int i = 0; i < length; i++)
            {
                message[i + 10] = data[i];
            }

            mf.SendPgnToLoop(message);
        }

        public int BuildCanFrameIdentifier(int pgn = 0xCB00, byte priority = 3, byte src_addr = 0x7F, byte dest_addr = 0xFF)
        {
            int id;
            id = (int)(priority & 0x07) << 26;

            /* if a peer to peer message, encode dest_addr */
            if ((pgn > 0 && pgn <= 0xEFFF) || (pgn > 0x10000 && pgn <= 0x1EFFF))
            {
                pgn = pgn & 0x01FF00;
                pgn = pgn | dest_addr;
            }
            id = id | (pgn << 8);
            id = id | src_addr;

            return id;
        }

        //the acronim PD is used for the ISOBUS "Process Data message", the PGN 0xCB00 (51968)
        public byte[] BuildPDdata(ushort identifier, int data, byte command = 0x0F, ushort elementNumber = 0x0FFF, int length = 4)
        {
            if (length < 1) length = 1;
            if (length > 4) length = 4;
            byte[] dataBytes = BitConverter.GetBytes(data);
            byte[] message = new byte[length + 4];
            byte temp = (byte)(elementNumber << 4);
            temp += (byte)(command & 0x0F);
            message[0] = temp;
            message[1] = (byte)(elementNumber >> 4);
            message[2] = (byte)(identifier & 0xFF);
            message[3] = (byte)(identifier >> 8);
            for (int i = 0; i < length; i++)
            {
                message[i + 4] = dataBytes[i];
            }

            return message;
        }

        public void SetGuidanceLineDeviation(int deviation)
        {
            if (deviation == lastGuidanceLineDeviation)
            {
                return;
            }
            if (DateTimeOffset.Now - guidanceLineDeviationTime < TimeSpan.FromMilliseconds(100))
            {
                return;
            }
            lastGuidanceLineDeviation = deviation;
            guidanceLineDeviationTime = DateTimeOffset.Now;
            SendCanbusMessage(BuildCanFrameIdentifier(), BuildPDdata(513, deviation));
        }

        public void SetActualSpeed(int speed)
        {
            if (speed == lastActualSpeed)
            {
                return;
            }
            if (DateTimeOffset.Now - actualSpeedTime < TimeSpan.FromMilliseconds(100))
            {
                return;
            }
            lastActualSpeed = speed;
            actualSpeedTime = DateTimeOffset.Now;
            SendCanbusMessage(BuildCanFrameIdentifier(), BuildPDdata(397, speed));
        }

        public void SetTotalDistance(int distance)
        {
            if (distance == lastTotalDistance)
            {
                return;
            }
            if (DateTimeOffset.Now - totalDistanceTime < TimeSpan.FromMilliseconds(100))
            {
                return;
            }
            lastTotalDistance = distance;
            totalDistanceTime = DateTimeOffset.Now;
            SendCanbusMessage(BuildCanFrameIdentifier(), BuildPDdata(597, distance));
        }

        public bool SectionControlEnabled
        {
            get => sectionControlEnabled;
            private set
            {
                if (sectionControlEnabled == value)
                    return;

                // Changed, act accordingly
                sectionControlEnabled = value;

                if (sectionControlEnabled)
                {
                    mf.btnIsobusSectionControl.Image = Properties.Resources.IsobusSectionControlOn;
                }
                else // Section control disabled
                {
                    mf.btnIsobusSectionControl.Image = Properties.Resources.IsobusSectionControlOff;
                }
            }
        }

        public bool IsAlive()
        {
            // Check if the timestamp is not older than 1 second
            bool isAlive = (timestamp != default && DateTimeOffset.Now - timestamp < TimeSpan.FromSeconds(1));

            mf.btnIsobusSectionControl.Visible = isAlive;

            return isAlive;
        }


        private static bool ReadBit(byte data, int bitIndex)
        {
            return (data & (1 << bitIndex)) != 0;
        }

        public bool DeserializeHeartbeat(byte[] data)
        {
            if (data.Length < 2)
            {
                // Make sure we can read at least the first bitmask and the number of sections
                return false;
            }

            SectionControlEnabled = ReadBit(data[0], 0);
            int numberOfSections = data[1];

            if (data.Length < 2 + (numberOfSections + 7) / 8)
            {
                // Make sure we have enough data to read all the section states
                return false;
            }

            this.actualSectionStates = Enumerable.Range(0, numberOfSections)
                .Select(i => ReadBit(data[2 + (i / 8)], i % 8)) // Section states starts at the 2nd byte
                .ToArray();

            timestamp = DateTimeOffset.Now;
            return true;
        }
    }
}
