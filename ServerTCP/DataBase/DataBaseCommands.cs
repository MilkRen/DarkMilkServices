using ServerTCP.Models;
using ServerTCP.Models.Data;

namespace ServerTCP.DataBase
{
    internal class DataBaseCommands
    {
        public static bool Insert(object table, MessageHeader.MessageType messageType)
        {
            using (var db = new ApplicationContext())
            {
                CheckConnect(db);

                switch (messageType)
                {
                    case MessageHeader.MessageType.Registration:
                        if (table is User user)
                        {
                            if (db.users.Count() != 0)
                            {
                                //Todo: не уверен, что это правильно!
                                var idMax = db.users?.Max(x => x.id) ?? 0;
                                if (idMax != 0)
                                    user.id = idMax + 1;
                            }
                            db.users.Add(user);
                        }
                        break;
                    case MessageHeader.MessageType.RecoveryAccount:
                        if (table is RecoveryAccount account)
                        {
                            //Todo: не уверен, что это правильно!
                            if (db.recovery_account.Count() != 0)
                            {
                                var idMax = db.recovery_account?.Max(x => x.id) ?? 0;
                                if (idMax != 0)
                                    account.id = idMax + 1;
                            }
                            db.recovery_account.Add(account);
                        }
                        break;
                } 
               return db.SaveChanges() == 1;
            }
        }

        public static object Select(MessageHeader.MessageType messageType, params string[] data )
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
                    case MessageHeader.MessageType.Login:
                        var user = db.users.Where(x => x.username == data[0] && x.password == data[1]).ToArray();
                        result = user.Length > 0;
                        break;
                    case MessageHeader.MessageType.Programs:
                        var prog = db.programs;
                        var progArray = new ProgramsForXml();
                        progArray.Programs = prog.ToArray();
                        return progArray;
                        break;
                    case MessageHeader.MessageType.Registration:
                        var userCheck = db.users.Where(x => x.username == data[0]).ToArray();
                        result = userCheck.Length > 0;
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
