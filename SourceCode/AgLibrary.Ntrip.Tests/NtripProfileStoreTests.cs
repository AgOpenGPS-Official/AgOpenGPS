using System;
using System.IO;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class NtripProfileStoreTests
    {
        private string _folder;
        private NtripProfileStore _store;

        [SetUp]
        public void SetUp()
        {
            _folder = Path.Combine(Path.GetTempPath(), "ntrip-profiles-" + Guid.NewGuid().ToString("N"));
            _store = new NtripProfileStore(_folder);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_folder)) Directory.Delete(_folder, true);
        }

        private static NtripProfileSet Sample(string name) => new NtripProfileSet
        {
            Name = name,
            Main = new ServiceDefinition { Host = "caster.example.org", Port = 2102, Mount = "NEAR", User = "farmer@example.org", Password = "pässwörd:1", Version = NtripVersion.V2, RawTcp = false },
            Fallback = new FallbackDefinition
            {
                Enabled = true,
                Type = FallbackType.NtripService,
                Service = new ServiceDefinition { Host = "rtk2go.com", Mount = "Basisdepeel", User = "farmer@example.org", Password = "x" },
                RadioPort = "COM5",
                RadioBaud = 19200,
                SwitchAfterFailures = 3,
                AutoSwitch = false,
            },
        };

        [Test]
        public void SaveAndLoad_KeepEverything()
        {
            _store.Save(Sample("Home"));
            NtripProfileSet r = _store.Load("Home");

            Assert.That(r, Is.Not.Null);
            Assert.That(r.Name, Is.EqualTo("Home"));
            Assert.That(r.Main.Host, Is.EqualTo("caster.example.org"));
            Assert.That(r.Main.Port, Is.EqualTo(2102));
            Assert.That(r.Main.Mount, Is.EqualTo("NEAR"));
            Assert.That(r.Main.Password, Is.EqualTo("pässwörd:1"), "non-ASCII and ':' survive");
            Assert.That(r.Main.Version, Is.EqualTo(NtripVersion.V2));
            Assert.That(r.Fallback.Enabled, Is.True);
            Assert.That(r.Fallback.Type, Is.EqualTo(FallbackType.NtripService));
            Assert.That(r.Fallback.Service.Host, Is.EqualTo("rtk2go.com"));
            Assert.That(r.Fallback.RadioPort, Is.EqualTo("COM5"));
            Assert.That(r.Fallback.RadioBaud, Is.EqualTo(19200));
            Assert.That(r.Fallback.SwitchAfterFailures, Is.EqualTo(3));
            Assert.That(r.Fallback.AutoSwitch, Is.False);
        }

        [Test]
        public void FixedPosition_BelongsToTheProfile_AndIsKept()
        {
            NtripProfileSet a = Sample("Field A");
            a.FixedPosition = new FixedPositionDefinition { Enabled = true, Latitude = 52.123456, Longitude = 5.654321, Altitude = 12.5 };
            NtripProfileSet b = Sample("Field B");
            b.FixedPosition = new FixedPositionDefinition { Enabled = false, Latitude = 51.0, Longitude = 4.0, Altitude = 1 };
            _store.Save(a);
            _store.Save(b);

            FixedPositionDefinition ra = _store.Load("Field A").FixedPosition;
            Assert.That(ra.Enabled, Is.True);
            Assert.That(ra.Latitude, Is.EqualTo(52.123456));
            Assert.That(ra.Longitude, Is.EqualTo(5.654321));
            Assert.That(ra.Altitude, Is.EqualTo(12.5));
            Assert.That(_store.Load("Field B").FixedPosition.Enabled, Is.False, "another profile has its own");
            Assert.That(_store.Load("Field B").FixedPosition.Latitude, Is.EqualTo(51.0));
        }

        [Test]
        public void AProfileFromBeforeTheFixedPositionWasKeptHere_HasNone()
        {
            _store.Save(Sample("Old"));   // Sample sets no fixed position, like a file written before
            Assert.That(_store.Load("Old").FixedPosition, Is.Null);
        }

        [Test]
        public void SerialRadioFallback_IsKept()
        {
            NtripProfileSet set = Sample("Radio");
            set.Fallback.Type = FallbackType.SerialRadio;
            _store.Save(set);
            Assert.That(_store.Load("Radio").Fallback.Type, Is.EqualTo(FallbackType.SerialRadio));
        }

        [Test]
        public void Names_AreSorted_AndOnlyProfilesCount()
        {
            _store.Save(Sample("field B"));
            _store.Save(Sample("Field a"));
            _store.Save(Sample("Home"));
            File.WriteAllText(Path.Combine(_folder, "notes.txt"), "not a profile");
            File.WriteAllText(Path.Combine(_folder, "Tractor.XML"), "<settings/>");

            Assert.That(_store.Names(), Is.EqualTo(new[] { "Field a", "field B", "Home" }));
        }

        [Test]
        public void MissingFolderOrProfile_IsHandled()
        {
            Assert.That(_store.Names(), Is.Empty);
            Assert.That(_store.Load("Nope"), Is.Null);
            Assert.That(_store.Exists("Nope"), Is.False);
            Assert.That(_store.Exists(""), Is.False);
            _store.Delete("Nope");   // nothing happens
        }

        [Test]
        public void ACorruptFile_IsListed_ButLoadsAsNull()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(Path.Combine(_folder, "Broken" + NtripProfileStore.Extension), "<NtripProfileSet><Main>");
            Assert.That(_store.Names(), Is.EqualTo(new[] { "Broken" }));
            Assert.That(_store.Load("Broken"), Is.Null);
        }

        [Test]
        public void AFileWithOnlyTheMainService_GetsDefaultsForTheRest()
        {
            Directory.CreateDirectory(_folder);
            File.WriteAllText(Path.Combine(_folder, "Small" + NtripProfileStore.Extension),
                "<?xml version=\"1.0\"?><NtripProfileSet><Main><Host>a.example</Host><Mount>M</Mount></Main></NtripProfileSet>");
            NtripProfileSet r = _store.Load("Small");
            Assert.That(r.Main.Host, Is.EqualTo("a.example"));
            Assert.That(r.Main.Port, Is.EqualTo(2101));
            Assert.That(r.Fallback.Enabled, Is.False);
            Assert.That(r.Fallback.SwitchAfterFailures, Is.EqualTo(2));
            Assert.That(r.Fallback.AutoSwitch, Is.True);
        }

        [Test]
        public void SavingAgain_ReplacesTheProfile_AndLeavesNoTemporaryFile()
        {
            _store.Save(Sample("Home"));
            NtripProfileSet changed = Sample("Home");
            changed.Main.Mount = "OTHER";
            _store.Save(changed);

            Assert.That(_store.Load("Home").Main.Mount, Is.EqualTo("OTHER"));
            Assert.That(Directory.GetFiles(_folder), Has.Length.EqualTo(1));
        }

        [Test]
        public void Delete_RemovesIt()
        {
            _store.Save(Sample("Home"));
            _store.Delete("Home");
            Assert.That(_store.Exists("Home"), Is.False);
        }

        [Test]
        public void Rename_MovesIt_AndRefusesATakenOrEmptyName()
        {
            _store.Save(Sample("Home"));
            _store.Save(Sample("Field"));

            Assert.That(_store.Rename("Home", "Farm"), Is.True);
            Assert.That(_store.Names(), Is.EqualTo(new[] { "Farm", "Field" }));
            Assert.That(_store.Rename("Farm", "Field"), Is.False, "name already taken");
            Assert.That(_store.Rename("Farm", "  "), Is.False);
            Assert.That(_store.Rename("Missing", "X"), Is.False);
            Assert.That(_store.Rename("Farm", "FARM"), Is.True, "only the capitals change");
        }

        [Test]
        public void Names_WithPathCharacters_AreMadeSafe()
        {
            Assert.That(NtripProfileStore.FileSafe("  a/b:c*d?  "), Is.EqualTo("a_b_c_d_"));
            Assert.That(NtripProfileStore.FileSafe("name."), Is.EqualTo("name"));
            Assert.That(NtripProfileStore.FileSafe(null), Is.EqualTo(""));

            var set = Sample("North/field");
            _store.Save(set);
            Assert.That(_store.Names(), Is.EqualTo(new[] { "North_field" }));
            Assert.That(_store.Load("North/field"), Is.Not.Null, "the same safe name finds it");
        }

        [Test]
        public void SavingWithoutAName_Throws()
        {
            Assert.Throws<ArgumentException>(() => _store.Save(Sample("  ")));
        }

        [Test]
        public void UniqueName_AddsANumber()
        {
            Assert.That(_store.UniqueName("Home"), Is.EqualTo("Home"));
            _store.Save(Sample("Home"));
            Assert.That(_store.UniqueName("Home"), Is.EqualTo("Home 2"));
            _store.Save(Sample("Home 2"));
            Assert.That(_store.UniqueName("Home"), Is.EqualTo("Home 3"));
            Assert.That(_store.UniqueName(""), Is.EqualTo("NTRIP profile"));
        }

        [Test]
        public void ProfileSet_BuildsTheConnectionSettings()
        {
            NtripProfileSet set = Sample("Home");
            NtripProfile main = set.MainProfile(10);
            Assert.That(main.Host, Is.EqualTo("caster.example.org"));
            Assert.That(main.GgaIntervalSeconds, Is.EqualTo(10));
            Assert.That(main.Version, Is.EqualTo(NtripVersion.V2));

            Assert.That(set.FallbackProfile(10).Host, Is.EqualTo("rtk2go.com"));

            set.Fallback.Enabled = false;
            Assert.That(set.FallbackProfile(10), Is.Null);
            set.Fallback.Enabled = true;
            set.Fallback.Type = FallbackType.SerialRadio;
            Assert.That(set.FallbackProfile(10), Is.Null, "a radio is not an NTRIP profile");

            set.Main.Host = "  ";
            Assert.That(set.MainProfile(10), Is.Null);
        }

        [Test]
        public void Clone_IsIndependent()
        {
            NtripProfileSet a = Sample("Home");
            NtripProfileSet b = a.Clone();
            b.Main.Mount = "CHANGED";
            b.Fallback.Service.Mount = "CHANGED";
            Assert.That(a.Main.Mount, Is.EqualTo("NEAR"));
            Assert.That(a.Fallback.Service.Mount, Is.EqualTo("Basisdepeel"));

            a.FixedPosition = new FixedPositionDefinition { Enabled = true, Latitude = 52 };
            NtripProfileSet c = a.Clone();
            c.FixedPosition.Latitude = 10;
            Assert.That(a.FixedPosition.Latitude, Is.EqualTo(52));
        }

        [Test]
        public void ASerialRadioAsTheMainSource_IsSavedAndLoaded()
        {
            NtripProfileSet set = Sample("Field radio");
            set.MainType = FallbackType.SerialRadio;
            set.MainRadioPort = "COM7";
            set.MainRadioBaud = 57600;
            _store.Save(set);

            NtripProfileSet r = _store.Load("Field radio");

            Assert.That(r.MainIsRadio, Is.True);
            Assert.That(r.MainRadioPort, Is.EqualTo("COM7"));
            Assert.That(r.MainRadioBaud, Is.EqualTo(57600));
            Assert.That(r.MainIsComplete, Is.True);
            Assert.That(r.MainProfile(10), Is.Null, "a radio is not an NTRIP profile");
        }

        [Test]
        public void ARadioWithoutAPort_IsNotComplete_AndAServiceNeedsAnAddress()
        {
            var set = new NtripProfileSet { MainType = FallbackType.SerialRadio };
            Assert.That(set.MainIsComplete, Is.False);
            set.MainRadioPort = "COM3";
            Assert.That(set.MainIsComplete, Is.True);

            set.MainType = FallbackType.NtripService;
            Assert.That(set.MainIsComplete, Is.False, "no address yet");
            set.Main.Host = "caster.example.org";
            Assert.That(set.MainIsComplete, Is.True);
        }

        [Test]
        public void AProfileFileFromBeforeTheRadioMain_StaysAnNtripService()
        {
            _store.Save(Sample("Old"));
            string path = Directory.GetFiles(_folder, "*" + NtripProfileStore.Extension)[0];
            string xml = File.ReadAllText(path);
            foreach (string tag in new[] { "MainType", "MainRadioPort", "MainRadioBaud" })
                xml = System.Text.RegularExpressions.Regex.Replace(xml, "<" + tag + ">[^<]*</" + tag + ">\\s*", "");
            File.WriteAllText(path, xml);

            NtripProfileSet r = _store.Load("Old");

            Assert.That(r, Is.Not.Null);
            Assert.That(r.MainIsRadio, Is.False);
            Assert.That(r.MainProfile(10).Host, Is.EqualTo("caster.example.org"));
        }

        [Test]
        public void RawTcpOnTheFallback_IsSavedAndReachesTheConnectionSettings()
        {
            NtripProfileSet set = Sample("Raw");
            set.Fallback.Service.RawTcp = true;
            _store.Save(set);

            NtripProfileSet r = _store.Load("Raw");

            Assert.That(r.Fallback.Service.RawTcp, Is.True);
            Assert.That(r.FallbackProfile(10).RawTcp, Is.True);
        }
    }
}
