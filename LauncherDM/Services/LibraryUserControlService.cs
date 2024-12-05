using LauncherDM.Services.Interfaces;
using ServerTCP;
using ServerTCP.Models;
using System.IO;
using System.Xml.Serialization;

namespace LauncherDM.Services
{
    class LibraryUserControlService : ILibraryUserControlService
    {
        private IServerRequestService _serverRequest;

        public LibraryUserControlService(ServerRequestService serverRequest)
        {
            _serverRequest = serverRequest;
        }

        public SaleGamesForXml GetGamesItem()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.GamesItemUser, true);
            var xml = new XmlSerializer(typeof(SaleGamesForXml));
            var programsForXml = new SaleGamesForXml();
            using (StringReader textReader = new StringReader(requestMessageServer.Message.ToString()))
            {
                try
                {
                    programsForXml = (SaleGamesForXml)xml.Deserialize(textReader);
                }
                catch
                { }
            }
            return programsForXml;
        }

        SaleProgramsForXml ILibraryUserControlService.GetProgramItem()
        {
            var requestMessageServer = _serverRequest.SendMessageRequest(MessageHeader.MessageType.ProgramsItemUser, true);

            var xml = new XmlSerializer(typeof(SaleProgramsForXml));
            var programsForXml = new SaleProgramsForXml();
            using (StringReader textReader = new StringReader(requestMessageServer.Message.ToString()))
            {
                try
                {
                    programsForXml = (SaleProgramsForXml)xml.Deserialize(textReader);
                }
                catch
                { }
            }
            return programsForXml;
        }


    }
}
