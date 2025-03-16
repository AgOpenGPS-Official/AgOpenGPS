using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.ViewModels;
using System.IO;

namespace AgOpenGPS.Core.Presenters
{
    public class ApplicationPresenter : IApplicationPresenter
    {
        private readonly ApplicationViewModel _applicationViewModel;

        public ApplicationPresenter(
            ApplicationViewModel applicationViewModel,
            IPanelPresenter panelPresenter,
            IErrorPresenter errorrPresenter)
        {
            _applicationViewModel = applicationViewModel;
            PanelPresenter = panelPresenter;
            ErrorPresenter = errorrPresenter;
        }

        public IPanelPresenter PanelPresenter { get; }

        public IErrorPresenter ErrorPresenter { get; }

        public void PresentStartNewField(StartNewFieldViewModel viewModel)
        {
            PanelPresenter.ShowStartNewFieldDialog(viewModel);
        }

    }
}
