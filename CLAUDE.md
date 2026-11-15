# AgOpenGPS Android - Claude Code Instructions

## Quick Start

Android tablet port of AgOpenGPS (GPS guidance for agriculture), C# on .NET 10 (`net10.0-android`).
The Windows WinForms app was removed from this repository; it is in git history and upstream at
agopengps-official/AgOpenGPS.

## Project Overview

```
SourceCode/
├── AgLibrary.Ntrip/                 # NTRIP client from AgIO (netstandard2.0)
└── Android/
    ├── AgOpenGPS.Android.sln
    ├── AgOpenGPS.Mobile.Core/       # Platform-neutral: NMEA/PGN decoding, link supervisor, NTRIP forwarding
    ├── AgOpenGPS.Mobile.Core.Tests/ # NUnit tests (run on any OS)
    ├── AgOpenGPS.Android/           # Android app: Bluetooth SPP link, tablet UI
    └── pi/                          # Raspberry Pi Bluetooth <-> serial bridge (Python)
```

Keep logic in `AgOpenGPS.Mobile.Core` and test it there; `AgOpenGPS.Android` should only hold
Android-specific code (Bluetooth, UI, permissions).

The PGN format (`[0x80, 0x81, source, pgn, length, data..., crc]`) and the NMEA sentences
(GGA, VTG, RMC, HDT, PANDA, PAOGI) must stay compatible with the Windows AgOpenGPS and AgIO,
so existing modules and bridges keep working.

## User Preferences

- **Language**: English
- **Comments**: All code comments must be in **English**
- **Style**: Practical working code > perfect architecture
- **Priority**: Get it working in the field

## Build

Run from `SourceCode/Android` (it has the `global.json` for .NET 10):

```bash
dotnet test AgOpenGPS.Mobile.Core.Tests
```

```bash
dotnet publish AgOpenGPS.Android -c Release
```
