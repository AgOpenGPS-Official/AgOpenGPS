// ============================================================================
// VistaXConfig.cs - Configuración adaptada a la infraestructura real VistaX
// Ubicación: SourceCode/GPS/AgroParallel/VistaX/VistaXConfig.cs
// Target: net48 (C# 7.3)
// ============================================================================

using System;
using System.IO;
using System.Text.Json;

namespace AgroParallel.VistaX
{
    public class VistaXConfig
    {
        public bool Enabled { get; set; }
        public string BrokerAddress { get; set; }
        public int BrokerPort { get; set; }
        public string ClientId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseTls { get; set; }
        public string TelemetriaTopic { get; set; }
        public string SpeedTopic { get; set; }
        public string SectionsTopic { get; set; }
        public string ImplementoJsonPath { get; set; }
        public int UiUpdateIntervalMs { get; set; }
        public int SensorTimeoutMs { get; set; }
        public bool LogToFieldRecord { get; set; }

        // Nueva propiedad para el servidor de interfaz moderna
        public string ServerUrl { get; set; }

        private static readonly string ConfigFileName = "vistaX.json";

        public VistaXConfig()
        {
            Enabled = false;
            BrokerAddress = "127.0.0.1";
            BrokerPort = 1883;
            ClientId = "AgOpenGPS_VistaX";
            Username = "";
            Password = "";
            UseTls = false;
            TelemetriaTopic = "vistax/nodos/telemetria";
            SpeedTopic = "aog/machine/speed";
            SectionsTopic = "sections/state";
            ImplementoJsonPath = "";
            UiUpdateIntervalMs = 500;
            SensorTimeoutMs = 3000;
            LogToFieldRecord = true;

            // Valor por defecto para el servidor local
            ServerUrl = "http://localhost:3001";
        }

        public static VistaXConfig Load()
        {
            string path = GetConfigPath();
            if (!File.Exists(path))
            {
                var def = new VistaXConfig();
                def.Save();
                return def;
            }

            try
            {
                string json = File.ReadAllText(path);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var config = JsonSerializer.Deserialize<VistaXConfig>(json, opts);

                return config ?? new VistaXConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Error config: " + ex.Message);
                return new VistaXConfig();
            }
        }

        public void Save()
        {
            try
            {
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(GetConfigPath(), JsonSerializer.Serialize(this, opts));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Error guardando: " + ex.Message);
            }
        }

        public ImplementoConfig LoadImplemento()
        {
            if (!string.IsNullOrEmpty(ImplementoJsonPath) && File.Exists(ImplementoJsonPath))
            {
                return ParseImplemento(ImplementoJsonPath);
            }

            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "implementos");
            if (Directory.Exists(dataDir))
            {
                string[] files = Directory.GetFiles(dataDir, "*.json");
                if (files.Length > 0)
                {
                    return ParseImplemento(files[0]);
                }
            }

            System.Diagnostics.Debug.WriteLine("[VistaX] No se encontró JSON de implemento");
            return new ImplementoConfig();
        }

        private static ImplementoConfig ParseImplemento(string path)
        {
            try
            {
                string json = File.ReadAllText(path);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var config = JsonSerializer.Deserialize<ImplementoConfig>(json, opts);
                System.Diagnostics.Debug.WriteLine("[VistaX] Implemento cargado: " + path
                    + " (" + (config.MapeoSensores != null ? config.MapeoSensores.Count : 0) + " sensores)");
                return config ?? new ImplementoConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] Error parseando implemento: " + ex.Message);
                return new ImplementoConfig();
            }
        }

        private static string GetConfigPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        }
    }
}