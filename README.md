# AgOpenGPS Android

An Android tablet port of [AgOpenGPS](https://github.com/agopengps-official/AgOpenGPS), the open
source GPS guidance and section control software for agriculture.

This repository holds only the Android app. The Windows (WinForms) application it started from was
removed. It is in this repository's git history, and maintained upstream at
[agopengps-official/AgOpenGPS](https://github.com/agopengps-official/AgOpenGPS).

## Status

First version: connects to an RTK receiver or Raspberry Pi over **Bluetooth**, shows the fix and
RTK status, and can run an NTRIP client and send the corrections back to the receiver over the same
link. Guidance, mapping and section control are not ported yet.

## Contents

| Folder | What it is |
|---|---|
| [`SourceCode/Android/`](SourceCode/Android/README.md) | The Android app, its platform-neutral core library and tests. **Start here.** |
| [`SourceCode/ESP32/`](SourceCode/ESP32/README.md) | Bluetooth bridge firmware for an ESP32 in the tractor (low-cost alternative to the Pi). |
| [`SourceCode/Android/pi/`](SourceCode/Android/pi/README.md) | Bluetooth bridge for a Raspberry Pi next to the receiver. |
| `SourceCode/AgLibrary.Ntrip/` | The NTRIP client from AgIO, used by the app. |

## User guide

Short instructions for the tablet: [English](docs/user-guide.en.md) · [Hrvatski](docs/user-guide.hr.md).
The same text is in the app under **Help / Pomoć**.

## Getting the app

Every push builds the APK on GitHub Actions (*Android* workflow): open the latest run under
**Actions** and download the `AgOpenGPS-Android` artifact. To build it yourself, see
[SourceCode/Android/README.md](SourceCode/Android/README.md).

## License

Apache License 2.0, like AgOpenGPS. See [LICENSE](LICENSE).
