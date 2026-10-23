# AgOpenGPS Architecture

## Overview

AgOpenGPS receives GPS/IMU data via AgIO, calculates guidance, and outputs steering commands.

```
GPS/IMU Hardware → AgIO → UDP (127.x.x.x) → AgOpenGPS → Steering Output
                                                    ↓
                                                 Guidance
                                                    ↓
                                              CAN/Serial
                                                    ↓
                                              AutoSteer
```

## Projects

| Project | Purpose |
|---------|---------|
| `GPS/` | Main application - core logic and state management |
| `AgOpenGPS.Core/` | **Shared library** - Geo models, conversions, helpers, file I/O, ViewModels |
| `AgIO/` | I/O handler - NTRIP, GPS, IMU, UDP |
| `GPS_Out/` | NMEA serial output (4s timeout) |
| `AgDiag/` | Diagnostic tools |
| `ModSim/` | Module simulator |
| `Keypad/` | Hardware keypad support |

## Business Logic Distribution

| Location | Purpose |
|----------|---------|
| `GPS/Classes/` | Main application logic, state management, guidance calculations |
| `AgOpenGPS.Core/Models/` | **Geo models** (GeoCoord, Wgs84, conversions), Field models, Guidance models |
| `AgOpenGPS.Core/DrawLib/` | OpenGL drawing utilities |
| `AgOpenGPS.Core/Streamers/` | File I/O (save/load field data) |
| `AgOpenGPS.Core/ViewModels/` | MVVM pattern for UI binding |

## Main Application Structure (GPS/)

| Directory | Purpose |
|-----------|---------|
| `Classes/` | Core application logic, state, guidance calculations |
| `Forms/` | UI (organized by feature: Config/, Field/, Guidance/, Sources/, Tram/) |
| `Properties/` | Settings (Vehicle/Tool/Environment) |
| `Controls/` | Custom UI controls |

## UDP Communication

### Network Configuration

| Parameter | Value |
|-----------|-------|
| **AOG Listen Port** | 15555 (loopback) |
| **AgIO Endpoint** | 127.255.255.255:17777 |
| **Protocol** | UDP |
| **Subnet** | 127.x.x.x (loopback) |
| **Buffer Size** | 1024 bytes |

### PGN Message Format

All UDP messages use the AgOpenGPS PGN format:

```
[0x80, 0x81, 0x7F, PGN, Length, Data..., CRC]
```

| Byte | Description |
|------|-------------|
| 0 | AOG Header (`0x80`) |
| 1 | PGN Header (`0x81`) |
| 2 | Source Address (`0x7F`) |
| 3 | PGN Identifier |
| 4 | Data Length |
| 5+ | Data Payload |
| N | CRC Checksum |

### Key PGN Messages

| PGN (Hex) | PGN (Dec) | Direction | Purpose |
|-----------|-----------|-----------|---------|
| 0xD6 | 214 | AgIO→AOG | GPS position (lat/lon, heading, speed, roll) |
| 0xD3 | 211 | AgIO→AOG | External IMU data |
| 0xD4 | 212 | AgIO→AOG | IMU disconnect notification |
| 0xFD | 253 | Module→AOG | Steer module response (angle, switches) |
| 0xFE | 254 | AOG→Module | AutoSteer data (speed, angle, sections) |
| 0xFC | 252 | AOG→Module | AutoSteer settings (Kp, PWM, WAS) |
| 0xEF | 239 | AOG→Module | Machine data (U-turn, hydraulics) |
| 0xEE | 238 | AOG→Module | Machine config (hydraulic times) |
| 0xEC | 236 | AOG→Module | Relay pin configuration |
| 0xEB | 235 | AOG→Module | Section dimensions |
| 0xF0 | 240 | TC→AOG | ISOBUS heartbeat (section states, num clients) |
| 0xF1 | 241 | AOG→TC | Section control enable request |
| 0xF2 | 242 | AOG→TC | Process data (guidance deviation, speed, distance) |
| 0xF3 | 243 | AOG→TC | Active field folder name (UTF-8, empty = closed) |
| 0xF4 | 244 | AOG→External | Guidance track context (ref ID, current/left/right track) |

See [PGN Protocol](pgn-protocol.md) for complete specification.

## Data Flow

### GPS Data Flow

```
1. GPS Hardware (NMEA)
   ↓
2. AgIO (NTRIP client, GPS parsing)
   ↓
3. UDP Port 15555 (PGN 0xD6)
   ↓
4. FormGPS.ReceiveFromAgIO()
   ↓
5. AppModel.CurrentLatLon (Wgs84)
   ↓
6. LocalPlane.ConvertWgs84ToGeoCoord()
   ↓
7. pn.fix (northing, easting)
```

### Guidance Data Flow

```
1. GPS Position (pn.fix)
   ↓
2. Guidance Line Calculation (CTrack, CABLine, CContour)
   ↓
3. CGuidance (Stanley/Pure Pursuit algorithm)
   ↓
4. Steer Angle Calculation
   ↓
5. PGN 0xFE to Steer Module
   ↓
6. CAN/Serial to AutoSteer Hardware
```

### Section Control Flow

```
1. Tool Position (CTool)
   ↓
2. Section State (CSection)
   ↓
3. Boundary Check (CFence)
   ↓
4. Headland Check (CHead)
   ↓
5. Relay Output (PGN 0xEF)
   ↓
6. Arduino Machine Module
```

## Steering Output

### AutoSteer Module Communication

The steering module communicates via UDP loopback:

```
AgOpenGPS → PGN 0xFE (AutoSteer Data) → Steer Module
                ↓
           PGN 0xFD (Response)
                ↓
        AgOpenGPS reads actual angle, switches
```

### Steering Algorithms

#### Stanley Algorithm

Used when `setVehicle_isStanleyUsed = true`.

**Formula:**
```
steerAngle = atan((distanceError * gain) / speed) + headingError * gain
```

**Gains:**
- `stanleyDistanceErrorGain` - Distance error multiplier
- `stanleyHeadingErrorGain` - Heading error multiplier
- `stanleyIntegralGainAB` - Integral gain for AB lines

**Code location:** `Classes/CGuidance.cs` - `DoSteerAngleCalc()`

#### Pure Pursuit Algorithm

Used when `setVehicle_isStanleyUsed = false`.

**Formula:**
```
goalPoint = findPointOnLine(lookAheadDistance)
steerAngle = atan2(2 * wheelbase * sin(error), lookahead)
```

**Gains:**
- `purePursuitIntegralGainAB` - Integral gain for AB lines
- `goalPointLookAheadHold` - Lookahead when holding
- `goalPointLookAheadMult` - Lookahead multiplier

**Code location:** `Classes/CTrackMethods.cs` - `GoalPoint()`

### Steering Parameters (Vehicle)

| Setting | Description |
|---------|-------------|
| `setAS_Kp` | Proportional gain |
| `setAS_highSteerPWM` | Maximum PWM output |
| `setAS_lowSteerPWM` | Minimum PWM when driving |
| `setAS_minSteerPWM` | Absolute minimum PWM |
| `setAS_countsPerDegree` | WAS sensor counts per degree |
| `setAS_wasOffset` | WAS sensor zero offset |
| `setAS_ackerman` | Ackerman compensation (%) |
| `setAS_sideHillComp` | Side-hill compensation factor |
| `setVehicle_maxSteerAngle` | Maximum steering angle (degrees) |
| `setVehicle_maxAngularVelocity` | Max turn rate (deg/sec) |

## Component Relationships

```
FormGPS (Main Form)
    ├── AppModel (Global state)
    ├── pn (Position/Navigation data)
    ├── ahrs (IMU/AHRS data)
    ├── vehicle (Vehicle config)
    ├── tool (Tool config)
    │
    ├── Guidance System
    │   ├── CTrack (Current track state)
    │   ├── CABLine (AB line guidance)
    │   ├── CContour (Contour guidance)
    │   ├── CGuidance (Steering calculations)
    │   └── CYouTurn (U-turn logic)
    │
    ├── Section Control
    │   ├── CSection (Individual section state)
    │   ├── CTool (Tool configuration)
    │   ├── CHead (Headland detection)
    │   └── CFence (Boundary/geofence)
    │
    ├── Communication
    │   ├── CModuleComm (Module communication)
    │   └── CISOBUS (ISOBUS support)
    │
    └── Data
        ├── CFieldData (Field management)
        ├── CBoundary (Boundary data)
        ├── CTram (Tramlines)
        └── CRecordedPath (Recorded tracks)
```

## Key Classes and Responsibilities

| Class | File | Purpose |
|-------|------|---------|
| `FormGPS` | Forms/FormGPS.cs | Main form, coordinator |
| `CGuidance` | Classes/CGuidance.cs | Steering angle calculation |
| `CTrack` | Classes/CTrack.cs | Current guidance track state |
| `CABLine` | Classes/CABLine.cs | AB line guidance |
| `CContour` | Classes/CContour.cs | Contour guidance |
| `CSection` | Classes/CSection.cs | Section control state |
| `CTool` | Classes/CTool.cs | Tool configuration |
| `CVehicle` | Classes/CVehicle.cs | Vehicle configuration |
| `CAHRS` | Classes/CAHRS.cs | IMU/heading fusion |
| `CModuleComm` | Classes/CModuleComm.cs | Steer module communication |
| `CYouTurn` | Classes/CYouTurn.cs | U-turn generation |
| `CFence` | Classes/CFence.cs | Geofence logic |
| `CFieldData` | Classes/CFieldData.cs | Field save/load |

See [Classes Documentation](classes.md) for detailed class documentation.

## Related Files

- [PGN Protocol](pgn-protocol.md) - Complete PGN message specification
- [Settings](settings.md) - Vehicle, Tool, and Environment settings
- [Classes](classes.md) - Core class documentation
