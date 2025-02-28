using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core
{
    public class ApplicationCore
    {

        public ApplicationCore(DirectoryInfo baseDirectory)
        {
            AppModel = new ApplicationModel(baseDirectory);
        }

        public ApplicationModel AppModel { get; }

        public Field CurrentField => AppModel.Fields.CurrentField;

    }
}
