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

            var requestMessageServer = serverRequest.SendMessageRequest(MessageHeader.MessageType.Check);
            return requestMessageServer.Message == "1";
        }

        public string GetTitle()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(MessageHeader.MessageType.TitleLoading);
            return requestMessageServer.Message;
        }

        public string DescInfoConnect()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(string.Empty,
                MessageHeader.MessageType.Check, string.Empty.Length);
            return requestMessageServer.Message;
        }
    }
}
