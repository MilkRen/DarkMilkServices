using LauncherDM.Models;
using LauncherDM.Services.Interfaces;
using ServerTCP;
using ServerTCP.Models;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LauncherDM.Services
{
    class StoreUserControlService : IStoreUserControlService
    {
        private IServerRequestService _serverRequest;
        
        public StoreUserControlService(ServerRequestService serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public ProgramsForXml GetPrograms()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.Programs);

            var xml = new XmlSerializer(typeof(ProgramsForXml));
            var programsForXml = new ProgramsForXml();
            using (StringReader textReader = new StringReader(requestMessageServer.Message.ToString()))
            {
                try
                {
                    programsForXml = (ProgramsForXml)xml.Deserialize(textReader);
                }
                catch
                {
                }
            }

            return programsForXml;
            //return requestMessageServer?.Message.ToString() == "1";
        }

        public string GetProgramsPath()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.ProgramsPath);
            return requestMessageServer.Message.ToString();
        }

        public object GetGames()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.Games);
            return null;
        }

        public string GetGamesPath()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.GamesPath);
            return null;
        }
    }
}
