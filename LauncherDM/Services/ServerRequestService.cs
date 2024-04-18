﻿using LauncherDM.Services.Interfaces;
using ServerTCP;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows.Markup;

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
            try
            {
                while (true)
                {
                    var tcpClient = new TcpClient();
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

                    //WriteColorTextCmd("Получил сообщение: ");
                    //Console.Write(returnDataStrBuild);
                    tcpClient.Close();
                    //return string.Empty;
                }

            }
            catch (Exception e)
            {
                IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
                //return string.Empty;
            }


            return null;
        }

        public MessageHeader SendMessageRequest(MessageHeader.MessageType messageType, int length)
        {
            TcpClient tcpClient = null;
            try
            {
                while (true)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(endPoint);

                    var messageHeader = new MessageHeader(messageType, length);
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
    }
}
