using System.Net;
using System.Net.Sockets;
using ServerTCP.DataBase;
using ServerTCP.Models;
using System.Security.Authentication;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ServerTCP
{
    internal class Program
    {
        private static readonly string ip = DataInfo.Ip;
        private static readonly int port = DataInfo.Port;

        static void Main(string[] args)
        {
            var ipAddress = IPAddress.Parse(ip);
            var endPoint = new IPEndPoint(ipAddress, port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(endPoint); // Связываем подлкючение к сокету, чтобы его прослушивать 
            tcpSocket.Listen(100); // количество подключеных в очередь

            // чтобы сделать многопоточное подключение, нужен новый сокет и т д 

            while (true)
            {
                try
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", endPoint);
                    var listener = tcpSocket.Accept(); // новый с   окет для нового клиента
                    Console.WriteLine("Подключился!");
                    var buffer = new byte[256]; // размер буфера .  максимум сообщение из 256 байт 
                    var size = 0; // количество реально полученных количества байт, потом чтобы оптимизировать память 
                    do
                    {
                        size = listener.Receive(buffer); // получение данных, количество, значение
                        var header = MessageHeader.FromArray(buffer.Take(size).ToArray());

                        MessageHeader headerRequest = null;
                        byte[] headerRequestBytes;
                        string message = string.Empty;
                        bool loadToken = false;
                        switch (header.Type)
                        {
                            case MessageHeader.MessageType.Check:
                                headerRequest = new MessageHeader("1", MessageHeader.MessageType.Check);
                                break;
                            case MessageHeader.MessageType.Token:
                                message = "1234567890";
                                loadToken = true;
                                headerRequest = new MessageHeader(message, MessageHeader.MessageType.Token);
                                break;
                            case MessageHeader.MessageType.Registration:
                                var user = new User
                                {
                                    id = 1, login = "Gustaf", email = "gustavo@milk.su", username = "Gustavo", password = "sjafhfjhlkj12pje12j31kl23j1l123j"
                                };
                                DataBaseCommands.Insert(user, MessageHeader.MessageType.Registration);
                                break;
                            case MessageHeader.MessageType.TitleLoading:
                                var tx = "Привет, славяне!";
                                headerRequest = new MessageHeader(tx, MessageHeader.MessageType.TitleLoading);
                                break;
                            case MessageHeader.MessageType.Version:
                                var version = DataBaseCommands.Select(MessageHeader.MessageType.Version);
                                headerRequest = new MessageHeader(version.ToString(), MessageHeader.MessageType.Version);
                                break;
                        }

                        if (headerRequest is not null)
                        {
                            headerRequestBytes = headerRequest.MessageServerToArray(loadToken);
                            listener.Send(headerRequestBytes);
                        }
                    }
                    while (listener.Available > 0); // до тех пор, пока в нашем подключение есть данные, будет продолжаться считывание

                    // надо отправить ответ, чтобы показать, что мы получили
                    //listener.Send(Encoding.UTF8.GetBytes("Сервер: Успех!")); // кодируем данные 
                    listener.Shutdown(SocketShutdown.Both); // закрываем подкоючение сокета и клиента, и у сервера 
                    listener.Close(); // отключает, закрывает сокет и освобождает ресурсы
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}