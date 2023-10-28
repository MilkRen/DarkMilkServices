using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LauncherDM.Server
{
    class ClientTCP
    {
        const string ip = "91.210.171.69";
        const int port = 8080;

        //var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port); // конечное подключение


        //    while (true)
        //{ 
        //    var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    Console.Write("Отправь сообщение: "); //  сообщение
        //    var message = Console.ReadLine();
        //    var data = Encoding.UTF8.GetBytes(message); // кодируем сообщение

        //    tcpSocket.Connect(tcpEndPoint); // подключаемся к серверу, как клиент
        //    tcpSocket.Send(data); // отправляем сообщение

        //    // получаем ответ от сервера

        //    var buffer = new byte[256]; // размер буфера . максимум сообщение из 256 байт 
        //    var size = 0; // количество реально полученных количества байт, потом чтобы оптимизировать память 
        //    var answer = new StringBuilder();

        //    do
        //    {
        //        size = tcpSocket.Receive(buffer); // получение данных, количество, значение
        //        answer.Append(Encoding.UTF8.GetString(buffer, 0, size)); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
        //    }
        //    while (tcpSocket.Available > 0); // до тех пор, пока в нашем подключение есть данные, будет продолжаться считывание

        //    Console.WriteLine(answer);

        //    tcpSocket.Shutdown(SocketShutdown.Both); // закрываем подкоючение сокета и клиента, и у сервера 
        //    tcpSocket.Close(); // отключает, закрывает сокет и освобождает ресурсы

        //}
    }
}
