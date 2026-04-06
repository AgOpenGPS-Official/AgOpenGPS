// ============================================================================
// SeedMonitor.cs - Motor de monitoreo adaptado a VistaX real
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/SeedMonitor.cs
// Target: net48 (C# 7.3)
//
// FIX CRITICO: La key del diccionario _surcos ahora incluye el tren:
//   key = tren + "-" + bajada + "-" + tipo
// Sin esto, tren1 surco 1 y tren2 surco 1 colisionan en "1-semilla"
// y el diccionario queda con 22 entries en vez de 43.
// ============================================================================

using AgOpenGPS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AgroParallel.VistaX
{
    public class SeedMonitor : IDisposable
    {
        private readonly VistaXConfig _config;
        private ImplementoConfig _implemento;
        private MqttClientWrapper _mqtt;
        private Timer _uiTimer;

        private readonly object _lock = new object();
        private readonly Dictionary<string, SurcoState> _surcos = new Dictionary<string, SurcoState>();
        private double _velocidad;
        private List<int> _seccionesT1 = new List<int>();
        private List<int> _seccionesT2 = new List<int>();
        private DateTime _lastDataTime = DateTime.MinValue;
        private bool _isConnected;
        private bool _disposed;
        private bool _monitoreoActivo;
        private MetodoInicioMonitoreo _metodoInicio;
        private DateTime _confirmacionInicio = DateTime.MinValue;

        public event Action<SeedMonitorSnapshot> SnapshotUpdated;
        public event Action<string> AlarmTriggered;

        public bool IsRunning { get; private set; }
        public bool IsConnected { get { return _isConnected; } }
        public bool MonitoreoActivo { get { return _monitoreoActivo; } }

        public SeedMonitor(FormGPS parent, VistaXConfig config)
        {
            _config = config ?? VistaXConfig.Load();
        }

        public async Task StartAsync()
        {
            if (!_config.Enabled)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Deshabilitado");
                return;
            }
            if (IsRunning) return;

            _implemento = _config.LoadImplemento();
            System.Diagnostics.Debug.WriteLine("[VistaX] Implemento: " + (_implemento.Nombre ?? "?")
                + " | Sensores: " + _implemento.MapeoSensores.Count);

            // Parsear método de inicio
            _monitoreoActivo = false;
            switch ((_config.MetodoInicio ?? "sensores").ToLowerInvariant())
            {
                case "herramienta": _metodoInicio = MetodoInicioMonitoreo.Herramienta; break;
                case "pintando": _metodoInicio = MetodoInicioMonitoreo.Pintando; break;
                case "manual": _metodoInicio = MetodoInicioMonitoreo.Manual; break;
                default: _metodoInicio = MetodoInicioMonitoreo.Sensores; break;
            }
            System.Diagnostics.Debug.WriteLine("[VistaX] Método inicio: " + _metodoInicio);

            // Inicializar surcos — KEY INCLUYE TREN
            lock (_lock)
            {
                foreach (var sensor in _implemento.MapeoSensores)
                {
                    if (!sensor.IsActive) continue;
                    int tren = sensor.Tren > 0 ? sensor.Tren : 0;
                    string key = tren + "-" + sensor.Bajada + "-" + sensor.Tipo;
                    if (!_surcos.ContainsKey(key))
                    {
                        _surcos[key] = new SurcoState
                        {
                            Bajada = sensor.Bajada,
                            Tipo = sensor.Tipo,
                            Tren = tren,
                            LastUpdate = DateTime.MinValue
                        };
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("[VistaX] Surcos inicializados: " + _surcos.Count);

            _mqtt = new MqttClientWrapper(_config);
            _mqtt.MessageReceived += OnMqttMessage;
            _mqtt.ConnectionStateChanged += delegate (bool c) { _isConnected = c; };
            _mqtt.ErrorOccurred += delegate (string err)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] " + err);
            };

            await _mqtt.ConnectAsync();

            _uiTimer = new Timer(
                delegate { EmitSnapshot(); },
                null,
                _config.UiUpdateIntervalMs,
                _config.UiUpdateIntervalMs);

            IsRunning = true;
            System.Diagnostics.Debug.WriteLine("[VistaX] Monitor iniciado — "
                + _surcos.Count + " surcos mapeados");
        }

        public async Task StopAsync()
        {
            if (!IsRunning) return;

            if (_uiTimer != null)
            {
                _uiTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _uiTimer.Dispose();
                _uiTimer = null;
            }

            if (_mqtt != null)
            {
                await _mqtt.DisconnectAsync();
                _mqtt.Dispose();
                _mqtt = null;
            }

            IsRunning = false;
        }

        // =====================================================================
        // MQTT Message Processing
        // =====================================================================

        private void OnMqttMessage(string topic, string payload)
        {
            try
            {
                if (topic == _config.SpeedTopic)
                {
                    double vel;
                    if (double.TryParse(payload, NumberStyles.Any, CultureInfo.InvariantCulture, out vel))
                    {
                        lock (_lock) { _velocidad = vel; }
                    }
                    return;
                }

                if (topic == _config.SectionsTopic)
                {
                    ProcessSections(payload);
                    return;
                }

                if (topic == _config.TelemetriaTopic)
                {
                    ProcessTelemetria(payload);
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Error: " + ex.Message);
            }
        }

        private void ProcessSections(string payload)
        {
            var data = JsonSerializer.Deserialize<SectionsStatePayload>(payload);
            if (data == null) return;

            lock (_lock)
            {
                if (data.T1 != null) _seccionesT1 = data.T1;
                if (data.T2 != null) _seccionesT2 = data.T2;
            }
        }

        private void ProcessTelemetria(string payload)
        {
            var data = JsonSerializer.Deserialize<EspTelemetriaPayload>(payload);
            if (data == null || data.Sensores == null || _implemento.MapeoSensores == null)
                return;

            string uidNodo = data.Uid;

            foreach (var sensorRaw in data.Sensores)
            {
                int cableFisico = sensorRaw.Cable;

                // Buscar en mapeo_sensores
                SensorConfig cfg = null;
                foreach (var s in _implemento.MapeoSensores)
                {
                    if (s.Uid != uidNodo) continue;
                    bool matchPin = s.Pin >= 0 && s.Pin == cableFisico - 1;
                    bool matchCable = s.Cable > 0 && s.Cable == cableFisico;
                    if (matchPin || matchCable)
                    {
                        cfg = s;
                        break;
                    }
                }

                if (cfg == null) continue;
                if (!cfg.IsActive) continue;

                double valorFlujo = sensorRaw.Valor;
                int rawPulsos = sensorRaw.Raw;
                int numTren = cfg.Tren > 0 ? cfg.Tren : 0;
                bool isSemilla = cfg.Tipo == "semilla";
                bool isFerti = cfg.Tipo != null && cfg.Tipo.Contains("ferti");

                // Calcular SPM
                double spm = 0;
                lock (_lock)
                {
                    if (_velocidad > 0.5)
                    {
                        double velMs = _velocidad / 3.6;
                        spm = valorFlujo / velMs;
                    }
                }

                // Evaluar alarma
                bool alerta = false;
                bool seccionCortada = false;

                if (isSemilla || isFerti)
                {
                    lock (_lock)
                    {
                        List<int> seccionesTren = numTren == 1 ? _seccionesT1 : _seccionesT2;
                        if (seccionesTren.Count > 0)
                        {
                            var surcosTren = _implemento.MapeoSensores
                                .Where(s2 => s2.IsActive && (s2.Tren > 0 ? s2.Tren : 0) == numTren && s2.Tipo == "semilla")
                                .OrderBy(s2 => s2.Bajada)
                                .ToList();

                            int idx = -1;
                            for (int i = 0; i < surcosTren.Count; i++)
                            {
                                if (surcosTren[i].Bajada == cfg.Bajada)
                                {
                                    idx = i;
                                    break;
                                }
                            }

                            if (idx >= 0 && idx < seccionesTren.Count)
                            {
                                seccionCortada = seccionesTren[idx] == 0;
                            }
                        }

                        if (!seccionCortada && _velocidad > 1.5)
                        {
                            if (valorFlujo == 0)
                            {
                                alerta = true;
                            }
                            else
                            {
                                double objetivo = GetObjetivoTren(numTren);
                                if (objetivo > 0 && valorFlujo < objetivo * 0.5)
                                {
                                    alerta = true;
                                }
                            }
                        }
                    }
                }

                // FIX CRITICO: key incluye tren
                string key = numTren + "-" + cfg.Bajada + "-" + cfg.Tipo;
                lock (_lock)
                {
                    SurcoState state;
                    if (!_surcos.TryGetValue(key, out state))
                    {
                        state = new SurcoState
                        {
                            Bajada = cfg.Bajada,
                            Tipo = cfg.Tipo,
                            Tren = numTren
                        };
                        _surcos[key] = state;
                    }

                    state.Valor = valorFlujo;
                    state.Spm = Math.Round(spm, 1);
                    state.NuevasSemillas = rawPulsos;
                    state.Alerta = alerta;
                    state.SeccionCortada = seccionCortada;
                    state.LastUpdate = DateTime.UtcNow;
                    _lastDataTime = DateTime.UtcNow;
                }

                if (alerta)
                {
                    var handler = AlarmTriggered;
                    if (handler != null)
                    {
                        handler(string.Format("FALLA surco {0} (tren {1}): flujo={2:F1}",
                            cfg.Bajada, numTren, valorFlujo));
                    }
                }
            }
        }

        // =====================================================================
        // Inicio de monitoreo configurable (4 modos)
        // =====================================================================

        private void EvaluarInicio()
        {
            if (_monitoreoActivo) return;

            switch (_metodoInicio)
            {
                case MetodoInicioMonitoreo.Sensores:
                    lock (_lock)
                    {
                        int activos = 0;
                        foreach (var s in _surcos.Values)
                        {
                            if (s.Tipo == "semilla" && s.Valor > 0) activos++;
                        }
                        if (activos >= _config.UmbralSensoresActivos)
                        {
                            if (_confirmacionInicio == DateTime.MinValue)
                            {
                                _confirmacionInicio = DateTime.UtcNow;
                            }
                            else if ((DateTime.UtcNow - _confirmacionInicio).TotalMilliseconds >= _config.TiempoConfirmacionMs)
                            {
                                IniciarMonitoreo("sensores (" + activos + " activos)");
                            }
                        }
                        else
                        {
                            _confirmacionInicio = DateTime.MinValue;
                        }
                    }
                    break;

                case MetodoInicioMonitoreo.Herramienta:
                    // Se evalúa desde ProcessSections cuando llegan datos de secciones
                    lock (_lock)
                    {
                        bool bajada = _seccionesT1.Count > 0 || _seccionesT2.Count > 0;
                        if (bajada)
                        {
                            bool algunaActiva = _seccionesT1.Any(s => s == 1) || _seccionesT2.Any(s => s == 1);
                            if (algunaActiva)
                                IniciarMonitoreo("herramienta bajada");
                        }
                    }
                    break;

                case MetodoInicioMonitoreo.Pintando:
                    lock (_lock)
                    {
                        bool pintando = _seccionesT1.Any(s => s == 1) || _seccionesT2.Any(s => s == 1);
                        if (pintando && _velocidad > 0.5)
                            IniciarMonitoreo("pintando (secciones activas)");
                    }
                    break;

                case MetodoInicioMonitoreo.Manual:
                    // Se inicia externamente via IniciarMonitoreoManual()
                    break;
            }
        }

        private void IniciarMonitoreo(string motivo)
        {
            _monitoreoActivo = true;
            _confirmacionInicio = DateTime.MinValue;
            System.Diagnostics.Debug.WriteLine("[VistaX] MONITOREO INICIADO — " + motivo);
        }

        public void DetenerMonitoreo()
        {
            _monitoreoActivo = false;
            _confirmacionInicio = DateTime.MinValue;
            System.Diagnostics.Debug.WriteLine("[VistaX] MONITOREO DETENIDO");
        }

        public void IniciarMonitoreoManual()
        {
            IniciarMonitoreo("manual (botón UI)");
        }

        // =====================================================================
        // Helpers
        // =====================================================================

        private double GetObjetivoTren(int numTren)
        {
            if (_implemento.Setup.ObjetivosTren != null)
            {
                double val;
                if (_implemento.Setup.ObjetivosTren.TryGetValue(numTren.ToString(), out val))
                    return val;
            }
            return _implemento.Setup.DensidadObjetivo > 0
                ? _implemento.Setup.DensidadObjetivo
                : 16;
        }

        // =====================================================================
        // Snapshot
        // =====================================================================

        private void EmitSnapshot()
        {
            if (_disposed) return;
            EvaluarInicio();
            var snap = CreateSnapshot();
            System.Diagnostics.Debug.WriteLine("[VistaX-DBG] SNAPSHOT: vel=" + snap.Velocidad.ToString("F1")
                + " surcos=" + (snap.Surcos != null ? snap.Surcos.Length : 0)
                + " semillas=" + snap.SurcosActivos
                + " fallas=" + snap.FallasActivas
                + " spm=" + snap.SpmPromedio.ToString("F1"));
            var handler = SnapshotUpdated;
            if (handler != null) handler(snap);
        }

        // Ancho de cada sensor (tubo) y spacing en píxeles — constantes de layout
        private const int SensorWidthPx = 32;
        private const int SensorSpacingPx = 4;
        private const int ContainerWidthPx = 1200;

        public SeedMonitorSnapshot CreateSnapshot()
        {
            lock (_lock)
            {
                var surcosList = _surcos.Values.ToArray();
                var semillas = surcosList.Where(s => s.Tipo == "semilla").ToArray();

                int fallas = 0;
                double sumaSpm = 0;
                int countSpm = 0;

                foreach (var s in semillas)
                {
                    if (s.Alerta) fallas++;
                    if (s.Spm > 0) { sumaSpm += s.Spm; countSpm++; }
                }

                double promedio = countSpm > 0 ? Math.Round(sumaSpm / countSpm, 1) : 0;

                bool hasAlarm = fallas > 0;
                string alarmMsg = "";
                if (fallas > 0)
                {
                    var fallasStr = string.Join(", ",
                        semillas.Where(s => s.Alerta)
                            .Select(s => "T" + s.Tren + ":" + s.Bajada)
                            .ToArray());
                    alarmMsg = "FALLA EN " + fallasStr;
                }

                // Agrupar surcos por tren y calcular layout de centrado
                var trenesGroup = surcosList
                    .GroupBy(s => s.Tren)
                    .OrderBy(g => g.Key);

                var trenes = new List<TrenLayout>();
                foreach (var grupo in trenesGroup)
                {
                    var surcosTren = grupo.OrderBy(s => s.Bajada).ToArray();
                    int count = surcosTren.Length;

                    int totalWidth = count * SensorWidthPx + (count > 1 ? (count - 1) * SensorSpacingPx : 0);

                    // Clamp: si el grupo excede el contenedor, reducir spacing
                    int effectiveSpacing = SensorSpacingPx;
                    if (totalWidth > ContainerWidthPx && count > 1)
                    {
                        effectiveSpacing = Math.Max(0, (ContainerWidthPx - count * SensorWidthPx) / (count - 1));
                        totalWidth = count * SensorWidthPx + (count - 1) * effectiveSpacing;
                    }

                    // Offset X para centrar horizontalmente dentro del contenedor
                    int offsetX = Math.Max(0, (ContainerWidthPx - totalWidth) / 2);

                    trenes.Add(new TrenLayout
                    {
                        Tren = grupo.Key,
                        Count = count,
                        SensorWidthPx = SensorWidthPx,
                        SpacingPx = effectiveSpacing,
                        TotalWidthPx = totalWidth,
                        OffsetXPx = offsetX,
                        Surcos = surcosTren
                    });
                }

                var snapshot = new SeedMonitorSnapshot();
                snapshot.Velocidad = _velocidad;
                snapshot.SpmPromedio = promedio;
                snapshot.FallasActivas = fallas;
                snapshot.SurcosActivos = semillas.Length;
                snapshot.Surcos = surcosList;
                snapshot.Trenes = trenes.ToArray();
                snapshot.ContainerWidthPx = ContainerWidthPx;
                snapshot.LastUpdate = _lastDataTime;
                snapshot.IsConnected = _isConnected;
                snapshot.HasAlarm = hasAlarm;
                snapshot.AlarmMessage = alarmMsg;
                snapshot.NombreImplemento = _implemento.Nombre ?? "";
                snapshot.MonitoreoActivo = _monitoreoActivo;
                snapshot.MetodoInicio = _metodoInicio;
                return snapshot;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_uiTimer != null) _uiTimer.Dispose();
            if (_mqtt != null) _mqtt.Dispose();
        }
    }
}
