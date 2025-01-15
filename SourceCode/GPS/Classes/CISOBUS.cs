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

        private bool wasAlive;
        private long timestamp;

        private bool sectionControlEnabled;
        private List<bool> actualSectionStates = new List<bool>();

        public CISOBUS(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public bool IsSectionOn(int section)
        {
            if (section < actualSectionStates.Count)
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
            data[4] = 0x02; // Length
            data[5] = (byte)(enabled ? 0x01 : 0x00); // Section control enabled request
            mf.SendPgnToLoop(data);
        }

        public bool IsSectionControlEnabled()
        {
            return sectionControlEnabled;
        }

        private void SetSectionControlEnabled(bool enabled)
        {
            if (sectionControlEnabled != enabled)
            {
                // Changed, act accordingly
                sectionControlEnabled = enabled;

                if (sectionControlEnabled)
                {
                    mf.btnIsobusSC.Image = Properties.Resources.IsobusSectionControlOn;
                }
                else // Section control disabled
                {
                    mf.btnIsobusSC.Image = Properties.Resources.IsobusSectionControlOff;
                }
            }
        }

        public bool IsAlive()
        {
            // Check if the timestamp is not older than 1 second
            bool isAlive = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timestamp < 1000;

     
            if (wasAlive && !isAlive)
            {
                // Lost connection
                mf.btnIsobusSC.Visible = false;
            }
            else if (!wasAlive && isAlive)
            {
                // Reconnected
                mf.btnIsobusSC.Visible = true;
            }

            wasAlive = isAlive;
            return isAlive;
        }


        private static bool ReadBit(byte[] data, int bitIndex)
        {
            int byteIndex = bitIndex / 8; // Find the byte
            if (data.Length <= byteIndex)
            {
                return false;
            }
            int bitPosition = bitIndex % 8; // Find the bit within the byte
            return (data[byteIndex] & (1 << bitPosition)) != 0;
        }

        private static void WriteBit(ref byte[] data, int bitIndex, bool value)
        {
            int byteIndex = bitIndex / 8; // Find the byte
            if (data.Length <= byteIndex)
            {
                Array.Resize(ref data, byteIndex + 1);
            }
            int bitPosition = bitIndex % 8; // Find the bit within the byte
            if (value)
            {
                data[byteIndex] |= (byte)(1 << bitPosition);
            }
            else
            {
                data[byteIndex] &= (byte)~(1 << bitPosition);
            }
        }


        public bool DeserializeHeartbeat(byte[] data)
        {
            if (data.Length < 2)
            {
                // Make sure we can read at least the first bitmask and the number of sections
                return false;
            }

            SetSectionControlEnabled(ReadBit(data, 0));
            int numberOfSections = data[1];

            if (data.Length != 2 + Math.Ceiling(numberOfSections / 8.0))
            {
                // Make sure we have enough data to read all the section states
                return false;
            }

            for (int i = 0; i < numberOfSections; i++)
            {
                bool sectionState = ReadBit(data, 16 + i);
                if (this.actualSectionStates.Count <= i)
                    this.actualSectionStates.Add(sectionState);
                else
                    this.actualSectionStates[i] = sectionState;
            }

            // Truncate the list if there are more elements than needed
            if (this.actualSectionStates.Count > numberOfSections)
            {
                this.actualSectionStates.RemoveRange(numberOfSections, this.actualSectionStates.Count - numberOfSections);
            }
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return true;
        }
    }
}
