﻿using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Translations;
using System;

namespace AgOpenGPS.Core.Presenters
{
    public class FieldStreamerPresenter : IFieldStreamerPresenter
    {
        private readonly IErrorPresenter _errorPresenter;

        public FieldStreamerPresenter(IErrorPresenter errorPresenter)
        {
            _errorPresenter = errorPresenter;
        }

        void IFieldStreamerPresenter.PresentBoundaryFileMissing()
        {
            ShowStreamingError(gStr.gsMissingBoundaryFile);
        }

        void IFieldStreamerPresenter.PresentBoundaryFileCorrupt()
        {
            ShowStreamingError(gStr.gsBoundaryLineFilesAreCorrupt);
        }

        void IFieldStreamerPresenter.PresentContourFileMissing()
        {
            ShowStreamingError(gStr.gsMissingContourFile);
        }

        void IFieldStreamerPresenter.PresentContourFileCorrupt()
        {
            ShowStreamingError(gStr.gsContourFileIsCorrupt);
        }

        void IFieldStreamerPresenter.PresentCurveLineFileCorrupt()
        {
            ShowStreamingError(gStr.gsCurveLineFileIsCorrupt);
        }

        void IFieldStreamerPresenter.PresentFlagsFileMissing()
        {
            ShowStreamingError(gStr.gsMissingFlagsFile);
        }

        void IFieldStreamerPresenter.PresentFlagsFileCorrupt()
        {
            ShowStreamingError(gStr.gsFlagFileIsCorrupt);
        }

        void IFieldStreamerPresenter.PresentRecordedPathFileCorrupt()
        {
            ShowStreamingError(gStr.gsRecordedPathFileIsCorrupt);
        }

        void IFieldStreamerPresenter.PresentSectionFileMissing()
        {
            ShowStreamingError(gStr.gsMissingSectionFile);
        }

        void IFieldStreamerPresenter.PresentSectionFileCorrupt()
        {
            ShowStreamingError("Section File is Corrupt");
        }

        void IFieldStreamerPresenter.PresentTramLinesFileCorrupt()
        {
            ShowStreamingError("Tram is corrupt");
        }

        private void ShowStreamingError(string errorMsg)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(2.0);
            _errorPresenter.PresentTimedMessage(timeSpan, errorMsg, gStr.gsButFieldIsLoaded);
        }
    }
}
