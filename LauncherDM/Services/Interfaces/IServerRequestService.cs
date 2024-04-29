using ServerTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherDM.Services.Interfaces
{
    interface IServerRequestService
    {
        public MessageHeader SendMessageRequest(string data, MessageHeader.MessageType messageType, int length);

        public MessageHeader SendMessageRequest(MessageHeader.MessageType messageType);
    }
}
