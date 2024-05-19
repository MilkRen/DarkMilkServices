using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using LauncherDM.Models;
using LauncherDM.Services.Interfaces;

namespace LauncherDM.Services
{
    internal class XmlService : IXmlService
    {
        private const string FileName = "Users.xml";

        public void SerializationUsersXml(UsersList userlist)
        {
            var xml = new XmlSerializer(typeof(UsersList));
            using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, userlist);
            }
        }

        public UsersList DeserializeUsersXMl()
        {
            var userslist = new UsersList();
            var xml = new XmlSerializer(typeof(UsersList));
            using (var fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                try
                {
                    userslist = (UsersList)xml.Deserialize(fs);
                }
                catch 
                {
                }
            }
            return userslist;
        }
    }
}
