using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.ViewModels;
using AgOpenGPS.WpfApp.Field;
using System;
using System.Windows;

namespace AgOpenGPS.WpfApp
{
    public class WpfPanelPresenter : IPanelPresenter
    {
        private StartNewFieldDialog _startNewFieldDialog;
        private SelectNearFieldDialog _selectNearFieldDialog;
        private CreateFromExistingFieldDialog _createFromExistingFieldDialog;
        private SelectFieldDialog _selectFieldDialog;

        public void ShowStartNewFieldDialog(StartNewFieldViewModel viewModel)
        {
            _startNewFieldDialog = new StartNewFieldDialog
            {
                DataContext = viewModel
            };
            _startNewFieldDialog.ShowDialog();
        }

        public void CloseStartNewFieldDialog()
        {
            _startNewFieldDialog?.Close();
        }

        void IPanelPresenter.ShowSelectNearFieldDialog(SelectNearFieldViewModel viewModel)
        {
            viewModel.UpdateFields();
            _selectNearFieldDialog = new SelectNearFieldDialog
            {
                DataContext = viewModel
            };
            _selectNearFieldDialog.ShowDialog();
        }

        void IPanelPresenter.CloseSelectNearFieldDialog()
        {
            _selectNearFieldDialog?.Close();
        }

        public void ShowCreateFromExistingFieldDialog(CreateFromExistingFieldViewModel viewModel)
        {
            viewModel.UpdateFields();
            _createFromExistingFieldDialog = new CreateFromExistingFieldDialog
            {
                DataContext = viewModel
            };
            _createFromExistingFieldDialog.ShowDialog();
        }

        public void CloseCreateFromExistingFieldDialog()
        {
            _createFromExistingFieldDialog?.Close();
        }

        void IPanelPresenter.ShowSelectFieldDialog(SelectFieldViewModel viewModel)
        {
            viewModel.UpdateFields();
            _selectFieldDialog = new SelectFieldDialog
            {
                DataContext = viewModel
            };
            _selectFieldDialog.ShowDialog();
        }

        void IPanelPresenter.CloseSelectFieldDialog()
        {
            _selectFieldDialog.Close();
        }
    }
}
