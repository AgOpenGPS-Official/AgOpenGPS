using System;
using System.Collections.Generic;
using System.IO;
using AgOpenGPS;
using NUnit.Framework;

namespace AgOpenGPS.Tests.Classes.AgShare
{
    public class AgShareSyncStoreTests
    {
        private string _fieldDirectory;

        [SetUp]
        public void SetUp()
        {
            _fieldDirectory = Path.Combine(Path.GetTempPath(), "AgShareSyncStoreTests_" + Guid.NewGuid());
            Directory.CreateDirectory(_fieldDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_fieldDirectory))
                Directory.Delete(_fieldDirectory, recursive: true);
        }

        private static CTrk MakeAbTrack(double aE, double aN, double bE, double bN)
        {
            return new CTrk
            {
                name = "AB1",
                mode = TrackMode.AB,
                ptA = new vec2(aE, aN),
                ptB = new vec2(bE, bN),
                curvePts = new List<vec3>()
            };
        }

        private static CTrk MakeCurveTrack(params (double e, double n, double h)[] points)
        {
            var curvePts = new List<vec3>();
            foreach (var p in points) curvePts.Add(new vec3(p.e, p.n, p.h));

            return new CTrk
            {
                name = "Curve1",
                mode = TrackMode.Curve,
                ptA = new vec2(),
                ptB = new vec2(),
                curvePts = curvePts
            };
        }

        // --- Save / Load round trip ---

        [Test]
        public void Test_SaveThenLoad_RoundTripsAllFields()
        {
            var state = new AgShareSyncState
            {
                FieldId = Guid.NewGuid(),
                BoundaryHash = "abc123",
                LineHashes = new Dictionary<string, string> { { "AB1", "hash1" }, { "Curve1", "hash2" } }
            };

            AgShareSyncStore.Save(_fieldDirectory, state);
            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded.FieldId, Is.EqualTo(state.FieldId));
            Assert.That(loaded.BoundaryHash, Is.EqualTo(state.BoundaryHash));
            Assert.That(loaded.LineHashes, Is.EquivalentTo(state.LineHashes));
        }

        [Test]
        public void Test_SaveThenLoad_NoLines_RoundTrips()
        {
            var state = new AgShareSyncState
            {
                FieldId = Guid.NewGuid(),
                BoundaryHash = "abc123",
                LineHashes = new Dictionary<string, string>()
            };

            AgShareSyncStore.Save(_fieldDirectory, state);
            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded.LineHashes, Is.Empty);
        }

        [Test]
        public void Test_SaveThenLoad_RoundTripsServer()
        {
            AgShareSyncStore.Save(_fieldDirectory, new AgShareSyncState { FieldId = Guid.NewGuid(), Server = "https://a.example", BoundaryHash = "abc" });

            Assert.That(AgShareSyncStore.Load(_fieldDirectory).Server, Is.EqualTo("https://a.example"));
        }

        [Test]
        public void Test_Load_Version1File_ReturnsNull()
        {
            File.WriteAllText(Path.Combine(_fieldDirectory, "agshare_sync.txt"), "$AgShareSync\n" + Guid.NewGuid() + "\nhash\n0\n");

            Assert.That(AgShareSyncStore.Load(_fieldDirectory), Is.Null);
        }

        [Test]
        public void Test_NormalizeServer_IgnoresCaseSpacesAndTrailingSlash()
        {
            Assert.That(AgShareSyncStore.NormalizeServer(" HTTPS://AgShare.Example/ "), Is.EqualTo("https://agshare.example"));
            Assert.That(AgShareSyncStore.NormalizeServer(null), Is.EqualTo(string.Empty));
        }

        [Test]
        public void Test_Delete_RemovesSavedState()
        {
            AgShareSyncStore.Save(_fieldDirectory, new AgShareSyncState { FieldId = Guid.NewGuid(), BoundaryHash = "abc" });

            AgShareSyncStore.Delete(_fieldDirectory);

            Assert.That(AgShareSyncStore.Load(_fieldDirectory), Is.Null);
        }

        [Test]
        public void Test_Delete_WithoutSidecar_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => AgShareSyncStore.Delete(_fieldDirectory));
        }

        [Test]
        public void Test_Save_Twice_OverwritesPreviousState()
        {
            AgShareSyncStore.Save(_fieldDirectory, new AgShareSyncState
            {
                FieldId = Guid.NewGuid(),
                BoundaryHash = "first",
                LineHashes = new Dictionary<string, string> { { "AB1", "h1" } }
            });

            var secondId = Guid.NewGuid();
            AgShareSyncStore.Save(_fieldDirectory, new AgShareSyncState
            {
                FieldId = secondId,
                BoundaryHash = "second",
                LineHashes = new Dictionary<string, string> { { "AB2", "h2" } }
            });

            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded.FieldId, Is.EqualTo(secondId));
            Assert.That(loaded.BoundaryHash, Is.EqualTo("second"));
            Assert.That(loaded.LineHashes.ContainsKey("AB1"), Is.False);
            Assert.That(loaded.LineHashes["AB2"], Is.EqualTo("h2"));
        }

        // --- Load robustness ---

        [Test]
        public void Test_Load_MissingFile_ReturnsNull()
        {
            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded, Is.Null);
        }

        [Test]
        public void Test_Load_WrongHeader_ReturnsNull()
        {
            File.WriteAllText(Path.Combine(_fieldDirectory, "agshare_sync.txt"), "not the right header\n");

            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded, Is.Null);
        }

        [Test]
        public void Test_Load_TruncatedFile_ReturnsNull()
        {
            // Header + field id, but missing the boundary hash / line count / lines
            File.WriteAllText(Path.Combine(_fieldDirectory, "agshare_sync.txt"), "$AgShareSync\n" + Guid.NewGuid() + "\n");

            var loaded = AgShareSyncStore.Load(_fieldDirectory);

            Assert.That(loaded, Is.Null);
        }

        // --- ComputeBoundaryHash ---

        [Test]
        public void Test_ComputeBoundaryHash_SameCoordinates_ReturnsSameHash()
        {
            var boundary = new List<List<vec3>> { new List<vec3> { new vec3(0, 0, 0), new vec3(10, 0, 0), new vec3(10, 10, 0) } };

            string hash1 = AgShareSyncStore.ComputeBoundaryHash(boundary);
            string hash2 = AgShareSyncStore.ComputeBoundaryHash(boundary);

            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void Test_ComputeBoundaryHash_DifferentCoordinates_ReturnsDifferentHash()
        {
            var boundaryA = new List<List<vec3>> { new List<vec3> { new vec3(0, 0, 0), new vec3(10, 0, 0), new vec3(10, 10, 0) } };
            var boundaryB = new List<List<vec3>> { new List<vec3> { new vec3(0, 0, 0), new vec3(20, 0, 0), new vec3(20, 10, 0) } };

            string hashA = AgShareSyncStore.ComputeBoundaryHash(boundaryA);
            string hashB = AgShareSyncStore.ComputeBoundaryHash(boundaryB);

            Assert.That(hashA, Is.Not.EqualTo(hashB));
        }

        [Test]
        public void Test_ComputeBoundaryHash_SubMillimeterDifference_ReturnsSameHash()
        {
            // Below the 3-decimal (millimeter) rounding used for hashing - must not be seen as a change.
            var boundaryA = new List<List<vec3>> { new List<vec3> { new vec3(0, 0, 0), new vec3(10.0001, 0, 0) } };
            var boundaryB = new List<List<vec3>> { new List<vec3> { new vec3(0, 0, 0), new vec3(10.0002, 0, 0) } };

            string hashA = AgShareSyncStore.ComputeBoundaryHash(boundaryA);
            string hashB = AgShareSyncStore.ComputeBoundaryHash(boundaryB);

            Assert.That(hashA, Is.EqualTo(hashB));
        }

        [Test]
        public void Test_ComputeBoundaryHash_EmptyOrNull_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => AgShareSyncStore.ComputeBoundaryHash(null));
            Assert.DoesNotThrow(() => AgShareSyncStore.ComputeBoundaryHash(new List<List<vec3>>()));
        }

        // --- ComputeLineHash ---

        [Test]
        public void Test_ComputeLineHash_IdenticalAbTracks_ReturnsSameHash()
        {
            var trackA = MakeAbTrack(0, 0, 10, 10);
            var trackB = MakeAbTrack(0, 0, 10, 10);

            Assert.That(AgShareSyncStore.ComputeLineHash(trackA), Is.EqualTo(AgShareSyncStore.ComputeLineHash(trackB)));
        }

        [Test]
        public void Test_ComputeLineHash_DifferentEndpoint_ReturnsDifferentHash()
        {
            var trackA = MakeAbTrack(0, 0, 10, 10);
            var trackB = MakeAbTrack(0, 0, 20, 10);

            Assert.That(AgShareSyncStore.ComputeLineHash(trackA), Is.Not.EqualTo(AgShareSyncStore.ComputeLineHash(trackB)));
        }

        [Test]
        public void Test_ComputeLineHash_DifferentMode_ReturnsDifferentHash()
        {
            var abTrack = MakeAbTrack(0, 0, 10, 10);
            var curveTrack = MakeCurveTrack((0, 0, 0), (10, 10, 0));

            Assert.That(AgShareSyncStore.ComputeLineHash(abTrack), Is.Not.EqualTo(AgShareSyncStore.ComputeLineHash(curveTrack)));
        }

        [Test]
        public void Test_ComputeLineHash_IdenticalCurveTracks_ReturnsSameHash()
        {
            var curveA = MakeCurveTrack((0, 0, 0), (5, 5, 0), (10, 10, 0));
            var curveB = MakeCurveTrack((0, 0, 0), (5, 5, 0), (10, 10, 0));

            Assert.That(AgShareSyncStore.ComputeLineHash(curveA), Is.EqualTo(AgShareSyncStore.ComputeLineHash(curveB)));
        }

        [Test]
        public void Test_ComputeLineHash_DifferentCurvePoints_ReturnsDifferentHash()
        {
            var curveA = MakeCurveTrack((0, 0, 0), (5, 5, 0), (10, 10, 0));
            var curveB = MakeCurveTrack((0, 0, 0), (5, 6, 0), (10, 10, 0));

            Assert.That(AgShareSyncStore.ComputeLineHash(curveA), Is.Not.EqualTo(AgShareSyncStore.ComputeLineHash(curveB)));
        }
    }
}
