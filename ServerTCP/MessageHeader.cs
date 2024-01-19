using System.Buffers.Binary;

namespace ServerTCP
{
    public class MessageHeader
    {
        public MessageType Type { get; }
        public int Length { get; }

        public enum MessageType : byte
        {
            Session = 0,
            Token= 1,
            Registration = 2,
            File = 3,
            Photo = 4,
            Data = 5,
        }

        public MessageHeader(MessageType type, int length)
        {
            Type = type;
            Length = length;
        }

        public byte[] ToArray()
        {
            var result = new byte[5];
            result[0] = (byte)Type;
            BinaryPrimitives.WriteInt32LittleEndian(result.AsSpan()[1..], Length);
            return result;
        }

        public static MessageHeader FromArray(ReadOnlySpan<byte> buffer)
        {
            return new MessageHeader((MessageType)buffer[0], BinaryPrimitives.ReadInt32LittleEndian(buffer[1..]));
        }
    }
}
