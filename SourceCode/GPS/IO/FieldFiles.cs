using AgOpenGPS.Core.Models;
using AgOpenGPS.IO;
using System.Collections.Generic;

namespace AgOpenGPS.Classes.IO
{
    /// <summary>
    /// Provides one-shot loading of all field-related files into a FieldData object.
    /// </summary>
    public static class FieldFiles
    {
        public static FieldData Load(string fieldDirectory)
        {
            var data = new FieldData
            {
                Origin = FieldPlaneFiles.LoadOrigin(fieldDirectory),
                Tracks = TrackFiles.Load(fieldDirectory),
                Sections = SectionsFiles.Load(fieldDirectory),
                Contours = ContourFiles.Load(fieldDirectory),
                Flags = FlagsFiles.Load(fieldDirectory),
                Boundaries = BoundaryFiles.Load(fieldDirectory),
                Tram = TramFiles.Load(fieldDirectory),
                RecPath = RecPathFiles.Load(fieldDirectory, "RecPath.txt"),
                BackPic = BackPicFiles.Load(fieldDirectory)
            };

            HeadlandFiles.AttachLoad(fieldDirectory, data.Boundaries);

            return data;
        }
    }


    /// <summary>
    /// DTO representing all field-related data as loaded from disk.
    /// </summary>
    public sealed class FieldData
    {
        public Wgs84 Origin { get; set; }

        public List<CTrk> Tracks { get; set; } = new List<CTrk>();

        public SectionsFiles.SectionsData Sections { get; set; } = new SectionsFiles.SectionsData();

        public List<List<vec3>> Contours { get; set; } = new List<List<vec3>>();

        public List<CFlag> Flags { get; set; } = new List<CFlag>();

        public List<CBoundaryList> Boundaries { get; set; } = new List<CBoundaryList>();

        public TramFiles.TramData Tram { get; set; } = new TramFiles.TramData();

        public List<CRecPathPt> RecPath { get; set; } = new List<CRecPathPt>();

        public BackPicFiles.BackPicInfo BackPic { get; set; } = new BackPicFiles.BackPicInfo();
    }
}
