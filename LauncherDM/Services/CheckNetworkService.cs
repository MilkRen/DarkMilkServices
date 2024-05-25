using LauncherDM.Services.Interfaces;
using System.Net.Http;
using System.Net.NetworkInformation;

namespace LauncherDM.Services
{
    class CheckNetworkService : ICheckNetworkService
    {
        public bool CheckingNetworkConnection()
        {
            IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
            IResourcesHelperService resourcesHelper = new ResourcesHelperService();
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
                dialogMessageBox.DialogShow(resourcesHelper.LocalizationGet("Error"), resourcesHelper.LocalizationGet("EthernetClose"));
                return false;
            }
        }

        public bool CheckingUriFileConnection(string uriFile)
        {
            //using (var client = new HttpClient())
            //using (var response = client.GetAsync(uriFile))
            //    return response.Id == 192;


            try
            {
                using var ping = new Ping();
                {
                    PingReply reply= ping.Send(uriFile, 3000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
