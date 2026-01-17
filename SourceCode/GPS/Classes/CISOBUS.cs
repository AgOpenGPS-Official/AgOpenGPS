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

        //const for the CANBUS sending
        private const int pdPGN = 0xCB00; //the acronim PD is used for the ISOBUS "Process Data message"
        private const byte priority = 5;
        private const byte sourceAddress = 0x7F; //for now we send the AOG address
        private const byte allDest = 255;
        private const byte notDefinedCommand = 0x0F;
        private const ushort notDefinedElementNumber = 0xFFFF;

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
            byte[] data = new byte[7];
            data[0] = 0x80; // standard AIO header
            data[1] = 0x81; // PGN header
            data[2] = 0x7F; // SRC address
            data[3] = 0xF1; // PGN
            data[4] = 1; // Length
            data[5] = (byte)(enabled ? 0x01 : 0x00); // Section control enabled request
            mf.SendPgnToLoop(data);
        }

        private void SendCanbusMessage(int pgn, byte priority, byte scr_addr, byte dest_addr, byte[] data)
        {
            byte[] pgnBytes = BitConverter.GetBytes(pgn);
            byte[] message = new byte[19];
            message[0] = 0x80; // standard AIO header
            message[1] = 0x81; // PGN header
            message[2] = scr_addr; // SRC address
            message[3] = 0xF2; // PGN
            message[4] = 13; // Length
            message[5] = pgnBytes[0];
            message[6] = pgnBytes[1];
            message[7] = pgnBytes[2]; //we need only the first 2 bytes and one bit, other stuff could be added eventualy in the remaning 7.
            message[8] = priority;
            message[9] = dest_addr;
            for (int i = 0; i < 8; i++)
            {
                message[i + 10] = data[i];
            }

            mf.SendPgnToLoop(message);
        }

        //the acronim PD is used for the ISOBUS "Process Data message", the PGN 0xCB00 (51968)
        private byte[] PDdata(byte command, ushort elementNumber, ushort identifier, int data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            byte[] message = new byte[8];
            byte temp = (byte)(elementNumber << 4);
            temp += (byte)command;
            message[0] = temp;
            message[1] = (byte)(elementNumber >> 4);
            message[2] = (byte)(identifier & 0xFF);
            message[3] = (byte)(identifier >> 8);
            message[4] = dataBytes[0];
            message[5] = dataBytes[1];
            message[6] = dataBytes[2];
            message[7] = dataBytes[3];

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
            SendCanbusMessage(pdPGN, priority, sourceAddress, allDest, PDdata(notDefinedCommand, notDefinedElementNumber, 513, deviation));
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
            SendCanbusMessage(pdPGN, priority, sourceAddress, allDest, PDdata(notDefinedCommand, notDefinedElementNumber, 397, speed));
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
            SendCanbusMessage(pdPGN, priority, sourceAddress, allDest, PDdata(notDefinedCommand, notDefinedElementNumber, 597, distance));
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

            if (data.Length != 2 + (numberOfSections + 7) / 8)
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
