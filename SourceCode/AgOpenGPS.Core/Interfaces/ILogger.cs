namespace AgOpenGPS.Core.Interfaces
{
    public interface ILogger
    {
        void LogError(string errorMsg);

        string BaseDir { set; }
    }
}
