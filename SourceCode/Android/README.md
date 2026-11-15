# AgOpenGPS for Android – Bluetooth RTK (MVP)

First step of the Android port: an Android tablet app that connects to the RTK receiver over
**Bluetooth**, shows the fix, and can run AgIO's **NTRIP client** and send the corrections back to
the receiver over the same link. Guidance, mapping and section control are **not** in it yet.

```
 RTK receiver (F9P, ...)                                Android tablet
 ┌──────────────────────┐   Bluetooth SPP (serial)   ┌──────────────────────────┐
 │ NMEA / PANDA / PAOGI │ ─────────────────────────► │ AgOpenGPS (this app)     │
 │                      │ ◄───────────────────────── │  - fix, RTK status       │
 └──────────────────────┘        RTCM corrections    │  - NTRIP client (AgIO's) │
   via HC-05 / ESP32 bridge,                         └──────────────┬───────────┘
   or a Raspberry Pi (pi/)                                          │ mobile data / Wi-Fi
                                                                    ▼
                                                              NTRIP caster
```

The app works out by itself what the link carries, so either kind of device works without a setting:

| Device sends | Example | NTRIP on the tablet |
|---|---|---|
| NMEA: GGA + VTG/RMC/HDT, or `$PANDA` / `$PAOGI` | F9P through an HC-05/ESP32 bridge, or a Pi running `pi/bt_gnss_bridge.py` | **On**: RTCM goes back over Bluetooth |
| AgOpenGPS PGN `0xD6` frames (the packet AgIO sends to AgOpenGPS) | A Pi that runs its own AgIO-like bridge and NTRIP | **Off**: the Pi gets corrections itself |

## Projects

| Folder | What it is |
|---|---|
| `AgOpenGPS.Mobile.Core/` | Platform-neutral .NET 10 library: stream decoder (NMEA + PGN in one stream), NMEA/PGN parsers, link supervisor (reconnect, silence watchdog), NTRIP-to-device forwarder. Reuses `SourceCode/AgLibrary.Ntrip` unchanged (now also built for `netstandard2.0`). |
| `AgOpenGPS.Mobile.Core.Tests/` | NUnit tests, including an end-to-end test against a fake NTRIP caster. Runs on Windows/Linux/macOS. |
| `AgOpenGPS.Android/` | The Android app (`net10.0-android`, Android 8.0+): Bluetooth SPP link and the tablet screen. |
| `pi/` | Bluetooth-to-serial bridge for a Raspberry Pi, see [pi/README.md](pi/README.md). |

## Getting the APK

**From GitHub:** every push that touches this folder runs the *Android* workflow
(`.github/workflows/android.yml`). Open the run under **Actions** and download the
`AgOpenGPS-Android` artifact. It contains `org.agopengps.android-Signed.apk`.

**Build it yourself (Windows):**

1. Install Visual Studio 2022 or newer with the **.NET Multi-platform App UI development** workload
   (it brings the .NET Android workload, the Android SDK and Java). You also need the .NET 10 SDK.
2. Open `SourceCode/Android/AgOpenGPS.Android.sln`.
3. On the tablet: enable *Developer options* → *USB debugging*, connect it with USB.
4. Pick the tablet in the device list and press Run.

**Command line (any OS with .NET 10 and the Android SDK):**

```bash
cd SourceCode/Android
```

```bash
dotnet workload install android
```

```bash
dotnet test AgOpenGPS.Mobile.Core.Tests
```

```bash
dotnet publish AgOpenGPS.Android -c Release
```

The APK is in `AgOpenGPS.Android/bin/Release/net10.0-android/publish/`.

> The APK is signed with a debug key. A build from a different machine or CI run has a different
> key, so Android asks you to uninstall the old version first. A proper release key comes later.

## Using it

1. **Pair** the receiver or Pi in Android's Bluetooth settings (the app's *Pair...* button opens
   them). HC-05 modules usually use PIN `1234` or `0000`.
2. Start the app, allow the *Nearby devices* permission, pick the device and press **Connect**.
3. For RTK through the tablet: tick *Send NTRIP corrections*, fill in caster, port, mountpoint,
   user and password, then Connect (again). For VRS / network mountpoints set *Send GGA every* to
   e.g. `10` seconds.
4. The big badge shows the fix: **RTK Fix** green, **RTK Float** orange, DGPS blue, no fix red.

The link is reconnected automatically when the receiver is switched off and on again, goes out of
range, or stops sending for 10 s. The screen stays on while the app is open.

**Serial speed:** Bluetooth SPP modules have a fixed baud rate to the receiver (HC-05 default
9600). For RTK at 5–10 Hz set the module and the receiver UART to 115200 or higher. Corrections
are sent at up to about 5 KB/s, like AgIO's serial output.

## Not done yet / next steps

- Guidance, field map, AB lines, section control: needs the logic in `SourceCode/GPS/Classes`
  separated from `FormGPS` first, and a new touch UI.
- Sending AutoSteer / machine PGNs to modules (UDP over Wi-Fi or over the same Bluetooth link).
- Running as a foreground service, so the link keeps going when the screen is off or another app is in front.
- Release signing, USB-OTG serial receivers, BLE receivers.
