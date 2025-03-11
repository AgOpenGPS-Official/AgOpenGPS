using AgOpenGPS.Core.ViewModels;

namespace AgOpenGPS.Core.Interfaces
{
    public interface IPanelPresenter
    {
        void ShowStartNewFieldDialog(StartNewFieldViewModel viewModel);
        void CloseStartNewFieldDialog();
        void ShowSelectNearFieldDialog(SelectNearFieldViewModel viewModel);
        void CloseSelectNearFieldDialog();
        void ShowSelectFieldDialog(SelectFieldViewModel viewModel);
        void CloseSelectFieldDialog();
        void ShowCreateFromExistingFieldDialog(CreateFromExistingFieldViewModel viewModel);
        void CloseCreateFromExistingFieldDialog();
    }
}
