using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    /// <summary>A source the test drives by hand: it reports the statuses and frames the test gives it.</summary>
    internal sealed class FakeSource : ICorrectionSource
    {
        private readonly int _frameType;

        public event NtripClient.FrameHandler FrameReceived;
        public event NtripClient.RawHandler RawDataReceived;
        public event Action<NtripStatus> StatusChanged;

        public FakeSource(string description, int frameType)
        {
            Description = description;
            _frameType = frameType;
        }

        public string Description { get; }
        public NtripStatus Status { get; private set; } = new NtripStatus { Rtcm = new RtcmStreamSnapshot(0, 0, 0, 0, new List<RtcmTypeStat>()) };
        public int Runs;
        public volatile bool Stopped;

        public Task RunAsync(CancellationToken ct)
        {
            Interlocked.Increment(ref Runs);
            Stopped = false;
            var done = new TaskCompletionSource<bool>();
            ct.Register(() => { Stopped = true; done.TrySetResult(true); });
            return done.Task;
        }

        public void Set(NtripState state, NtripCategory? error = null, int failures = 0)
        {
            Status = new NtripStatus
            {
                State = state,
                LastError = error,
                ConsecutiveFailures = failures,
                Rtcm = new RtcmStreamSnapshot(0, 0, 0, 0, new List<RtcmTypeStat>()),
            };
            StatusChanged?.Invoke(Status);
        }

        public void Frame()
        {
            byte[] message = Rtcm.Message(_frameType, 10);
            FrameReceived?.Invoke(_frameType, message);
        }

        public void Raw(int length)
        {
            RawDataReceived?.Invoke(new byte[length]);
        }
    }

    [TestFixture]
    public class CorrectionSourceManagerTests
    {
        private const int MainType = 1005;
        private const int FallbackType = 1077;

        private sealed class Rig : IDisposable
        {
            public readonly FakeSource Main = new FakeSource("MAIN on main.example", MainType);
            public readonly List<FakeSource> Fallbacks = new List<FakeSource>();
            public readonly List<int> Frames = new List<int>();
            public readonly List<string> Notices = new List<string>();
            public readonly CorrectionSourceManager Manager;
            private readonly CancellationTokenSource _cts = new CancellationTokenSource();
            private readonly Task _run;

            public Rig(bool withFallback = true, int switchAfter = 2, bool autoSwitch = true, bool keepMain = true)
            {
                Func<ICorrectionSource> factory = null;
                if (withFallback)
                {
                    factory = () =>
                    {
                        var f = new FakeSource("FALLBACK on fallback.example", FallbackType);
                        lock (Fallbacks) Fallbacks.Add(f);
                        return f;
                    };
                }
                Manager = new CorrectionSourceManager(Main, factory, switchAfter, autoSwitch, keepMain);
                Manager.FrameReceived += (type, frame) => { lock (Frames) Frames.Add(type); };
                Manager.Notice += (kind, title, message) => { lock (Notices) Notices.Add(title + "|" + message); };
                _run = Task.Run(() => Manager.RunAsync(_cts.Token));
                WaitFor(() => Main.Runs == 1);
            }

            public FakeSource Fallback => Fallbacks[Fallbacks.Count - 1];

            public int FrameCount(int type)
            {
                lock (Frames) return Frames.FindAll(t => t == type).Count;
            }

            public int NoticeCount(string title)
            {
                lock (Notices) return Notices.FindAll(n => n.StartsWith(title)).Count;
            }

            public bool WaitFor(Func<bool> condition, int ms = 3000)
            {
                var until = DateTime.UtcNow.AddMilliseconds(ms);
                while (DateTime.UtcNow < until)
                {
                    if (condition()) return true;
                    Thread.Sleep(10);
                }
                return false;
            }

            public void Dispose()
            {
                _cts.Cancel();
                try { _run.Wait(2000); } catch (Exception) { }
            }
        }

        [Test]
        public void WithoutAFallback_NothingEverSwitches()
        {
            using (var rig = new Rig(withFallback: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 6);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Manager.Status.FallbackConfigured, Is.False);
                Assert.That(rig.Notices, Is.Empty);
            }
        }

        [Test]
        public void SwitchesAfterTwoFailuresInARow_AndSaysSo()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 1);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main), "one failure is not enough");

                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterBusy, 2);   // another kind of failure still counts
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(1), "the fallback is tried in the background");
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main), "and used only when it delivers corrections");
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(rig.WaitFor(() => rig.Fallback.Runs == 1), Is.True, "the fallback is started");
                Assert.That(rig.Notices[0], Does.StartWith("Switched to the fallback"));
                Assert.That(rig.Notices[0], Does.Contain("FALLBACK on fallback.example"));
                Assert.That(rig.Notices[0], Does.Contain("lose RTK"));
            }
        }

        [Test]
        public void TheLimitIsASetting()
        {
            using (var rig = new Rig(switchAfter: 4))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 3);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 4);
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
            }
        }

        [Test]
        public void MainGivingUp_TriesTheFallbackAtOnce()
        {
            using (var rig = new Rig(switchAfter: 5))
            {
                rig.Main.Set(NtripState.GaveUp, NtripCategory.LoginRejected, 3);
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(1));
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
            }
        }

        [Test]
        public void ConnectingOrStreaming_NeverSwitches()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Connecting, null, 5);
                rig.Main.Set(NtripState.Streaming, null, 0);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
            }
        }

        [Test]
        public void OnlyTheActiveSourcePassesFramesOn()
        {
            using (var rig = new Rig())
            {
                rig.Main.Frame();                                         // main is active
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Frame();                                         // main is only watched now
                rig.Fallback.Frame();
                rig.Fallback.Frame();

                Assert.That(rig.FrameCount(MainType), Is.EqualTo(1));
                Assert.That(rig.FrameCount(FallbackType), Is.EqualTo(2));
            }
        }

        [Test]
        public void RawStreams_FollowTheSameRule()
        {
            using (var rig = new Rig())
            {
                long bytes = 0;
                rig.Manager.RawDataReceived += data => Interlocked.Add(ref bytes, data.Length);
                rig.Main.Raw(10);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Raw(100);
                rig.Fallback.Raw(1000);
                Assert.That(Interlocked.Read(ref bytes), Is.EqualTo(1010));
            }
        }

        [Test]
        public void MainReturning_IsOnlyAnnounced_NeverActed_OnByItself()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);                       // a second update does not ask again

                Assert.That(rig.NoticeCount("The main service is back"), Is.EqualTo(1));
                Assert.That(rig.Manager.Status.MainBack, Is.True);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback), "waits for the user");

                rig.Main.Frame();
                rig.Fallback.Frame();
                Assert.That(rig.FrameCount(MainType), Is.EqualTo(0));
                Assert.That(rig.FrameCount(FallbackType), Is.EqualTo(1));
            }
        }

        [Test]
        public void SwitchBack_UsesTheMainService_AndStopsTheFallback()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                FakeSource first = rig.Fallback;
                first.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);

                rig.Manager.SwitchBackToMain();

                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Manager.Status.MainBack, Is.False);
                Assert.That(rig.WaitFor(() => first.Stopped), Is.True, "the fallback is stopped");
                Assert.That(rig.NoticeCount("Back on the main service"), Is.EqualTo(1));

                rig.Main.Frame();
                first.Frame();
                Assert.That(rig.FrameCount(MainType), Is.EqualTo(1));
                Assert.That(rig.FrameCount(FallbackType), Is.EqualTo(0));
            }
        }

        [Test]
        public void AFallbackThatDoesNotWork_IsNotSwitchedTo_SoADroppedConnectionChangesNothing()
        {
            // A phone hotspot drops: the main service fails and so does the fallback.
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 5);
                rig.Fallback.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 5);
                rig.Fallback.Set(NtripState.Connecting);
                rig.Fallback.Set(NtripState.Connected);   // accepted, but no corrections yet

                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Notices, Is.Empty);
                rig.Main.Frame();
                Assert.That(rig.FrameCount(MainType), Is.EqualTo(1), "the main service still passes its data on");
            }
        }

        // A fallback whose first messages (base station position, raw bytes) come before it reports Streaming.
        private static CorrectionSourceManager ManagerWithAStationFallback(List<FakeSource> fallbacks, List<int> types, long[] raw)
        {
            var main = new FakeSource("MAIN", 1005);
            var manager = new CorrectionSourceManager(main, () =>
            {
                var f = new FakeSource("FALLBACK", 1005);
                lock (fallbacks) fallbacks.Add(f);
                return f;
            }, 2);
            manager.FrameReceived += (t, f) => { lock (types) types.Add(t); };
            manager.RawDataReceived += d => Interlocked.Add(ref raw[0], d.Length);
            main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);   // starts the fallback
            return manager;
        }

        [Test]
        public void WhatTheFallbackSentBeforeTheSwitch_IsReplayedAfterIt_NotLost()
        {
            var fallbacks = new List<FakeSource>();
            var types = new List<int>();
            var raw = new long[1];
            using (var cts = new CancellationTokenSource())
            {
                CorrectionSourceManager manager = ManagerWithAStationFallback(fallbacks, types, raw);
                Task run = Task.Run(() => manager.RunAsync(cts.Token));
                Thread.Sleep(50);
                FakeSource f = fallbacks[0];

                f.Frame();      // the base position, before the fallback says it streams
                f.Frame();      // the same message again: only the latest of a type is kept
                f.Raw(300);
                lock (types) Assert.That(types, Is.Empty, "not the active source yet");

                f.Set(NtripState.Streaming);   // the switch
                Assert.That(manager.Active, Is.EqualTo(SourceRole.Fallback));
                lock (types) Assert.That(types, Is.EqualTo(new[] { 1005 }));
                Assert.That(Interlocked.Read(ref raw[0]), Is.EqualTo(300));

                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void ObservationsOfTheFallback_AreNotReplayed_TheyAreOldBeforeLong()
        {
            var main = new FakeSource("MAIN", 1005);
            var fallback = new FakeSource("FALLBACK", 1077);   // observations
            var manager = new CorrectionSourceManager(main, () => fallback, 2);
            var types = new List<int>();
            manager.FrameReceived += (t, f) => { lock (types) types.Add(t); };
            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => manager.RunAsync(cts.Token));
                main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                Thread.Sleep(50);
                fallback.Frame();
                fallback.Set(NtripState.Streaming);
                Assert.That(manager.Active, Is.EqualTo(SourceRole.Fallback));
                lock (types) Assert.That(types, Is.Empty);
                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void NetworkBack_WhenBothFail_TriesTheMainServiceFirst_AndTheFallbackOnlyWhenMainFailsAgain()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                FakeSource first = rig.Fallback;
                first.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                Thread.Sleep(100);   // the watcher is waiting for the signal

                NetworkWake.Signal();

                Assert.That(rig.WaitFor(() => first.Stopped), Is.True, "the fallback must not race the main service");
                first.Set(NtripState.Streaming);   // a late report of the stopped one changes nothing
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));

                // Main tries and fails again: only now the fallback gets its turn.
                rig.Main.Set(NtripState.Connecting);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 3);
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(2));
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
            }
        }

        [Test]
        public void NetworkBack_WhileTheMainServiceDelivers_ChangesNothing()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Streaming);
                Thread.Sleep(100);
                NetworkWake.Signal();
                Thread.Sleep(100);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Notices, Is.Empty);
            }
        }

        [Test]
        public void NetworkBack_OnAFallbackThatLostItsConnection_GoesBackToMainQuietly()
        {
            using (var rig = new Rig(keepMain: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(rig.Manager.Status.MainPaused, Is.True);

                rig.Fallback.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 1);   // internet gone
                Thread.Sleep(100);
                NetworkWake.Signal();

                Assert.That(rig.WaitFor(() => rig.Manager.Active == SourceRole.Main), Is.True);
                Assert.That(rig.WaitFor(() => rig.Main.Runs == 2), Is.True, "the paused main service is started again");
                Assert.That(rig.NoticeCount("Back on the main service"), Is.EqualTo(0), "not known to work yet");
            }
        }

        [Test]
        public void NetworkBack_OnAFallbackChosenByHand_StaysThere()
        {
            using (var rig = new Rig())
            {
                rig.Manager.SwitchToFallbackNow();
                rig.Fallback.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 1);
                Thread.Sleep(100);
                NetworkWake.Signal();
                Thread.Sleep(150);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
            }
        }

        [Test]
        public void TheMainServiceReturningFirst_StopsTheFallbackThatWasTried()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                FakeSource tried = rig.Fallback;

                rig.Main.Set(NtripState.Streaming);

                Assert.That(rig.WaitFor(() => tried.Stopped), Is.True);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                tried.Set(NtripState.Streaming);   // a late report of the stopped one changes nothing
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Notices, Is.Empty);
            }
        }

        [Test]
        public void AfterTheStop_NoSwitchStartsAnythingAnymore()
        {
            var fallbacks = new List<FakeSource>();
            var main = new FakeSource("MAIN", MainType);
            var manager = new CorrectionSourceManager(main, () =>
            {
                var f = new FakeSource("FALLBACK", FallbackType);
                lock (fallbacks) fallbacks.Add(f);
                return f;
            }, 2, true, false);   // the same login: the main service is paused on the fallback

            var cts = new CancellationTokenSource();
            Task run = Task.Run(() => manager.RunAsync(cts.Token));
            Thread.Sleep(50);
            cts.Cancel();
            Assert.That(run.Wait(3000), Is.True, "stopped");

            // a switch button that was still on the screen, confirmed after the stop
            manager.SwitchToFallbackNow();
            manager.TryMainAgain();
            manager.SwitchBackToMain();

            lock (fallbacks) Assert.That(fallbacks, Is.Empty, "no fallback was started: nothing would stop it");
            Assert.That(manager.Active, Is.EqualTo(SourceRole.Main));
            Assert.That(manager.Status.Using, Is.EqualTo("Main"));
        }

        [Test]
        public void TheFallback_StopsWithTheManager()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);   // the fallback is tried
                FakeSource tried = rig.Fallback;
                Assert.That(rig.WaitFor(() => tried.Runs == 1), Is.True, "the fallback runs");

                rig.Dispose();   // the run is cancelled
                Assert.That(rig.WaitFor(() => tried.Stopped), Is.True, "the fallback stops with the run");
            }
        }

        [Test]
        public void SwitchBack_WhenNotOnTheFallback_DoesNothing()
        {
            using (var rig = new Rig())
            {
                rig.Manager.SwitchBackToMain();
                Assert.That(rig.Notices, Is.Empty);
            }
        }

        [Test]
        public void MainFailingAgainAfterSwitchBack_SwitchesAgain_WithAFreshFallback()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);
                rig.Manager.SwitchBackToMain();

                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);

                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(2));
                Assert.That(rig.NoticeCount("Switched to the fallback"), Is.EqualTo(2));
            }
        }

        [Test]
        public void MainBackFlag_ClearsWhenMainDropsAgain()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 1);
                Assert.That(rig.Manager.Status.MainBack, Is.False);
                rig.Main.Set(NtripState.Streaming);
                Assert.That(rig.NoticeCount("The main service is back"), Is.EqualTo(2));
            }
        }

        [Test]
        public void Status_SaysWhichSourceIsInUse()
        {
            using (var rig = new Rig())
            {
                Assert.That(rig.Manager.Status.Using, Is.EqualTo("Main"));
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);

                ManagerStatus s = rig.Manager.Status;
                Assert.That(s.Using, Is.EqualTo("Fallback: FALLBACK on fallback.example"));
                Assert.That(s.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(s.Status.State, Is.EqualTo(NtripState.Streaming), "the card shows the source in use");
                Assert.That(s.Main.State, Is.EqualTo(NtripState.Waiting));
            }
        }

        [Test]
        public void ManualMode_DoesNotSwitchByItself_ButAsksOnce_WhenTheFallbackWorks()
        {
            using (var rig = new Rig(autoSwitch: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 3);
                Assert.That(rig.NoticeCount("The main service has a problem"), Is.EqualTo(0), "no question while the fallback is not known to work");

                rig.Fallback.Set(NtripState.Streaming);
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(1));
                Assert.That(rig.NoticeCount("The main service has a problem"), Is.EqualTo(1), "asks once, not per update");
                Assert.That(rig.Manager.Status.MainFailing, Is.True);

                rig.Manager.SwitchToFallbackNow();
                Assert.That(rig.Fallbacks.Count, Is.EqualTo(1), "the fallback that works is the one used");
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(rig.Manager.Status.MainFailing, Is.False);
            }
        }

        [Test]
        public void ManualMode_AsksAgainAfterTheMainServiceHasWorkedInBetween()
        {
            using (var rig = new Rig(autoSwitch: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Streaming);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.NoticeCount("The main service has a problem"), Is.EqualTo(2));
                Assert.That(rig.Manager.Status.MainFailing, Is.True);
            }
        }

        [Test]
        public void SwitchingByHand_WorksAtAnyTime_AndDoesNotAskToGoBack()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Streaming);
                rig.Manager.SwitchToFallbackNow();

                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(rig.Notices[0], Does.StartWith("Switched to the fallback"));
                Assert.That(rig.Notices[0], Does.Contain("You switched to the fallback."));

                rig.Main.Set(NtripState.Streaming);                       // the main service is fine: no "it is back"
                Assert.That(rig.NoticeCount("The main service is back"), Is.EqualTo(0));
                Assert.That(rig.Manager.Status.MainBack, Is.False);

                rig.Manager.SwitchBackToMain();
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
            }
        }

        [Test]
        public void SwitchingByHand_WithoutAFallback_DoesNothing()
        {
            using (var rig = new Rig(withFallback: false))
            {
                rig.Manager.SwitchToFallbackNow();
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.Notices, Is.Empty);
            }
        }

        [Test]
        public void SameLogin_MainIsPausedOnTheFallback_AndTriedAgainOnRequest()
        {
            using (var rig = new Rig(keepMain: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(rig.WaitFor(() => rig.Main.Stopped), Is.True, "the main service is not tried in the background");
                Assert.That(rig.Manager.Status.MainPaused, Is.True);

                rig.Manager.TryMainAgain();
                Assert.That(rig.WaitFor(() => rig.Main.Runs == 2 && !rig.Main.Stopped), Is.True, "tried again on request");
                Assert.That(rig.Manager.Status.MainPaused, Is.False);

                rig.Main.Set(NtripState.Streaming);
                Assert.That(rig.NoticeCount("The main service is back"), Is.EqualTo(1));
            }
        }

        [Test]
        public void SwitchingBack_WhileTheMainServiceIsPaused_StartsIt()
        {
            using (var rig = new Rig(keepMain: false))
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Fallback.Set(NtripState.Streaming);
                rig.WaitFor(() => rig.Main.Stopped);

                rig.Manager.SwitchBackToMain();
                Assert.That(rig.Manager.Active, Is.EqualTo(SourceRole.Main));
                Assert.That(rig.WaitFor(() => rig.Main.Runs == 2 && !rig.Main.Stopped), Is.True);
            }
        }

        [Test]
        public void TryMainAgain_WhenNothingIsPaused_DoesNothing()
        {
            using (var rig = new Rig())
            {
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                rig.Manager.TryMainAgain();
                Thread.Sleep(100);
                Assert.That(rig.Main.Runs, Is.EqualTo(1));
            }
        }

        [Test]
        public void SameAccount_IsDecidedOnHostPortAndUser()
        {
            var main = new NtripProfile("rtk2go.com", 2101, "A", "me@example.org", "x");
            Assert.That(FallbackPlan.SameAccount(main, new NtripProfile("RTK2GO.com", 2101, "B", "me@example.org", "y")), Is.True);
            Assert.That(FallbackPlan.SameAccount(main, new NtripProfile("rtk2go.com", 2101, "B", "other@example.org")), Is.False);
            Assert.That(FallbackPlan.SameAccount(main, new NtripProfile("caster.centipede.fr", 2101, "B", "me@example.org")), Is.False);
            Assert.That(FallbackPlan.SameAccount(main, new NtripProfile("rtk2go.com", 2102, "B", "me@example.org")), Is.False);
            Assert.That(FallbackPlan.SameAccount(main, null), Is.False);
        }

        [Test]
        public void ManagerStatusEvents_ComeFromBothSources()
        {
            using (var rig = new Rig())
            {
                int count = 0;
                rig.Manager.StatusChanged += s => Interlocked.Increment(ref count);
                rig.Main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                int afterSwitch = count;
                rig.Fallback.Set(NtripState.Streaming);
                Assert.That(count, Is.GreaterThan(afterSwitch));
            }
        }
    }
}
