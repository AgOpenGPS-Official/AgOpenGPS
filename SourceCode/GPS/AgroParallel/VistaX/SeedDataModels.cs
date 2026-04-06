// ============================================================================
// SeedDataModels.cs - Modelos para payloads MQTT reales de VistaX
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/SeedDataModels.cs
// Target: net48 (C# 7.3)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AgroParallel.VistaX
{
    // =========================================================================
    // Payload crudo del ESP32: vistax/nodos/telemetria
    // { "uid": "VX-S3-A1", "sensores": [{ "cable": 1, "valor": 14.2, "raw": 5 }] }
    // =========================================================================

    public class EspTelemetriaPayload
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("sensores")]
        public List<EspSensorData> Sensores { get; set; }
    }

    public class EspSensorData
    {
        [JsonPropertyName("cable")]
        public int Cable { get; set; }

        [JsonPropertyName("valor")]
        public double Valor { get; set; }

        [JsonPropertyName("raw")]
        public int Raw { get; set; }
    }

    // =========================================================================
    // Payload de secciones AOG: sections/state
    // { "t1": [1,1,0,1,...], "t2": [1,1,1,...] }
    // =========================================================================

    public class SectionsStatePayload
    {
        [JsonPropertyName("t1")]
        public List<int> T1 { get; set; }

        [JsonPropertyName("t2")]
        public List<int> T2 { get; set; }
    }

    // =========================================================================
    // Configuración de un sensor (mapeo_sensores del JSON de implemento)
    // =========================================================================

    public class SensorConfig
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("cable")]
        public int Cable { get; set; }

        [JsonPropertyName("pin")]
        public int Pin { get; set; }

        [JsonPropertyName("bajada")]
        public int Bajada { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("tren")]
        public int Tren { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        public SensorConfig()
        {
            Tren = 1;
            IsActive = true;
            Tipo = "semilla";
        }
    }

    // =========================================================================
    // Configuración del implemento (perfil JSON completo)
    // =========================================================================

    public class ImplementoSetup
    {
        [JsonPropertyName("densidad_objetivo")]
        public double DensidadObjetivo { get; set; }

        [JsonPropertyName("tolerancia_desvio")]
        public double ToleranciaDesvio { get; set; }

        [JsonPropertyName("distancia_entre_surcos")]
        public double DistanciaEntreSurcos { get; set; }

        [JsonPropertyName("factor_k_default")]
        public double FactorK { get; set; }

        [JsonPropertyName("objetivos_tren")]
        public Dictionary<string, double> ObjetivosTren { get; set; }

        public ImplementoSetup()
        {
            DensidadObjetivo = 16;
            ToleranciaDesvio = 20;
            DistanciaEntreSurcos = 0.191;
            FactorK = 0.15;
        }
    }

    public class ImplementoConfig
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("setup")]
        public ImplementoSetup Setup { get; set; }

        [JsonPropertyName("mapeo_sensores")]
        public List<SensorConfig> MapeoSensores { get; set; }

        public ImplementoConfig()
        {
            Setup = new ImplementoSetup();
            MapeoSensores = new List<SensorConfig>();
        }
    }

    // =========================================================================
    // Estado de cada surco procesado
    // =========================================================================

    public enum RowState
    {
        Ok = 0,
        Failure = 1,
        NoData = 2,
        LowRate = 3,
        HighRate = 4
    }

    public class SurcoState
    {
        public int Bajada { get; set; }
        public string Tipo { get; set; }
        public int Tren { get; set; }
        public double Valor { get; set; }
        public double Spm { get; set; }
        public int NuevasSemillas { get; set; }
        public bool Alerta { get; set; }
        public bool SeccionCortada { get; set; }
        public DateTime LastUpdate { get; set; }

        public RowState State
        {
            get
            {
                if (SeccionCortada) return RowState.NoData;
                if (Alerta) return RowState.Failure;
                if (Spm <= 0) return RowState.NoData;
                return RowState.Ok;
            }
        }
    }

    // =========================================================================
    // Snapshot para UI (thread-safe)
    // =========================================================================

    public class SeedMonitorSnapshot
    {
        public double Velocidad { get; set; }
        public double SpmPromedio { get; set; }
        public int FallasActivas { get; set; }
        public int SurcosActivos { get; set; }
        public SurcoState[] Surcos { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsConnected { get; set; }
        public bool HasAlarm { get; set; }
        public string AlarmMessage { get; set; }
        public string NombreImplemento { get; set; }

        public SeedMonitorSnapshot()
        {
            Surcos = new SurcoState[0];
            AlarmMessage = "";
            NombreImplemento = "";
        }
    }
}
