using AgOpenGPS.Core.Interfaces;

namespace AgOpenGPS.Core.ViewModels
{
    public class SelectNearFieldViewModel : NearFieldTableViewModel
    {
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
