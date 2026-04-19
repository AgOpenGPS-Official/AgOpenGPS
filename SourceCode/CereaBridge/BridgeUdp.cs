using System;
using System.Net;
using System.Net.Sockets;

namespace CereaBridge
{
    internal sealed partial class BridgeService
    {
        private void BeginReceive(UdpClient client)
        {
            client.BeginReceive(OnReceive, client);
        }

        private void OnReceive(IAsyncResult ar)
        {
            if (!_running) return;
            if (ar.AsyncState is not UdpClient client) return;

            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
            byte[]? data = null;

            try
            {
                data = client.EndReceive(ar, ref remote);
            }
            catch
            {
            }
            finally
            {
                if (_running)
                {
                    try { BeginReceive(client); } catch { }
                }
            }

            if (data == null || data.Length < 5) return;
            if (data[0] != 0x80 || data[1] != 0x81) return;

            switch (data[3])
            {
                case 0xFE:
                    if (data.Length >= 13)
                    {
                        _speedKph = BitConverter.ToUInt16(data, 5) * 0.1;
                        _autosteerEnabled = data[7] == 1;
                        _desiredAngleDeg = BitConverter.ToInt16(data, 8) * 0.01;
                    }
                    break;

                case 0xFC:
                    if (data.Length >= 13)
                    {
                        _kp = data[5];
                        _highPwm = data[6];
                        _minPwm = data[8];
                        _countsPerDegree = data[9] == 0 ? _cfg.CountsPerDegreeFallback : data[9];
                        _wasOffset = (short)((data[11] << 8) | data[10]);
                    }
                    break;

                case 0xFB:
                    if (data.Length >= 8)
                    {
                        _minSpeedX10 = data[7];
                    }
                    break;
            }
        }

        private void SendPacket(byte[] packet)
        {
            _sender.Send(packet, packet.Length, _agioEndpoint);
        }
    }
}
