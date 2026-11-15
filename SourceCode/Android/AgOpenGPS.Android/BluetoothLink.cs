using System;
using System.IO;
using System.Threading;
using Android.Bluetooth;
using Android.Runtime;
using AgOpenGPS.Mobile.Core;
using Java.Util;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Bluetooth Classic serial port (SPP / RFCOMM) to an already paired device: an RTK receiver with
    /// a Bluetooth bridge (HC-05, ESP32, the receiver's own module) or a Raspberry Pi.
    /// </summary>
    public sealed class BluetoothLink : IGnssLink
    {
        /// <summary>The standard Serial Port Profile service id.</summary>
        public static readonly UUID SppUuid = UUID.FromString("00001101-0000-1000-8000-00805F9B34FB");

        private readonly BluetoothSocket _socket;
        private int _disposed;

        private BluetoothLink(BluetoothSocket socket)
        {
            _socket = socket;
            Input = socket.InputStream;
            Output = socket.OutputStream;
        }

        public Stream Input { get; }
        public Stream Output { get; }

        /// <summary>
        /// Connects, trying in turn: the SPP service record (secure, then insecure), then RFCOMM
        /// channel 1 directly. The last one is for devices that do not publish an SPP record, like a
        /// Raspberry Pi whose bluetoothd runs without --compat.
        /// </summary>
        public static BluetoothLink Connect(BluetoothAdapter adapter, string address, CancellationToken ct)
        {
            if (adapter == null) throw new IOException("This tablet has no Bluetooth");
            if (!adapter.IsEnabled) throw new IOException("Bluetooth is off");
            if (!BluetoothAdapter.CheckBluetoothAddress(address)) throw new IOException("No device selected");

            BluetoothDevice device = adapter.GetRemoteDevice(address);
            Exception last = null;

            foreach (Func<BluetoothSocket> make in new Func<BluetoothSocket>[]
            {
                () => device.CreateRfcommSocketToServiceRecord(SppUuid),
                () => device.CreateInsecureRfcommSocketToServiceRecord(SppUuid),
                () => CreateOnChannel(device, 1),
            })
            {
                ct.ThrowIfCancellationRequested();
                BluetoothSocket socket = null;
                try
                {
                    socket = make();
                    if (socket == null) continue;
                    using (ct.Register(() => { try { socket.Close(); } catch (Java.IO.IOException) { } }))
                    {
                        socket.Connect(); // blocks for up to ~12 s
                    }
                    ct.ThrowIfCancellationRequested();
                    return new BluetoothLink(socket);
                }
                catch (OperationCanceledException)
                {
                    try { socket?.Close(); } catch (Java.IO.IOException) { }
                    throw;
                }
                catch (Exception ex)
                {
                    last = ex;
                    try { socket?.Close(); } catch (Java.IO.IOException) { }
                }
            }

            throw new IOException(last?.Message ?? "Could not connect", last);
        }

        // BluetoothDevice.createRfcommSocket(int) is hidden API, reachable by reflection.
        private static BluetoothSocket CreateOnChannel(BluetoothDevice device, int channel)
        {
            Java.Lang.Reflect.Method m = device.Class.GetMethod("createRfcommSocket", Java.Lang.Integer.Type);
            Java.Lang.Object result = m.Invoke(device, Java.Lang.Integer.ValueOf(channel));
            return result?.JavaCast<BluetoothSocket>();
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0) return;
            try { _socket.Close(); } catch (Java.IO.IOException) { }
        }
    }
}
