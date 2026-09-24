# PGN Protocol Specification

## Overview

AgOpenGPS uses a custom PGN (Parameter Group Number) protocol for UDP communication between AgIO and the main application. The protocol wraps data in a specific format with headers, PGN identifier, length, data payload, and CRC checksum.

## PGN Byte Structure

| Byte | Description | Value |
|------|-------------|-------|
| 0 | Standard AOG header | `0x80` |
| 1 | PGN header | `0x81` |
| 2 | Source address | `0x7F` |
| 3 | PGN identifier | varies |
| 4 | Data length | varies |
| 5+ | Data payload | varies |
| N | CRC checksum | sum of bytes 2 to N-1 |

**Example PGN structure:**
```
[0x80, 0x81, 0x7F, PGN, Length, Data..., CRC]
```

## CRC Calculation

The CRC is a simple checksum calculated as the sum of bytes from index 2 (source address) through the last data byte:

```csharp
byte crc = 0;
for (int i = 2; i < pgn.Length - 1; i++)
{
    crc += pgn[i];
}
pgn[pgn.Length - 1] = (byte)crc;
```

## UDP Network Configuration

| Parameter | Value |
|-----------|-------|
| **AOG Listen Port** | 15555 (loopback) |
| **AgIO Endpoint** | 127.255.255.255:17777 |
| **Protocol** | UDP |
| **Subnet** | 127.x.x.x (loopback) |

**Connection setup:**
```csharp
// AOG binds to loopback port 15555
loopBackSocket.Bind(new IPEndPoint(IPAddress.Loopback, 15555));

// Send to AgIO on 127.255.255.255:17777
EndPoint epAgIO = new IPEndPoint(IPAddress.Parse("127.255.255.255"), 17777);
```

## PGN Messages

### Receiving PGNs (from AgIO)

#### PGN 0xD6 (214) - GPS Position Data

**Length:** 52 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5-12 | Longitude | double | GPS longitude |
| 13-20 | Latitude | double | GPS latitude |
| 21-24 | Heading Dual | float | Dual antenna heading (or MaxValue) |
| 25-28 | Heading True | float | Single antenna heading |
| 29-32 | Speed | float | Ground speed |
| 33-36 | Roll | float | IMU roll angle |
| 37-40 | Altitude | float | GPS altitude |
| 41-42 | Satellites | ushort | Number of satellites |
| 43 | Fix Quality | byte | GPS fix quality |
| 44-45 | HDOP | ushort | Horizontal dilution of precision |
| 46-47 | Age | ushort | Data age |
| 48-49 | IMU Heading | ushort | External IMU heading |
| 50-51 | IMU Roll | short | External IMU roll |
| 52-53 | IMU Pitch | short | External IMU pitch |
| 54-55 | IMU Yaw Rate | short | Angular velocity |

**Special values:**
- `double.MaxValue` / `float.MaxValue` = data not available
- `ushort.MaxValue` / `short.MaxValue` = sensor not connected

#### PGN 0xD3 (211) - External IMU Data

**Length:** 8 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5-6 | Heading | Int16 | IMU heading (div 10 for degrees) |
| 7-8 | Roll | Int16 | IMU roll (div 10 for degrees) |
| 9-10 | Angular Velocity | Int16 | Rotation rate (div -2) |

#### PGN 0xD4 (212) - IMU Disconnect

**Length:** 6 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5 | Disconnect | byte | 1 = IMU disconnected |

#### PGN 253 (0xFD) - Steer Module Response

**Length:** 14 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5-6 | Actual Steer Angle | Int16 | Sensor reading (div 100 for degrees) |
| 7-8 | Heading | Int16 | Module heading (9999 = N/A) |
| 9-10 | Roll | Int16 | Module roll (8888 = N/A) |
| 11 | Switch Status | byte | Bit 0 = work switch, Bit 1 = steer switch |
| 12 | PWM | byte | Actual PWM output |

#### PGN 250 (0xFA) - Sensor Data

**Length:** 6 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5 | Sensor Data | byte | Raw sensor value |

#### PGN 234 (0xEA) - Remote Switches

**Length:** 14 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5-12 | Switch Data | byte[8] | Section switch states |

#### PGN 221 (0xDD) - Display Message

**Length:** variable

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 4 | Length | byte | Number of bytes |
| 5 | Display Time | byte | Seconds to display |
| 6 | Color | byte | 0 = Salmon, other = Bisque |
| 7+ | Message | UTF-8 | Text message |

#### PGN 222 (0xDE) - Remote Commands

**Length:** 7 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5 | Mask | byte | Command mask |
| 6 | Command | byte | Command value |

**Commands:**
- Bit 0: Nudge line (0 = left, 1 = right)
- Bit 1: Cycle line (0 = forward, 1 = backward)

#### PGN 0xF0 (240) - ISOBUS Heartbeat (TC→AOG)

**Length:** variable (min 2 data bytes)

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5 | Status | byte | Bit 0 = section control enabled, Bits 1-3 = number of clients (0-7) |
| 6 | Num Sections | byte | Number of sections (0 = no implement) |
| 7+ | Section States | byte[] | Bitmask per section, 1 bit per section |

### Sending PGNs (to AgIO/Steer Module/TC)

#### PGN 0xF1 (241) - Section Control Enable Request (AOG→TC)

**Length:** 7 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5 | Enabled | byte | 0x01 = enable, 0x00 = disable |

#### PGN 0xF2 (242) - Process Data (AOG→TC)

**Length:** 12 bytes

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 5-6 | Identifier | ushort | Process data identifier (513 = guidance deviation, 397 = speed, 597 = total distance) |
| 7-10 | Value | int32 | Process data value |

#### PGN 0xF3 (243) - Field Name (AOG→TC)

**Length:** variable

| Bytes | Field | Type | Description |
|-------|-------|------|-------------|
| 4 | Length | byte | Number of name bytes (0 = field closed, max 248) |
| 5+ | Name | UTF-8 | Active field folder name |

Sent when:
- Field is opened or changed
- Field is closed (length = 0)
- TC reconnects after >1s absence (current state, open or closed)

#### PGN 0xF4 (244) - Guidance Track Context (AOG→External)

**Length:** 18 bytes (12 data bytes + header/CRC)

**Direction:** AOG → AgIO → UDP network (broadcast to all external listeners)

**Purpose:** Exposes the current guidance-track context so external applications (e.g. ISOBUS Task Controllers implementing AEF TRACK Generation 1) can identify the active guidance pattern and the vehicle's current pass/swath number, including orientation-aware left/right neighbour tracks.

| Byte | Field | Type | Description |
|------|-------|------|-------------|
| 5 | Sequence | byte | Rolling counter (0–255), increments on each send. Use together with Flags to detect stale or out-of-order messages. |
| 6 | Flags | byte | See flag bits below. |
| 7-8 | Guidance Reference ID | uint16 (LE) | Stable identifier for the active guidance pattern. `0` = no active track. Equals `trackIndex + 1` from AOG's internal track list. Changes when the operator switches to a different AB line / curve. |
| 9-10 | Current Track Number | int16 (LE) | Signed pass/swath number relative to the guidance reference. `0` = on the reference line, positive = one side, negative = the other. |
| 11-12 | Track Number Left | int16 (LE) | Track number of the pass immediately to the **left** of the vehicle, relative to its current direction of travel. |
| 13-14 | Track Number Right | int16 (LE) | Track number of the pass immediately to the **right** of the vehicle, relative to its current direction of travel. |
| 15-16 | Swath Width | uint16 (LE) | Distance between adjacent guidance tracks in **millimetres** (tool width minus overlap, the same spacing the track numbers are derived from). `0` when no active track (Valid flag clear). |
| 17 | CRC | byte | Checksum (sum of bytes 2–16) |

**Flag bits (byte 6):**

| Bit | Name | Description |
|-----|------|-------------|
| 0 | Valid | `1` = data is valid (active guidance track exists). `0` = no active guidance; track fields should be ignored. |
| 1 | Heading Same Way | `1` = vehicle is travelling in the same direction as the guidance reference heading. `0` = vehicle is travelling opposite to the reference heading. |
| 2 | Mode | `0` = AB line guidance, `1` = curve guidance. |
| 3-7 | Reserved | Reserved for future use, always `0`. |

**Behaviour and timing:**

- Sent whenever the track context **changes** (pass number, reference ID, heading direction, swath width, or validity).
- Minimum send interval: **100 ms** (rate-limited to avoid flooding).
- When no guidance track is active (e.g. field just opened, no AB line created), the message is sent with `Flags = 0x00` and `Guidance Reference ID = 0`.

**Guidance Reference ID stability:**

- The ID remains constant while the operator drives passes on the same AB line or curve (e.g. passes …, -2, -1, 0, 1, 2, …).
- The ID changes when the operator selects a different guidance pattern (different AB line or curve).
- The ID is `trackIndex + 1` where `trackIndex` is the zero-based index into AOG's internal track list. This means the ID is a small positive integer (1, 2, 3, …) that is stable within a session.

**Left/Right track determination:**

Left and right are determined relative to the **vehicle's direction of travel**, not the guidance reference heading:

- When `Heading Same Way = 1` (vehicle going same direction as reference):
  - `trackLeft = currentTrack - 1`
  - `trackRight = currentTrack + 1`
- When `Heading Same Way = 0` (vehicle going opposite to reference):
  - `trackLeft = currentTrack + 1`
  - `trackRight = currentTrack - 1`

This means an external application does **not** need to reproduce AOG's heading/orientation logic — the left and right values are always correct from the operator's perspective.

**ISOBUS TRACK mapping:**

For implementors mapping this to AEF ISOBUS TRACK Generation 1:

| ISOBUS TRACK field | PGN 0xF4 field |
|---|---|
| Guidance Reference ID | Guidance Reference ID (bytes 7-8) |
| Actual track number | Current Track Number (bytes 9-10) |
| Track to the left | Track Number Left (bytes 11-12) |
| Track to the right | Track Number Right (bytes 13-14) |
| Guidance line swath width (DDI 512) | Swath Width (bytes 15-16, mm) |

**Decoding example (C#):**

```csharp
// Assuming 'data' is the received PGN byte array
if (data[0] == 0x80 && data[1] == 0x81 && data[3] == 0xF4)
{
    byte sequence   = data[5];
    byte flags      = data[6];
    bool isValid    = (flags & 0x01) != 0;
    bool headingSame = (flags & 0x02) != 0;
    bool isCurve    = (flags & 0x04) != 0;

    ushort refId       = BitConverter.ToUInt16(data, 7);
    short currentTrack = BitConverter.ToInt16(data, 9);
    short trackLeft    = BitConverter.ToInt16(data, 11);
    short trackRight   = BitConverter.ToInt16(data, 13);
    ushort swathWidthMm = BitConverter.ToUInt16(data, 15);

    if (isValid)
    {
        // Use refId, currentTrack, trackLeft, trackRight, swathWidthMm
    }
}
```

#### PGN 0xD0 (208) - Latitude/Longitude

**Length:** 14 bytes

```csharp
byte[] latLong = new byte[] { 0x80, 0x81, 0x7F, 0xD0, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
```

**Encoding:**
```csharp
int encodedAngle = (int)(lat * (0x7FFFFFFF / 90.0));
int encodedAngle = (int)(lon * (0x7FFFFFFF / 180.0));
```

#### PGN 0xFE (254) - AutoSteer Data

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Speed Lo | Speed low byte |
| 6 | Speed Hi | Speed high byte |
| 7 | Status | AutoSteer status |
| 8 | Steer Angle Lo | Commanded steer angle low |
| 9 | Steer Angle Hi | Commanded steer angle high |
| 10 | Line Distance | Distance from guidance line |
| 11 | Section Control 1-8 | Bitmask for sections |
| 12 | Section Control 9-16 | Bitmask for sections |

#### PGN 0xFC (252) - AutoSteer Settings

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Gain Proportional | Kp value |
| 6 | High PWM | Maximum PWM |
| 7 | Low PWM | Minimum PWM when driving |
| 8 | Min PWM | Absolute minimum PWM |
| 9 | Counts Per Degree | WAS sensor counts |
| 10 | WAS Offset Lo | Low byte |
| 11 | WAS Offset Hi | High byte |
| 12 | Ackerman | Ackerman compensation |

#### PGN 0xFB (251) - AutoSteer Config

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Set0 | Configuration byte 0 |
| 6 | Max Pulse | Maximum pulse count |
| 7 | Min Speed | Minimum speed for steering |
| 8 | Ackerman Fix | Ackerman fix value |
| 9 | Angular Velocity | Max angular velocity |

#### PGN 0xEF (239) - Machine Data

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | U-Turn | U-Turn state |
| 6 | Speed | Vehicle speed |
| 7 | Hyd Lift | Hydraulic lift state |
| 8 | Tram | Tramline state |
| 9 | Geo Stop | Geofence stop |
| 11 | Section Control 1-8 | Section states |
| 12 | Section Control 9-16 | Section states |

#### PGN 0xEE (238) - Machine Config

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Raise Time | Hydraulic raise time |
| 6 | Lower Time | Hydraulic lower time |
| 7 | Enable Hyd | Hydraulics enabled |
| 8 | Set0 | User setting 0 |
| 9 | User1 | User defined 1 |
| 10 | User2 | User defined 2 |
| 11 | User3 | User defined 3 |
| 12 | User4 | User defined 4 |

#### PGN 0xEC (236) - Relay Config

**Length:** 29 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5-28 | Pin Config | Pin configuration for 24 relays |

**Pin config format:** Comma-separated string `"1,2,3,0,..."` converted to bytes.

#### PGN 0xEB (235) - Section Dimensions

**Length:** 38 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5-6 | Section 0 | Width for section 0 (Lo, Hi) |
| 7-8 | Section 1 | Width for section 1 |
| ... | ... | ... |
| 35-36 | Section 15 | Width for section 15 |
| 37 | Num Sections | Number of sections |

#### PGN 0xE5 (229) - Section Control (Extended)

**Length:** 16 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Section 1-8 | Bitmask |
| 6 | Section 9-16 | Bitmask |
| 7 | Section 17-24 | Bitmask |
| 8 | Section 25-32 | Bitmask |
| 9 | Section 33-40 | Bitmask |
| 10 | Section 41-48 | Bitmask |
| 11 | Section 49-56 | Bitmask |
| 12 | Section 57-64 | Bitmask |
| 13 | Tool Left Speed | Left tool speed |
| 14 | Tool Right Speed | Right tool speed |

#### PGN 0xE4 (228) - Rate Control

**Length:** 14 bytes

| Byte Index | Field | Description |
|------------|-------|-------------|
| 5 | Rate 0 | Rate setting 0 |
| 6 | Rate 1 | Rate setting 1 |
| 7 | Rate 2 | Rate setting 2 |

## 4-Second Timeout Mechanism

GPS_Out uses a 4-second timeout for NMEA sentence forwarding:

```csharp
// If no valid GPS data for 4 seconds, stop sending
if (age > 4.0) {
    // Stop output
}
```

This prevents stale data from being sent to external devices.

## Integration Example

```csharp
// Sending PGN to AgIO
public void SendPgnToLoop(byte[] byteData)
{
    if (loopBackSocket != null && byteData.Length > 2)
    {
        // Calculate CRC
        int crc = 0;
        for (int i = 2; i + 1 < byteData.Length; i++)
        {
            crc += byteData[i];
        }
        byteData[byteData.Length - 1] = (byte)crc;

        // Send to endpoint
        loopBackSocket.BeginSendTo(byteData, 0, byteData.Length,
            SocketFlags.None, epAgIO, SendAsyncLoopData, null);
    }
}
```

```csharp
// Receiving PGN from AgIO
private void ReceiveFromAgIO(byte[] data)
{
    // Validate header
    if (data.Length > 4 && data[0] == 0x80 && data[1] == 0x81)
    {
        // Validate CRC
        int Length = Math.Max((data[4]) + 5, 5);
        byte CK_A = 0;
        for (int j = 2; j < Length; j++)
        {
            CK_A += data[j];
        }

        if (data[Length] != (byte)CK_A)
        {
            return; // CRC mismatch
        }

        // Process by PGN
        switch (data[3])
        {
            case 0xD6: // GPS Position
                // Process...
                break;
            // ... other PGNs
        }
    }
}
```

## Related Files

- `Forms/UDPComm.Designer.cs` - UDP communication implementation
- `Forms/PGN.Designer.cs` - PGN message definitions
- `Classes/CISOBUS.cs` - ISOBUS PGN handling
