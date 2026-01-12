using AgOpenGPS.Core.Streamers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace AgOpenGPS.Core.Performance
{
    public struct PerformanceTimeSpan
    {
        public long StartTick;
        public long StopTick;
        public int TimerId;
    }
    public struct PerformanceTimerResult
    {
        public int SpanCount;
        public double AverageTicks;
        public double AverageTime;
        public double Fraction;
    }

    // An instance of PerformanceTool can help to analyse performance.
    // To use it, uncomment the call to InitializePerformanceTool() and register more timers,
    // (instances of PerformanceTimer) if needed.
    // The resulting report will be written to the Fields directory.
    // Unfortunately, we do not have decent tooling yet to view the report, graphically.
    // This version of PerformanceTool is not yet multi-thread safe.
    public class PerformanceTool
    {

        private readonly DirectoryInfo _reportDirectory;
        private readonly Stopwatch _stopwatch;
        private readonly List<PerformanceTimer> _registeredTimers;
        public readonly List<PerformanceTimeSpan> _timeSpans;
        private int _nextTimerId;
        private bool _isRunning;
        private long _startTick;
        private long _stopTick;

        public PerformanceTool(DirectoryInfo reportDirectory)
        {
            _reportDirectory = reportDirectory;
            _registeredTimers = new List<PerformanceTimer>();
            _stopwatch = new Stopwatch();
            _timeSpans = new List<PerformanceTimeSpan>();
        }

        public long Tick => _stopwatch.ElapsedTicks;
        public int TimerCount => _registeredTimers.Count;
        public double TotalTime => (_stopTick - _startTick) / (double)Stopwatch.Frequency;
        public ReadOnlyCollection<PerformanceTimeSpan> TimeSpans => _timeSpans.AsReadOnly();

        public int RegisterTimer(PerformanceTimer timer)
        {
            int timerId = _nextTimerId++;
            _registeredTimers.Add(timer);
            return timerId;
        }

        public void Start()
        {
            _timeSpans.Clear();
            _stopwatch.Restart();
            _startTick = _stopwatch.ElapsedTicks;
            _isRunning = true;
        }

        public void Stop()
        {
            _stopTick = _stopwatch.ElapsedTicks;
            _stopwatch.Stop();
            _isRunning = false;
            Write();
        }

        public void StartStop()
        {
            if (_isRunning)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        public void StoreTimeSpan(PerformanceTimer timer)
        {
            if (!_isRunning) return;
            PerformanceTimeSpan timeSpan = new PerformanceTimeSpan
            {
                StartTick = timer.StartTick,
                StopTick = _stopwatch.ElapsedTicks,
                TimerId = timer.Id
            };
            _timeSpans.Add(timeSpan);
        }

        private void Write()
        {
            var streamer = new PerformanceReportStreamer(null);
            streamer.Write(this, _reportDirectory);
        }

        public string GetName(int timerId)
        {
            return _registeredTimers[timerId].Name;
        }

        public PerformanceTimerResult GetResult(int timerId)
        {
            int spanCount = 0;
            long totalTicks = 0;

            for (int t = 0; t < _timeSpans.Count; t++)
            {
                PerformanceTimeSpan timeSpan = _timeSpans[t];
                if (timeSpan.TimerId == timerId)
                {
                    spanCount++;
                    totalTicks += timeSpan.StopTick - timeSpan.StartTick;
                }
            }
            double averageTicks = (spanCount > 0) ? totalTicks / (double)spanCount : 0.0;
            return new PerformanceTimerResult
            {
                SpanCount = spanCount,
                AverageTicks = averageTicks,
                AverageTime = averageTicks / Stopwatch.Frequency,
                Fraction = totalTicks / (double)(_stopTick - _startTick)
            };
        }
    }
}
