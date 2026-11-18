# AgOpenGPS Android - Claude Code Instructions

## Quick Start

Android tablet port of AgOpenGPS (GPS guidance for agriculture), C# on .NET 10 (`net10.0-android`).
The Windows WinForms app was removed from this repository; it is in git history and upstream at
agopengps-official/AgOpenGPS.

## Project Overview

```
SourceCode/
├── AgLibrary.Ntrip/                 # NTRIP client from AgIO (netstandard2.0)
├── ESP32/AgOpenGPS_BT_Bridge/       # ESP32 firmware: receiver UART <-> Bluetooth SPP (Arduino)
└── Android/
    ├── AgOpenGPS.Android.sln
    ├── AgOpenGPS.Mobile.Core/       # Platform-neutral: NMEA/PGN decoding, link supervisor, NTRIP forwarding,
    │                                #   Guidance/ (local plane, A-B lines, passes, nudge, vehicle offsets, fields, coverage,
    │                                #   map tiles), Simulator
    ├── AgOpenGPS.Mobile.Core.Tests/ # NUnit tests (run on any OS)
    ├── AgOpenGPS.Android/           # Android app: driving + settings screens, Bluetooth link, background service
    └── pi/                          # Raspberry Pi Bluetooth <-> serial bridge (Python)
```

Keep logic in `AgOpenGPS.Mobile.Core` and test it there; `AgOpenGPS.Android` should only hold
Android-specific code (Bluetooth, UI, permissions).

The PGN format (`[0x80, 0x81, source, pgn, length, data..., crc]`) and the NMEA sentences
(GGA, VTG, RMC, HDT, PANDA, PAOGI) must stay compatible with the Windows AgOpenGPS and AgIO,
so existing modules and bridges keep working.

## Translations

The app is in English and Croatian. Every text shown on screen goes through `Tr.T("English text")`
or `Tr.F("English {0} format", args)` (`AgOpenGPS.Android/Tr.cs`), and needs a Croatian entry in
`Tr.cs` with the same `{0}` placeholders. Build whole sentences with placeholders, never by
gluing pieces ("Delete " + name), because the word order differs. Messages from the core library
stay English there and are translated where they are shown.

## User guide

`docs/user-guide.en.md` and `docs/user-guide.hr.md` are the short user guide, shown in the app's
Help screen (packed into the APK, converted by `HelpText`). When a feature changes what the user
sees or does, update both guides. Keep to headings, `-` bullets, `1.` steps, **bold** and *italic*.

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

The ESP32 firmware is compiled by `.github/workflows/esp32.yml` (arduino-esp32 2.0.x and 3.x),
or locally with Arduino IDE / arduino-cli, board `esp32:esp32:esp32`.
