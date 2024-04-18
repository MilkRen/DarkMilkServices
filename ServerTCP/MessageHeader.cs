using System;
using System.Buffers.Binary;
using System.Drawing;
using System.Text;

namespace ServerTCP
{
    /// <summary>
    /// Для обработки и отправки сообщений на сервер
    /// </summary>
    public class MessageHeader
    {
        public const int LengthAndDataType = 6; // 1 байт тип данных, 5 байт размер 

        public MessageType Type { get; }

        public int Length { get; }

        public string Message { get; }

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
        }

        public MessageHeader(MessageType type, int length)
        {
            Type = type;
            Length = length;
        }

        public MessageHeader(string message, MessageType type, int length)
        {
            Message = message;
            Type = type;
            Length = length;
        }

        public byte[] MessageToArray()
        {
            byte[] message;
            var result = new byte[Length + LengthAndDataType];
            result[0] = (byte)Type;
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..5], Length);

            switch (Type)
            {
                case MessageType.Session:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                case MessageType.Token:
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
                case MessageType.Check:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
                default:
                    message = Encoding.UTF8.GetBytes("1");
                    break;
            }
            Array.Copy(message, 0, result, 6, Length);

            return result;
        }

        public static MessageHeader FromArray(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length <= LengthAndDataType)
                return new MessageHeader((MessageType)buffer[0], 0);
            else
                return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
