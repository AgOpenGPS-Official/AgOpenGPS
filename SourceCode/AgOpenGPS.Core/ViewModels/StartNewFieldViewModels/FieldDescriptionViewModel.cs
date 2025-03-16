using System.IO;

namespace AgOpenGPS.Core.ViewModels
{
    public class FieldDescriptionViewModel
    {
        public FieldDescriptionViewModel(
            DirectoryInfo directoryInfo,
            string area,
            string distance)
        {
            DirectoryInfo = directoryInfo;
            FieldArea = area;
            FieldDistance = distance;
        }

        public DirectoryInfo DirectoryInfo { get; }
        public string FieldName => DirectoryInfo.Name;
        public string FieldArea{ get; }
        public string FieldDistance { get; }

    }

}
