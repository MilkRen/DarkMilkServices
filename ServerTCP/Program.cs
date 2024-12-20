﻿using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using ServerTCP.DataBase;
using ServerTCP.Models;

namespace ServerTCP
{
    internal class Program
    {
        private static readonly string ip = DataInfo.Ip;
        private static readonly int port = DataInfo.Port;

        private static string _publicKey;
        private static string _privateKey;

        private static Random rand = new Random();
        private static string SaltPassword = "sdlfjkpo213ndarkmilk";

        private const string ProgramsPathConst = @"https://darkmilk.store/Launcher/ProgramImage/";

        private const string GamesPathConst = @"https://darkmilk.store/Launcher/GamesImage/";
        static void Main(string[] args)
        {
            //LoadingRSA();
            var ipAddress = IPAddress.Parse(ip);
            var endPoint = new IPEndPoint(ipAddress, port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(endPoint); // Связываем подлкючение к сокету, чтобы его прослушивать 
            tcpSocket.Listen(100); // количество подключеных в очередь

            // чтобы сделать многопоточное подключение, нужен новый сокет и т д 

            while (true)
            {
                Console.WriteLine("Ожидаем соединение через порт {0}", endPoint);
                var listener = tcpSocket.Accept(); // новый с   окет для нового клиента
                try
                {
                    ConsoleExtension.WriteLineColor("Подключился!", ConsoleColor.DarkYellow);
                    var buffer = new byte[listener.ReceiveBufferSize]; // размер буфера .  максимум сообщение из 256 байт 
                    var size = 0; // количество реально полученных количества байт, потом чтобы оптимизировать память 
                    do
                    {
                        size = listener.Receive(buffer); // получение данных, количество, значение
                        var header = MessageHeader.ServerFromArray(buffer.Take(size).ToArray());

                        MessageHeader headerRequest = null;
                        byte[] headerRequestBytes;
                        var message = string.Empty;
                        var loadToken = false;
                        bool result;
                        Console.WriteLine($"Пришёл запрос: {header.Type}");
                        switch (header.Type)
                        {
                            case MessageHeader.MessageType.Check:
                                headerRequest = new MessageHeader("1", header.Type);
                                break;

                            case MessageHeader.MessageType.Login:
                                var userInfo = header.Message.ToString().Split(',');
                                var hashLogin = SHA256.HashData(Encoding.UTF8.GetBytes(userInfo[1] + SaltPassword));
                                var convertHashLogin = Convert.ToHexString(hashLogin);

                                var login = DataBaseCommands.Select(header.Type, userInfo[0], convertHashLogin);

                                if ((bool)login)
                                {
                                    headerRequest = new MessageHeader(
                                        Token.GenerateToken(userInfo[0], convertHashLogin),
                                        header.Type);
                                }
                                else
                                    headerRequest = new MessageHeader("0", header.Type);

                                break;

                            case MessageHeader.MessageType.Registration:
                                var newuserInfo = header.Message.ToString().Split(',');

                                if (newuserInfo.Length < 3)
                                    throw new ArgumentException(newuserInfo.Length.ToString());

                                var hash = SHA256.HashData(Encoding.UTF8.GetBytes(newuserInfo[1] + SaltPassword));
                                var convertHash = Convert.ToHexString(hash);
                                var user = new User
                                {
                                    login = newuserInfo[0], email = newuserInfo[2], username = newuserInfo[0],
                                    password = convertHash
                                };

                                if ((bool)DataBaseCommands.Select(header.Type, user.login))
                                    result = false;
                                else
                                 result = DataBaseCommands.Insert(user, header.Type);

                                headerRequest = result
                                    ? new MessageHeader("1", header.Type)
                                    : new MessageHeader("0", header.Type);
                                break;

                            case MessageHeader.MessageType.TitleLoading:
                                var text = rand.Next(0, 3) == 0
                                    ? ResourcesHelper.LocalizationGet("LoadingText", header.Language)
                                    : ResourcesHelper.LocalizationGet("LoadingTextTwo", header.Language);
                                headerRequest = new MessageHeader(text, header.Type);
                                break;

                            case MessageHeader.MessageType.Version:
                                var version = DataBaseCommands.Select(header.Type);
                                headerRequest = new MessageHeader(version.ToString(), header.Type);
                                break;

                            case MessageHeader.MessageType.PublicKey:
                                headerRequest = new MessageHeader(_privateKey, header.Type);
                                break;

                            case MessageHeader.MessageType.Games:
                                 var games = DataBaseCommands.Select(header.Type);
                                headerRequest = new MessageHeader(games, header.Type);
                                break;

                            case MessageHeader.MessageType.GamesPath:
                                headerRequest = new MessageHeader(GamesPathConst, header.Type);
                                break;

                            case MessageHeader.MessageType.Programs:
                                var prog = DataBaseCommands.Select(header.Type);
                                headerRequest = new MessageHeader(prog, header.Type);
                                break;

                            case MessageHeader.MessageType.ProgramsPath:
                                headerRequest = new MessageHeader(ProgramsPathConst, header.Type);
                                break;

                            case MessageHeader.MessageType.RecoveryAccount:
                                var accountInfo = header.Message.ToString().Split(',');
                                if (accountInfo.Length != 2)
                                    throw new ArgumentException(accountInfo.Length.ToString());

                                var recAcc = new RecoveryAccount()
                                {
                                    login = accountInfo[0],
                                    email = accountInfo[1],
                                };
                                result = DataBaseCommands.Insert(recAcc, header.Type);
                                headerRequest = result
                                    ? new MessageHeader("1", header.Type)
                                    : new MessageHeader("0", header.Type);
                                break;

                            case MessageHeader.MessageType.Sale:
                                var messageSale = header.Message.ToString().Split(MessageHeader.EndMessage);
                                if (messageSale.Length != 2)
                                    throw new ArgumentException(messageSale.Length.ToString());

                                var token = string.Empty;
                                try
                                {
                                    token = Token.Tokens[messageSale[1]];
                                }
                                catch 
                                {
                                    ConsoleExtension.WriteLineColor("Токена не существует!", ConsoleColor.DarkRed);
                                }

                                if(string.IsNullOrEmpty(token))
                                    result = false;
                                else
                                {
                                    var insertMessage = messageSale[0].ToString() + "," + token;
                                    result = DataBaseCommands.Insert(insertMessage, header.Type);
                                }

                                headerRequest = result
                                    ? new MessageHeader("1", header.Type)
                                    : new MessageHeader("0", header.Type);
                                break;

                            case MessageHeader.MessageType.GamesItemUser:
                                var messageAll = header.Message.ToString();
                                var tokenAll = string.Empty;
                                try
                                {
                                    tokenAll = Token.Tokens[messageAll];
                                }
                                catch
                                {
                                    ConsoleExtension.WriteLineColor("Токена не существует!", ConsoleColor.DarkRed);
                                }

                                if (string.IsNullOrEmpty(tokenAll))
                                {
                                    headerRequest = new MessageHeader(new SaleGamesForXml(), header.Type);
                                }
                                else
                                {
                                    var insertMessage = messageAll + "," + tokenAll;
                                    var gameSelect = DataBaseCommands.Select(header.Type, insertMessage);
                                    headerRequest = new MessageHeader(gameSelect, header.Type);
                                }
                                break;

                            case MessageHeader.MessageType.ProgramsItemUser:
                                messageAll = header.Message.ToString();
                                tokenAll = string.Empty;
                                try
                                {
                                    tokenAll = Token.Tokens[messageAll];
                                }
                                catch
                                {
                                    ConsoleExtension.WriteLineColor("Токена не существует!", ConsoleColor.DarkRed);
                                }

                                if (string.IsNullOrEmpty(tokenAll))
                                {
                                    headerRequest = new MessageHeader(new SaleProgramsForXml(), header.Type);
                                }
                                else
                                {
                                    var insertMessage = messageAll + "," + tokenAll;
                                    var progSelect = DataBaseCommands.Select(header.Type, insertMessage);
                                    headerRequest = new MessageHeader(progSelect, header.Type);
                                }
                                break;
                        }

                        if (headerRequest is not null)
                        {
                            headerRequestBytes = headerRequest.MessageServerToArray(loadToken);
                            listener.Send(headerRequestBytes);
                        }
                    } while (listener.Available > 0); // до тех пор, пока в нашем подключение есть данные, будет продолжаться считывание

                    // надо отправить ответ, чтобы показать, что мы получили
                    //listener.Send(Encoding.UTF8.GetBytes("Сервер: Успех!")); // кодируем данные 
                }
                catch (Exception e)
                {
                    ConsoleExtension.WriteLineColor(e.Message, ConsoleColor.DarkRed);
                }
                finally
                {
                    listener.Shutdown(SocketShutdown.Both); // закрываем подкоючение сокета и клиента, и у сервера 
                    listener.Close(); // отключает, закрывает сокет и освобождает ресурсы
                    ConsoleExtension.WriteLineColor("Отключился!", ConsoleColor.DarkYellow);
                }
            }
        }

        //private static void LoadingRSA()
        //{
        //    var (publicKey, privateKey) = CryptoRsa.GenerateKey();
        //    _publicKey = publicKey;
        //    _privateKey = privateKey;
        //}
    }
}