// ============================================================================
// QuantiXConfig.cs - Configuracion de la salida UDP de dosis variable (QuantiX)
// Ubicación: SourceCode/GPS/AgroParallel/QuantiX/QuantiXConfig.cs
// Target: net48 (C# 7.3)
//
// Vive en <BaseDirectory>/quantiX.json (al lado de vistaX.json y
// agroParallelModules.json). Se lee al arrancar AgOpenGPS y al abrir un campo.
// Si Enabled = true, QuantiXSender envia cada 1/SampleRateHz segundos un JSON
// UDP al host:puerto configurados con la dosis actual muestreada del shapefile.
// ============================================================================

using System;
using System.IO;
using System.Text.Json;

namespace AgroParallel.QuantiX
{
    public class QuantiXConfig
    {
        public bool Enabled { get; set; }
        public string UdpHost { get; set; }
        public int UdpPort { get; set; }
        public double SampleRateHz { get; set; }
        public double OutsideValue { get; set; }
        public bool SendOnlyOnChange { get; set; }
        public bool IncludePosition { get; set; }
        public string DoseUnit { get; set; }

        private static readonly string FileName = "quantiX.json";

        public QuantiXConfig()
        {
            Enabled = false;
            UdpHost = "127.0.0.1";
            UdpPort = 17770;
            SampleRateHz = 5.0;
            OutsideValue = 0.0;
            SendOnlyOnChange = false;
            IncludePosition = true;
            DoseUnit = "";
        }

        public static QuantiXConfig Load()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            if (!File.Exists(path))
            {
                var def = new QuantiXConfig();
                def.Save();
                return def;
            }

            try
            {
                string json = File.ReadAllText(path);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cfg = JsonSerializer.Deserialize<QuantiXConfig>(json, opts);
                return cfg ?? new QuantiXConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] Error leyendo "
                    + FileName + ": " + ex.Message);
                return new QuantiXConfig();
            }
        }

        public void Save()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(path, JsonSerializer.Serialize(this, opts));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] Error guardando "
                    + FileName + ": " + ex.Message);
            }
        }
    }

    // Snapshot de la muestra a enviar por UDP. El provider de FormGPS arma
    // uno de estos en cada tick del sender.
    public class QuantiXSample
    {
        public double Dose;
        public bool Inside;
        public string FieldName;
        public double? Latitude;
        public double? Longitude;
        public double? HeadingRad;
    }
}
