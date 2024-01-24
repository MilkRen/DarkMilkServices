using System;
using System.Buffers.Binary;
using System.Drawing;
using System.Text;

namespace ServerTCP
{
    public class MessageHeader<T>
    {
        public const int LengthAndDataType = 6; // 1 байт тип данных, 5 байт размер 

        public MessageType Type { get; }
        public int Length { get; }
        public T Message { get; }
        public string MessageString { get; }

        public enum MessageType : byte
        {
            Session = 0,
            Token= 1,
            Registration = 2,
            Log = 3,
            File = 4,
            Photo = 5,
            Data = 6,
            Check = 7
        }

        public MessageHeader(MessageType type, int length)
        {
            Type = type;
            Length = length;
        }

        public MessageHeader(T message, MessageType type, int length)
        {
            Message = message;
            Type = type;
            Length = length;
        }

        public MessageHeader(string message, MessageType type, int length)
        {
            MessageString = message;
            Type = type;
            Length = length;
        }

        public byte[] MessageToArray()
        {
            byte[] message;
            var result = new byte[Length + LengthAndDataType];
            result[0] = (byte)Type;
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..5], Length);
            if (Message is not null)
            {
                if (!string.IsNullOrEmpty(Message?.ToString()))
                {
                    message = Encoding.UTF8.GetBytes(Message.ToString());
                    Array.Copy(message, 0, result, 6, Length);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(MessageString))
                {
                    message = Encoding.UTF8.GetBytes(MessageString);
                    Array.Copy(message, 0, result, 6, Length);
                }
            }
            return result;
        }

        public static MessageHeader<T> FromArray(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length <= LengthAndDataType)
                return new MessageHeader<T>((MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
            else
                return new MessageHeader<T>(Encoding.UTF8.GetString(buffer.ToArray(), LengthAndDataType, buffer.Length - LengthAndDataType), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
