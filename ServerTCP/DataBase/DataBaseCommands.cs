﻿using System.Threading.Channels;
using ServerTCP.Models;
using ServerTCP.Models.Data;

namespace ServerTCP.DataBase
{
    internal class DataBaseCommands
    {
        public static void Insert(object table, MessageHeader.MessageType messageType)
        {
            using (var db = new ApplicationContext())
            {
                CheckConnect(db);

                switch (messageType)
                {
                    case MessageHeader.MessageType.Registration:
                        if (table is User user)
                            db.users.Add(user);
                        break;
                }
                db.SaveChanges();
            }
        }

        public static object Select(MessageHeader.MessageType messageType)
        {
            using (var db = new ApplicationContext())
            {
                CheckConnect(db);
                object result = null;
                switch (messageType)
                {
                    case MessageHeader.MessageType.Version:
                        var appWPF = db.apps.Where(x => x.parametername == "wpfDMVersion").ToArray();
                        result = appWPF[0].parametervalue;
                        break;
                    default: 
                        result = null;                     
                        break;
                }

                if (result is null)
                    throw new ArgumentNullException(nameof(result));

                return result;
            }
        }

        private static void CheckConnect(ApplicationContext db)
        {
            if (db.Database.CanConnect())
                ConsoleExtension.WriteLineColor("База данных доступна.", ConsoleColor.DarkGreen);
            else
                ConsoleExtension.WriteLineColor("База данных не доступна!", ConsoleColor.DarkRed);
        }

    }
}
