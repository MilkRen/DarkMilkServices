using LauncherDM.Services.Interfaces;
using LauncherDM.ViewModel;
using LauncherDM.ViewModels;
using LauncherDM.Views.Windows;
using System;
using System.Windows;
using LauncherDM.ViewModels.UserControlsVM;
using System.Net;
using System.ComponentModel;
using LauncherDM.Infrastructure.ReactiveUI;
using LauncherDM.Infrastructure;

namespace LauncherDM.Services
{
    class DialogWindowService : IDialogWindowService
    {

        #region Service

        public ResourcesHelperService ResourcesHelper = new ResourcesHelperService();

        public ServerRequestService ServerRequest = new ServerRequestService();

        #endregion

        public Action CloseAction { get; set; }

        public Action HideAction { get; set; }

        public Action DragMoveAction { get; set; }

        public void OpenWindow(object viewModel)
        {
            if (viewModel is LoadingWindowViewModel)
            {
                var authorization = new AuthorizationWindow();
                var authorizationVM = new AuthorizationWindowViewModel(authorization.DragMove, authorization.Close,
                    new ToolbarToWindowViewModel(new WindowService(authorization), visibilitySettings: Visibility.Visible),
                    ResourcesHelper);
                UpdateUI.LanguagesPull.Subscribe(authorizationVM);
                authorization.DataContext = authorizationVM;
                authorization.Owner = Application.Current.MainWindow;
                authorization.Show();
            }
            else if (viewModel is AccountUserControlViewModel or AuthorizationWindowViewModel)
            {
                var regAndLogWindow = new RegAndLogWindow();
                regAndLogWindow.DataContext = new RegAndLogWindowViewModel(regAndLogWindow.DragMove, regAndLogWindow.Close, 
                    new ToolbarToWindowViewModel(new WindowService(regAndLogWindow), regAndLogWindow.Close),
                    ResourcesHelper);
                regAndLogWindow.ShowDialog();
            }
            else if (viewModel is ToolbarToWindowViewModel)
            {
                var settingsMini = new SettingsMiniWindow();
                var toolbarVM = new ToolbarToWindowViewModel(new WindowService(settingsMini), settingsMini.Close,
                    "Settings",
                    visibilityMinBut: Visibility.Hidden, resourcesHelper: ResourcesHelper);
                UpdateUI.LanguagesPull.Subscribe(toolbarVM);
                settingsMini.DataContext = new SettingsMiniWindowViewModel(settingsMini.DragMove, toolbarVM, ResourcesHelper);
                settingsMini.ShowDialog();
            }
            else if (viewModel is RegAndLogWindowViewModel)
            {
                //((ViewModel.Base.ViewModel)viewModel).Dispose();
                OpenMainWindow();
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
                new WindowService(loadingWindow), ResourcesHelper, ServerRequest);
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
                ResourcesHelper, ServerRequest);
            mainWindow.Owner = Application.Current.MainWindow;
            mainWindow.Show();
        }
    }
}
