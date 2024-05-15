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

        private static string _publicKey;
        private static string _privateKey;

        private static Random rand = new Random();

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
                try
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", endPoint);
                    var listener = tcpSocket.Accept(); // новый с   окет для нового клиента
                    Console.WriteLine("Подключился!");
                    var buffer = new byte[listener.ReceiveBufferSize]; // размер буфера .  максимум сообщение из 256 байт 
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
                            case MessageHeader.MessageType.Login:
                              //  byte[] s = (byte[])header.Message;
                              //var r =  CryptoRsa.Decrypt(_privateKey, s);
                              //Console.WriteLine(r);
                              Console.WriteLine(header.Message.ToString());
                                break;
                            case MessageHeader.MessageType.Registration:
                                var user = new User
                                {
                                    id = 1, login = "Gustaf", email = "gustavo@milk.su", username = "Gustavo", password = "sjafhfjhlkj12pje12j31kl23j1l123j"
                                };
                                DataBaseCommands.Insert(user, MessageHeader.MessageType.Registration);
                                break;
                            case MessageHeader.MessageType.TitleLoading:
                                var text = rand.Next(0, 3) == 0
                                    ? ResourcesHelper.LocalizationGet("LoadingText", header.Language)
                                    : ResourcesHelper.LocalizationGet("LoadingTextTwo", header.Language);
                                headerRequest = new MessageHeader(text, MessageHeader.MessageType.TitleLoading);
                                break;
                            case MessageHeader.MessageType.Version:
                                var version = DataBaseCommands.Select(MessageHeader.MessageType.Version);
                                headerRequest = new MessageHeader(version.ToString(), MessageHeader.MessageType.Version);
                                break;
                            case MessageHeader.MessageType.PublicKey:
                                headerRequest = new MessageHeader(_privateKey, MessageHeader.MessageType.PublicKey);
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

        //private static void LoadingRSA()
        //{
        //    var (publicKey, privateKey) = CryptoRsa.GenerateKey();
        //    _publicKey = publicKey;
        //    _privateKey = privateKey;
        //}
    }
}