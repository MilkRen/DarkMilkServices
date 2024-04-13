using System.Net.NetworkInformation;
using LauncherDM.Services;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.Models
{
    public class ConnectivityCheckNetworkModel
    {
        public bool CheckingNetworkConnection()
        {
            IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
            try
            {
                using var ping = new Ping();
                {
                    PingReply replyGoogle = ping.Send("www.google.com", 3000); // Пинг к google.com с ожиданием 3 секунды
                    PingReply replyYandex = ping.Send("www.yandex.ru", 3000); // Пинг к yandex.ru с ожиданием 3 секунды
                    return replyGoogle.Status == IPStatus.Success || replyYandex.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                dialogMessageBox.DialogShow("Ethernet error", "Ethernet error");
                return false;
            }
        }
    }
}
