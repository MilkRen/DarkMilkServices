using LauncherDM.Services.Interfaces;
using System;
using System.Windows;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWIndow(){};
                //authorization.DataContext = new AuthorizationWIndowViewModel(authorization.Close);
                authorization.DataContext = new AuthorizationWindowViewModel();
                authorization.Show();
                return;
            }
            if(viewModel is AuthorizationWindowViewModel)
                return;
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
