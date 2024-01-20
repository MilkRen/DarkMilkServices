using System.Buffers.Binary;
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
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..], Length);
            //BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..], Encoding.UTF8.GetBytes(Message).);
            Array.Copy(Encoding.UTF8.GetBytes(Message), 0, result, 6, result.Length);
            return result;
        }

        public static MessageHeader FromArray(ReadOnlySpan<byte> buffer)
        {
            return new MessageHeader((MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
