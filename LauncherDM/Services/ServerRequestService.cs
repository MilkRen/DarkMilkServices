using LauncherDM.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Markup;
using ServerTCP;

namespace LauncherDM.Services
{
    /// <summary>
    /// Для запросов на сервер
    /// </summary>
    class ServerRequestService : IServerRequestService
    {
        private static readonly string ip = DataInfo.Ip;
        private static readonly int port = DataInfo.Port;

        private IPAddress ipAddress;
        private IPEndPoint endPoint;

        public ServerRequestService()
        {
            ipAddress = IPAddress.Parse(ip);
            endPoint = new IPEndPoint(ipAddress, port);
        }

        public MessageHeader SendMessageRequest(string data, MessageHeader.MessageType messageType, int length)
        {
            TcpClient tcpClient = null;
            try
            {
                while (true)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);

                    var messageHeader = new MessageHeader(data, messageType, length + MessageHeader.LengthAndDataType);
                    byte[] headerBytes = messageHeader.MessageToArray();

                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);

                    do
                    {
                        byte[] getBytes = new byte[tcpClient.ReceiveBufferSize];
                        tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);
                        var defaultData = getBytes.Skip(1).Take(5);
                        var size = 0;
                        foreach (var sizeData in defaultData)
                            size += sizeData;
                        getBytes = getBytes.Take(size + MessageHeader.LengthAndDataType).ToArray();
                        return MessageHeader.FromArray(getBytes);
                    } while (tcpStream.DataAvailable);
                }

            }
            catch (Exception e)
            {
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
                return null;
            }
            finally
            {
                tcpClient?.Close();
            }
        }

        public MessageHeader SendMessageRequest(MessageHeader.MessageType messageType)
        {
            TcpClient tcpClient = null;
            try
            {
                while (true)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);

                    var messageHeader = new MessageHeader(messageType, 0);
                    byte[] headerBytes = messageHeader.MessageToArray();
                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);

                    do
                    {
                        byte[] getBytes = new byte[tcpClient.ReceiveBufferSize];
                        var lengthByte =  tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);
                        return MessageHeader.FromArray(getBytes.Take(lengthByte).ToArray());
                    } while (tcpStream.DataAvailable);
                }
            }
            catch (Exception e)
            {
                //IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                //dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
                return null;
            }
            finally
            {
                tcpClient?.Close();
            }
        }
    }
}
