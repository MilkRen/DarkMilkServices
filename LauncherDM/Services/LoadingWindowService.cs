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

            var requestMessageServer = serverRequest.SendMessageRequestT<string>(string.Empty,
                MessageHeader<string>.MessageType.Check, string.Empty.Length);

            return !string.IsNullOrEmpty(requestMessageServer);
        }

        public void GetTitle()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequestT<string>(string.Empty,
                    MessageHeader<string>.MessageType.Check, string.Empty.Length);
        }

        public void DescInfoConnect()
        {
            if (serverRequest is null)
                serverRequest = new ServerRequestService();

            var requestMessageServer = serverRequest.SendMessageRequestT<string>(string.Empty,
                MessageHeader<string>.MessageType.Check, string.Empty.Length);
        }
    }
}
