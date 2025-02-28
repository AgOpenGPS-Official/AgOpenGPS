using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class FieldStreamer
    {
        private readonly Field _field;

        private readonly BoundaryStreamer _boundaryStreamer;
        private readonly ContourStreamer _contourStreamer;
        private readonly FlagListStreamer _flagsStreamer;
        private readonly RecordedPathStreamer _recordedPathStreamer;
        private readonly TramLinesStreamer _tramLinesStreamer;
        private readonly WorkedAreaStreamer _workedAreaStreamer;

        private IFieldStreamerPresenter _presenter;

        public FieldStreamer(Field field)
        {
            _field = field;
            _boundaryStreamer = new BoundaryStreamer();
            _contourStreamer = new ContourStreamer();
            _flagsStreamer = new FlagListStreamer();
            _recordedPathStreamer = new RecordedPathStreamer();
            _tramLinesStreamer = new TramLinesStreamer();
            _workedAreaStreamer = new WorkedAreaStreamer();
        }

        public void SetPresenter(IFieldStreamerPresenter presenter)
        {
            _presenter = presenter;
            _boundaryStreamer.SetPresenter(presenter);
            _contourStreamer.SetPresenter(presenter);
            _flagsStreamer.SetPresenter(presenter);
            _recordedPathStreamer.SetPresenter(presenter);
            _tramLinesStreamer.SetPresenter(presenter);
            _workedAreaStreamer.SetPresenter(presenter);
        }

        public DirectoryInfo CurrentFieldDirectory { get; set; }

        public Boundary ReadBoundary(DirectoryInfo fieldDirectory)
        {
            return _boundaryStreamer.Read(fieldDirectory);
        }

        public void ReadBoundary()
        {
            _field.Boundary = _boundaryStreamer.Read(CurrentFieldDirectory);
        }

        public void WriteBoundary()
        {
            _boundaryStreamer.Write(_field.Boundary, CurrentFieldDirectory);
        }

        public void ReadFlagList()
        {
            _field.Flags = _flagsStreamer.TryRead(CurrentFieldDirectory);
        }

        public void WriteFlagList()
        {
            _flagsStreamer.TryWrite(_field.Flags, CurrentFieldDirectory);
        }

        public void ReadContour()
        {
            _field.Contour = _contourStreamer.TryRead(CurrentFieldDirectory);
        }

        public void ContourAppendUnsavedWork()
        {
            _contourStreamer.AppendUnsavedWork(_field.Contour, CurrentFieldDirectory);
        }

        public void ReadRecordedPath(string fileName = null)
        {
            _field.RecordedPath = _recordedPathStreamer.TryRead(CurrentFieldDirectory, fileName);
        }

        public void WriteRecordedPath(string fileName = null)
        {
            _recordedPathStreamer.Write(_field.RecordedPath, CurrentFieldDirectory, fileName);
        }

        public void CreateRecordedPathFile()
        {
            _recordedPathStreamer.CreateFile(CurrentFieldDirectory);
        }

        public void ReadTramLines()
        {
            _field.TramLines = _tramLinesStreamer.TryRead(CurrentFieldDirectory);
        }

        public void WriteTramLines()
        {
            _tramLinesStreamer.Write(_field.TramLines, CurrentFieldDirectory);
        }

        public void ReadWorkedAera()
        {
            _field.WorkedArea = _workedAreaStreamer.Read(CurrentFieldDirectory);
        }

        public void WorkedAreaAppendUnsaveWork()
        {
            _workedAreaStreamer.AppendUnsavedWork(_field.WorkedArea, CurrentFieldDirectory);
        }
    }
}
