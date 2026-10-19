using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// The folder with the NTRIP profile files, one file per profile (name.ntrip.xml). Saving writes a
    /// temporary file first, so a power cut in the middle of a save cannot leave half a profile behind.
    /// </summary>
    public sealed class NtripProfileStore
    {
        public const string Extension = ".ntrip.xml";

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(NtripProfileSet));
        private readonly string _folder;

        public NtripProfileStore(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) throw new ArgumentException("folder is empty", nameof(folder));
            _folder = folder;
        }

        public string Folder => _folder;

        /// <summary>A name that can be a file name: no path or reserved characters, trimmed. Empty stays empty.</summary>
        public static string FileSafe(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var sb = new StringBuilder();
            char[] bad = Path.GetInvalidFileNameChars();
            foreach (char c in name.Trim())
                sb.Append(Array.IndexOf(bad, c) >= 0 ? '_' : c);
            return sb.ToString().Trim().TrimEnd('.');
        }

        private string PathOf(string name) => Path.Combine(_folder, FileSafe(name) + Extension);

        /// <summary>The names of the profiles, sorted without regard to capitals.</summary>
        public IReadOnlyList<string> Names()
        {
            var names = new List<string>();
            if (!Directory.Exists(_folder)) return names;
            foreach (string file in Directory.GetFiles(_folder, "*" + Extension))
            {
                string stem = Path.GetFileName(file);
                names.Add(stem.Substring(0, stem.Length - Extension.Length));
            }
            names.Sort(StringComparer.CurrentCultureIgnoreCase);
            return names;
        }

        public bool Exists(string name) => FileSafe(name).Length > 0 && File.Exists(PathOf(name));

        /// <summary>Null when the profile does not exist or cannot be read.</summary>
        public NtripProfileSet Load(string name)
        {
            if (!Exists(name)) return null;
            try
            {
                using (FileStream stream = File.OpenRead(PathOf(name)))
                {
                    var set = (NtripProfileSet)Serializer.Deserialize(stream);
                    if (set.Main == null) set.Main = new ServiceDefinition();
                    if (set.Fallback == null) set.Fallback = new FallbackDefinition();
                    if (set.Fallback.Service == null) set.Fallback.Service = new ServiceDefinition();
                    set.Name = FileSafe(name);
                    return set;
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is IOException || ex is XmlException || ex is UnauthorizedAccessException)
            {
                return null;
            }
        }

        /// <summary>Saves the profile under its name; an existing profile with that name is replaced.</summary>
        public void Save(NtripProfileSet set)
        {
            string safe = FileSafe(set.Name);
            if (safe.Length == 0) throw new ArgumentException("The profile has no name.", nameof(set));

            Directory.CreateDirectory(_folder);
            string target = PathOf(safe);
            string temp = target + ".tmp";

            NtripProfileSet copy = set.Clone();
            copy.Name = safe;
            using (FileStream stream = File.Create(temp))
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) }))
            {
                Serializer.Serialize(writer, copy);
            }

            if (File.Exists(target)) File.Delete(target);
            File.Move(temp, target);
        }

        public void Delete(string name)
        {
            if (Exists(name)) File.Delete(PathOf(name));
        }

        /// <summary>False when the new name is empty or already taken, or the old profile is missing.</summary>
        public bool Rename(string oldName, string newName)
        {
            string safe = FileSafe(newName);
            if (safe.Length == 0 || !Exists(oldName)) return false;
            if (string.Equals(FileSafe(oldName), safe, StringComparison.Ordinal)) return true;
            if (File.Exists(PathOf(safe)) && !string.Equals(FileSafe(oldName), safe, StringComparison.OrdinalIgnoreCase)) return false;

            File.Move(PathOf(oldName), PathOf(safe));
            return true;
        }

        /// <summary>The wanted name when it is free, otherwise "name 2", "name 3", ...</summary>
        public string UniqueName(string wanted)
        {
            string baseName = FileSafe(wanted);
            if (baseName.Length == 0) baseName = "NTRIP profile";
            if (!Exists(baseName)) return baseName;
            for (int i = 2; ; i++)
            {
                string candidate = baseName + " " + i;
                if (!Exists(candidate)) return candidate;
            }
        }
    }
}
