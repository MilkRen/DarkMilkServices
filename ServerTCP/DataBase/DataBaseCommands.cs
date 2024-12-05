using ServerTCP.Models;
using ServerTCP.Models.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                    case MessageHeader.MessageType.Sale:
                        var data = table.ToString().Split(",");

                        var games = db.games.Where(x => x.name == data[0]).ToArray();
                        var programs  = db.programs.Where(x => x.name == data[0]).ToArray();
                        var users  = db.users.Where(x => x.login == data[1]).ToArray();

                        SalePrograms salePrograms = null;
                        SaleGames saleGames = null;

                        if (games.Length > 0)
                        {
                            saleGames = new SaleGames()
                            {
                                id = 1,
                                game_id = games[0].id,
                                user_id = users[0].id
                            };
                            var checkData = db.sale_games.Where(x => x.id == saleGames.id 
                                                                     && x.game_id == saleGames.game_id 
                                                                     && x.user_id == saleGames.user_id).ToArray();
                            if (checkData.Length > 0)
                                return false;

                            if (db.sale_games.Count() != 0)
                            {
                                var idMax = db.sale_games?.Max(x => x.id) ?? 0;
                                if (idMax != 0)
                                    saleGames.id = idMax + 1;
                            }

                            checkData = db.sale_games.Where(x => x.id == saleGames.id
                                                                     && x.game_id == saleGames.game_id
                                                                     && x.user_id == saleGames.user_id).ToArray();
                            if (checkData.Length > 0)
                                return false;

                            db.sale_games.Add(saleGames);
                        }
                        else
                        {
                            salePrograms = new SalePrograms()
                            {
                                id = 1,
                                program_id = programs[0].id,
                                user_id = users[0].id
                            };
                            var checkData = db.sale_programs.Where(x => x.id == salePrograms.id
                                                                     && x.program_id == salePrograms.program_id
                                                                     && x.user_id == salePrograms.user_id).ToArray();
                            if (checkData.Length > 0)
                                return false;

                            if (db.sale_programs.Count() != 0)
                            {
                                var idMax = db.sale_programs?.Max(x => x.id) ?? 0;
                                if (idMax != 0)
                                    salePrograms.id = idMax + 1;
                            }

                            checkData = db.sale_programs.Where(x => x.id == salePrograms.id
                                                                        && x.program_id == salePrograms.program_id
                                                                        && x.user_id == salePrograms.user_id).ToArray();
                            if (checkData.Length > 0)
                                return false;

                            db.sale_programs.Add(salePrograms);
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
                        progArray.ProgramsArray = prog.ToArray();
                        return progArray;
                        break;
                    case MessageHeader.MessageType.Games:
                        var games = db.games;
                        var gamesArray = new GamesForXml();
                        gamesArray.GamesArray = games.ToArray();
                        return gamesArray;
                        break;
                    case MessageHeader.MessageType.Registration:
                        var userCheck = db.users.Where(x => x.username == data[0]).ToArray();
                        result = userCheck.Length > 0;
                        break;
                    case MessageHeader.MessageType.GamesItemUser:
                        var dataMessageForGame = data[0].ToString().Split(",");
                        var getUserForGame = db.users.Where(x => x.login == dataMessageForGame[1]).ToArray();
                        var saleGames = db.sale_games.Where(x => x.user_id == getUserForGame[0].id);

                        var saleGameXml = new SaleGamesForXml();
                        saleGameXml.SaleGamesArray = saleGames.ToArray();
                        return saleGameXml;
                        break;
                    case MessageHeader.MessageType.ProgramsItemUser:
                        var dataMessageForProg = data[0].ToString().Split(",");
                        var getUserForProg = db.users.Where(x => x.login == dataMessageForProg[1]).ToArray();
                        var salePrograms = db.sale_programs.Where(x => x.user_id == getUserForProg[0].id).ToArray();

                        var saleProgXml = new SaleProgramsForXml();
                        saleProgXml.SaleProgramsArray = salePrograms.ToArray();
                        return saleProgXml;
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
