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
        public Action CloseAction { get; set; }

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWIndow();
                authorization.DataContext = new AuthorizationWindowViewModel(authorization.Close); 
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

        public void CloseWindow()
        {
            CloseAction();
        }

        public void ShowWindow(Window window)
        {
            throw new NotImplementedException();
        }

        public void HideWindow(Window window)
        { 
            window.Hide();
        }

        public void OpenLoginWindow()
        {
            var loadWindow = new LoginWindow();
            //loadWindow.DataContext = new LoadingWindowViewModel();
            loadWindow.Show();
        }

        public void OpenRegistrationWindow()
        {
            var registrationWindow = new RegistrationWindow();
            //loadWindow.DataContext = new LoadingWindowViewModel();
            registrationWindow.Show();
        }
    }
}
