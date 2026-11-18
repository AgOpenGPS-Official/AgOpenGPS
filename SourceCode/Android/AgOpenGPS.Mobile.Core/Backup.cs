using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// One backup file: all settings (as typed key/value pairs) plus the fields as the GeoJSON of
    /// <see cref="Guidance.FieldExport"/>. Restoring it on another tablet sets the app up the same way.
    /// </summary>
    public static class Backup
    {
        public const string Type = "AgOpenGPSAndroidBackup";

        /// <param name="settings">Setting name to value (string or bool).</param>
        public static string Write(IDictionary<string, object> settings, string fieldsGeoJson, DateTime nowUtc)
        {
            using (var ms = new MemoryStream())
            {
                using (var w = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true }))
                {
                    w.WriteStartObject();
                    w.WriteString("type", Type);
                    w.WriteNumber("version", 1);
                    w.WriteString("created", nowUtc.ToString("o", CultureInfo.InvariantCulture));
                    w.WriteStartObject("settings");
                    foreach (KeyValuePair<string, object> kv in settings)
                    {
                        if (kv.Value is bool b) w.WriteBoolean(kv.Key, b);
                        else if (kv.Value != null) w.WriteString(kv.Key, Convert.ToString(kv.Value, CultureInfo.InvariantCulture));
                    }
                    w.WriteEndObject();
                    w.WritePropertyName("fields");
                    if (string.IsNullOrWhiteSpace(fieldsGeoJson)) w.WriteNullValue();
                    else using (JsonDocument f = JsonDocument.Parse(fieldsGeoJson)) f.RootElement.WriteTo(w);
                    w.WriteEndObject();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>Reads a backup. Throws FormatException if the file is not one.</summary>
        public static void Read(string text, out Dictionary<string, object> settings, out string fieldsGeoJson)
        {
            settings = new Dictionary<string, object>();
            fieldsGeoJson = null;
            JsonDocument doc;
            try { doc = JsonDocument.Parse(text ?? ""); }
            catch (JsonException) { throw new FormatException("Not an AgOpenGPS backup file"); }
            using (doc)
            {
                JsonElement root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("type", out JsonElement t) || t.GetString() != Type)
                    throw new FormatException("Not an AgOpenGPS backup file");
                if (root.TryGetProperty("settings", out JsonElement s) && s.ValueKind == JsonValueKind.Object)
                {
                    foreach (JsonProperty p in s.EnumerateObject())
                    {
                        if (p.Value.ValueKind == JsonValueKind.True || p.Value.ValueKind == JsonValueKind.False) settings[p.Name] = p.Value.GetBoolean();
                        else if (p.Value.ValueKind == JsonValueKind.String) settings[p.Name] = p.Value.GetString();
                    }
                }
                if (root.TryGetProperty("fields", out JsonElement f) && f.ValueKind == JsonValueKind.Object)
                    fieldsGeoJson = f.GetRawText();
            }
        }
    }
}
