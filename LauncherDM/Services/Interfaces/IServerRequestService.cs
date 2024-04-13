﻿using ServerTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherDM.Services.Interfaces
{
    interface IServerRequestService
    {
        public string SendMessageRequestT<T>(T data, MessageHeader<T>.MessageType messageType, int length);
    }
}