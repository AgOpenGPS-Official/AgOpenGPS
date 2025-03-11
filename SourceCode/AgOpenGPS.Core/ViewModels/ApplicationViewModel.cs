using AgLibrary.ViewModels;
using AgOpenGPS.Core.Presenters;
using AgOpenGPS.Core.ViewModels;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    // TODO implement Day and Night mode
    // TODO implement Metric and Imperial mode
    public class ApplicationViewModel : ViewModel
    {
        private readonly ApplicationModel _appModel;
        private ApplicationPresenter _applicationPresenter;
        private StartNewFieldViewModel _startNewFieldViewModel;

        public ApplicationViewModel(ApplicationModel appModel)
        {
            _appModel = appModel;
            StartNewFieldCommand = new RelayCommand(StartNewField);
        }

        public void SetPresenter(ApplicationPresenter appPresenter)
        {
            _applicationPresenter = appPresenter;
            StartNewFieldViewModel.PanelPresenter = appPresenter.PanelPresenter;
        }

        public StartNewFieldViewModel StartNewFieldViewModel
        {
            get
            {
                if (_startNewFieldViewModel == null) _startNewFieldViewModel = new StartNewFieldViewModel(_appModel);
                return _startNewFieldViewModel;
            }
        }

        public ICommand StartNewFieldCommand { get; }
        private void StartNewField()
        {
            _applicationPresenter.PresentStartNewField(StartNewFieldViewModel);
        }
    }
}
