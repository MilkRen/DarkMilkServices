using ServerTCP.DataBase;
using ServerTCP.Models;
using System.Buffers.Binary;
using System.Text;

namespace ServerTCP
{
    /// <summary>
    /// Для обработки и отправки сообщений на сервер
    /// </summary>
    public class MessageHeader
    {
        public const int LengthAndDataType = 6; // 1 байт тип данных, 5 байт размер 
        public const int TokenLength = 10; // 10 байт под токен 

        public enum MessageType : byte
        {
            Session,
            Token,
            Registration,
            Log,
            File,
            Photo,
            Data,
            Check,
            Title,
            TitleLoading,
        }

        public enum Languages
        {
            rus,
            eng
        }

        public MessageType Type { get; }

        public Languages Language { get; }

        public int Length { get; }

        public object Message { get; }
        public string Token { get; }

        public MessageHeader(MessageType type, Languages lang, string token, int length)
        {
            Type = type;
            Language = lang;
            Token = token;
            Length = length;
        }

        public MessageHeader(object message, MessageType type, Languages lang, string token, int length)
        {
            Message = message;
            Type = type;
            Language = lang;
            Token = token;
            Length = length;
        }

        public MessageHeader(object message, MessageType type, int length)
        {
            Message = message;
            Type = type;
            Length = length;
        }

        public MessageHeader(MessageType type, Languages lang, int length = 0)
        {
            Type = type;
            Language = lang;
            Length = length;
        }

        public MessageHeader(MessageType type, int length = 0)
        {
            Type = type;
            Length = length;
        }

        /// <summary>
        /// Упаковка сообщения от клиента
        /// </summary>
        /// <param name="loadToken"></param>
        /// <returns></returns>
        public byte[] MessageToArray(bool loadToken = false)
        {
            byte[] message;
            var result = loadToken ? new byte[Length + TokenLength + LengthAndDataType] : new byte[Length + LengthAndDataType];
            result[0] = (byte)Type;
            result[1] = (byte)Language;
            result[2] = loadToken ? (byte)1 : (byte)0; // 1 - токен есть | 0 - без токена(размер токена 10 байт)
            if(Length > 0)
                BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[3..5], Length);

            switch (Type)
            {
                case MessageType.Session:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Registration:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Log:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.File:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Photo:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Data:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                default:
                    return result;
                    break;
            }

            if (loadToken)
            {
                Array.Copy(Encoding.UTF8.GetBytes("1234567890"), 0, result, LengthAndDataType, TokenLength);
                Array.Copy(message ?? [0], 0, result, LengthAndDataType + TokenLength, Length);
            }
            else
                Array.Copy(message ?? [0], 0, result, 6, Length);
                
            return result;
        }

        /// <summary>
        /// Упаковка сообщения от сервера
        /// </summary>
        /// <param name="loadToken"></param>
        /// <returns></returns>
        internal byte[] MessageServerToArray(bool loadToken = false)
        {
            byte[] message;
            int lengthByte = loadToken ? Length + TokenLength + LengthAndDataType : Length + LengthAndDataType;

            var result = new byte[lengthByte];
            result[0] = (byte)Type;
            result[1] = (byte)Language;
            result[2] = loadToken ? (byte)1 : (byte)0; // 1 - токен есть | 0 - без токена(размер токена 10 байт)
            if (Length > 0)
                BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan().Slice(3,5), lengthByte);

            switch (Type)
            {
                case MessageType.Session:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Token:
                    message = Encoding.UTF8.GetBytes("1234567890");
                    break;
                case MessageType.Registration:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Log:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.File:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Photo:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Data:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                default:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
            }

            if (loadToken)
                Array.Copy(message ?? [0], 0, result, LengthAndDataType, Length);
            else
                Array.Copy(message ?? [0], 0, result, LengthAndDataType, Length);

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
                case MessageHeader.MessageType.Check:
                    return new MessageHeader((MessageType)buffer[0], (Languages)buffer[1], 0);  
                    break;
                case MessageHeader.MessageType.Session:
                    return null;
                    break;
                case MessageHeader.MessageType.Token:
                    //return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..])); // сохраняем, добавляем данные. Данные передаются в кодированном формате, будем использовать кодировку UTF8, раскодируем байты
                    return new MessageHeader((MessageType)buffer[0], (Languages)buffer[1], Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), buffer[3] + buffer[4] + buffer[5]);
                    break;
                case MessageHeader.MessageType.Registration:
                case MessageHeader.MessageType.Log:
                case MessageHeader.MessageType.File:
                case MessageHeader.MessageType.Photo:
                case MessageHeader.MessageType.Data:
                case MessageHeader.MessageType.Title:
                case MessageHeader.MessageType.TitleLoading:
                default:
                    return null;
                break;
            }

            

            //if (buffer.Length <= LengthAndDataType)
            //    return new MessageHeader((MessageType)buffer[0], (Languages)buffer[1], 0);
            //else if (buffer[2] == 1)
            //    return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
            //else
            //    return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
