using AgLibrary.ViewModels;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectFieldViewModel : FieldTableViewModel
    {
        private readonly ISelectFieldPanelPresenter _selectFieldPanelPresenter;
        public SelectFieldViewModel(
            ApplicationModel appModel,
            FieldDescriptionStreamer fieldDescriptionStreamer,
            FieldStreamer fieldStreamer,
            ISelectFieldPanelPresenter selectFieldPanelPresenter
        )
            : base(appModel, fieldDescriptionStreamer, fieldStreamer)
        {
            _selectFieldPanelPresenter = selectFieldPanelPresenter;
            DeleteFieldCommand = new RelayCommand(DeleteField);
        }

        public ICommand DeleteFieldCommand { get; }

        private void DeleteField()
        {
            var selectedField = LocalSelectedField;
            if (null != selectedField)
            {
                if (_selectFieldPanelPresenter.ShowConfirmDeleteMessageBox(selectedField.FieldName))
                {
                    _appModel.Fields.DeleteField(selectedField.DirectoryInfo);
                    LocalSelectedField = null;
                    UpdateFields();
                }
            }
        }

        protected override void SelectField()
        {
            _selectFieldPanelPresenter.CloseSelectFieldDialog();
            base.SelectField();
        }

    }
}
