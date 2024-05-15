using System.Reflection;
using LauncherDM.Models;
using LauncherDM.Services.Interfaces;
using ServerTCP;

namespace LauncherDM.Services
{
    class LoadingWindowService : ILoadingWindowService
    {
        private IServerRequestService _serverRequest;

        public LoadingWindowService(ServerRequestService serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public bool CheckRequestServer()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.Check);
            return requestMessageServer?.Message.ToString() == "1";
        }

        public string GetTitle()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.TitleLoading);
            return requestMessageServer?.Message.ToString();
        }

        public bool CheckUpdate()
        {
            var version = StaticFields.VersionPatch;
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.Version);
            return requestMessageServer?.Message.ToString() == version;
        }
    }
}
