using AgLibrary.ViewModels;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.Globalization;
using System;
using System.Windows.Input;
using AgOpenGPS.Core.Streamers;

namespace AgOpenGPS.Core.ViewModels
{
    public class CreateFromExistingFieldViewModel : FieldTableViewModel
    {
        private readonly ISelectFieldPanelPresenter _selectFieldPanelPresenter;
        private string _newFieldName = "";

        public CreateFromExistingFieldViewModel(
            ApplicationModel appModel,
            FieldDescriptionStreamer fieldDescriptionStreamer,
            FieldStreamer fieldStreamer,
            ISelectFieldPanelPresenter selectFieldPanelPresenter
        )
            : base(appModel, fieldDescriptionStreamer, fieldStreamer)
        {
            _selectFieldPanelPresenter = selectFieldPanelPresenter;
            AddVehicleCommand = new RelayCommand(AddVehicle);
            AddDateCommand = new RelayCommand(AddDate);
            AddTimeCommand = new RelayCommand(AddTime);
            BackSpaceCommand = new RelayCommand(BackSpace);
        }

        public new FieldDescriptionViewModel LocalSelectedField
        {
            get { return _localSelectedField; }
            set
            {
                if (value != _localSelectedField)
                {
                    _localSelectedField = value;
                    NotifyPropertyChanged();
                    NewFieldName = _localSelectedField.FieldName;
                }
            }
        }

        public string NewFieldName
        {
            get { return _newFieldName; }
            set {
                if (value != _newFieldName )
                {
                    _newFieldName = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand AddVehicleCommand { get; }
        public ICommand AddDateCommand { get; }
        public ICommand AddTimeCommand { get; }
        public ICommand BackSpaceCommand { get; }


        public bool MustCopyFlags { get; set; }
        public bool MustCopyMapping { get; set; }
        public bool MustCopyHeadland { get; set; }
        public bool MustCopyLines { get; set; }

        private void AddVehicle()
        {
            NewFieldName += " " + "Vehicle"; // TODO RegistrySettings.vehicleFileName;
        }

        private void AddDate()
        {
            NewFieldName += " " + DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private void AddTime()
        {
            NewFieldName += " " + DateTime.Now.ToString("HH-mm", CultureInfo.InvariantCulture);
        }

        private void BackSpace()
        {
            if (NewFieldName.Length > 0) NewFieldName = NewFieldName.Remove(NewFieldName.Length - 1);
        }

        protected override void SelectField()
        {
            _selectFieldPanelPresenter.CloseCreateFromExistingFieldDialog();
            // TODO finish streamers, finish Copy method in Fields, finish this.
            base.SelectField();
        }
    }
}
