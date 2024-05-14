using LauncherDM.Services.Interfaces;
using ServerTCP;
using ServerTCP.Сryptographies;

namespace LauncherDM.Services
{
    internal class AuthorizationService : IAuthorizationService
    {
        private IServerRequestService _serverRequest;
        public bool Authorization(string login, string password)
        {
            //var requestPublicKey = _serverRequest.SendMessageRequest(MessageHeader.MessageType.PublicKey);
            //var publicKey = requestPublicKey.Message.ToString();

            var data = string.Concat(login, ",", password);
            //var cryptMessage = CryptoRsa.Encrypt(publicKey, data);
            //var requestToken =  _serverRequest.SendMessageRequest(cryptMessage, MessageHeader.MessageType.Login);

            var requestToken = _serverRequest.SendMessageRequest(data, MessageHeader.MessageType.Login);
            return false;
        }

        public AuthorizationService()
        {
            _serverRequest = new ServerRequestService();
        }
    }
}
