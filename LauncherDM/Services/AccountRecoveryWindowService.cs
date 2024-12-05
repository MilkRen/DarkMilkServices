using LauncherDM.Services.Interfaces;
using ServerTCP;

namespace LauncherDM.Services
{
    class AccountRecoveryWindowService : IAccountRecoveryWindowService
    {
        private IServerRequestService _serverRequest;

        public AccountRecoveryWindowService()
        {
            _serverRequest = new ServerRequestService();
        }

        public bool SendMessage(string data)
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(data, MessageHeader.MessageType.RecoveryAccount);
            return requestMessageServer.Message.ToString() == "1";
        }
    }
}
