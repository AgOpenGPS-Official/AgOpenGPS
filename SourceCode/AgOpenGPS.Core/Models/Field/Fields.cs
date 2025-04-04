using AgOpenGPS.Core.Streamers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    public class Fields
    {
        private readonly DirectoryInfo _fieldsDirectory;
        private Field _activeField;
        private FieldStreamer _fieldStreamer;

        public Fields(DirectoryInfo fieldsDirectory)
        {
            _fieldsDirectory = fieldsDirectory;
        }

        public DirectoryInfo CurrentField { get; set; }
        public string CurrentFieldName => CurrentField != null ? CurrentField.Name : "";

        // Is null if no job is started.
        public Field ActiveField
        {
            get { return _activeField; }
            private set
            {
                _activeField = value;
                _fieldStreamer = _activeField != null ? new FieldStreamer(_activeField) : null;
            }
        }

        public FieldStreamer FieldStreamer
        {
            get { return _fieldStreamer; }
        }

        public ReadOnlyCollection<FieldDescription> GetFieldDescriptions()
        {
            DirectoryInfo[] fieldDirectories = _fieldsDirectory.GetDirectories();
            List<FieldDescription> list = new List<FieldDescription>();

            foreach(DirectoryInfo fieldDirectory in fieldDirectories)
            {
                FileInfo[] fileInfos = fieldDirectory.GetFiles("Field.txt");

                if (0 < fileInfos.Length)
                {
                    var fieldDescription = FieldDescription.CreateFieldDescription(fieldDirectory);
                    if (fieldDescription.Wgs84Start.HasValue)
                    {
                        list.Add(fieldDescription);
                    }
                }
            }
            return list.AsReadOnly();
        }

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
