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
            Login,
            Registration,
            Version,
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

            switch (Type)
            {
                case MessageType.Login:
                    //message = (byte[])Message;
                    Encoding.UTF8.GetBytes(Message.ToString());
                    break;
            }

            var messageLength = message.Length;
            int lengthByte = loadToken ? messageLength + TokenLength + LengthAndDataType : messageLength + LengthAndDataType;

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
                    Array.Copy(bytes, 0, result, 3, bytes.Length);
                    Array.Copy(message ?? [0], 0, result, LengthAndDataType, messageLength);
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

            switch (Type)
            {
                case MessageType.Token:
                case MessageType.TitleLoading:
                case MessageType.PublicKey:
                case MessageType.Version:
                case MessageType.Check:
                    message = Encoding.UTF8.GetBytes(Message.ToString());
                    break;
            }

            var messageLength = message.Length;
            var lengthByte = loadToken ? messageLength + TokenLength + LengthAndDataType : messageLength + LengthAndDataType;
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
                case MessageHeader.MessageType.PublicKey:
                case MessageHeader.MessageType.Login:
                case MessageHeader.MessageType.TitleLoading:
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
                case MessageHeader.MessageType.Registration:
                case MessageHeader.MessageType.Log:
                case MessageHeader.MessageType.File:
                case MessageHeader.MessageType.Photo:
                case MessageHeader.MessageType.Data:
                case MessageHeader.MessageType.Title:
                default:
                    return null;
                break;
            }
        }
    }
}
