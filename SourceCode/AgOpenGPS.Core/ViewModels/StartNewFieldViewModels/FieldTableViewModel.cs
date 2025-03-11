using AgLibrary.ViewModels;
using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class FieldTableViewModel : ViewModel
    {
        protected readonly ApplicationModel _appModel;
        private ObservableCollection<FieldDescriptionViewModel> _fieldDescriptions;
        protected FieldDescriptionViewModel _localSelectedField;

        public FieldTableViewModel(ApplicationModel appModel)
        {
            _appModel = appModel;
            SelectFieldCommand = new RelayCommand(SelectField);
            SortCommand = new RelayCommand(Sort);
        }

        public ICommand SelectFieldCommand { get; }
        public ICommand SortCommand { get; }

        public ObservableCollection<FieldDescriptionViewModel> FieldDescriptionViewModels
        {
            get { return _fieldDescriptions; }
            set
            {
                _fieldDescriptions = value;
                NotifyPropertyChanged();
            }
        }

        // The field that is selected in the table (if any). 
        // Probably different from the field that is currently selected in the application

        public FieldDescriptionViewModel LocalSelectedField
        {
            get { return _localSelectedField; }
            set
            {
                if (value != _localSelectedField)
                {
                    _localSelectedField = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public void UpdateFields()
        {
            ObservableCollection<FieldDescriptionViewModel> viewModels = new ObservableCollection<FieldDescriptionViewModel>();
            var descriptions = _appModel.Fields.GetFieldDescriptions();
            foreach (FieldDescription fd in descriptions)
            {
                FieldDescriptionViewModel viewModel = new FieldDescriptionViewModel(
                    fd.FieldDirectory,
                    fd.Area.HasValue ? fd.Area.Value.ToString() : "Error",
                    fd.Wgs84Start.HasValue ? _appModel.CurrentLatLon.DistanceInKiloMeters(fd.Wgs84Start.Value).ToString() : "Error");
                viewModels.Add(viewModel);
            }
            FieldDescriptionViewModels = viewModels;
        }

        protected virtual void SelectField()
        {
            var selectedField = LocalSelectedField;
            if (null != selectedField)
            {
                LocalSelectedField = null;
                _appModel.Fields.SelectField(selectedField.DirectoryInfo);
            }
        }

        private void Sort()
        {
            // TODO implement sorting
        }
    }

}
