using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// A virtual tractor with an RTK fix, for trying the app without a receiver. Bicycle model:
    /// the heading turns by speed / wheelbase * tan(wheel angle). Produces GGA + VTG like a real
    /// receiver, so everything after the link works unchanged. Thread safe.
    /// </summary>
    public sealed class GnssSimulator
    {
        public const double MaxWheelAngle = 35;
        public const double MaxSpeedKmh = 30;

        private readonly object _lock = new object();
        private double _lat, _lon, _heading, _speed, _wheel;

        public GnssSimulator(double latitude, double longitude, double headingDeg = 0)
        {
            _lat = latitude;
            _lon = longitude;
            _heading = LocalPlane.Wrap360(headingDeg);
        }

        public double Wheelbase { get; set; } = 3.0;

        /// <summary>Puts the tractor somewhere else (e.g. into the field just opened), stopped, wheel straight.</summary>
        public void MoveTo(double latitude, double longitude)
        {
            lock (_lock)
            {
                _lat = latitude;
                _lon = longitude;
                _speed = 0;
                _wheel = 0;
            }
            AutoSteer = false;
        }

        /// <summary>Steers itself along the active line (pure pursuit), like AutoSteer would.</summary>
        public bool AutoSteer { get; set; }

        /// <summary>Where to steer to while <see cref="AutoSteer"/> is on; null = hold the wheel.</summary>
        public Func<SteerResult> SteerSource { get; set; }

        public double Latitude { get { lock (_lock) return _lat; } }
        public double Longitude { get { lock (_lock) return _lon; } }
        public double Heading { get { lock (_lock) return _heading; } }

        public double SpeedKmh
        {
            get { lock (_lock) return _speed; }
            set { lock (_lock) _speed = Math.Max(-10, Math.Min(MaxSpeedKmh, value)); }
        }

        /// <summary>Front wheel angle in degrees, positive = right.</summary>
        public double WheelAngle
        {
            get { lock (_lock) return _wheel; }
            set { lock (_lock) _wheel = Math.Max(-MaxWheelAngle, Math.Min(MaxWheelAngle, value)); }
        }

        /// <summary>Moves the tractor on by <paramref name="seconds"/>.</summary>
        public void Step(double seconds)
        {
            if (AutoSteer)
            {
                SteerResult target = SteerSource?.Invoke();
                if (target != null) WheelAngle = Steering.PurePursuitWheelAngle(target.SteerDegrees, target.LookaheadMeters, Wheelbase);
            }

            lock (_lock)
            {
                double v = _speed / 3.6, dist = v * seconds;
                _heading = LocalPlane.Wrap360(_heading + LocalPlane.ToDeg(dist / Wheelbase * Math.Tan(LocalPlane.ToRad(_wheel))));
                var plane = new LocalPlane(_lat, _lon);
                double h = LocalPlane.ToRad(_heading);
                plane.ToWgs84(new GeoPoint(Math.Cos(h) * dist, Math.Sin(h) * dist), out _lat, out _lon);
            }
        }

        /// <summary>GGA + VTG for the current state (RTK fix, 14 satellites), with CR/LF.</summary>
        public string Sentences(DateTime utc)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            double lat, lon, heading, speed;
            lock (_lock) { lat = _lat; lon = _lon; heading = _heading; speed = _speed; }

            string gga = string.Format(ci, "$GNGGA,{0},{1},{2},4,14,0.7,120.0,M,45.0,M,1.0,0000",
                utc.ToString("HHmmss.00", ci), Coord(lat, 2, 'N', 'S'), Coord(lon, 3, 'E', 'W'));
            double cog = speed < 0 ? LocalPlane.Wrap360(heading + 180) : heading;
            string vtg = string.Format(ci, "$GNVTG,{0:0.00},T,,M,{1:0.000},N,{2:0.000},K,R", cog, Math.Abs(speed) / 1.852, Math.Abs(speed));
            return gga + "*" + Nmea.Checksum(gga) + "\r\n" + vtg + "*" + Nmea.Checksum(vtg) + "\r\n";
        }

        private static string Coord(double value, int degDigits, char pos, char neg)
        {
            double a = Math.Abs(value);
            int deg = (int)a;
            double min = (a - deg) * 60.0;
            string d = deg.ToString(new string('0', degDigits), CultureInfo.InvariantCulture);
            return d + min.ToString("00.0000000", CultureInfo.InvariantCulture) + "," + (value >= 0 ? pos : neg);
        }
    }

    /// <summary>A <see cref="IGnssLink"/> fed by a <see cref="GnssSimulator"/> at a fixed rate.</summary>
    public sealed class SimulatorLink : IGnssLink
    {
        public SimulatorLink(GnssSimulator simulator, int hz = 10)
        {
            Input = new SimStream(simulator, hz);
        }

        public Stream Input { get; }
        public Stream Output { get; } = Stream.Null; // corrections go nowhere

        public void Dispose() => Input.Dispose();

        private sealed class SimStream : Stream
        {
            private readonly GnssSimulator _sim;
            private readonly int _periodMs;
            private readonly ManualResetEventSlim _closed = new ManualResetEventSlim(false);
            private byte[] _pending = Array.Empty<byte>();
            private int _pos;
            private DateTime _next = DateTime.UtcNow;

            public SimStream(GnssSimulator sim, int hz)
            {
                _sim = sim;
                _periodMs = 1000 / Math.Max(1, hz);
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_pos >= _pending.Length)
                {
                    TimeSpan wait = _next - DateTime.UtcNow;
                    if (wait > TimeSpan.Zero && _closed.Wait(wait)) return 0;
                    if (_closed.IsSet) return 0;
                    _next = (wait > TimeSpan.Zero ? _next : DateTime.UtcNow).AddMilliseconds(_periodMs);
                    _sim.Step(_periodMs / 1000.0);
                    _pending = Encoding.ASCII.GetBytes(_sim.Sentences(DateTime.UtcNow));
                    _pos = 0;
                }
                int n = Math.Min(count, _pending.Length - _pos);
                Array.Copy(_pending, _pos, buffer, offset, n);
                _pos += n;
                return n;
            }

            protected override void Dispose(bool disposing)
            {
                _closed.Set();
                base.Dispose(disposing);
            }

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();
            public override long Position { get => 0; set => throw new NotSupportedException(); }
            public override void Flush() { }
            public override long Seek(long o, SeekOrigin s) => throw new NotSupportedException();
            public override void SetLength(long v) => throw new NotSupportedException();
            public override void Write(byte[] b, int o, int c) => throw new NotSupportedException();
        }
    }
}
