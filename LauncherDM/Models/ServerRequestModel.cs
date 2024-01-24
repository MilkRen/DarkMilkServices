using LauncherDM.Services.Interfaces;
using LauncherDM.Services;
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

        public string SendMessageRequestT<T>(T data, MessageHeader<T>.MessageType messageType, int length)
        {
            try
            {
                while (true)
                {
                    var tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);

                    var messageHeader = new MessageHeader<T>(data, messageType, length + MessageHeader<T>.LengthAndDataType);
                    byte[] headerBytes = messageHeader.MessageToArray();

                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);

                    //WriteColorTextCmd("Сообщение отправил!");
                    //var returnDataStrBuild = new StringBuilder();

                    do
                    {
                        byte[] getBytes = new byte[tcpClient.ReceiveBufferSize];
                        tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);

                        var header2 = MessageHeader<T>.FromArray(getBytes);
                        switch (header2.Type)
                        {
                            case MessageHeader<T>.MessageType.Check:
                                return header2.MessageString;
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
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
                return string.Empty;
            }
        }
    }
}
