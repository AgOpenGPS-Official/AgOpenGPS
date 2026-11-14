using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    // Tracks what was last successfully uploaded to AgShare for a field, so the next upload
    // only has to send the boundary and/or guidance lines that actually changed.
    public class AgShareSyncState
    {
        public Guid FieldId { get; set; }
        // Normalized URL of the AgShare server this baseline belongs to (see NormalizeServer).
        public string Server { get; set; }
        public string BoundaryHash { get; set; }
        public Dictionary<string, string> LineHashes { get; set; } = new Dictionary<string, string>();
    }

    public static class AgShareSyncStore
    {
        private const string FileName = "agshare_sync.txt";

        // Version 2 added the server line. A version 1 file is treated as unknown state, which just
        // means one full upload that writes the new format.
        private const string HeaderV2 = "$AgShareSync2";

        // Returns null when the sidecar file is missing or malformed - callers treat that as
        // "no known sync state", which safely falls back to a full upload.
        public static AgShareSyncState Load(string fieldDirectory)
        {
            var path = Path.Combine(fieldDirectory, FileName);
            if (!File.Exists(path)) return null;

            try
            {
                using (var reader = new StreamReader(path))
                {
                    var header = reader.ReadLine();
                    if (header == null || header.Trim() != HeaderV2) return null;

                    var fieldId = Guid.Parse(reader.ReadLine().Trim());
                    var server = reader.ReadLine() ?? string.Empty;
                    var boundaryHash = reader.ReadLine() ?? string.Empty;
                    var lineCount = int.Parse(reader.ReadLine().Trim(), CultureInfo.InvariantCulture);

                    var lineHashes = new Dictionary<string, string>();
                    for (int i = 0; i < lineCount; i++)
                    {
                        var line = reader.ReadLine();
                        if (line == null) return null;
                        var parts = line.Split('\t');
                        if (parts.Length != 2) return null;
                        lineHashes[parts[0]] = parts[1];
                    }

                    return new AgShareSyncState
                    {
                        FieldId = fieldId,
                        Server = server,
                        BoundaryHash = boundaryHash,
                        LineHashes = lineHashes
                    };
                }
            }
            catch
            {
                // Corrupt sidecar file - treat as unknown state rather than failing the upload.
                return null;
            }
        }

        // A baseline is only valid for the server it was uploaded to. Compare servers through this, so
        // that case and a trailing slash do not count as a different server.
        public static string NormalizeServer(string server)
        {
            return (server ?? string.Empty).Trim().TrimEnd('/').ToLowerInvariant();
        }

        // Forgets the sync baseline so the next upload is a full replace. Used when the baseline can no
        // longer be trusted (the local data was replaced by a download, or an upload partly failed).
        public static void Delete(string fieldDirectory)
        {
            try
            {
                var path = Path.Combine(fieldDirectory, FileName);
                if (File.Exists(path)) File.Delete(path);
            }
            catch (Exception)
            {
                // Best effort; a stale sidecar only costs a missed cleanup, never data.
            }
        }

        public static void Save(string fieldDirectory, AgShareSyncState state)
        {
            var path = Path.Combine(fieldDirectory, FileName);
            using (var writer = new StreamWriter(path, false))
            {
                writer.WriteLine(HeaderV2);
                writer.WriteLine(state.FieldId.ToString());
                writer.WriteLine(state.Server ?? string.Empty);
                writer.WriteLine(state.BoundaryHash ?? string.Empty);
                writer.WriteLine(state.LineHashes.Count.ToString(CultureInfo.InvariantCulture));
                foreach (var kvp in state.LineHashes)
                {
                    writer.WriteLine(kvp.Key + "\t" + kvp.Value);
                }
            }
        }

        // Hashes the local (pre-WGS84-conversion) boundary rings. Hashing the local coordinates,
        // rounded the same way they're already written to Boundary.txt, keeps this comparison
        // exact and independent of WGS84 conversion/origin round-tripping.
        public static string ComputeBoundaryHash(List<List<vec3>> boundaries)
        {
            var sb = new StringBuilder();
            if (boundaries != null)
            {
                foreach (var ring in boundaries)
                {
                    foreach (var pt in ring)
                    {
                        sb.Append(FileIoUtils.FormatDouble(pt.easting, 3));
                        sb.Append(',');
                        sb.Append(FileIoUtils.FormatDouble(pt.northing, 3));
                        sb.Append(';');
                    }
                    sb.Append('|');
                }
            }
            return ComputeHash(sb.ToString());
        }

        // Hashes a single guidance line's local geometry (endpoints for AB lines, curve points for curves).
        public static string ComputeLineHash(CTrk track)
        {
            var sb = new StringBuilder();
            sb.Append((int)track.mode);
            sb.Append('|');
            sb.Append(FileIoUtils.FormatDouble(track.ptA.easting, 3));
            sb.Append(',');
            sb.Append(FileIoUtils.FormatDouble(track.ptA.northing, 3));
            sb.Append('|');
            sb.Append(FileIoUtils.FormatDouble(track.ptB.easting, 3));
            sb.Append(',');
            sb.Append(FileIoUtils.FormatDouble(track.ptB.northing, 3));
            sb.Append('|');

            var curvePts = track.curvePts ?? Enumerable.Empty<vec3>();
            foreach (var pt in curvePts)
            {
                sb.Append(FileIoUtils.FormatDouble(pt.easting, 3));
                sb.Append(',');
                sb.Append(FileIoUtils.FormatDouble(pt.northing, 3));
                sb.Append(';');
            }

            return ComputeHash(sb.ToString());
        }

        private static string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(bytes.Length * 2);
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2", CultureInfo.InvariantCulture));
                }
                return sb.ToString();
            }
        }
    }
}
