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
            FieldsDirectory = baseDirectory.CreateSubdirectory("Fields");
            VehiclesDirectory = baseDirectory.CreateSubdirectory("Vehicles");
            SharedFieldProperties = new SharedFieldProperties();

            Fields = new Fields(FieldsDirectory);
        }

        public void SetPresenter(IApplicationPresenter applicationPresenter)
        {
            _applicationPresenter = applicationPresenter;
        }

        public DirectoryInfo FieldsDirectory { get; }
        public DirectoryInfo VehiclesDirectory { get; }

        public SharedFieldProperties SharedFieldProperties { get; }
        public Fields Fields { get; }

        public Wgs84 CurrentLatLon { get; set; }
        public GeoDir FixHeading { get; set; }

        public LocalPlane LocalPlane { get; set; }
    }
}
