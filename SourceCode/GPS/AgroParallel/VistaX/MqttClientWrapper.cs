// ============================================================================
// MqttClientWrapper.cs - Cliente MQTT con reconexión automática
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/MqttClientWrapper.cs
// Target: net48 (C# 7.3)
// Dependencias: MQTTnet 4.3.x (NuGet)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace AgroParallel.VistaX
{
    public class MqttClientWrapper : IDisposable
    {
        private IMqttClient _client;
        private MqttClientOptions _options;
        private readonly VistaXConfig _config;
        private readonly List<string> _topics = new List<string>();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool _disposed;

        public event Action<string, string> MessageReceived;
        public event Action<bool> ConnectionStateChanged;
        public event Action<string> ErrorOccurred;

        public bool IsConnected
        {
            get { return _client != null && _client.IsConnected; }
        }

        public MqttClientWrapper(VistaXConfig config)
        {
            _config = config;
        }

        public async Task ConnectAsync()
        {
            if (_disposed) return;

            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            var builder = new MqttClientOptionsBuilder()
                .WithTcpServer(_config.BrokerAddress, _config.BrokerPort)
                .WithClientId(_config.ClientId)
                .WithCleanSession(true)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(30))
                .WithTimeout(TimeSpan.FromSeconds(10));

            if (!string.IsNullOrEmpty(_config.Username))
                builder.WithCredentials(_config.Username, _config.Password ?? "");

            if (_config.UseTls)
                builder.WithTlsOptions(o => o.WithCertificateValidationHandler(x => true));

            _options = builder.Build();

            _client.ApplicationMessageReceivedAsync += OnMessageAsync;
            _client.DisconnectedAsync += OnDisconnectedAsync;
            _client.ConnectedAsync += OnConnectedAsync;

            // Tópicos reales de VistaX
            _topics.Clear();
            if (!string.IsNullOrWhiteSpace(_config.TelemetriaTopic))
                _topics.Add(_config.TelemetriaTopic);
            if (!string.IsNullOrWhiteSpace(_config.SectionsTopic))
                _topics.Add(_config.SectionsTopic);
            if (!string.IsNullOrWhiteSpace(_config.SpeedTopic))
                _topics.Add(_config.SpeedTopic);

            try
            {
                await _client.ConnectAsync(_options, _cts.Token);
            }
            catch (Exception ex)
            {
                var handler = ErrorOccurred;
                if (handler != null) handler("Conexión MQTT fallida: " + ex.Message);
            }
        }

        private async Task OnConnectedAsync(MqttClientConnectedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("[VistaX] MQTT conectado");
            var handler = ConnectionStateChanged;
            if (handler != null) handler(true);

            foreach (var topic in _topics)
            {
                try
                {
                    await _client.SubscribeAsync(
                        new MqttTopicFilterBuilder()
                            .WithTopic(topic)
                            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                            .Build());
                    System.Diagnostics.Debug.WriteLine("[VistaX] Suscrito: " + topic);
                }
                catch (Exception ex)
                {
                    var errHandler = ErrorOccurred;
                    if (errHandler != null) errHandler("Error suscribiendo a " + topic + ": " + ex.Message);
                }
            }
        }

        private async Task OnDisconnectedAsync(MqttClientDisconnectedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("[VistaX] MQTT desconectado");
            var handler = ConnectionStateChanged;
            if (handler != null) handler(false);

            if (_disposed || _cts.IsCancellationRequested) return;

            int delay = 2000;
            const int maxDelay = 30000;

            while (!_disposed && !_cts.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(delay, _cts.Token);
                    await _client.ConnectAsync(_options, _cts.Token);
                    System.Diagnostics.Debug.WriteLine("[VistaX] Reconectado");
                    return;
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[VistaX] Reintento fallido: " + ex.Message);
                    delay = Math.Min(delay * 2, maxDelay);
                }
            }
        }

        private Task OnMessageAsync(MqttApplicationMessageReceivedEventArgs args)
        {
            try
            {
                string topic = args.ApplicationMessage.Topic;
                var seg = args.ApplicationMessage.PayloadSegment;
                string payload = seg.Count > 0
                    ? Encoding.UTF8.GetString(seg.Array, seg.Offset, seg.Count)
                    : "";
                var handler = MessageReceived;
                if (handler != null) handler(topic, payload);
            }
            catch (Exception ex)
            {
                var errHandler = ErrorOccurred;
                if (errHandler != null) errHandler("Error procesando mensaje: " + ex.Message);
            }
            return Task.CompletedTask;
        }

        public async Task DisconnectAsync()
        {
            if (_client != null && _client.IsConnected)
            {
                try
                {
                    await _client.DisconnectAsync(
                        new MqttClientDisconnectOptionsBuilder()
                            .WithReason(MqttClientDisconnectOptionsReason.NormalDisconnection)
                            .Build());
                }
                catch { }
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            _cts.Cancel();
            try { DisconnectAsync().GetAwaiter().GetResult(); } catch { }
            if (_client != null) _client.Dispose();
            _cts.Dispose();
        }
    }
}
