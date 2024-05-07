using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System;
using System.Windows;
using LauncherDM.ViewModels.UserControlsVM;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public Action CloseAction { get; set; }

        public Action HideAction { get; set; }

        public Action MaxWindowAction { get; set; }

        public Action MinWindowAction { get; set; }

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWindow();
                Action minAction = () => authorization.WindowState = WindowState.Minimized;
                authorization.DataContext = new AuthorizationWindowViewModel(new ToolbarToWindowViewModel(minAction, minAction),
                    authorization.Close); 
                authorization.Show();
                return;
            }
            else if (viewModel is AuthorizationWindowViewModel)
            {
                var regAndLogWindow = new RegAndLogWindow();
                Action minAction = () => regAndLogWindow.WindowState = WindowState.Minimized; 
                regAndLogWindow.DataContext = new RegAndLogWindowViewModel(new ToolbarToWindowViewModel(minAction, minAction), 
                    new ResourcesHelperService());
                regAndLogWindow.Show();
                return;
            }
            else if (viewModel is RegAndLogWindowViewModel)
            {
                var mainWindow = new MainWindow(); 
                mainWindow.DataContext = new MainWindowViewModel();
                mainWindow.Owner = Application.Current.MainWindow;
                mainWindow.Show();
                return;
            }
        }

        public void CloseWindow()
        {
            CloseAction();
        }

        public void HideWindow()
        {
            HideAction();
        }

        public void MaxWindow()
        {
            MaxWindowAction();
        }

        public void MinWindow()
        {
            MinWindowAction();
        }

        public void OpenLoadingWindow()
        {
            var loadingWindow = new LoadingWindow();
            loadingWindow.DataContext = new LoadingWindowViewModel(loadingWindow.Hide, new ResourcesHelperService());
            loadingWindow.Show();
        }

        public void OpenAccountRecovery()
        {
            var accountRecovery = new AccountRecoveryWindow();
            accountRecovery.DataContext = new AccountRecoveryWindowViewModel();
            accountRecovery.Show();
        }
    }
}
