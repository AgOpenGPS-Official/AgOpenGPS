using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectFieldMenuViewModel : DayNightAndUnitsViewModel
    {
        private readonly ApplicationModel _appModel;
        private readonly FieldDescriptionStreamer _fieldDescriptionStreamer;
        private readonly FieldStreamer _fieldStreamer;
        private readonly ISelectFieldPanelPresenter _selectFieldPanelPresenter;
        private SelectNearFieldViewModel _selectNearFieldViewModel;
        private CreateFromExistingFieldViewModel _createFromExistingFieldViewModel;
        private SelectFieldViewModel _selectFieldViewModel;

        public SelectFieldMenuViewModel(
            ApplicationModel appModel,
            FieldDescriptionStreamer fieldDescriptionStreamer,
            FieldStreamer fieldStreamer,
            ISelectFieldPanelPresenter selectFieldPanelPresenter)
        {
            _appModel = appModel;
            _fieldDescriptionStreamer = fieldDescriptionStreamer;
            _fieldStreamer = fieldStreamer;
            _selectFieldPanelPresenter = selectFieldPanelPresenter;
            StartSelectNearFieldCommand = new RelayCommand(StartSelectNearField);
            StartCreateFieldFromExistingCommand = new RelayCommand(StartCreateFieldFromExisting);
            StartSelectFieldCommand = new RelayCommand(StartSelectField);
            CancelCommand = new RelayCommand(Cancel);
        }

        public SelectNearFieldViewModel SelectNearFieldViewModel
        {
            get
            {
                if (_selectNearFieldViewModel == null)
                {
                    _selectNearFieldViewModel =
                        new SelectNearFieldViewModel(
                            _appModel,
                            _fieldDescriptionStreamer,
                            _fieldStreamer,
                            _selectFieldPanelPresenter);
                    AddChild(_selectNearFieldViewModel);
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
                        new CreateFromExistingFieldViewModel(
                            _appModel,
                            _fieldDescriptionStreamer,
                            _fieldStreamer,
                            _selectFieldPanelPresenter);
                    AddChild(_createFromExistingFieldViewModel);
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
                    _selectFieldViewModel =
                        new SelectFieldViewModel(
                            _appModel,
                            _fieldDescriptionStreamer,
                            _fieldStreamer,
                            _selectFieldPanelPresenter);
                    AddChild(_selectFieldViewModel);
                }
                return _selectFieldViewModel;
            }
        }

        public ICommand StartSelectNearFieldCommand { get; }
        public ICommand StartCreateFieldFromExistingCommand { get; }
        public ICommand StartSelectFieldCommand { get; }
        public ICommand CancelCommand { get; }

        public string CurrentFieldName => _appModel.Fields.CurrentFieldName;

        private void StartSelectNearField()
        {
            _selectFieldPanelPresenter.CloseSelectFieldMenuDialog();
            var viewModel = SelectNearFieldViewModel;
            viewModel.UpdateFields();

            var fieldCount = viewModel.FieldDescriptionViewModels?.Count ?? 0;
            if (fieldCount == 0)
            {
                return;
            }

            if (fieldCount == 1)
            {
                ActivateField(viewModel.FieldDescriptionViewModels[0]);
                return;
            }

            _selectFieldPanelPresenter.ShowSelectNearFieldDialog(viewModel);
        }

        private void StartCreateFieldFromExisting()
        {
            _selectFieldPanelPresenter.CloseSelectFieldMenuDialog();
            var viewModel = CreateFromExistingFieldViewModel;
            viewModel.UpdateFields();

            var fieldCount = viewModel.FieldDescriptionViewModels?.Count ?? 0;
            if (fieldCount == 0)
            {
                return;
            }

            if (fieldCount == 1)
            {
                viewModel.LocalSelectedField = viewModel.FieldDescriptionViewModels[0];
            }

            _selectFieldPanelPresenter.ShowCreateFromExistingFieldDialog(viewModel);
        }

        private void StartSelectField()
        {
            _selectFieldPanelPresenter.CloseSelectFieldMenuDialog();
            var viewModel = SelectFieldViewModel;
            viewModel.UpdateFields();

            var fieldCount = viewModel.FieldDescriptionViewModels?.Count ?? 0;
            if (fieldCount == 0)
            {
                return;
            }

            if (fieldCount == 1)
            {
                ActivateField(viewModel.FieldDescriptionViewModels[0]);
                return;
            }

            _selectFieldPanelPresenter.ShowSelectFieldDialog(viewModel);
        }

        private void ActivateField(FieldDescriptionViewModel selectedField)
        {
            if (selectedField == null)
            {
                return;
            }

            var field = new Field(selectedField.DirectoryInfo);
            _fieldStreamer.ReadFlagList(field);
            _appModel.Fields.ActiveField = field;
        }

        private void Cancel()
        {
            _selectFieldPanelPresenter.CloseSelectFieldMenuDialog();
        }

    }
}
