using System;
using LauncherDM.Services.Interfaces;
using ServerTCP;

namespace LauncherDM.Services
{
    class LoadingWindowService : ILoadingWindowService
    {
        private IServerRequestService serverRequest;

        public bool CheckRequestServer()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            //var requestMessageServer = serverRequest.SendMessageRequest(MessageHeader.MessageType.Check);
            //return requestMessageServer.Message == "1";

            var a = serverRequest.SendMessageRequest(MessageHeader.MessageType.Token);
            return false;
        }

        public string GetTitle()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(MessageHeader.MessageType.TitleLoading);
            return requestMessageServer.Message.ToString();
        }

        public string DescInfoConnect()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(string.Empty,
                MessageHeader.MessageType.Check, string.Empty.Length);
            return requestMessageServer.Message.ToString();
        }
    }
}
