// ============================================================================
// ShapefilePersistence.cs - Persistencia por-campo del shape cargado + estilo
// Ubicación: SourceCode/GPS/AgroParallel/Common/ShapefilePersistence.cs
// Target: net48 (C# 7.3)
//
// Guarda/lee un shapefile.json dentro del directorio del campo activo para
// que al reabrir ese campo el shape + estilo se restauren automaticamente.
// El archivo vive junto al Field.txt y demas archivos del campo, asi que
// desaparece al borrar el campo.
// ============================================================================

using System;
using System.IO;
using System.Text.Json;

namespace AgroParallel.Common
{
    public class ShapefileFieldConfig
    {
        public string ShpPath { get; set; }
        public string StyleField { get; set; }
        public bool Visible { get; set; } = true;
        public bool ShowFill { get; set; } = true;
        public bool ShowOutline { get; set; } = true;
    }

    public static class ShapefilePersistence
    {
        public const string FileName = "shapefile.json";

        public static ShapefileFieldConfig Load(string fieldDirectoryFullPath)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectoryFullPath)) return null;

            string path = Path.Combine(fieldDirectoryFullPath, FileName);
            if (!File.Exists(path)) return null;

            try
            {
                string json = File.ReadAllText(path);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<ShapefileFieldConfig>(json, opts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] Error leyendo " + path
                    + ": " + ex.Message);
                return null;
            }
        }

        public static void Save(string fieldDirectoryFullPath, ShapefileFieldConfig cfg)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectoryFullPath)) return;
            if (cfg == null) return;
            if (!Directory.Exists(fieldDirectoryFullPath)) return;

            string path = Path.Combine(fieldDirectoryFullPath, FileName);
            try
            {
                var opts = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(path, JsonSerializer.Serialize(cfg, opts));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] Error guardando " + path
                    + ": " + ex.Message);
            }
        }

        public static void Delete(string fieldDirectoryFullPath)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectoryFullPath)) return;
            string path = Path.Combine(fieldDirectoryFullPath, FileName);
            try { if (File.Exists(path)) File.Delete(path); }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] Error borrando " + path
                    + ": " + ex.Message);
            }
        }
    }
}
