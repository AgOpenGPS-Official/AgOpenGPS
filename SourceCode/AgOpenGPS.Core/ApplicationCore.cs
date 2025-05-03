using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Presenters;
using AgOpenGPS.Core.Streamers;
using AgOpenGPS.Core.ViewModels;
using System.IO;

namespace AgOpenGPS.Core
{
    public class ApplicationCore
    {
        private readonly IPanelPresenter _panelPresenter;
        private readonly IErrorPresenter _errorPresenter;

        public ApplicationCore(
            DirectoryInfo baseDirectory,
            IPanelPresenter panelPresenter,
            IErrorPresenter errorPresenter)
        {
            AppModel = new ApplicationModel(baseDirectory);

            FieldStreamerPresenter fieldStreamerPresenter = new FieldStreamerPresenter(errorPresenter);
            FieldStreamer fieldStreamer = new FieldStreamer(fieldStreamerPresenter);
            FieldDescriptionStreamer fieldDescriptionStreamer =
                new FieldDescriptionStreamer(AppModel.FieldsDirectory, fieldStreamerPresenter);

            _panelPresenter = panelPresenter;
            _errorPresenter = errorPresenter;
            AppViewModel = new ApplicationViewModel(AppModel, fieldDescriptionStreamer, fieldStreamer);
            AppPresenter = new ApplicationPresenter(
                AppViewModel,
                _panelPresenter,
                _errorPresenter);
            AppViewModel.SetPresenter(AppPresenter);
        }

        public ApplicationModel AppModel { get; }
        public ApplicationViewModel AppViewModel { get; }
        public ApplicationPresenter AppPresenter { get; }

        public Field ActiveField => AppModel.Fields.ActiveField;

    }
}
