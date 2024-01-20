using System;
using System.Windows;
using LauncherDM.Models;
using ServerTCP;

namespace LauncherDM.ViewModels
{
    class LoadingWindowViewModel : ViewModel.Base.ViewModel
    {

        #region Модели

        private ConnectivityCheckServerModel checkServer;

        ServerRequestModel serverRequest;

        #endregion

        #region Заголовок окна
        private string _title = "DarkMilk";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        #endregion


        public LoadingWindowViewModel()
        {
            checkServer = new ConnectivityCheckServerModel();
            serverRequest = new ServerRequestModel();
            if (checkServer.CheckingNetworkConnection())
            {
                Title = serverRequest.sendRequest("Sral", MessageHeader.MessageType.Check);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
