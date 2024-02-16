using LauncherDM.Services.Interfaces;
using System;
using System.Windows;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System.Net;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWIndow();
                //authorization.DataContext = new AuthorizationWIndowViewModel(authorization.Close);
                authorization.DataContext = new AuthorizationWindowViewModel();
                authorization.Owner = Application.Current.MainWindow;
                authorization.Show();
                return;
            }
            else if (viewModel is AuthorizationWindowViewModel)
            {
                var mainWindow = new MainWindow(); 
                mainWindow.DataContext = new MainWindowViewModel();
                mainWindow.Owner = Application.Current.MainWindow;
                mainWindow.Show();
                return;
            }
        }

        public void CloseWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void ShowWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void HideWindow(Window window)
        { 
            window.Hide();
        }

        public void OpenLoadingWindow()
        {
            var loadWindow = new LoadingWindow();
            loadWindow.DataContext = new LoadingWindowViewModel();
            loadWindow.Show();
        }
    }
}
