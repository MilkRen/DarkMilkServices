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

        public void SerializationUsersXml(UsersForXml userlist)
        {
            var xml = new XmlSerializer(typeof(UsersForXml));
            using (var fs = new FileStream(FileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, userlist);
            }
        }

        public UsersForXml DeserializeUsersXMl()
        {
            var userslist = new UsersForXml();
            var xml = new XmlSerializer(typeof(UsersForXml));
            using (var fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                try
                {
                    userslist = (UsersForXml)xml.Deserialize(fs);
                }
                catch 
                {
                }
            }
            return userslist;
        }
    }
}
