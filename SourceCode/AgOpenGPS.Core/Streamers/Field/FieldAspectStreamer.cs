using AgOpenGPS.Core.Interfaces;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public abstract class FieldAspectStreamer
    {
        protected readonly ILogger _logger;
        protected readonly string _defaultFileName;

        protected IFieldStreamerPresenter _presenter;

        public FieldAspectStreamer(
            ILogger logger,
            string defaultFileName)
        {
            _logger = logger;
            _defaultFileName = defaultFileName;
        }

        public void SetPresenter(IFieldStreamerPresenter presenter)
        {
            _presenter = presenter;
        }

        public string FullPath(string fieldPath, string fileName = null)
        {
            return Path.Combine(fieldPath, fileName ?? _defaultFileName);
        }

        public void CreateDirectory(string fieldPath)
        {
            if (0 < fieldPath.Length && !Directory.Exists(fieldPath))
            {
                Directory.CreateDirectory(fieldPath);
            }
        }
    }
}
