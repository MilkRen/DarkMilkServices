using System.Collections.Generic;
using LauncherDM.Models;

namespace LauncherDM.Services.Interfaces
{
    internal interface IXmlService
    {
        void SerializationUsersXml(UsersList userlist);

        UsersList DeserializeUsersXMl();
    }
}
