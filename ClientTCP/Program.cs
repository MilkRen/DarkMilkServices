using ServerTCP;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string ip = DataInfo.Ip;
            const int port = DataInfo.Port;

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);

            try
            {
                while (true)
                {
                    TcpClient tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);
                    WriteColorTextCmd("К Серверу подключился! " + endPoint, ConsoleColor.DarkGreen);

                    string sendText = Console.ReadLine();

                    //MessageHeader header = new MessageHeader(MessageHeader.MessageType.File, 12345);
                    //byte[] headerBytes = header.MessageToArray();

                    NetworkStream tcpStream = tcpClient.GetStream();
                    //byte[] sendBytes = Encoding.UTF8.GetBytes(sendText);
                    //tcpStream.Write(headerBytes);

                    WriteColorTextCmd("Сообщение отправил!");
                    StringBuilder returnDataStrBuild = new StringBuilder();

                    do
                    {
                        byte[] getBytes = new byte[tcpClient.ReceiveBufferSize];
                        tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);
                        returnDataStrBuild.Append(Encoding.UTF8.GetString(getBytes));

                    } while (tcpStream.DataAvailable);

                    WriteColorTextCmd("Получил сообщение: ");
                    Console.Write(returnDataStrBuild);
                    tcpClient.Close();
                }

            }
            catch (Exception e)
            {
                WriteColorTextCmd("Сервер не доступен! " + e.ToString());
            }



            Console.ReadLine();
        }

        private static void WriteColorTextCmd(string text, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}