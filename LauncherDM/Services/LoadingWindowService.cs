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

            var requestMessageServer = serverRequest.SendMessageRequest(MessageHeader.MessageType.Check, 0);
            return requestMessageServer.Message == "1";
        }

        public void GetTitle()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(string.Empty,
                    MessageHeader.MessageType.Check, string.Empty.Length);
        }

        public void DescInfoConnect()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequest(string.Empty,
                MessageHeader.MessageType.Check, string.Empty.Length);
        }
    }
}
