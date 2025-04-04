using AgLibrary.ViewModels;
using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public enum FieldSortMode { ByName, ByDistance, ByArea };

    public class FieldTableViewModel : DayNightAndUnitsViewModel
    {
        protected readonly ApplicationModel _appModel;
        protected FieldDescriptionViewModel _localSelectedField;
        private Collection<FieldDescriptionViewModel> _fieldDescriptions;
        private FieldSortMode _fieldSortMode;

        public FieldTableViewModel(ApplicationModel appModel)
        {
            _appModel = appModel;
            SelectFieldCommand = new RelayCommand(SelectField);
            NextSortModeCommand = new RelayCommand(NextSortMode);
            SortMode = FieldSortMode.ByName;
        }
        public Visibility ByNameVisibility => (SortMode == FieldSortMode.ByName) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ByDistanceVisibility => (SortMode == FieldSortMode.ByDistance) ? Visibility.Visible : Visibility.Collapsed;
        public Visibility ByAreaVisibility => (SortMode == FieldSortMode.ByArea) ? Visibility.Visible : Visibility.Collapsed;

        public ICommand SelectFieldCommand { get; }
        public ICommand NextSortModeCommand { get; }

        public FieldSortMode SortMode
        {
            get { return _fieldSortMode; }
            set
            {
                if (value != _fieldSortMode)
                {
                    _fieldSortMode = value;
                    NotifyAllPropertiesChanged();
                }
            }
        }

        public Collection<FieldDescriptionViewModel> FieldDescriptionViewModels
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

        public virtual void UpdateFields()
        {
            Collection<FieldDescriptionViewModel> viewModels = new Collection<FieldDescriptionViewModel>();
            var descriptions = _appModel.Fields.GetFieldDescriptions();
            foreach (FieldDescription description in descriptions)
            {
                FieldDescriptionViewModel viewModel = new FieldDescriptionViewModel(
                    description,
                    _appModel.CurrentLatLon);
                viewModels.Add(viewModel);
            }
            // The Winforms views do not update when elements inside the ObservableCollection are changed.
            // Therefore change the ObservableCollection as a whole.
            FieldDescriptionViewModels = viewModels;
        }

        protected virtual void SelectField()
        {
            var selectedField = LocalSelectedField;
            if (null != selectedField)
            {
                LocalSelectedField = null;
                _appModel.Fields.OpenField(selectedField.DirectoryInfo);
            }
        }

        private void NextSortMode()
        {
            switch (SortMode)
            {
                case FieldSortMode.ByName:
                    SortMode = FieldSortMode.ByDistance;
                    break;
                case FieldSortMode.ByDistance:
                    SortMode = FieldSortMode.ByArea;
                    break;
                case FieldSortMode.ByArea:
                    SortMode = FieldSortMode.ByName;
                    break;
            }
        }
    }

}
