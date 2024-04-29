using System.Threading.Channels;
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
                if (db.Database.CanConnect())
                    ConsoleExtension.WriteLineColor("База данных доступна.", ConsoleColor.DarkGreen);
                else
                    ConsoleExtension.WriteLineColor("База данных не доступна!", ConsoleColor.DarkRed);
                
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

    }
}
