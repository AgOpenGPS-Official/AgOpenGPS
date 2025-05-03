using AgOpenGPS.Core.Streamers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    public class Fields
    {
        private readonly DirectoryInfo _fieldsDirectory;

        public Fields(DirectoryInfo fieldsDirectory)
        {
            _fieldsDirectory = fieldsDirectory;
        }

        public DirectoryInfo CurrentField { get; set; }
        public string CurrentFieldName => CurrentField != null ? CurrentField.Name : "";

        // Null if no job is started.
        public Field ActiveField { get; set; }

        public void DeleteField(DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Delete(true);
        }

        public void SetCurrentFieldByName(string fieldName)
        {
            CurrentField = new DirectoryInfo(Path.Combine(_fieldsDirectory.FullName, fieldName));
        }

        public void OpenField(DirectoryInfo fieldDirectory)
        {
            CurrentField = fieldDirectory;
            ActiveField = new Field(fieldDirectory);
        }

        // Open the already selected field
        public void OpenField()
        {
            ActiveField = new Field(CurrentField);
        }

        public void CloseField()
        {
            ActiveField = null;
        }

    }
}
