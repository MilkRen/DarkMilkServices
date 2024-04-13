using System;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System.Windows;
using System.Threading;
using System.Reflection;
using LauncherDM.ViewModel;

namespace LauncherDM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name);
        [STAThread] //  Это означает, что все потоки в этой программе выполняются в рамках одного процесса, а управление программой осуществляется одним главным потоком
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //new MainWindow(){DataContext = new MainWindowViewModel()}.Show();
            //var authorization = new AuthorizationWIndow();
            //authorization.DataContext = new AuthorizationWindowViewModel(authorization.Close);

            //authorization.Show();


            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                base.OnStartup(e);
                IDialogWindowService windowService = new DialogWindowService();
                windowService.OpenLoadingWindow();
                mutex.ReleaseMutex();
            }
        }
    }
}
