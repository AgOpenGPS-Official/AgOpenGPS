#!/usr/bin/env python3
"""
Bluetooth SPP <-> serial bridge for a Raspberry Pi next to the RTK receiver.

The tablet connects over Bluetooth (RFCOMM channel 1). Everything the receiver sends on its
serial/USB port (NMEA, or anything else) goes to the tablet, and everything the tablet sends
(RTCM corrections from its NTRIP client) goes to the receiver.

    sudo apt install python3-serial
    sudo python3 bt_gnss_bridge.py --serial /dev/ttyACM0 --baud 460800

See README.md in this folder for pairing and running it as a service.
"""

import argparse
import os
import socket
import subprocess
import sys
import threading
import time

try:
    import serial  # pyserial
except ImportError:
    sys.exit("pyserial is missing: sudo apt install python3-serial")


def log(msg):
    print(time.strftime("%H:%M:%S"), msg, flush=True)


def register_spp(channel):
    """Publish an SPP service record so Android finds the port by UUID. Needs bluetoothd started
    with --compat; without it the tablet falls back to connecting to channel 1 directly."""
    try:
        subprocess.run(["sdptool", "add", "--channel=%d" % channel, "SP"],
                       check=True, capture_output=True, timeout=10)
        log("SPP service record registered on channel %d" % channel)
    except (OSError, subprocess.SubprocessError) as e:
        log("No SPP record (%s); the tablet will use channel %d directly" % (e, channel))


def open_serial(path, baud):
    while True:
        try:
            port = serial.Serial(path, baud, timeout=0.05)
            log("Receiver on %s at %d baud" % (path, baud))
            return port
        except serial.SerialException as e:
            log("Waiting for receiver on %s: %s" % (path, e))
            time.sleep(2)


def pump_serial_to_bt(port, client, stop):
    try:
        while not stop.is_set():
            data = port.read(4096)
            if data:
                client.sendall(data)
    except (OSError, serial.SerialException) as e:
        log("Link to tablet or receiver lost: %s" % e)
    finally:
        stop.set()


def pump_bt_to_serial(port, client, stop, counters):
    try:
        while not stop.is_set():
            data = client.recv(4096)
            if not data:
                break
            port.write(data)
            counters["rtcm"] += len(data)
    except (OSError, serial.SerialException) as e:
        log("Link to tablet or receiver lost: %s" % e)
    finally:
        stop.set()


def serve(args):
    if not hasattr(socket, "AF_BLUETOOTH"):
        sys.exit("This Python has no Bluetooth socket support (needs Linux with BlueZ).")

    register_spp(args.channel)
    server = socket.socket(socket.AF_BLUETOOTH, socket.SOCK_STREAM, socket.BTPROTO_RFCOMM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind((socket.BDADDR_ANY, args.channel))
    server.listen(1)
    log("Waiting for the tablet on RFCOMM channel %d" % args.channel)

    while True:
        client, addr = server.accept()
        log("Tablet connected: %s" % addr[0])
        port = open_serial(args.serial, args.baud)
        stop = threading.Event()
        counters = {"rtcm": 0}
        threads = [
            threading.Thread(target=pump_serial_to_bt, args=(port, client, stop), daemon=True),
            threading.Thread(target=pump_bt_to_serial, args=(port, client, stop, counters), daemon=True),
        ]
        for t in threads:
            t.start()
        last = 0
        while not stop.wait(5):
            if counters["rtcm"] != last:
                log("Corrections to receiver: %d bytes" % counters["rtcm"])
                last = counters["rtcm"]
        for s in (client, port):
            try:
                s.close()
            except Exception:
                pass
        log("Tablet disconnected")


def main():
    p = argparse.ArgumentParser(description=__doc__, formatter_class=argparse.RawDescriptionHelpFormatter)
    p.add_argument("--serial", default=os.environ.get("GNSS_SERIAL", "/dev/ttyACM0"),
                   help="receiver port (default /dev/ttyACM0, the F9P over USB)")
    p.add_argument("--baud", type=int, default=int(os.environ.get("GNSS_BAUD", "460800")))
    p.add_argument("--channel", type=int, default=1, help="RFCOMM channel (the tablet falls back to 1)")
    serve(p.parse_args())


if __name__ == "__main__":
    main()
