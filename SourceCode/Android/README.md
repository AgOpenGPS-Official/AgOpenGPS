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

A short guide for users is in [docs/user-guide.en.md](../../docs/user-guide.en.md) and
[docs/user-guide.hr.md](../../docs/user-guide.hr.md); the app shows the same text under **Help**
(the files are packed into the APK, so help works offline). The sections below go into more detail.

### Language

*Settings → Language / Jezik*: **System / Sustav** (Croatian if the tablet is set to Croatian),
**English** or **Hrvatski**. The screens switch at once. The Croatian terms follow the AgOpenGPS
translation: parcela, međa, AB linija, krivulja, preklop, prohod, pomak.

### First time: Settings

1. **Pair** the receiver, ESP32 or Pi in Android's Bluetooth settings (*Settings → Pair...* opens
   them). HC-05 modules usually use PIN `1234` or `0000`.
2. Start the app, allow the *Nearby devices* permission, open **Settings**:
   - **Receiver:** pick the device. *Connect automatically* (on by default) connects at every start.
   - **Implement:** implement width and overlap in metres. Passes are *width − overlap* apart;
     the worked area uses the full width.
   - **Worked area:** record it or not, show it on the map or not.
   - **NTRIP:** for RTK through the tablet tick *Send NTRIP corrections* and fill in caster, port,
     mountpoint, user and password. For VRS / network mountpoints set *Send GGA every* to e.g. `10`.
   - **Diagnostics:** link and NTRIP status, byte counters, raw NMEA.
3. Go **Back**. Changes are saved; a running connection reconnects with the new settings.

### Driving screen

- **Top bar:** fix badge (**RTK Fix** green, **RTK Float** orange, DGPS blue, no fix red),
  satellites, HDOP, rate, speed, NTRIP state, and **Connect / Disconnect**.
- **Guidance view**, heading up: the current pass bold yellow, the recorded line (pass 0) blue,
  neighbouring passes grey, the vehicle as a white arrow.
- **Moving the map:** **pinch** or **+ / −** to zoom, **drag** to look around, **double tap**
  to zoom in. Dragging stops following the tractor and keeps the map still; **Follow** (orange
  while you are looking around) returns to the tractor, heading up. **Field** shows the whole
  field, north up. Without a position the open field is shown. The width of the view is shown
  bottom left.
- **Lightbar** across the top: how much and which way to steer. The app takes a goal point on
  the pass a few metres ahead (3 m when slow, about 6 m at 12 km/h, up to 15 m) and lights one
  segment per degree between the tractor's heading and that point, towards the side to steer:
  green near the centre, then yellow, then red. Only the centre lit = on course. Because it looks
  ahead, it eases off as soon as you are turning back, instead of showing the full offset until
  you get there. The goal point is the white dot on the line.
- **Distance off the line**, large: `◀ 12 cm` means you are 12 cm right of the pass, steer left;
  `12 cm ▶` steer right. Green under 5 cm, yellow under 15 cm, red above. Below it: line name,
  pass number, line heading and heading error.
- **Accuracy warning:** without RTK Fix an orange *RTK FLOAT* or red *NO RTK* banner shows and the
  lightbar and distance turn grey: the centimetres are not reliable then. The top bar shows the
  correction age.

### Vehicle, antenna offsets and nudge

*Settings → Vehicle* (metres, measured from the middle of the rear axle; left and behind are
negative): antenna height, antenna ahead of the rear axle, antenna right of the centre line,
implement working line behind the rear axle, implement centre right of the centre line, and the
wheelbase. From the antenna position and heading the app works out:

- the **guide point**: the implement's centre line, level with the rear axle. Lines are recorded
  and guidance is given for this point;
- the **implement point**: where the worked area is painted;
- with **antenna height** and roll from the receiver (`$PANDA` / `$PAOGI` or a PGN bridge with an
  IMU): the tilt on slopes is corrected (3 m high at 5° is 26 cm).

**Nudge** (driving screen): **◀ / ▶** move the active line one step (Settings, default 5 cm) to
the left / right as seen from the seat, **Snap** moves it so the nearest pass runs exactly through
the tractor now, **Reset nudge** puts it back. The nudge is saved with the line and shown as
"nudged +5 cm".

### Simulator

*Settings → Simulator → Use the simulator instead of the receiver*; going back connects to it
(the badge shows **SIM** in purple while the simulator drives, and the top bar says whether the
simulator or Bluetooth is connected). A virtual tractor
with an RTK fix starts in the open field (or at the start position typed in Settings, or the last
position). On the driving screen: **− / +** speed, **< | >** steering wheel, **Auto** steers along
the active line by itself (pure pursuit, as AutoSteer would), **To field** puts the tractor in
the middle of the open field (this also happens when a field is opened or imported). Everything else (lines, boundary,
worked area, lightbar) works as with a real receiver. No NTRIP in the simulator.

### Satellite map

*Settings → Satellite map*. Each 256 px map square is downloaded **once** and kept on the tablet
(files/maptiles); the squares in use are also kept in memory (about 12 MB), so drawing never waits
for the network. A square is fetched only the first time it is needed; a failed one is retried
after a minute. **Download map for the open field** fetches zoom 14–19 for the field (or 500 m
around the tractor) in advance, e.g. at home over Wi-Fi; a 12 ha field is a few hundred squares,
a few MB. **Clear map cache** deletes them, to get newer imagery.

Where a square is missing at the wanted zoom or is still loading, the matching part of a
coarser square is shown enlarged instead of a hole. Many sources do not report missing imagery
as an error but send a "Map data not yet available" picture (Esri does from zoom 19 in many
rural areas); such placeholders are recognised (known ones, and any identical small image sent
for different places), never shown, and remembered with a small marker so they are not asked for
again. So zooming in closer than the source's best level enlarges its real imagery.
*Most detailed zoom to download* (default 19) stops the app asking for levels the source does
not have; closer views enlarge the most detailed one.

The default source is Esri World Imagery, free with attribution (shown on the map); check its terms,
especially for commercial use. Any `{z}/{x}/{y}` tile URL can be set instead.

### Fields and boundaries

A field has a name, an outer boundary and its own A-B lines. Everything is saved on the tablet
and stays after closing the app or restarting the tablet.

1. **Field: ...** opens the field list: *New field* (asks for a name), or pick a field to
   *Open*, *Rename* or *Delete* it (deleting also deletes its boundary and lines). *Close field*
   drives without a field.
   *Import from file (GeoJSON / KML)...* at the end of the list reads field outlines exported
   from GIS programs, Google Earth or geojson.io (latitude/longitude, WGS84); every polygon
   becomes a field with its boundary, and the first one is opened.

   **Export / backup:** *Export to a file...* in a field's menu, or *Export all fields to a
   file...* in the list, writes one GeoJSON file: each field's boundary (with name and area) and
   all its A-B lines (straight and curved, with their nudge). Android asks where to save it:
   tablet storage, Downloads, a USB stick or Google Drive. Importing that file restores the
   fields with their lines, on this or another tablet; GIS programs (QGIS, geojson.io) show it as
   ordinary shapes. The worked area is not included.
2. **Record boundary**: choose what follows the field edge while you drive round with the
   implement up:
   - *Antenna path*: the antenna is driven along the edge,
   - *Left / right end of implement*: the boundary is half the working width to that side
     (e.g. drive counter-clockwise with the right end of the implement on the edge).
3. Drive all the way round and press **Finish boundary** within 30 m of where you started. The
   boundary is closed, smoothed and saved, and the area is shown in hectares.

   **Corner mode** (choose *Corners (straight sides)* when starting): drive to each corner and
   press **Corner**; **Undo** removes the last one. After the last corner press **Finish
   boundary**: the corners are joined with straight lines and closed back to the first one, so
   there is no need to drive along the edges or back to the start. At least 3 corners; the
   implement side applies as above. Best for fields with straight sides; for a curved side use
   the driving mode or add extra corners along the curve.

While driving, the boundary is drawn in green. Within half the working width plus 3 m of the edge
an orange **Field edge 2.4 m** warning shows, outside it a red **OUTSIDE FIELD**.

### Worked area and statistics

Press **Work: ON** when the implement is working (down) and **Work: OFF** when it is lifted, e.g.
at the headland. While on, the ground under the full implement width is painted green on the map;
ground worked twice is red. Under the button:

- **Worked**: area worked, counted once (inside the boundary when the field has one),
- **Left** and **% done**: field area minus worked area (only with a field boundary),
- **Overlap**: area worked more than once.

The worked area is kept per field and saved every 30 s, when Work is switched off and when another
field is opened, so a job can be continued another day. *Fields → (field) → Clear worked area*
starts the next job. Without an open field the worked area is not saved.

*Settings → Worked area* switches recording off completely, or only hides it from the map.
Recording takes about 0.02 ms per position (measured on a PC; a tablet is slower but still well
under 1 % CPU); drawing uses cached 16 m tiles. Implement width is the full width; overlap only
sets the distance between passes.

### Recording a line

1. Choose **Straight** (A-B line) or **Curve** (follows the path you drive, for fields without a
   straight edge).
2. Drive to the start and press **A**.
3. Drive along the edge (straight, or following the curve) and press **B**, at least 5 m from A.
   The line is saved in the open field as *AB 1*, *AB 2*, ... and used straight away.
4. **Lines...** lists the open field's lines: use, rename or delete one, or *No line*.

The recorded line is pass 0; passes ±1, ±2, ... are one working width apart on either side. Both
ends of a line continue straight, so guidance keeps working past A and B. A curve is recorded
every metre while driving and smoothed (points within 3 cm of a straight run are dropped).

Guidance is for the **antenna** position: mount the antenna above the middle of the implement's
working path, or expect a constant offset. Antenna and implement offsets are on the list below.

### Headland, job log, colours, backup

- **Headland** (*Settings → Headland*): a strip of the set width along the boundary, drawn dashed;
  *Headland ahead* alert; optionally Work off when the implement enters it and on again in the
  main part (only after Work was switched on by hand; a manual Work off ends the automatic).
- **Job log** (*Jobs...*): one job per field and day while Work is on: date, start/end, working
  time, hectares newly worked, distance, average speed, implement width. Export as CSV (UTF-8,
  semicolons, opens in Excel) or PDF (A4 table). Saved in `jobs.json`.
- **Colours** (*Settings → Colours*): Night (dark) or Day (light).
- **Backup** (*Settings → Backup*): one JSON file with all settings (except the paired device)
  and the fields as GeoJSON; restoring replaces the settings and adds the fields.
- The active line's **A** and **B** are marked on the map.

### Sound alerts

*Settings → Sound alerts*, each with its own sound and on/off switch:

| Alert | When | Default |
|---|---|---|
| Field edge ahead | within the set distance (5 m) of the boundary, inside the field | on |
| Left the field | the guide point crosses the boundary outwards | on |
| RTK Fix lost | the fix drops from RTK Fix for 1.5 s | on |
| RTK Fix is back | RTK Fix again for 1 s after it was lost | on |
| NTRIP corrections stopped | no correction for 15 s after they were arriving | on |
| Connection to the receiver lost | no position for 3 s | on |
| Off the line | more than the set distance (20 cm) off for 2 s while moving with RTK | off |

Each alert sounds **once** per event and is armed again only when the situation has clearly
cleared, so a fix flickering between Fix and Float or wobbling at the edge does not keep beeping.
Sounds use the alarm channel (heard with the tablet on silent) and also play in the background;
a short message shows on the driving screen. *Test sound* checks the volume. Pressing Disconnect
gives no alert.

### Connection and running in the background

While connected, the app keeps running in the background: with the screen off, with another app
in front, or after Back. A notification shows the fix, NTRIP state and the distance off the line;
tap it to return to the app, or press **Disconnect** in it. Allow notifications when Android asks
(Android 13+), otherwise the connection still runs but the notification is hidden.

The link is reconnected automatically when the receiver is switched off and on again, goes out of
range, or stops sending for 10 s. The screen stays on while the app is in front.

**Serial speed:** Bluetooth SPP modules have a fixed baud rate to the receiver (HC-05 default
9600). For RTK at 5–10 Hz set the module and the receiver UART to 115200 or higher. Corrections
are sent at up to about 5 KB/s, like AgIO's serial output.

## Not done yet / next steps

- Inner boundaries (obstacles), coverage map, headlands, U-turns, section control.
- Import and export of fields (AgOpenGPS field folders, KML / shapefiles).
- Sending AutoSteer / machine PGNs to modules (UDP over Wi-Fi or over the same Bluetooth link).
- Release signing, USB-OTG serial receivers, BLE receivers.
