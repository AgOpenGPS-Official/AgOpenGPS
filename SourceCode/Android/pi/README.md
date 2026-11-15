# Raspberry Pi Bluetooth bridge

`bt_gnss_bridge.py` makes the RTK receiver plugged into the Pi (USB or UART) available to the
tablet as a Bluetooth serial port, in both directions:

- receiver → tablet: NMEA (or whatever the receiver sends)
- tablet → receiver: RTCM corrections from the tablet's NTRIP client

If the Pi does NTRIP itself and sends AgOpenGPS PGN packets instead, the app reads those as well.
In that case leave NTRIP off on the tablet.

Tested only for syntax so far. Please report how it behaves on your Pi.

## 1. Install

```bash
sudo apt install python3-serial bluez
```

Copy `bt_gnss_bridge.py` to the Pi, e.g. to `/home/pi/`.

## 2. Optional: publish an SPP service record

Android normally finds the serial port through an SDP service record. BlueZ 5 only allows
publishing one in compatibility mode. Without it, the app falls back to RFCOMM channel 1 directly,
which usually works too, so this step is optional.

```bash
sudo sed -i 's#^ExecStart=.*bluetoothd.*#& --compat#' /lib/systemd/system/bluetooth.service
```

```bash
sudo systemctl daemon-reload && sudo systemctl restart bluetooth
```

## 3. Pair the tablet with the Pi

```bash
sudo bluetoothctl
```

In the `bluetoothctl` prompt, run `power on`, `agent on`, `default-agent`, `discoverable on` and
`pairable on`. Then pair from the tablet (Android Bluetooth settings) and confirm the code on both
sides. Run `trust <tablet MAC>`, then `quit`.

## 4. Run

```bash
sudo python3 /home/pi/bt_gnss_bridge.py --serial /dev/ttyACM0 --baud 460800
```

`/dev/ttyACM0` is a u-blox F9P on USB. For the Pi's UART use `/dev/serial0` with the receiver's
UART baud rate.

## 5. Start it at boot

Create `/etc/systemd/system/bt-gnss-bridge.service`:

```ini
[Unit]
Description=Bluetooth GNSS bridge for AgOpenGPS Android
After=bluetooth.target
Requires=bluetooth.target

[Service]
ExecStart=/usr/bin/python3 /home/pi/bt_gnss_bridge.py --serial /dev/ttyACM0 --baud 460800
Restart=always
RestartSec=3

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl enable --now bt-gnss-bridge
```

```bash
journalctl -u bt-gnss-bridge -f
```
