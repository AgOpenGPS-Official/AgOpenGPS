using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class FieldDescriptionStreamer
    {
        private readonly DirectoryInfo _fieldsDirectory;
        private readonly OverviewStreamer _overviewStreamer;
        private readonly BoundaryStreamer _boundaryStreamer;

        public FieldDescriptionStreamer(
            DirectoryInfo fieldsDirectory,
            IFieldStreamerPresenter presenter)
        {
            _fieldsDirectory = fieldsDirectory;
            _overviewStreamer = new OverviewStreamer(presenter);
            _boundaryStreamer = new BoundaryStreamer(presenter);
        }

        public FieldDescription CreateFieldDescription(DirectoryInfo fieldDirectory)
        {
            Wgs84? wgs84Start = null;
            double? area = null;
            try
            {
                var overview = _overviewStreamer.Read(fieldDirectory);
                wgs84Start = overview.Start;
            }
            catch (Exception)
            {
                Log.EventWriter("Field (" + fieldDirectory.Name + ") file (Field.txt) could not be read.");
            }
            try
            {
                var boundary = _boundaryStreamer.Read(fieldDirectory);
                area = boundary.Area;
            }
            catch (Exception)
            {
                Log.EventWriter("Field (" + fieldDirectory.Name + ") file (Boundary.txt) could not be read.");
            }
            return new FieldDescription(fieldDirectory, wgs84Start, area);
        }

        public ReadOnlyCollection<FieldDescription> GetFieldDescriptions()
        {
            DirectoryInfo[] fieldDirectories = _fieldsDirectory.GetDirectories();
            List<FieldDescription> list = new List<FieldDescription>();

            foreach (DirectoryInfo fieldDirectory in fieldDirectories)
            {
                FileInfo[] fileInfos = fieldDirectory.GetFiles("Field.txt");

                if (0 < fileInfos.Length)
                {
                    var fieldDescription = CreateFieldDescription(fieldDirectory);
                    if (fieldDescription.Wgs84Start.HasValue)
                    {
                        list.Add(fieldDescription);
                    }
                }
            }
            return list.AsReadOnly();
        }
    }
}
