using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using LauncherDM.Properties;
using ServerTCP;

namespace LauncherDM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex mutex = new Mutex(true, "fjoasdkjff8139darkmilk");
        [STAThread] //  Это означает, что все потоки в этой программе выполняются в рамках одного процесса, а управление программой осуществляется одним главным потоком
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                base.OnStartup(e);

                if (SettingsApp.Default.Language == "ru")
                    MessageLanguages.Language = MessageLanguages.Languages.rus;
                else if (SettingsApp.Default.Language == "en")
                    MessageLanguages.Language = MessageLanguages.Languages.eng;
                else
                    MessageLanguages.Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "ru" ? MessageLanguages.Languages.rus : MessageLanguages.Languages.eng;
                
                IDialogWindowService windowService = new DialogWindowService();
                windowService.OpenLoadingWindow();
                mutex.ReleaseMutex();
            }
            else
            {
                IResourcesHelperService resourcesHelper = new ResourcesHelperService();
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow(resourcesHelper.LocalizationGet("Attention"), resourcesHelper.LocalizationGet("AppMutexOn"));
                Environment.Exit(0);
            }
        }
    }
}
