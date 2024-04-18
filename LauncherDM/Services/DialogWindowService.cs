using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System;
using System.Windows;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public Action CloseAction { get; set; }

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWindow();
                authorization.DataContext = new AuthorizationWindowViewModel(authorization.Close); 
                authorization.Owner = Application.Current.MainWindow;
                authorization.Show();
                return;
            }
            else if (viewModel is AuthorizationWindowViewModel)
            {
                var regAndLogWindow = new RegAndLogWindow();
                regAndLogWindow.DataContext = new RegAndLogWindowViewModel();
                regAndLogWindow.Owner = Application.Current.MainWindow;
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

        public void ShowWindow(Window window)
        { 
            window.Show();
        }

        public void HideWindow(Window window)
        { 
            window.Hide();
        }

        public void OpenLoadingWindow()
        {
            var loadingWindow = new LoadingWindow();
            loadingWindow.DataContext = new LoadingWindowViewModel();
            loadingWindow.Show();
        }
    }
}
