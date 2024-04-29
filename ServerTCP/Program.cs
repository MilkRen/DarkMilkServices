using System.Net;
using System.Net.Sockets;
using ServerTCP.DataBase;
using ServerTCP.Models;

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
                    var listener = tcpSocket.Accept(); // новый сокет для нового клиента
                    Console.WriteLine("Подключился!");
                    var buffer = new byte[256]; // размер буфера .  максимум сообщение из 256 байт 
                    var size = 0; // количество реально полученных количества байт, потом чтобы оптимизировать память 
                    do
                    {
                        size = listener.Receive(buffer); // получение данных, количество, значение
                        //buffer = buffer.Where(x => x != 0).ToArray();
                        var header = MessageHeader.FromArray(buffer);

                        MessageHeader headerRequest = null;
                        byte[] headerRequestBytes;
                        switch (header.Type)
                        {
                            case MessageHeader.MessageType.Check:
                                headerRequest = new MessageHeader("1", MessageHeader.MessageType.Check, 1);
                                headerRequestBytes = headerRequest.MessageToArray();
                                listener.Send(headerRequestBytes);
                                break;
                            case MessageHeader.MessageType.Session:
                                break;
                            case MessageHeader.MessageType.Token:
                                break;
                            case MessageHeader.MessageType.Registration:
                                var user = new User
                                {
                                    id = 1, login = "Gustaf", email = "gustavo@milk.su", username = "Gustavo", password = "sjafhfjhlkj12pje12j31kl23j1l123j"
                                };
                                DataBaseCommands.Insert(user, MessageHeader.MessageType.Registration);
                                break;
                            case MessageHeader.MessageType.Log:
                                break;
                            case MessageHeader.MessageType.File:
                                break;
                            case MessageHeader.MessageType.Photo:
                                break;
                            case MessageHeader.MessageType.Data:
                                break;
                            case MessageHeader.MessageType.Title:
                                break;
                            case MessageHeader.MessageType.TitleLoading:
                                headerRequest = new MessageHeader("1", MessageHeader.MessageType.TitleLoading, 1);
                                headerRequestBytes = headerRequest.MessageToArray();
                                listener.Send(headerRequestBytes);
                                break;

                                //case MessageHeader<string>.MessageType.Title:
                                //    string messageRequest = "Привет, пупс!";
                                //    var header1 = new MessageHeader<string>(messageRequest, MessageHeader<string>.MessageType.Check, Encoding.UTF8.GetBytes(messageRequest).Length);
                                //    byte[] headerBytes = header1.MessageToArray();
                                //    listener.Send(headerBytes);
                                //    break;
                        }
                        //data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
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