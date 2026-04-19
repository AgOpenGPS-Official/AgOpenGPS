using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Phidget22;
using Tinkerforge;

namespace CereaBridge
{
    internal sealed partial class BridgeService : IDisposable
    {
        private readonly BridgeConfig _cfg;
        private readonly UdpClient _listenPrimary;
        private readonly UdpClient _listenFallback;
        private readonly UdpClient _sender;
        private readonly IPEndPoint _agioEndpoint;
        private readonly Timer _telemetryTimer;
        private readonly Timer _helloTimer;

        private DCMotor? _motor;
        private Encoder? _encoder;
        private IPConnection? _ipcon;
        private BrickIMUV2? _imu;

        private volatile bool _running;
        private double _desiredAngleDeg;
        private bool _autosteerEnabled;
        private double _speedKph;
        private byte _kp = 20;
        private byte _highPwm = 120;
        private byte _minPwm = 25;
        private double _countsPerDegree = 30.0;
        private int _wasOffset;
        private byte _minSpeedX10 = 5;
        private int _lastPwm;

        public bool IsMotorConnected { get; private set; }
        public bool IsEncoderConnected { get; private set; }
        public bool IsImuConnected { get; private set; }

        public BridgeService(BridgeConfig cfg)
        {
            _cfg = cfg;
            _listenPrimary = new UdpClient(cfg.ListenPort);
            _listenFallback = new UdpClient(cfg.ListenPortFallback);
            _sender = new UdpClient();
            _agioEndpoint = new IPEndPoint(ResolveHostAddress(cfg.AgioHost), cfg.AgioPort);
            _telemetryTimer = new Timer(OnTelemetryTick, null, Timeout.Infinite, Timeout.Infinite);
            _helloTimer = new Timer(OnHelloTick, null, Timeout.Infinite, Timeout.Infinite);
            _wasOffset = cfg.WasOffsetFallback;
            _countsPerDegree = cfg.CountsPerDegreeFallback;
        }

        public void Start()
        {
            _running = true;
            OpenDevices();
            BeginReceive(_listenPrimary);
            BeginReceive(_listenFallback);

            var telemetryPeriod = Math.Max(20, _cfg.TelemetryPeriodMs);
            var helloPeriod = Math.Max(20, _cfg.HelloPeriodMs);
            _telemetryTimer.Change(telemetryPeriod, telemetryPeriod);
            _helloTimer.Change(helloPeriod, helloPeriod);
        }

        public void Dispose()
        {
            _running = false;
            _telemetryTimer.Dispose();
            _helloTimer.Dispose();
            CloseDevices();
            _listenPrimary.Dispose();
            _listenFallback.Dispose();
            _sender.Dispose();
        }

        private static IPAddress ResolveHostAddress(string host)
        {
            if (IPAddress.TryParse(host, out var parsed))
            {
                return parsed;
            }

            var addresses = Dns.GetHostAddresses(host);
            foreach (var address in addresses)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return address;
                }
            }

            return addresses.Length > 0 ? addresses[0] : IPAddress.Loopback;
        }
    }
}
