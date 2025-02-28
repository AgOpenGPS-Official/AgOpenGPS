using AgOpenGPS.Core.Interfaces;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public abstract class FieldAspectStreamer
    {
        protected readonly string _defaultFileName;

        protected IFieldStreamerPresenter _presenter;

        public FieldAspectStreamer(string defaultFileName)
        {
            _defaultFileName = defaultFileName;
        }

        public void SetPresenter(IFieldStreamerPresenter presenter)
        {
            _presenter = presenter;
        }

        public string FullPath(DirectoryInfo fieldDirectory, string fileName = null)
        {
            return Path.Combine(fieldDirectory.FullName, fileName ?? _defaultFileName);
        }

    }
}
