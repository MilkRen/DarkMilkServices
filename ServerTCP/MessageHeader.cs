using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;
using ServerTCP.Models;

namespace ServerTCP
{
    /// <summary>
    /// Для обработки и отправки сообщений на сервер
    /// </summary>
    public class MessageHeader
    {
        public const int LengthAndDataType = 6; // 1 байт тип данных, 5 байт размер 

        public const string EndMessage = "|0|"; // 1 байт тип данных, 5 байт размер 

        public enum MessageType : byte
        {
            Session,
            Token,
            Login,
            Sale,
            Registration,
            Version,
            Programs,
            ProgramsPath,
            Games,
            AllGamesOrPrograms,
            GamesPath,
            RecoveryAccount,
            Log,
            File,
            Photo,
            Data,
            Check,
            Title,
            TitleLoading,
            PublicKey,
        }

        public MessageType Type { get; }

        public MessageLanguages.Languages Language { get; }

        public object Message { get; }
        public string Token { get; }

        public MessageHeader(MessageType type, MessageLanguages.Languages lang, string token)
        {
            Type = type;
            Language = lang;
            Token = token;
        }

        public MessageHeader(MessageType type, string token)
        {
            Type = type;
            Token = token;
        }

        public MessageHeader(object message, MessageType type, MessageLanguages.Languages lang, string token)
        {
            Message = message;
            Type = type;
            Language = lang;
            Token = token;
        }

        public MessageHeader(object message, MessageType type, MessageLanguages.Languages lang)
        {
            Message = message;
            Type = type;
            Language = lang;
        }

        public MessageHeader(object message, MessageType type)
        {
            Message = message;
            Type = type;
        }

        public MessageHeader(MessageType type, MessageLanguages.Languages lang)
        {
            Type = type;
            Language = lang;
        }

        public MessageHeader(MessageType type)
        {
            Type = type;
        }

        /// <summary>
        /// Упаковка сообщения от клиента
        /// </summary>
        /// <param name="loadToken"></param>
        /// <returns></returns>
        public byte[] MessageToArray(bool loadToken = false)
        {
            var message = new byte[]{};
            var token = new byte[]{};

            switch (Type)
            {
                case MessageType.Login: 
                case MessageType.Registration: 
                case MessageType.RecoveryAccount: 
                    message = Encoding.UTF8.GetBytes(Message.ToString());
                    break;
                case MessageType.Sale:
                    message = Encoding.UTF8.GetBytes(Message.ToString() + EndMessage);
                    break;
            }

            if (loadToken)
                token = Encoding.UTF8.GetBytes(Token);

            var tokenLength = token.Length;
            var messageLength = message.Length;
            var lengthByte = messageLength + tokenLength + LengthAndDataType;

            var result = new byte[lengthByte];
            result[0] = (byte)Type;
            result[1] = (byte)Language;
            result[2] = loadToken ? (byte)1 : (byte)0; // 1 - токен есть | 0 - без токена(размер токена 10 байт)
            var bytes = BitConverter.GetBytes(lengthByte).Where(x => x != 0).ToArray();

            if (bytes.Length > 3)
                throw new ArgumentException(nameof(bytes));


            switch (Type)
            {
                case MessageType.Login:
                case MessageType.Registration:
                case MessageType.RecoveryAccount:
                    Array.Copy(bytes, 0, result, 3, bytes.Length);
                    Array.Copy(message ?? [0], 0, result, LengthAndDataType, messageLength);
                    break;
                case MessageType.Sale:
                    Array.Copy(bytes, 0, result, 3, bytes.Length);
                    Array.Copy(message ?? [0], 0, result, LengthAndDataType, messageLength);
                    Array.Copy(token ?? [0], 0, result, LengthAndDataType + messageLength, tokenLength);
                    break;
                case MessageType.AllGamesOrPrograms:
                    Array.Copy(token ?? [0], 0, result, LengthAndDataType + messageLength, tokenLength);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Упаковка сообщения от сервера
        /// </summary>
        /// <param name="loadToken"></param>
        /// <returns></returns>
        internal byte[] MessageServerToArray(bool loadToken = false)
        {
            var message = new byte[] { };
            var token = new byte[] { };

            switch (Type)
            {
                case MessageType.Token:
                case MessageType.Sale:
                case MessageType.Login:
                case MessageType.AllGamesOrPrograms:
                case MessageType.Registration:
                case MessageType.TitleLoading:
                case MessageType.ProgramsPath:
                case MessageType.RecoveryAccount:
                case MessageType.GamesPath:
                case MessageType.PublicKey:
                case MessageType.Version:
                case MessageType.Check:
                    message = Encoding.UTF8.GetBytes(Message.ToString());
                    break;
                case MessageType.Programs:
                    var xmlProg = new XmlSerializer(typeof(ProgramsForXml));
                    using (var textWriter = new StringWriter())
                    {
                        xmlProg.Serialize(textWriter, Message);
                        message = Encoding.UTF8.GetBytes(textWriter.ToString());
                    }
                    break;
                case MessageType.Games:
                    var xmlGame = new XmlSerializer(typeof(GamesForXml));
                    using (var textWriter = new StringWriter())
                    {
                        xmlGame.Serialize(textWriter, Message);
                        message = Encoding.UTF8.GetBytes(textWriter.ToString());
                    }
                    break;
            }


            if (loadToken)
                token = Encoding.UTF8.GetBytes(Token);

            var tokenLength = token.Length;
            var messageLength = message.Length;
            var lengthByte = messageLength + tokenLength + LengthAndDataType;

            var result = new byte[lengthByte];
            result[0] = (byte)Type;
            result[1] = (byte)Language;
            result[2] = loadToken ? (byte)1 : (byte)0; // 1 - токен есть | 0 - без токена(размер токена 10 байт)
            var bytes = BitConverter.GetBytes(lengthByte).Where(x => x != 0).ToArray();

            if (bytes.Length > 3)
                throw new ArgumentException(nameof(bytes));

            Array.Copy(bytes, 0, result, 3, bytes.Length);

            Array.Copy(message ?? [0], 0, result, LengthAndDataType, messageLength);
            return result;
        }

        /// <summary>
        /// Распаковка сообщения
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static MessageHeader FromArray(ReadOnlySpan<byte> buffer)
        {
            switch ((MessageType)buffer[0])
            {
                case MessageHeader.MessageType.Version:
                case MessageHeader.MessageType.Sale:
                case MessageHeader.MessageType.PublicKey:
                case MessageHeader.MessageType.Login:
                case MessageHeader.MessageType.Registration:
                case MessageHeader.MessageType.AllGamesOrPrograms:
                case MessageHeader.MessageType.TitleLoading:
                case MessageHeader.MessageType.RecoveryAccount:
                case MessageHeader.MessageType.Programs:
                case MessageHeader.MessageType.ProgramsPath:
                case MessageHeader.MessageType.Games:
                case MessageHeader.MessageType.GamesPath:
                case MessageHeader.MessageType.Check: 
                    return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], (MessageLanguages.Languages)buffer[1]);  
                    break;
                case MessageHeader.MessageType.Session:
                    //return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..])); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
                    return new MessageHeader((MessageType)buffer[0],
                        (MessageLanguages.Languages)buffer[1], 
                        Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType));
                    break;
                case MessageHeader.MessageType.Token:
                    return new MessageHeader((MessageType)buffer[0],
                        Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType));
                    break;
                //case MessageHeader.MessageType.Login:
                //    var result = buffer[LengthAndDataType..buffer.Length];
                //    return new MessageHeader(result.ToArray(),
                //       (MessageType)buffer[0]);
                //    break;
                default:
                    return null;
                break;
            }
        }


        /// <summary>
        /// Распаковка сообщения для сервера
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static MessageHeader ServerFromArray(ReadOnlySpan<byte> buffer)
        {
            switch ((MessageType)buffer[0])
            {
                case MessageHeader.MessageType.Version:
                case MessageHeader.MessageType.PublicKey:
                case MessageHeader.MessageType.Login:
                case MessageHeader.MessageType.Programs:
                case MessageHeader.MessageType.AllGamesOrPrograms:
                case MessageHeader.MessageType.ProgramsPath:
                case MessageHeader.MessageType.RecoveryAccount:
                case MessageHeader.MessageType.Games:
                case MessageHeader.MessageType.GamesPath:
                case MessageHeader.MessageType.Registration:
                case MessageHeader.MessageType.TitleLoading:
                case MessageHeader.MessageType.Sale:
                case MessageHeader.MessageType.Check:
                    return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], (MessageLanguages.Languages)buffer[1]);
                    break;
                case MessageHeader.MessageType.Session:
                    //return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..])); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
                    return new MessageHeader((MessageType)buffer[0],
                        (MessageLanguages.Languages)buffer[1],
                        Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType));
                    break;
                case MessageHeader.MessageType.Token:
                    return new MessageHeader((MessageType)buffer[0],
                        Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType));
                    break;
                default:
                    return null;
                    break;
            }
        }
    }
}
