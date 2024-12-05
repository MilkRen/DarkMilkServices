using System;
using System.Collections.Generic;
using LauncherDM.Models;

namespace LauncherDM.Services.Interfaces
{
    internal interface IXmlService
    {
        void SerializationUsersXml(UsersForXml userlist);

        UsersForXml DeserializeUsersXMl();

        void DeleteFileUsers();
    }
}
