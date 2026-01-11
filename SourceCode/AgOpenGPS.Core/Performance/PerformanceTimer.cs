namespace AgOpenGPS.Core.Performance
{
    public class PerformanceTimer
    {
        private readonly PerformanceTool _performanceTool;

        public PerformanceTimer(
            PerformanceTool performanceTool,
            string name)
        {
            _performanceTool = performanceTool;
            Name = name;
            Id = performanceTool.RegisterTimer(this);
        }

        public string Name { get; private set; }

        public int Id { get; private set; }
        public long StartTick { get; private set; }

        public void Start()
        {
            StartTick = _performanceTool.Tick;
        }

        public void Stop()
        {
            _performanceTool.StoreTimeSpan(this);
        }
    }
}
