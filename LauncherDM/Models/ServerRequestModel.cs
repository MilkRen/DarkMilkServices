using ServerTCP;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace LauncherDM.Models
{
    class ServerRequestModel
    {
        const string ip = DataInfo.Ip;
        const int port = DataInfo.Port;

        private IPAddress ipAddress;
        private IPEndPoint endPoint;
        public ServerRequestModel()
        {
            ipAddress = IPAddress.Parse(ip);
            endPoint = new IPEndPoint(ipAddress, port);

        }

        public string sendRequest(string text, MessageHeader.MessageType messageType)
        {
            try
            {
                while (true)
                {
                    var tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);

                    var header = new MessageHeader(text, MessageHeader.MessageType.File, 12345);
                    byte[] headerBytes = header.ToArray();

                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);

                    //WriteColorTextCmd("Сообщение отправил!");
                    //var returnDataStrBuild = new StringBuilder();

                    do
                    {
                        byte[] getBytes = new byte[tcpClient.ReceiveBufferSize];
                        tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);

                        var header2 = MessageHeader.FromArray(getBytes);
                        switch (header.Type)
                        {
                            case MessageHeader.MessageType.Check:
                                Console.WriteLine("TExt");
                                break;
                        }

                    } while (tcpStream.DataAvailable);

                    //WriteColorTextCmd("Получил сообщение: ");
                    //Console.Write(returnDataStrBuild);
                    tcpClient.Close();
                    return string.Empty;
                }

            }
            catch (Exception e)
            {
                //WriteColorTextCmd("Сервер не доступен! " + e.ToString());
                return string.Empty;
            }
        }


       
    }
}
