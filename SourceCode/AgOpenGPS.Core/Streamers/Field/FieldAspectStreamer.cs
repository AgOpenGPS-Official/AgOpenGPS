using AgOpenGPS.Core.Interfaces;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public abstract class FieldAspectStreamer
    {
        protected readonly string _defaultFileName;
        protected IFieldStreamerPresenter _presenter;

        public FieldAspectStreamer(
            string defaultFileName,
            IFieldStreamerPresenter presenter)
        {
            _defaultFileName = defaultFileName;
            _presenter = presenter;
        }

        public FileInfo GetFileInfo(DirectoryInfo fieldDirectory, string fileName = null)
        {
            return new FileInfo(Path.Combine(fieldDirectory.FullName, fileName ?? _defaultFileName));
        }

    }
}
