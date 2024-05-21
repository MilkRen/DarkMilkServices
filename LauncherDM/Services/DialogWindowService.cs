using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System;
using System.Windows;
using LauncherDM.ViewModels.UserControlsVM;
using System.Net;
using System.ComponentModel;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {
        public Action CloseAction { get; set; }

        public Action HideAction { get; set; }

        public Action DragMoveAction { get; set; }

        public ResourcesHelperService ResourcesHelperService = new ResourcesHelperService();

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWindow();
                authorization.DataContext = new AuthorizationWindowViewModel(authorization.DragMove, authorization.Close,
                    new ToolbarToWindowViewModel(new WindowService(authorization), visibilitySettings: Visibility.Visible),
                    ResourcesHelperService);
                authorization.Owner = Application.Current.MainWindow;
                authorization.Show();
                return;
            }
            else if (viewModel is AccountUserControlViewModel or AuthorizationWindowViewModel)
            {
                var regAndLogWindow = new RegAndLogWindow();
                regAndLogWindow.DataContext = new RegAndLogWindowViewModel(regAndLogWindow.DragMove, regAndLogWindow.Close, 
                    new ToolbarToWindowViewModel(new WindowService(regAndLogWindow), regAndLogWindow.Close),
                    ResourcesHelperService);
                regAndLogWindow.ShowDialog();
                return;
            }
            else if (viewModel is ToolbarToWindowViewModel)
            {
                var settingsMini = new SettingsMiniWindow();
                settingsMini.DataContext = new SettingsMiniWindowViewModel(settingsMini.DragMove, 
                    new ToolbarToWindowViewModel(new WindowService(settingsMini), settingsMini.Close, ResourcesHelperService.LocalizationGet("Settings"),
                        visibilityMinBut:Visibility.Hidden), ResourcesHelperService);
                settingsMini.ShowDialog();
                return;
            }
            else if (viewModel is RegAndLogWindowViewModel)
            {
                //((ViewModel.Base.ViewModel)viewModel).Dispose();
                OpenMainWindow();
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
                new WindowService(loadingWindow), ResourcesHelperService);
            loadingWindow.Show();
        }

        public void OpenAccountRecovery()
        {
            var accountRecovery = new AccountRecoveryWindow();
            accountRecovery.DataContext = new AccountRecoveryWindowViewModel();
            accountRecovery.Show();
        }

        public void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel(mainWindow.DragMove,
                new ToolbarToWindowViewModel(new WindowService(mainWindow), mainWindow.Hide, widthMax: 30),
                ResourcesHelperService);
            //mainWindow.Owner = Application.Current.MainWindow;
            mainWindow.Show();
        }
    }
}
