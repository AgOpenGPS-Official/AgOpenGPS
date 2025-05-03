using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class FieldStreamer
    {
        private readonly BoundaryStreamer _boundaryStreamer;
        private readonly ContourStreamer _contourStreamer;
        private readonly FlagListStreamer _flagsStreamer;
        private readonly RecordedPathStreamer _recordedPathStreamer;
        private readonly TramLinesStreamer _tramLinesStreamer;
        private readonly WorkedAreaStreamer _workedAreaStreamer;

        public FieldStreamer(IFieldStreamerPresenter fieldStreamerPresenter)
        {
            _boundaryStreamer = new BoundaryStreamer(fieldStreamerPresenter);
            _contourStreamer = new ContourStreamer(fieldStreamerPresenter);
            _flagsStreamer = new FlagListStreamer(fieldStreamerPresenter);
            _recordedPathStreamer = new RecordedPathStreamer(fieldStreamerPresenter);
            _tramLinesStreamer = new TramLinesStreamer(fieldStreamerPresenter);
            _workedAreaStreamer = new WorkedAreaStreamer(fieldStreamerPresenter);
        }

        public Boundary ReadBoundary(DirectoryInfo fieldDirectory)
        {
            return _boundaryStreamer.Read(fieldDirectory);
        }

        public void ReadBoundary(Field field)
        {
            field.Boundary = _boundaryStreamer.Read(field.FieldDirectory);
        }

        public void WriteBoundary(Field field)
        {
            _boundaryStreamer.Write(field.Boundary, field.FieldDirectory);
        }

        public void ReadFlagList(Field field)
        {
            field.Flags = _flagsStreamer.TryRead(field.FieldDirectory);
        }

        public void WriteFlagList(Field field)
        {
            _flagsStreamer.TryWrite(field.Flags, field.FieldDirectory);
        }

        public void ReadContour(Field field)
        {
            field.Contour = _contourStreamer.TryRead(field.FieldDirectory);
        }

        public void ContourAppendUnsavedWork(Field field)
        {
            _contourStreamer.AppendUnsavedWork(field.Contour, field.FieldDirectory);
        }

        public void ReadRecordedPath(Field field, string fileName = null)
        {
            field.RecordedPath = _recordedPathStreamer.TryRead(field.FieldDirectory, fileName);
        }

        public void WriteRecordedPath(Field field, string fileName = null)
        {
            _recordedPathStreamer.Write(field.RecordedPath, field.FieldDirectory, fileName);
        }

        public void CreateRecordedPathFile(Field field)
        {
            _recordedPathStreamer.CreateFile(field.FieldDirectory);
        }

        public void ReadTramLines(Field field)
        {
            field.TramLines = _tramLinesStreamer.TryRead(field.FieldDirectory);
        }

        public void WriteTramLines(Field field)
        {
            _tramLinesStreamer.Write(field.TramLines, field.FieldDirectory);
        }

        public void ReadWorkedAera(Field field)
        {
            field.WorkedArea = _workedAreaStreamer.Read(field.FieldDirectory);
        }

        public void WorkedAreaAppendUnsaveWork(Field field)
        {
            _workedAreaStreamer.AppendUnsavedWork(field.WorkedArea, field.FieldDirectory);
        }
    }
}
