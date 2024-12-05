using LauncherDM.Properties;
using LauncherDM.Services.Interfaces;
using ServerTCP;
using System.Text;
using System;
using System.Security.Cryptography;

namespace LauncherDM.Services
{
    internal class SignUpService : ISignUpService
    {
        private IServerRequestService _serverRequest;
        public bool SignUp(string login, string email, string password)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException(login);

            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException(email);

            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(password);

            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var convertHash = Convert.ToHexString(hash);
            var data = string.Concat(login, ",", convertHash, ",", email);
            var requestToken = _serverRequest.SendMessageRequest(data, MessageHeader.MessageType.Registration);

            if (requestToken?.Message.ToString() == "0")
                return false;

            SettingsApp.Default.Token = requestToken?.Message.ToString();
            SettingsApp.Default.Save();
            return true;
        }
        
        public SignUpService()
        {
            _serverRequest = new ServerRequestService();
        }
    }
}
