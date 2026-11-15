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
| `AgOpenGPS.Mobile.Core/` | Platform-neutral .NET 10 library: stream decoder (NMEA + PGN in one stream), NMEA/PGN parsers, link supervisor (reconnect, silence watchdog), NTRIP-to-device forwarder, and `Guidance/`: local plane (same conversion as AgOpenGPS), straight and curved A-B lines, passes, heading, saved lines. Uses the NTRIP client in `SourceCode/AgLibrary.Ntrip`, taken unchanged from AgIO. |
| `AgOpenGPS.Mobile.Core.Tests/` | NUnit tests, including an end-to-end test against a fake NTRIP caster. Runs on Windows/Linux/macOS. |
| `AgOpenGPS.Android/` | The Android app (`net10.0-android`, Android 8.0+): driving screen with guidance view, settings screen, Bluetooth SPP link. |
| `pi/` | Bluetooth-to-serial bridge for a Raspberry Pi, see [pi/README.md](pi/README.md). |
| `../ESP32/` | The same bridge as ESP32 firmware, for the tractor, see [../ESP32/README.md](../ESP32/README.md). |

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

### First time: Settings

1. **Pair** the receiver, ESP32 or Pi in Android's Bluetooth settings (*Settings → Pair...* opens
   them). HC-05 modules usually use PIN `1234` or `0000`.
2. Start the app, allow the *Nearby devices* permission, open **Settings**:
   - **Receiver:** pick the device. *Connect automatically* (on by default) connects at every start.
   - **Implement:** working width and overlap in metres. Passes are *width − overlap* apart.
   - **NTRIP:** for RTK through the tablet tick *Send NTRIP corrections* and fill in caster, port,
     mountpoint, user and password. For VRS / network mountpoints set *Send GGA every* to e.g. `10`.
   - **Diagnostics:** link and NTRIP status, byte counters, raw NMEA.
3. Go **Back**. Changes are saved; a running connection reconnects with the new settings.

### Driving screen

- **Top bar:** fix badge (**RTK Fix** green, **RTK Float** orange, DGPS blue, no fix red),
  satellites, HDOP, rate, speed, NTRIP state, and **Connect / Disconnect**.
- **Guidance view**, heading up: the current pass bold yellow, the recorded line (pass 0) blue,
  neighbouring passes grey, the vehicle as a white arrow.
- **Distance off the line**, large: `◀ 12 cm` means you are 12 cm right of the pass, steer left;
  `12 cm ▶` steer right. Green under 5 cm, yellow under 15 cm, red above. Below it: line name,
  pass number, line heading and heading error.

### Recording a line

1. Choose **Straight** (A-B line) or **Curve** (follows the path you drive, for fields without a
   straight edge).
2. Drive to the start and press **A**.
3. Drive along the edge (straight, or following the curve) and press **B**, at least 5 m from A.
   The line is saved as *AB 1*, *AB 2*, ... and used straight away.
4. **Lines...** lists the saved lines: use, rename or delete one, or *No line*.

The recorded line is pass 0; passes ±1, ±2, ... are one working width apart on either side. Both
ends of a line continue straight, so guidance keeps working past A and B. A curve is recorded
every metre while driving and smoothed (points within 3 cm of a straight run are dropped).

Guidance is for the **antenna** position: mount the antenna above the middle of the implement's
working path, or expect a constant offset. Antenna and implement offsets are on the list below.

### Connection

The link is reconnected automatically when the receiver is switched off and on again, goes out of
range, or stops sending for 10 s. The screen stays on while the app is open. Leaving the app with
Back disconnects.

**Serial speed:** Bluetooth SPP modules have a fixed baud rate to the receiver (HC-05 default
9600). For RTK at 5–10 Hz set the module and the receiver UART to 115200 or higher. Corrections
are sent at up to about 5 KB/s, like AgIO's serial output.

## Not done yet / next steps

- Antenna position (forward/back and sideways offset from the pivot) and implement offset.
- Nudging a line sideways, snapping to the current position.
- Fields and boundaries, coverage map, headlands, U-turns, section control.
- Sending AutoSteer / machine PGNs to modules (UDP over Wi-Fi or over the same Bluetooth link).
- Running as a foreground service, so the link keeps going when the screen is off or another app is in front.
- Release signing, USB-OTG serial receivers, BLE receivers.
