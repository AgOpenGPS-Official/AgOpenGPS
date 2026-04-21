// ============================================================================
// AgroParallelModulesConfig.cs - Registro extensible de módulos AgroParallel
// Ubicación: SourceCode/GPS/AgroParallel/Common/AgroParallelModulesConfig.cs
// Target: net48 (C# 7.3)
//
// Lee/escribe agroParallelModules.json en el directorio base de la app.
// Cada módulo declara Name, Enabled, Url, Emoji, IconPath, color, y tamaño
// del popup. Los modulos habilitados se listan en el menu de configuracion
// de AgOpenGPS y abren una ventana popup (ModulePopupForm).
// ============================================================================

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace AgroParallel.Common
{
    public class AgroParallelModuleEntry
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Emoji { get; set; }
        public string IconPath { get; set; }
        public string AccentColorHex { get; set; }
        public int PopupWidth { get; set; }
        public int PopupHeight { get; set; }
        public string PopupTitle { get; set; }
    }

    public class AgroParallelModulesConfig
    {
        public List<AgroParallelModuleEntry> Modules { get; set; }

        private static readonly string ConfigFileName = "agroParallelModules.json";
        private static readonly Color DefaultAccent = Color.FromArgb(0, 180, 80);

        public AgroParallelModulesConfig()
        {
            Modules = new List<AgroParallelModuleEntry>();
        }

        public static AgroParallelModulesConfig Load()
        {
            string path = GetConfigPath();
            if (!File.Exists(path))
            {
                var def = CreateDefault();
                def.Save();
                return def;
            }

            try
            {
                string json = File.ReadAllText(path);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var cfg = JsonSerializer.Deserialize<AgroParallelModulesConfig>(json, opts);

                if (cfg == null || cfg.Modules == null)
                    return CreateDefault();

                return cfg;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[AgroParallel] Error leyendo "
                    + ConfigFileName + ": " + ex.Message);
                return CreateDefault();
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
                System.Diagnostics.Debug.WriteLine("[AgroParallel] Error guardando "
                    + ConfigFileName + ": " + ex.Message);
            }
        }

        public static Color ResolveAccentColor(AgroParallelModuleEntry m)
        {
            if (m == null || string.IsNullOrWhiteSpace(m.AccentColorHex))
                return DefaultAccent;

            string hex = m.AccentColorHex.Trim();
            if (hex.StartsWith("#")) hex = hex.Substring(1);

            if (hex.Length != 6 && hex.Length != 8)
                return DefaultAccent;

            try
            {
                int offset = hex.Length == 8 ? 2 : 0;
                int r = Convert.ToInt32(hex.Substring(offset + 0, 2), 16);
                int g = Convert.ToInt32(hex.Substring(offset + 2, 2), 16);
                int b = Convert.ToInt32(hex.Substring(offset + 4, 2), 16);
                return Color.FromArgb(r, g, b);
            }
            catch
            {
                return DefaultAccent;
            }
        }

        public static string ResolveIconFullPath(string iconPath)
        {
            if (string.IsNullOrWhiteSpace(iconPath)) return null;
            if (Path.IsPathRooted(iconPath)) return iconPath;
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iconPath);
        }

        private static string GetConfigPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigFileName);
        }

        private static AgroParallelModulesConfig CreateDefault()
        {
            var cfg = new AgroParallelModulesConfig();
            cfg.Modules.Add(new AgroParallelModuleEntry
            {
                Name = "VistaX",
                Enabled = true,
                Url = "http://localhost:3000/bar",
                Emoji = "\U0001F33F",
                IconPath = "",
                AccentColorHex = "#00B450",
                PopupWidth = 1200,
                PopupHeight = 800,
                PopupTitle = "VistaX"
            });
            cfg.Modules.Add(new AgroParallelModuleEntry
            {
                Name = "FlowX",
                Enabled = false,
                Url = "http://localhost:3001/",
                Emoji = "\U0001F4A7",
                IconPath = "",
                AccentColorHex = "#1E90FF",
                PopupWidth = 1100,
                PopupHeight = 750,
                PopupTitle = "FlowX"
            });
            cfg.Modules.Add(new AgroParallelModuleEntry
            {
                Name = "QuantiX",
                Enabled = false,
                Url = "http://localhost:3002/",
                Emoji = "\U0001F4CA",
                IconPath = "",
                AccentColorHex = "#FFB000",
                PopupWidth = 1100,
                PopupHeight = 750,
                PopupTitle = "QuantiX"
            });
            cfg.Modules.Add(new AgroParallelModuleEntry
            {
                Name = "StormX",
                Enabled = false,
                Url = "http://localhost:3003/",
                Emoji = "⛈️",
                IconPath = "",
                AccentColorHex = "#B040FF",
                PopupWidth = 1100,
                PopupHeight = 750,
                PopupTitle = "StormX"
            });
            return cfg;
        }
    }
}
