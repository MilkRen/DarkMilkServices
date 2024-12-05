using System;
using System.Security.Cryptography;
using System.Text;
using LauncherDM.Properties;
using LauncherDM.Services.Interfaces;
using ServerTCP;

namespace LauncherDM.Services
{
    internal class AuthorizationService : IAuthorizationService
    {
        private IServerRequestService _serverRequest;
        public bool Authorization(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException(login);

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);


            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var convertHash = Convert.ToHexString(hash);
            //var requestPublicKey = _serverRequest.SendMessageRequest(MessageHeader.MessageType.PublicKey);
            //var publicKey = requestPublicKey.Message.ToString();

            var data = string.Concat(login, ",", convertHash);
            //var cryptMessage = CryptoRsa.Encrypt(publicKey, data);
            //var requestToken =  _serverRequest.SendMessageRequest(cryptMessage, MessageHeader.MessageType.Login);
            var requestToken = _serverRequest.SendMessageRequest(data, MessageHeader.MessageType.Login);

            if (requestToken?.Message.ToString() == "0")
                return false;

            SettingsApp.Default.Token = requestToken?.Message.ToString();
            SettingsApp.Default.Save();
            return true;
        }

        public AuthorizationService()
        {
            _serverRequest = new ServerRequestService();
        }
    }
}
