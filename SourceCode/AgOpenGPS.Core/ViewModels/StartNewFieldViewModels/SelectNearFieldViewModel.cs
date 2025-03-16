using AgLibrary.ViewModels;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectNearFieldViewModel : FieldTableViewModel
    {
        // TODO filter and sort field rows.
        private readonly IPanelPresenter _panelPresenter;

        public SelectNearFieldViewModel(
            ApplicationModel appModel,
            IPanelPresenter panelPresenter
        )
            : base(appModel)
        {
            _panelPresenter = panelPresenter;
        }

        protected override void SelectField()
        {
            _panelPresenter.CloseSelectNearFieldDialog();
            base.SelectField();
        }
    }
}
