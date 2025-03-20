using AgOpenGPS.Core.Interfaces;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectNearFieldViewModel : FieldTableViewModel
    {
        // TODO filter fields
        private readonly IPanelPresenter _panelPresenter;

        public SelectNearFieldViewModel(
            ApplicationModel appModel,
            IPanelPresenter panelPresenter
        )
            : base(appModel)
        {
            _panelPresenter = panelPresenter;
            SortMode = FieldSortMode.ByDistance;
        }

        protected override void SelectField()
        {
            _panelPresenter.CloseSelectNearFieldDialog();
            base.SelectField();
        }
    }
}
