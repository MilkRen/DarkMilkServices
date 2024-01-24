using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = DataInfo.Ip;
            const int port = DataInfo.Port;

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            tcpSocket.Bind(endPoint); // Связываем подлкючение к сокету, чтобы его прослушивать 

            tcpSocket.Listen(100); // количество подключеных в очередь, 6 не добавят в очередь 

            // чтобы сделать многопоточное подключение, нужен новый сокет и т д 


            while (true)
            {
                Console.WriteLine("Ожидаем соединение через порт {0}", endPoint);
                var listener = tcpSocket.Accept(); // новый сокет для нового клиента
                Console.WriteLine("Подключился!");
                var buffer = new byte[256]; // размер буфера .  максимум сообщение из 256 байт 
                var size = 0; // количество реально полученных количества байт, потом чтобы оптимизировать память 

                //var data = new StringBuilder();

                do
                {
                    size = listener.Receive(buffer); // получение данных, количество, значение

                    MessageHeader<string> header = MessageHeader<string>.FromArray(buffer);
                    switch (header.Type)
                    {
                        case MessageHeader<string>.MessageType.Check:
                            string messageRequest = "Привет, пупс!";
                            var header1 = new MessageHeader<string>(messageRequest, MessageHeader<string>.MessageType.Check, Encoding.UTF8.GetBytes(messageRequest).Length);
                            byte[] headerBytes = header1.MessageToArray();
                            listener.Send(headerBytes);
                            break;
                    }
                    //data.Append(Encoding.UTF8.GetString(buffer, 0, size)); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
                }
                while (listener.Available > 0); // до тех пор, пока в нашем подключение есть данные, будет продолжаться считывание
                                                // 
                //Console.WriteLine(data);
                // надо отправить ответ, чтобы показать, что мы получили

                //listener.Send(Encoding.UTF8.GetBytes("Сервер: Успех!")); // кодируем данные 
                listener.Shutdown(SocketShutdown.Both); // закрываем подкоючение сокета и клиента, и у сервера 
                listener.Close(); // отключает, закрывает сокет и освобождает ресурсы
            }
        }
    }
}