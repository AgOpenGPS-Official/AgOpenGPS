using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using System.IO;

namespace AgOpenGPS.Core
{
    public class ApplicationModel
    {

        private IApplicationPresenter _applicationPresenter;

        public ApplicationModel(DirectoryInfo baseDirectory)
        {
            BaseDirectories = new BaseDirectories(baseDirectory);
            Fields = new Fields(BaseDirectories.FieldsDirectory);
        }

        public void SetPresenter(IApplicationPresenter applicationPresenter)
        {
            _applicationPresenter = applicationPresenter;
            FieldStreamer.SetPresenter(_applicationPresenter.FieldStreamerPresenter);
        }

        public BaseDirectories BaseDirectories { get; }

        public Fields Fields { get; }

        public FieldStreamer FieldStreamer => Fields.FieldStreamer;

    }
}
