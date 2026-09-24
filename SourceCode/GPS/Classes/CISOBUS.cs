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
        private int numClients;
        private bool[] actualSectionStates;

        private int lastGuidanceLineDeviation;
        private DateTimeOffset guidanceLineDeviationTime;
        private int lastActualSpeed;
        private DateTimeOffset actualSpeedTime;
        private int lastTotalDistance;
        private DateTimeOffset totalDistanceTime;

        // Guidance Track Context (PGN 0xF4)
        private byte trackContextSequence;
        private int lastTrackContextRefId;
        private int lastTrackContextCurrent;
        private int lastTrackContextLeft;
        private int lastTrackContextRight;
        private int lastTrackContextWidth;
        private byte lastTrackContextFlags;
        private DateTimeOffset trackContextTime;


        public CISOBUS(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public bool IsSectionOn(int section)
        {
            // If no section states available, don't override - return false to let AOG control sections
            if (actualSectionStates == null || actualSectionStates.Length == 0)
            {
                return false;
            }
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

        /// <summary>
        /// Send the active field folder name to TC (PGN 0xF3).
        /// Call with empty string when field is closed.
        /// </summary>
        public void SendFieldName(string fieldName)
        {
            byte[] nameBytes = string.IsNullOrEmpty(fieldName)
                ? Array.Empty<byte>()
                : Encoding.UTF8.GetBytes(fieldName);

            // Cap at 248 bytes to stay within PGN length byte range
            if (nameBytes.Length > 248) nameBytes = nameBytes.Take(248).ToArray();

            byte[] message = new byte[6 + nameBytes.Length];
            message[0] = 0x80; // standard AgIO header
            message[1] = 0x81;
            message[2] = 0x7F; // SRC address
            message[3] = 0xF3; // PGN: field name
            message[4] = (byte)nameBytes.Length; // 0 = field closed
            Array.Copy(nameBytes, 0, message, 5, nameBytes.Length);
            mf.SendPgnToLoop(message);
        }

        private void SendProcessData(ushort identifier, int data)
        {
            byte[] dataBytes = BitConverter.GetBytes(data);
            byte[] message = new byte[12];
            message[0] = 0x80; // standard AIO header
            message[1] = 0x81; // PGN header
            message[2] = 0x7F; // SRC address
            message[3] = 0xF2; // PGN
            message[4] = 6; // Length
            message[5] = (byte)(identifier & 0xFF);
            message[6] = (byte)(identifier >> 8);
            message[7] = dataBytes[0];
            message[8] = dataBytes[1];
            message[9] = dataBytes[2];
            message[10] = dataBytes[3];
            mf.SendPgnToLoop(message);
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
            SendProcessData(513, deviation);
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
            SendProcessData(397, speed);
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
            SendProcessData(597, distance);
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
                UpdateButtonImage();
            }
        }

        /// <summary>
        /// Number of clients connected to the Task Controller (0-7)
        /// </summary>
        public int NumClients => numClients;

        /// <summary>
        /// Number of sections reported by TC (0 = no section control capability)
        /// </summary>
        private int numberOfSections;

        private void UpdateButtonImage()
        {
            // 3 states:
            // - Idle: TC running but no implement connected (numberOfSections == 0)
            // - On: TC running with implement, section control enabled
            // - Off: TC running with implement, section control disabled
            if (numberOfSections == 0)
            {
                mf.btnIsobusSectionControl.Image = Properties.Resources.IsobusSectionControlIdle;
            }
            else if (sectionControlEnabled)
            {
                mf.btnIsobusSectionControl.Image = Properties.Resources.IsobusSectionControlOn;
            }
            else
            {
                mf.btnIsobusSectionControl.Image = Properties.Resources.IsobusSectionControlOff;
            }
        }

        /// <summary>
        /// Send guidance track context (PGN 0xF4) to external applications.
        /// Provides: reference ID, current track number, left/right track numbers, swath width.
        /// </summary>
        public void SendGuidanceTrackContext()
        {
            bool hasActiveTrack = mf.trk.gArr != null
                && mf.trk.gArr.Count > 0
                && mf.trk.idx >= 0
                && mf.trk.idx < mf.trk.gArr.Count;

            ushort refId = 0;
            short currentTrack = 0;
            short trackLeft = 0;
            short trackRight = 0;
            ushort swathWidthMm = 0;
            byte flags = 0;

            if (hasActiveTrack)
            {
                CTrk track = mf.trk.gArr[mf.trk.idx];
                refId = (ushort)(mf.trk.idx + 1);

                // Mode bits (bits 2-3): 0=AB, 1=Curve
                if (track.mode == TrackMode.Curve)
                    flags |= 0x04;

                int pathsAway;
                bool isHeadingSameWay;

                if (track.mode == TrackMode.AB)
                {
                    pathsAway = mf.ABLine.howManyPathsAway;
                    isHeadingSameWay = mf.ABLine.isHeadingSameWay;
                }
                else
                {
                    pathsAway = mf.curve.howManyPathsAway;
                    isHeadingSameWay = mf.curve.isHeadingSameWay;
                }

                // Bit 1: heading same way
                if (isHeadingSameWay)
                    flags |= 0x02;

                currentTrack = (short)pathsAway;

                // Left/right relative to vehicle direction of travel.
                // When heading same way as reference: left = current-1, right = current+1
                // When heading opposite:              left = current+1, right = current-1
                if (isHeadingSameWay)
                {
                    trackLeft = (short)(currentTrack - 1);
                    trackRight = (short)(currentTrack + 1);
                }
                else
                {
                    trackLeft = (short)(currentTrack + 1);
                    trackRight = (short)(currentTrack - 1);
                }

                // Track spacing as used for howManyPathsAway in CABLine / CABCurve (widthMinusOverlap)
                double swathWidth = Math.Round((mf.tool.width - mf.tool.overlap) * 1000.0);
                swathWidthMm = (ushort)Math.Max(0, Math.Min(65535, swathWidth));

                // Bit 0: data valid
                flags |= 0x01;
            }

            // Change detection — only send when something changed, with 100 ms minimum interval
            if (refId == lastTrackContextRefId
                && currentTrack == lastTrackContextCurrent
                && trackLeft == lastTrackContextLeft
                && trackRight == lastTrackContextRight
                && swathWidthMm == lastTrackContextWidth
                && flags == lastTrackContextFlags)
            {
                return;
            }

            if (DateTimeOffset.Now - trackContextTime < TimeSpan.FromMilliseconds(100))
            {
                return;
            }

            lastTrackContextRefId = refId;
            lastTrackContextCurrent = currentTrack;
            lastTrackContextLeft = trackLeft;
            lastTrackContextRight = trackRight;
            lastTrackContextWidth = swathWidthMm;
            lastTrackContextFlags = flags;
            trackContextTime = DateTimeOffset.Now;

            byte seq = trackContextSequence++;

            // PGN 0xF4 — Guidance Track Context, 18 bytes total
            byte[] message = new byte[18];
            message[0] = 0x80; // AOG header
            message[1] = 0x81; // PGN header
            message[2] = 0x7F; // SRC address
            message[3] = 0xF4; // PGN: Guidance Track Context
            message[4] = 12;   // data payload length

            message[5] = seq;                                      // sequence counter
            message[6] = flags;                                    // validity / heading / mode flags
            message[7] = (byte)(refId & 0xFF);                     // ref ID lo
            message[8] = (byte)(refId >> 8);                       // ref ID hi

            byte[] curBytes = BitConverter.GetBytes(currentTrack);
            message[9] = curBytes[0];                              // current track lo
            message[10] = curBytes[1];                             // current track hi

            byte[] leftBytes = BitConverter.GetBytes(trackLeft);
            message[11] = leftBytes[0];                            // track left lo
            message[12] = leftBytes[1];                            // track left hi

            byte[] rightBytes = BitConverter.GetBytes(trackRight);
            message[13] = rightBytes[0];                           // track right lo
            message[14] = rightBytes[1];                           // track right hi

            message[15] = (byte)(swathWidthMm & 0xFF);             // swath width mm lo
            message[16] = (byte)(swathWidthMm >> 8);               // swath width mm hi

            // CRC is computed by SendPgnToLoop into message[17]
            mf.SendPgnToLoop(message);
        }

        public bool IsAlive()
        {
            // Button visible = TC is running (heartbeat received within 1 second)
            // Button image indicates implement status (idle/on/off)
            bool isAlive = (timestamp != default &&
                           DateTimeOffset.Now - timestamp < TimeSpan.FromSeconds(1));

            mf.btnIsobusSectionControl.Visible = isAlive;

            return isAlive;
        }

        /// <summary>
        /// Check if TC has active clients with section control capability
        /// </summary>
        public bool HasActiveClients => numberOfSections > 0;


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

            // Detect reconnect: TC was dead (no heartbeat for >1s) and is now alive again
            bool wasAlive = timestamp != default && DateTimeOffset.Now - timestamp < TimeSpan.FromSeconds(1);
            if (!wasAlive)
                SendFieldName(mf.isJobStarted ? mf.currentFieldDirectory : string.Empty);

            // Extract fields from byte 0 (backward compatible):
            // Bit 0: Section control enabled (existing)
            // Bits 1-3: Number of clients (0-7) - may be 0 for older TC versions
            // Bits 4-7: Reserved
            bool newSectionControlEnabled = ReadBit(data[0], 0);
            int newNumClients = (data[0] >> 1) & 0x07;
            int newNumberOfSections = data[1];

            // Store client count for informational purposes (backward compatible: old TCs send 0)
            numClients = newNumClients;

            // If no sections, don't expect section states - show idle state
            // This handles: no clients, clients without sections, or old TCs that don't support section control
            if (newNumberOfSections == 0)
            {
                numberOfSections = 0;
                sectionControlEnabled = newSectionControlEnabled;
                actualSectionStates = null;
                timestamp = DateTimeOffset.Now;
                UpdateButtonImage();
                return true;
            }

            // Validate we have enough data for section states
            if (data.Length != 2 + (newNumberOfSections + 7) / 8)
            {
                // Make sure we have enough data to read all the section states
                return false;
            }

            // Has sections - proceed with section control (backward compatible with old TCs)
            this.numberOfSections = newNumberOfSections;
            this.SectionControlEnabled = newSectionControlEnabled;
            this.actualSectionStates = Enumerable.Range(0, newNumberOfSections)
                .Select(i => ReadBit(data[2 + (i / 8)], i % 8)) // Section states starts at the 2nd byte
                .ToArray();

            timestamp = DateTimeOffset.Now;
            return true;
        }
    }
}
