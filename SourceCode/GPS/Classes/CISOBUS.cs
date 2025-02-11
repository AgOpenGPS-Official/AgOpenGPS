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
            data[4] = 0x01; // Length
            data[5] = (byte)(enabled ? 0x01 : 0x00); // Section control enabled request
            mf.SendPgnToLoop(data);
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
