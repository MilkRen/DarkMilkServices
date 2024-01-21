using System.Buffers.Binary;
using System.Drawing;
using System.Text;

namespace ServerTCP
{
    public class MessageHeader
    {
        public MessageType Type { get; }
        public int Length { get; }
        public string Message { get; }

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

        public MessageHeader(string messageText, MessageType type, int length)
        {
            Message = messageText;
            Type = type;
            Length = length;
        }

        public byte[] ToArray()
        {
            var message = Encoding.UTF8.GetBytes(Message);
            var result = new byte[message.Length + 6];
            result[0] = (byte)Type;
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..5], Length);
            Array.Copy(message, 0, result, 6, message.Length);
            return result;
        }

        public static MessageHeader FromArray(ReadOnlySpan<byte> buffer)
        {
            if(buffer.Length <= 6)
                return new MessageHeader((MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
            else
                return new MessageHeader(Encoding.UTF8.GetString(buffer.ToArray(), 6, buffer.Length - 6), (MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
