using ServerTCP;
using static ServerTCP.MessageLanguages;

namespace LauncherDM.Services.Interfaces
{
    interface IServerRequestService
    {
        public MessageHeader SendMessageRequest(object data, MessageHeader.MessageType messageType, bool loadToken = false);

        public MessageHeader SendMessageRequest(MessageHeader.MessageType messageType, bool loadToken = false);
    }
}
