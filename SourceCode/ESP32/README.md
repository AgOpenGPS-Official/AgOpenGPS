# ESP32 Bluetooth bridge (tractor)

Firmware that turns a classic ESP32 into a Bluetooth link between the rover RTK receiver and the
AgOpenGPS Android app. It is the low-cost alternative to the Raspberry Pi bridge in
`SourceCode/Android/pi/`: it starts in under a second and does not mind the ignition cutting the power.

```
 RTK base station ──► NTRIP caster ──internet──► Android tablet (AgOpenGPS app, NTRIP on)
                                                     ▲  │
                                       NMEA position │  │ RTCM corrections
                                                     │  ▼  Bluetooth SPP
                                                 ESP32 (this firmware)
                                                     ▲  │
                                                     │  ▼  UART
                                                 Rover receiver (F9P)
```

The ESP32 only passes bytes along. The tablet does NTRIP, so the app settings are:
pick **AgOpenGPS-GNSS** as the device and turn **Send NTRIP corrections** on.

## What you need

- A **classic ESP32** board: ESP32-WROOM-32 / "ESP32 DevKit V1" or similar.
  **Not** ESP32-S2, S3, C3 or C6: they have no Bluetooth Classic, only BLE.
- The rover receiver with a UART (e.g. an F9P board with TX1/RX1 pins).
- A 12 V to 5 V converter for the tractor supply (at least 1 A if the receiver also runs from it).

## Wiring

| Receiver (F9P) | ESP32 DevKit |
|---|---|
| TX1 | GPIO16 (RX2) |
| RX1 | GPIO17 (TX2) |
| GND | GND |
| 5V (if the receiver board takes 5 V) | VIN / 5V |

Both sides use 3.3 V logic, so the wires connect directly. Cross TX to RX. The pins can be
changed at the top of the sketch.

## Flashing (Arduino IDE)

1. Arduino IDE 2: *Boards Manager* → install **esp32 by Espressif Systems**, version 2.0.x or 3.x
   (tested to compile with 2.0.17 and 3.3.12). Do not use 4.x: it drops the Bluetooth serial
   library by default. A "BluetoothSerial is deprecated" warning with 3.x is expected and harmless.
2. Open `AgOpenGPS_BT_Bridge/AgOpenGPS_BT_Bridge.ino`.
3. Board: **ESP32 Dev Module**. Select the USB port of the ESP32.
4. Check the settings at the top of the sketch (`BT_NAME`, `GNSS_BAUD`, pins).
5. Upload. If the upload does not start, hold the board's **BOOT** button until it does.
6. *Serial Monitor* at 115200 baud should print
   `AgOpenGPS BT bridge ready: 'AgOpenGPS-GNSS', receiver UART 115200 baud ...`

## Receiver setup (u-blox F9P, once, with u-center)

The F9P must send NMEA on UART1 and accept RTCM on UART1:

1. *View → Configuration View → PRT (Ports)*: target **UART1**, protocol in **UBX+NMEA+RTCM3**,
   protocol out **UBX+NMEA**, baud **115200** (or 460800, then also change `GNSS_BAUD`).
2. *MSG (Messages)*: enable **NMEA GxGGA** and **NMEA GxVTG** on UART1. Turn other NMEA messages
   off on UART1 to save bandwidth.
3. *RATE*: measurement period **100 ms** (10 Hz) or 200 ms (5 Hz).
4. *CFG*: **Save current configuration** (BBR + Flash), or the settings are lost at power off.

Many AgOpenGPS F9P configuration files already do this. If yours sends `$PANDA` / `$PAOGI`
instead, that works too.

**Bandwidth:** 10 Hz GGA + VTG is about 1.5 KB/s and RTCM up to about 5 KB/s. 115200 baud
carries about 11 KB/s each way, which is enough. Use 460800 if you enable more messages.

## Using it

1. Power the ESP32. The LED gives a short blink every second: waiting for the tablet.
2. On the tablet: Android Bluetooth settings → pair **AgOpenGPS-GNSS** (no PIN needed).
3. In the AgOpenGPS app pick it and press **Connect**. The LED is solid while connected and
   flickers while data flows.
4. Fill in the NTRIP caster of your base station and turn *Send NTRIP corrections* on. The app's
   status should go to **RTK Float**, then **RTK Fix**.

| LED | Meaning |
|---|---|
| Short blink every second | No tablet connected (receiver data is dropped) |
| Solid | Tablet connected, no data at the moment |
| Fast flicker | Data flowing |
| Fast blinking, all the time | Bluetooth failed to start |

## Troubleshooting

- **Connected, but "Waiting for position"**: no NMEA from the receiver. Check TX1 → GPIO16, the
  baud rate, and that NMEA output is on for UART1. The Serial Monitor shows `to tablet 0 B`.
- **RTK Float never becomes Fix, or no Float at all**: corrections do not reach the receiver.
  Check RX1 ← GPIO17 and RTCM3 *in* on UART1. The Serial Monitor shows `to receiver ... B` growing
  when the app sends corrections.
- **Device not in the Android list**: it is not a classic ESP32, or it was flashed with the wrong
  board setting.
