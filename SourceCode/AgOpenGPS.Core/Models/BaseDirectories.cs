using System.IO;

namespace AgOpenGPS.Core.Models
{
    public class BaseDirectories
    {
        public BaseDirectories(DirectoryInfo baseDirectory)
        {
            FieldsDirectory = baseDirectory.CreateSubdirectory("Fields");
            VehiclesDirectory = baseDirectory.CreateSubdirectory("Vehicles");
        }

        public DirectoryInfo FieldsDirectory { get; }
        public DirectoryInfo VehiclesDirectory { get; }

    }
}
