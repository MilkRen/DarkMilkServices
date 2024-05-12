using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System;
using System.Windows;
using LauncherDM.ViewModels.UserControlsVM;
using System.Net;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public Action CloseAction { get; set; }

        public Action HideAction { get; set; }

        public Action DragMoveAction { get; set; }

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWindow();
                authorization.DataContext = new AuthorizationWindowViewModel(authorization.DragMove ,authorization.Close,
                    new ToolbarToWindowViewModel(new WindowService(authorization), visibilitySettings: Visibility.Visible)); 
                authorization.Show();
                return;
            }
            else if (viewModel is AuthorizationWindowViewModel)
            {
                var regAndLogWindow = new RegAndLogWindow();
                regAndLogWindow.DataContext = new RegAndLogWindowViewModel(new ToolbarToWindowViewModel(new WindowService(regAndLogWindow)), 
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

        public void DragMoveWindow()
        {
            DragMoveAction();
        }

        public void OpenLoadingWindow()
        {
            var loadingWindow = new LoadingWindow();
            loadingWindow.DataContext = new LoadingWindowViewModel(loadingWindow.DragMove, loadingWindow.Hide,
                new WindowService(loadingWindow), new ResourcesHelperService());
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
