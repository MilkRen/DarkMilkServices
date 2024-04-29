using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private readonly Window _loadingWindow = Application.Current.MainWindow;

        #endregion

        #region Bindings

        #region Заголовок окна

        private string _title = "DarkMilk";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion

        #region Описание

        private string _descInfoConnect;

        /// <summary>Заголовок окна</summary>
        public string DescInfoConnect
        {
            get => _descInfoConnect;
            set => Set(ref _descInfoConnect, value);
        }

        #endregion

        #endregion

        #region Commmands

        #region MoveWindowCommand

        public Command MoveWindowCommand { get; }
        private bool CanMoveWindowCommandExecute(object p) => true;
        private void OnMoveWindowCommandExecuted(object p)
        { 
            if (Mouse.LeftButton == MouseButtonState.Pressed) 
                _loadingWindow.DragMove();
        }

        #endregion

        #endregion

        #region Ctor

        public LoadingWindowViewModel()
        {
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            Load();
        }

        #endregion

        private void Load()
        {
            ICheckNetworkService checkNetwork = new CheckNetworkService();
            ILoadingWindowService Server = new LoadingWindowService();

            Task.Run(() =>
            { 
                if (checkNetwork.CheckingNetworkConnection())
                {
                    if (Server.CheckRequestServer())
                    {
                        DescInfoConnect = Server.GetTitle();

                        Thread.Sleep(5000);
                        _loadingWindow.Dispatcher.Invoke(() =>
                        {
                            IDialogWindowService windowService = new DialogWindowService();
                            windowService.OpenWindow(this);
                            _loadingWindow.Hide();
                        });
                    }
                    else
                    {
                        IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                        dialogMessageBox.DialogShow("s","s");
                    }
                }
                else
                    _loadingWindow.Dispatcher.Invoke(() =>
                    {
                        Environment.Exit(0);
                    });
            });
        }
    }
}
