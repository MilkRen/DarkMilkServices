using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System.Windows;

namespace LauncherDM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IDialogWindowService windowService = new DialogWindowService();
            windowService.OpenWindow(new LoadingWindow(){DataContext = new LoadingWindowViewModel()});
        }
    }
}
