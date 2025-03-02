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

        public Wgs84 CurrentLatLon { get; set; }
        public double latitude => CurrentLatLon.Latitude;
        public double longitude => CurrentLatLon.Longitude;

        //local plane geometry
        public Wgs84 StartLatLon { get; set; }
        public double latStart => StartLatLon.Latitude;
        public double lonStart => StartLatLon.Longitude;
    }
}
