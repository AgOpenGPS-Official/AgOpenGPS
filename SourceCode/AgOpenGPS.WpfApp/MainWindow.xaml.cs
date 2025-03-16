using AgOpenGPS.Core;
using System.IO;
using System;
using System.Windows;

namespace AgOpenGPS.WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // TODO move RegistrySettings to Core and use correct basePath
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            DirectoryInfo baseDirectory = new DirectoryInfo(basePath);
            WpfPanelPresenter wpfPanelPresenter = new WpfPanelPresenter();
            WpfErrorPresenter wpfErrorPresenter = new WpfErrorPresenter();

            ApplicationCore applicationCore = new ApplicationCore(
                baseDirectory,
                wpfPanelPresenter,
                wpfErrorPresenter);

            DataContext = applicationCore.AppViewModel;
        }
    }
}
