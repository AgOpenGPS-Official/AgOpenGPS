# AgOpenGPS for Android – User guide

## What you need

- An Android tablet (Android 8 or newer) with Bluetooth.
- An RTK receiver (e.g. u-blox F9P) connected over Bluetooth through an **ESP32** or a **Raspberry Pi**.
- For RTK: corrections from an NTRIP caster (e.g. your own base station), and mobile internet on the tablet.
- To try the app without a tractor, use the **simulator**.

## First setup

1. Pair the ESP32 or Pi in Android's Bluetooth settings (**Settings → Pair...** opens them).
2. Open **Settings**:
   - **Language**: System, English or Hrvatski. **Colours**: *Night* (dark) or *Day* (light, for bright sun).
   - **Receiver**: pick the paired device. *Connect automatically* connects at every start.
   - **Implement**: the full **implement width** and the **overlap**. Passes are width minus overlap apart.
   - **Vehicle**: where the antenna sits, measured from the middle of the rear axle (left and behind are negative). Leave 0 if unsure.
   - **NTRIP**: caster host, port, mountpoint, user and password, and tick *Send NTRIP corrections*.
3. Go **Back** and press **Connect**.

## The driving screen

- **Badge** (top left): **RTK Fix** green = centimetre accuracy, **RTK Float** orange = about 30 cm, **GPS** = 1–2 m.
- **Lightbar** (top): the lit segments show which way and how much to steer. Only the centre lit = on course.
- **◀ 12 cm**: you are 12 cm right of the line, steer left. Green under 5 cm, yellow under 15 cm, red above.
- **Map**: pinch or **+ / −** to zoom, drag to look around. **Follow** returns to the tractor, **Field** shows the whole field.

## Fields

- **Field: ...** (right panel) opens the list: **New field**, open, rename, delete.
- **Import from file** reads field outlines (GeoJSON or KML). **Export** saves fields with their boundaries and lines to a file, as a backup or for another tablet.

### Recording the boundary

Press **Record boundary**, then choose:

- **Drive along the edge**: drive round the field with the implement up and press **Finish boundary** within 30 m of the start.
- **Corners (straight sides)**: drive to each corner and press **Corner** (**Undo** removes the last). After the last corner press **Finish boundary**. No need to drive back to the start.

Then choose what follows the edge: the middle of the implement, or its left or right end.

## Guidance lines

1. Choose **Straight** or **Curve**.
2. Drive to the start and press **A**.
3. Drive along the edge (at least 5 m, the longer the better) and press **B**.

The line is saved in the open field. Passes lie one working width apart on both sides. The blue **A** and **B** markers show where the active line was recorded. **Lines...** lists the saved lines.

**Nudge**: **◀ / ▶** move the line a few cm, **Snap** moves it to where the tractor is now, **Reset nudge** puts it back.

## Worked area

Press **Work: ON** when the implement works and **Work: OFF** at the headland. The worked ground turns green on the map (overlap red). Below the button: **worked**, **left**, **% done** and **overlap** in hectares. It is saved per field.

## Headland

**Settings → Headland**: set the **headland width** (e.g. two implement widths). A dashed line on the map marks the headland strip along the boundary; the *Headland ahead* alert sounds before you reach it.

With *Work off in the headland, on again in the main part*, Work switches off when the implement enters the headland and on again when it is back in the main part (the button shows **Work: headland** meanwhile). This happens only after you switched Work on yourself; switching it off by hand stops the automatic.

## Job log

A job starts with **Work: ON** in a field and records the date, start and end, working time, hectares worked, distance and average speed. Changing the field or disconnecting ends it. **Jobs...** (under the statistics) lists the jobs: **Export...** saves them as **CSV** (Excel) or **PDF** (for printing), **End job** closes the current one, tap a job to delete it.

## Sound alerts

**Settings → Sound alerts**: field edge ahead, left the field, RTK Fix lost and back, corrections stopped, connection lost, off the line. Each sounds once per event, also with the tablet on silent. **Test sound** checks the volume.

## Satellite map

**Settings → Satellite map**. Map squares are downloaded once and kept on the tablet. **Download map for the open field** fetches the whole field in advance (best over Wi-Fi), so the map also works without internet.

## Backup

**Settings → Backup → Save backup** writes one file with all settings and all fields (boundaries and lines). **Restore backup** on this or another tablet sets everything up again. The file contains the NTRIP password: keep it private.

## Simulator

**Settings → Simulator → Use the simulator**, then go back: a virtual tractor with RTK fix starts in the open field. Use **− / +** for speed, **< | >** to steer, **Auto** to follow the line, **To field** to jump into the field.

## When something is wrong

- **No position / Bluetooth: connected, but no data**: the receiver is off or not wired to the ESP32/Pi, or the baud rate differs.
- **RTK FLOAT / NO RTK**: no corrections. Check the internet and the NTRIP settings; the top bar shows the NTRIP state.
- **The map stays empty**: no internet when the area was first shown. Download the field map over Wi-Fi.
- **Lines are off by a constant amount**: check the antenna offsets in **Settings → Vehicle**, or use **Snap**.
