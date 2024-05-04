using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LauncherDM.Properties;
using System.Diagnostics;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private readonly Window _loadingWindow = Application.Current.MainWindow;

        #endregion

        #region Services

        private readonly IResourcesHelperService _resourcesHelper;

        #endregion

        #region Bindings

        #region Заголовок окна

        private string _title = "DarkMilk";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region Описание

        private string _descInfoConnect;

        public string DescInfoConnect
        {
            get => _descInfoConnect;
            set => Set(ref _descInfoConnect, value);
        }

        #endregion

        #region MenuItem

        public string CloseApp => _resourcesHelper.LocalizationGet("CloseApp");


        #endregion

        #endregion

        #region Commmands

        #region MoveWindow

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        { 
            if (Mouse.LeftButton == MouseButtonState.Pressed) 
                _loadingWindow.DragMove();
        }

        #endregion

        #region CloseWindow

        public Command CloseWindowCommand { get; }
        private bool CanCloseWindowCommandExecute(object p) => true;
        private void OnCloseWindowCommandExecuted(object p)
        {
            Environment.Exit(0);
        }

        #endregion

        #endregion

        #region Ctor

        public LoadingWindowViewModel(ResourcesHelperService resourcesHelper)
        {
            _resourcesHelper = resourcesHelper;
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            CloseWindowCommand = new lambdaCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            Loading();
        }

        #endregion

        private void Loading()
        {
            ICheckNetworkService checkNetwork = new CheckNetworkService();
            ILoadingWindowService server = new LoadingWindowService();

            Task.Run(() =>
            {
                DescInfoConnect = _resourcesHelper.LocalizationGet("Сonnection");
                if (!checkNetwork.CheckingNetworkConnection())
                {
                    OpenWindow();
                    return;
                }

                var countMs = 0;
                while (true)
                {
                    if (server.CheckRequestServer())
                    {
                        DescInfoConnect = server.GetTitle();
                        server.CheckUpdate();
                        Thread.Sleep(5000);
                        OpenWindow();
                    }
                    else
                    {
                        if (countMs >= 10000)
                        {
                            IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                            dialogMessageBox.DialogShow(_resourcesHelper.LocalizationGet("Error"), _resourcesHelper.LocalizationGet("ServerClose"));
                            break;
                        }

                        countMs += 2000;
                        DescInfoConnect = string.Format(CultureInfo.InvariantCulture,
                            _resourcesHelper.LocalizationGet("Reconnection"), countMs);
                        Thread.Sleep(5000);
                    }
                }
                OpenWindow();
            });
        }

        private void OpenWindow()
        {
            _loadingWindow.Dispatcher.Invoke(() =>
            {
                IDialogWindowService windowService = new DialogWindowService();
                windowService.OpenWindow(this);
                _loadingWindow.Hide();
            });
        }
    }
}
