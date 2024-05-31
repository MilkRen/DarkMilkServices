using LauncherDM.Services.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms.VisualStyles;
using ServerTCP;
using LauncherDM.Properties;
using static ServerTCP.MessageLanguages;

namespace LauncherDM.Services
{
    /// <summary>
    /// Для запросов на сервер
    /// </summary>
    class ServerRequestService : IServerRequestService
    {
        private IPAddress _ipAddress;
        private IPEndPoint _endPoint;

        public ServerRequestService()
        {
            _ipAddress = IPAddress.Parse(SettingsApp.Default.Ip);
            _endPoint = new IPEndPoint(_ipAddress, int.Parse(SettingsApp.Default.Port));
        }

        public MessageHeader SendMessageRequest(object data, MessageHeader.MessageType messageType, bool loadToken = false)
        {
            TcpClient tcpClient = null;
            try
            {
                while (true)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(_endPoint);
                    MessageHeader messageHeader = null;
                    if (loadToken) 
                        messageHeader = new MessageHeader(data, messageType, MessageLanguages.Language, SettingsApp.Default.Token);
                    else 
                        messageHeader = new MessageHeader(data, messageType, MessageLanguages.Language);
                    byte[] headerBytes = messageHeader.MessageToArray(loadToken);
                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);
                    System.Threading.Thread.Sleep(1000);
                    do
                    {
                        var getBytes = new byte[tcpClient.ReceiveBufferSize];
                        var lengthByte = tcpStream.Read(getBytes, 0, tcpClient.ReceiveBufferSize);
                        return MessageHeader.FromArray(getBytes.Take(lengthByte).ToArray());
                    } while (tcpStream.DataAvailable);
                }

            }
            catch (Exception e)
            {
                ////IResourcesHelperService resourcesHelper = new ResourcesHelperService();
                //IDialogMessageBoxService dialogMessageBox = new DialogMessageBoxService();
                //dialogMessageBox.DialogShow("Error Server Reques", "Error Server Reques");
                return null;
            }
            finally
            {
                tcpClient?.Close();
            }
        }

        public MessageHeader SendMessageRequest(MessageHeader.MessageType messageType, bool loadToken = false)
        {
            TcpClient tcpClient = null;
            try
            {
                while (true)
                {
                    tcpClient = new TcpClient();
                    tcpClient.Connect(_endPoint);

                    MessageHeader messageHeader = null;
                    if (loadToken)
                        messageHeader = new MessageHeader(messageType, MessageLanguages.Language, SettingsApp.Default.Token);
                    else
                        messageHeader = new MessageHeader(messageType, MessageLanguages.Language);
                    byte[] headerBytes = messageHeader.MessageToArray(loadToken);
                    NetworkStream tcpStream = tcpClient.GetStream();
                    tcpStream.Write(headerBytes);
                    System.Threading.Thread.Sleep(1000); // Todo: Костыль с получениями байтов, намеренная задержка, чтобы все байты были отправлены
                    do
                    {
                        var getBytes = new byte[tcpClient.ReceiveBufferSize];
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
