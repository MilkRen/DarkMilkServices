using LauncherDM.Services.Interfaces;
using ServerTCP;

namespace LauncherDM.Services
{
    class LibraryUserControlService : ILibraryUserControlService
    {
        private IServerRequestService _serverRequest;

        public LibraryUserControlService(ServerRequestService serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public string GetAllItems()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.AllGamesOrPrograms, true);
            return requestMessageServer.Message.ToString();
        }
    }
}
