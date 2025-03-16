using AgLibrary.ViewModels;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class StartNewFieldViewModel
    {
        private readonly ApplicationModel _appModel;
        private SelectNearFieldViewModel _selectNearFieldViewModel;
        private CreateFromExistingFieldViewModel _createFromExistingFieldViewModel;
        private SelectFieldViewModel _selectFieldViewModel;

        public StartNewFieldViewModel(ApplicationModel appModel)
        {
            _appModel = appModel;
            StartSelectNearFieldCommand = new RelayCommand(StartSelectNearField);
            StartCreateFieldFromExistingCommand = new RelayCommand(StartCreateFieldFromExisting);
            StartSelectFieldCommand = new RelayCommand(StartSelectField);
            CancelCommand = new RelayCommand(Cancel);
        }

        public IPanelPresenter PanelPresenter { set; private get; }

        public SelectNearFieldViewModel SelectNearFieldViewModel
        {
            get
            {
                if (_selectNearFieldViewModel == null)
                {
                    _selectNearFieldViewModel =
                        new SelectNearFieldViewModel(_appModel, PanelPresenter);
                }
                return _selectNearFieldViewModel;
            }
        }

        public CreateFromExistingFieldViewModel CreateFromExistingFieldViewModel
        {
            get
            {
                if (_createFromExistingFieldViewModel == null)
                {
                    _createFromExistingFieldViewModel =
                        new CreateFromExistingFieldViewModel(_appModel, PanelPresenter);
                }
                return _createFromExistingFieldViewModel;
            }
        }

        public SelectFieldViewModel SelectFieldViewModel
        {
            get
            {
                if (_selectFieldViewModel == null)
                {
                    _selectFieldViewModel = new SelectFieldViewModel(_appModel, PanelPresenter);
                }
                return _selectFieldViewModel;
            }
        }

        public ICommand StartSelectNearFieldCommand { get;}
        public ICommand StartCreateFieldFromExistingCommand { get; }
        public ICommand StartSelectFieldCommand { get; }
        public ICommand CancelCommand { get; }

        public string CurrentFieldName => _appModel.Fields.CurrentFieldName;

        private void StartSelectNearField()
        {
            // TODO implement different behaviour if number of fields is 0 or 1
            PanelPresenter.CloseStartNewFieldDialog();
            SelectNearFieldViewModel.UpdateFields();
            PanelPresenter.ShowSelectNearFieldDialog(SelectNearFieldViewModel);
        }

        private void StartCreateFieldFromExisting()
        {
            // TODO implement different behaviour if number of fields is 0 or 1
            PanelPresenter.CloseStartNewFieldDialog();
            CreateFromExistingFieldViewModel.UpdateFields();
            PanelPresenter.ShowCreateFromExistingFieldDialog(CreateFromExistingFieldViewModel);
        }

        private void StartSelectField()
        {
            // TODO implement different behaviour if number of fields is 0 or 1
            PanelPresenter.CloseStartNewFieldDialog();
            SelectFieldViewModel.UpdateFields();
            PanelPresenter.ShowSelectFieldDialog(SelectFieldViewModel);
        }

        private void Cancel()
        {
            PanelPresenter.CloseStartNewFieldDialog();
        }

    }
}
