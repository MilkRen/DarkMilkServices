using System;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System.Windows;
using System.Threading;
using System.Reflection;

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
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                base.OnStartup(e);
                IDialogWindowService windowService = new DialogWindowService();
                windowService.OpenWindow(new LoadingWindow() { DataContext = new LoadingWindowViewModel() });
                mutex.ReleaseMutex();
            }
        }
    }
}
