using AgOpenGPS.Core.Streamers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    public class Fields
    {
        private readonly DirectoryInfo _fieldsDirectory;
        private Field _currentField;
        private FieldStreamer _fieldStreamer;

        public Fields(DirectoryInfo fieldsDirectory)
        {
            _fieldsDirectory = fieldsDirectory;
        }

        public Field CurrentField
        {
            get { return _currentField; }
            private set
            {
                _currentField = value;
                _fieldStreamer = new FieldStreamer(CurrentField);
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
            fieldDirectory.Delete();
        }

        public void SelectField(string fieldName)
        {
            CurrentField = new Field(fieldName);
        }

    }
}
