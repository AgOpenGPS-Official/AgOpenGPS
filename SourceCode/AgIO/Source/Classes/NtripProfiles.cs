using System.IO;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// The NTRIP profiles of AgIO: each is its own file with a main and a fallback service. The AgIO settings
    /// file only remembers the name of the profile in use (setNTRIP_profileName), so another settings file
    /// brings its own NTRIP profile, while choosing another NTRIP profile leaves the other settings alone.
    /// </summary>
    internal static class NtripProfiles
    {
        public static NtripProfileStore Store
        {
            get { return new NtripProfileStore(Path.Combine(RegistrySettings.profileDirectory, "NtripProfiles")); }
        }

        /// <summary>The profile this settings file points to. When there is none (an older settings file, or the
        /// file is not on this computer), the loose settings of the old window are used.</summary>
        public static NtripProfileSet LoadActive()
        {
            string name = Properties.Settings.Default.setNTRIP_profileName;
            if (!string.IsNullOrWhiteSpace(name))
            {
                NtripProfileSet set = Store.Load(name);
                if (set != null) return set;
            }
            return FromLooseSettings();
        }

        /// <summary>The main service as the old settings window kept it. Never has a fallback.</summary>
        public static NtripProfileSet FromLooseSettings()
        {
            var s = Properties.Settings.Default;
            string host = !string.IsNullOrWhiteSpace(s.setNTRIP_casterURL) ? s.setNTRIP_casterURL : s.setNTRIP_casterIP;
            return new NtripProfileSet
            {
                Name = s.setNTRIP_profileName ?? "",
                Main = new ServiceDefinition
                {
                    Host = host ?? "",
                    Port = s.setNTRIP_casterPort,
                    Mount = s.setNTRIP_mount ?? "",
                    User = s.setNTRIP_userName ?? "",
                    Password = s.setNTRIP_userPassword ?? "",
                    Version = s.setNTRIP_isHTTP10 ? NtripVersion.V1 : NtripVersion.Auto,
                    RawTcp = s.setNTRIP_isTCP,
                },
            };
        }

        /// <summary>An older settings file has the service in the loose settings only: make a profile of it once.</summary>
        public static void MigrateLooseSettings()
        {
            var s = Properties.Settings.Default;
            if (!s.setNTRIP_isOn || !string.IsNullOrWhiteSpace(s.setNTRIP_profileName)) return;

            NtripProfileSet set = FromLooseSettings();
            if (!set.Main.IsComplete) return;

            NtripProfileStore store = Store;
            set.Name = store.UniqueName("Default");
            try
            {
                store.Save(set);
                s.setNTRIP_profileName = set.Name;
            }
            catch (System.Exception)
            {
                // The folder cannot be written: keep working from the loose settings.
            }
        }

        /// <summary>Save the profile and make it the one this settings file uses.</summary>
        public static void SaveAndUse(NtripProfileSet set)
        {
            Store.Save(set);
            Properties.Settings.Default.setNTRIP_profileName = NtripProfileStore.FileSafe(set.Name);
        }
    }
}
