using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>
    /// Recognises "no imagery here" tiles. Map servers often answer a zoom level they do not have
    /// with a normal-looking image ("Map data not yet available") instead of an error, so it
    /// would be shown instead of the coarser real imagery. Known placeholders are listed; others
    /// are learned: the same small image arriving for two different tiles is a placeholder, not
    /// a photo of the ground. Thread safe.
    /// </summary>
    public sealed class TilePlaceholders
    {
        /// <summary>Real imagery is larger than this; placeholders are plain images and smaller.</summary>
        public const int MaxPlaceholderBytes = 6000;
        /// <summary>Only learned at detailed levels (coarse levels exist everywhere).</summary>
        public const int MinLearnZoom = 15;

        // MD5 of Esri World Imagery's grey "Map data not yet available" tile.
        private static readonly string[] Known = { "f27d9de7f80c13501f470595e327aa6d" };

        private readonly object _lock = new object();
        private readonly HashSet<string> _placeholders = new HashSet<string>(Known);
        private readonly Dictionary<string, TileId> _seenSmall = new Dictionary<string, TileId>();
        private readonly string _file;

        /// <param name="file">Where learned placeholders are kept between runs, or null.</param>
        public TilePlaceholders(string file = null)
        {
            _file = file;
            try
            {
                if (_file != null && File.Exists(_file))
                    foreach (string line in File.ReadAllLines(_file))
                        if (line.Trim().Length == 32) _placeholders.Add(line.Trim());
            }
            catch (IOException) { }
        }

        /// <summary>True if <paramref name="data"/> is a "no imagery" placeholder (and learns new ones).</summary>
        public bool IsPlaceholder(TileId tile, byte[] data)
        {
            if (data == null || data.Length > MaxPlaceholderBytes) return false;
            string hash = Hash(data);
            lock (_lock)
            {
                if (_placeholders.Contains(hash)) return true;
                if (tile.Z < MinLearnZoom) return false;

                if (_seenSmall.TryGetValue(hash, out TileId first) && !first.Equals(tile))
                {
                    _placeholders.Add(hash);
                    _seenSmall.Remove(hash);
                    Persist(hash);
                    return true;
                }
                if (_seenSmall.Count < 5000) _seenSmall[hash] = tile;
                return false;
            }
        }

        private void Persist(string hash)
        {
            if (_file == null) return;
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_file));
                File.AppendAllText(_file, hash + "\n");
            }
            catch (IOException) { }
        }

        private static string Hash(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
                return Convert.ToHexString(md5.ComputeHash(data)).ToLowerInvariant();
        }
    }
}
