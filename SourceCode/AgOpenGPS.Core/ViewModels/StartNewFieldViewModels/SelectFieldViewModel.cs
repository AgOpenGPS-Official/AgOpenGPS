using AgLibrary.ViewModels;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectFieldViewModel : FieldTableViewModel
    {
        private readonly IPanelPresenter _panelPresenter;
        public SelectFieldViewModel(
            ApplicationModel appModel,
            IPanelPresenter panelPresenter
        )
            : base(appModel)
        {
            _panelPresenter = panelPresenter;
            DeleteFieldCommand = new RelayCommand(DeleteField);
        }

        public ICommand DeleteFieldCommand { get; }

        private void DeleteField()
        {
            // TODO implement confirmation dialog
            var selectedField = LocalSelectedField;
            if (null != selectedField)
            {
                _appModel.Fields.DeleteField(selectedField.DirectoryInfo);
                LocalSelectedField = null;
                UpdateFields();
            }
        }

        protected override void SelectField()
        {
            _panelPresenter.CloseSelectFieldDialog();
            base.SelectField();
        }

    }
}
