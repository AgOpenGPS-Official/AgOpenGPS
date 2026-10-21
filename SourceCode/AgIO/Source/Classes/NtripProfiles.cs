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

        /// <summary>The profile this settings file points to. When there is none (an older settings file that is not
        /// migrated yet), the loose settings of the old window are used.</summary>
        public static NtripProfileSet LoadActive()
        {
            string name = Properties.Settings.Default.setNTRIP_profileName;
            if (!string.IsNullOrWhiteSpace(name))
            {
                NtripProfileSet set = Store.Load(name);
                if (set != null) return set;

                // The profile this settings file points to is not on this computer: do not guess a service.
                return new NtripProfileSet { Name = name };
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

        /// <summary>An older AgIO profile keeps the service in the loose settings only: make an NTRIP profile of it
        /// once, named after the AgIO profile, and empty the loose settings so it is never done twice.</summary>
        public static void MigrateLooseSettings()
        {
            var s = Properties.Settings.Default;
            if (!string.IsNullOrWhiteSpace(s.setNTRIP_profileName)) return;

            NtripProfileSet set = FromLooseSettings();
            if (!set.Main.IsComplete) return;
            if (!s.setNTRIP_isOn && !LooseSettingsWereChanged(s)) return;   // only the factory values: nothing to keep

            string wanted = string.IsNullOrWhiteSpace(RegistrySettings.profileName) ? "Default" : RegistrySettings.profileName;
            NtripProfileStore store = Store;
            try
            {
                set.Name = store.UniqueName(wanted);
                store.Save(set);
            }
            catch (System.Exception)
            {
                // The folder cannot be written: keep working from the loose settings.
                return;
            }

            s.setNTRIP_profileName = set.Name;
            ClearLooseSettings(s);

            // Save at once, so a crash cannot make the next start migrate the same service again.
            if (RegistrySettings.profileLoadResult == AgLibrary.Settings.LoadResult.Ok) s.Save();
        }

        private static bool LooseSettingsWereChanged(Properties.Settings s)
        {
            var factory = new Properties.Settings();
            return s.setNTRIP_casterURL != factory.setNTRIP_casterURL || s.setNTRIP_casterIP != factory.setNTRIP_casterIP
                || s.setNTRIP_mount != factory.setNTRIP_mount || !string.IsNullOrEmpty(s.setNTRIP_userName);
        }

        private static void ClearLooseSettings(Properties.Settings s)
        {
            var factory = new Properties.Settings();
            s.setNTRIP_casterURL = factory.setNTRIP_casterURL;
            s.setNTRIP_casterIP = factory.setNTRIP_casterIP;
            s.setNTRIP_casterPort = factory.setNTRIP_casterPort;
            s.setNTRIP_mount = factory.setNTRIP_mount;
            s.setNTRIP_userName = "";
            s.setNTRIP_userPassword = "";
            s.setNTRIP_isHTTP10 = false;
            s.setNTRIP_isTCP = false;
        }

        /// <summary>Save the profile and make it the one this settings file uses.</summary>
        public static void SaveAndUse(NtripProfileSet set)
        {
            Store.Save(set);
            Properties.Settings.Default.setNTRIP_profileName = NtripProfileStore.FileSafe(set.Name);
        }
    }
}
