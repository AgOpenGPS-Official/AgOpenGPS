# AgOpenGPS - Claude Code Instructions

## Quick Start

AgOpenGPS is a GPS-guidance steering system for agriculture in C# WinForms (.NET Framework 4.8).

**Main project**: `SourceCode/GPS/`

## Documentation Index

| Document | Description |
|----------|-------------|
| [Architecture](docs/architecture.md) | Communication architecture, dataflow, project structure |
| [Settings](docs/settings.md) | Vehicle/Tool/Environment settings system |
| [Classes](docs/classes.md) | Core classes and their responsibilities |
| [PGN Protocol](docs/pgn-protocol.md) | PGN message format and communication |

## Project Overview

```
SourceCode/
├── GPS/                    # MAIN APPLICATION
│   ├── Classes/            # Core logic & calculations
│   ├── Forms/              # UI (organised by feature)
│   └── Properties/         # Settings
├── AgOpenGPS.Core/         # Shared library
├── AgIO/                   # I/O handler (NTRIP, GPS, IMU)
├── GPS_Out/                # NMEA serial output
├── AgDiag/                 # Diagnostics
├── ModSim/                 # Module simulator
└── Keypad/                 # Hardware keypad
```

## User Preferences

- **Language**: English
- **Comments**: All code comments must be in **English**
- **Style**: Practical working code > perfect architecture
- **Priority**: Get it working in the field

## Build

```bash
msbuild SourceCode/AgOpenGPS.sln /p:Configuration=Release
```
