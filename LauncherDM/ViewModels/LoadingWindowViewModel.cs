using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
using System;
using System.Windows;
using System.Windows.Input;
using ServerTCP;
using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
using LauncherDM.Views.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {
        #region Fields

        private readonly Window _loadingWindow = Application.Current.MainWindow;

        #region Models

        private ConnectivityCheckNetworkModel _checkNetwork;

        private ServerRequestModel _serverRequest;

        #endregion

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
            LoadAlgo();
        }

        void LoadAlgo()
        {
            _checkNetwork = new ConnectivityCheckNetworkModel();
            _serverRequest = new ServerRequestModel();

            Task.Run(() =>
            { 
                if (_checkNetwork.CheckingNetworkConnection())
                {
                    var requestMessageServer = _serverRequest.SendMessageRequestT<string>(string.Empty,
                        MessageHeader<string>.MessageType.Check, string.Empty.Length);

                    if (!string.IsNullOrEmpty(requestMessageServer))
                    {
                        DescInfoConnect = "sd";

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

        #endregion
    }
}
