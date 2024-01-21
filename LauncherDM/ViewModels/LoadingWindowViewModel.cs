using LauncherDM.Infastructure.Commands;
using LauncherDM.Infastructure.Commands.Base;
using LauncherDM.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {

        #region Fields

        Window mainWindow = Application.Current.MainWindow;

        #region Models

        private ConnectivityCheckServerModel checkServer;

        ServerRequestModel serverRequest;

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
                mainWindow.DragMove();
        }

        #endregion

        #endregion

        #region Ctor

        public LoadingWindowViewModel()
        {
            MoveWindowCommand = new lambdaCommand(OnMoveWindowCommandExecuted, CanMoveWindowCommandExecute);
            checkServer = new ConnectivityCheckServerModel();
            serverRequest = new ServerRequestModel();
            if (checkServer.CheckingNetworkConnection())
            {
                DescInfoConnect = "Lfpsad";
                //Title = serverRequest.SendMessageRequest("Sral", MessageHeader.MessageType.Check);
            }
            else
                Environment.Exit(0);
        }

        #endregion
    }
}
