using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Satellite map tiles. Each tile is downloaded once and kept on the tablet for good
    /// (files/maptiles/source/z/x/y); the tiles in use are kept decoded in memory. Loading and
    /// downloading run in the background; the screen simply shows a tile once it is there.
    /// </summary>
    public sealed class MapTileProvider
    {
        private const int MemoryTiles = 96;           // about 12 MB as RGB565
        private const int ParallelDownloads = 4;
        private static readonly TimeSpan RetryAfter = TimeSpan.FromSeconds(60);
        private static readonly HttpClient Http = CreateClient();

        private readonly string _root;
        private readonly object _lock = new object();
        private readonly Dictionary<string, LinkedListNode<(string Key, Bitmap Bitmap)>> _memory =
            new Dictionary<string, LinkedListNode<(string, Bitmap)>>();
        private readonly LinkedList<(string Key, Bitmap Bitmap)> _lru = new LinkedList<(string, Bitmap)>();
        private readonly HashSet<string> _pending = new HashSet<string>();
        private readonly Dictionary<string, DateTime> _failed = new Dictionary<string, DateTime>();
        private readonly SemaphoreSlim _downloads = new SemaphoreSlim(ParallelDownloads);

        public MapTileProvider(string root)
        {
            _root = root;
        }

        /// <summary>The map source, e.g. <see cref="MapTiles.DefaultUrl"/>.</summary>
        public string UrlTemplate { get; set; } = MapTiles.DefaultUrl;

        private static HttpClient CreateClient()
        {
            var c = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            c.DefaultRequestHeaders.UserAgent.ParseAdd("AgOpenGPS-Android/0.6");
            return c;
        }

        private string FilePath(TileId t) => System.IO.Path.Combine(_root, MapTiles.SourceKey(UrlTemplate),
            t.Z.ToString(), t.X.ToString(), t.Y + ".img");

        /// <summary>The tile if it is in memory; otherwise null, and it is loaded (from the tablet or the server) in the background.</summary>
        public Bitmap Get(TileId t)
        {
            string key = MapTiles.SourceKey(UrlTemplate) + "/" + t;
            lock (_lock)
            {
                if (_memory.TryGetValue(key, out var node))
                {
                    _lru.Remove(node);
                    _lru.AddFirst(node);
                    return node.Value.Bitmap;
                }
                if (_pending.Contains(key)) return null;
                if (_failed.TryGetValue(key, out DateTime when) && DateTime.UtcNow - when < RetryAfter) return null;
                _pending.Add(key);
            }
            string url = MapTiles.Url(UrlTemplate, t);
            string path = FilePath(t);
            Task.Run(() => LoadAsync(key, url, path));
            return null;
        }

        private async Task LoadAsync(string key, string url, string path)
        {
            Bitmap bmp = null;
            try
            {
                if (!File.Exists(path)) await DownloadAsync(url, path, CancellationToken.None).ConfigureAwait(false);
                if (File.Exists(path))
                {
                    var opts = new BitmapFactory.Options { InPreferredConfig = Bitmap.Config.Rgb565 };
                    bmp = BitmapFactory.DecodeFile(path, opts);
                    if (bmp == null) File.Delete(path); // damaged file: fetch it again next time
                }
            }
            catch (Exception)
            {
                bmp = null; // offline, server refused, storage full: retry later
            }

            lock (_lock)
            {
                _pending.Remove(key);
                if (bmp == null)
                {
                    _failed[key] = DateTime.UtcNow;
                    return;
                }
                _failed.Remove(key);
                _memory[key] = _lru.AddFirst((key, bmp));
                while (_lru.Count > MemoryTiles)
                {
                    // Not recycled: the screen may still be drawing it; the garbage collector frees it.
                    _memory.Remove(_lru.Last.Value.Key);
                    _lru.RemoveLast();
                }
            }
        }

        private async Task DownloadAsync(string url, string path, CancellationToken ct)
        {
            await _downloads.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                using (HttpResponseMessage r = await Http.GetAsync(url, ct).ConfigureAwait(false))
                {
                    r.EnsureSuccessStatusCode();
                    byte[] data = await r.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
                    if (data.Length < 100) throw new IOException("empty tile");
                    Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                    string tmp = path + ".tmp";
                    File.WriteAllBytes(tmp, data);
                    File.Move(tmp, path, true);
                }
            }
            finally
            {
                _downloads.Release();
            }
        }

        /// <summary>
        /// Downloads the tiles of an area that are not on the tablet yet (for use without internet).
        /// Reports (done, total); returns the number that failed.
        /// </summary>
        public async Task<int> PrefetchAsync(IReadOnlyList<TileId> tiles, Action<int, int> progress, CancellationToken ct)
        {
            int done = 0, failed = 0;
            var tasks = new List<Task>();
            foreach (TileId t in tiles)
            {
                string path = FilePath(t), url = MapTiles.Url(UrlTemplate, t);
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (!File.Exists(path)) await DownloadAsync(url, path, ct).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception) { Interlocked.Increment(ref failed); }
                    progress?.Invoke(Interlocked.Increment(ref done), tiles.Count);
                }, ct));
            }
            try { await Task.WhenAll(tasks).ConfigureAwait(false); }
            catch (OperationCanceledException) { }
            lock (_lock) _failed.Clear();
            return failed;
        }

        /// <summary>Size of the saved tiles in bytes.</summary>
        public long DiskBytes()
        {
            try
            {
                if (!Directory.Exists(_root)) return 0;
                long sum = 0;
                foreach (string f in Directory.EnumerateFiles(_root, "*", SearchOption.AllDirectories)) sum += new FileInfo(f).Length;
                return sum;
            }
            catch (IOException) { return 0; }
        }

        /// <summary>Deletes all saved tiles, so they are fetched fresh (e.g. after the imagery was updated).</summary>
        public void ClearDisk()
        {
            lock (_lock)
            {
                _memory.Clear();
                _lru.Clear();
                _failed.Clear();
            }
            try { if (Directory.Exists(_root)) Directory.Delete(_root, true); }
            catch (IOException) { }
        }
    }
}
