using System.Text;

namespace SimpleBinarySerializer;

public static class Serializer 
{
    public static byte[] Serialize(Message message) => SerializeInner(message.Validate());

    public static Message Deserialize(byte[] data)
    {
        try
        {
            var output = DeserializeInner(data);
            return new Message(output.Headers, output.Payload).Validate();
        }
        catch
        {
            throw new ArgumentException("Unable to decode provided data.");
        }
    }
    
    private static byte[] SerializeInner(Message message)
    {
        var offset = 0;
        
        var buffer = new byte[message.Size()];
        buffer[offset++] = (byte) message.Headers.Count;
        
        foreach (var header in message.Headers) 
        {
            var nameBytes = Encoding.ASCII.GetBytes(header.Key);
            var valueBytes = Encoding.ASCII.GetBytes(header.Value);
            
            WriteHeader(buffer, ref offset, (short) nameBytes.Length);
            Buffer.BlockCopy(nameBytes, 0, buffer, offset, nameBytes.Length);
            
            offset += nameBytes.Length;
            
            WriteHeader(buffer, ref offset, (short) valueBytes.Length);
            Buffer.BlockCopy(valueBytes, 0, buffer, offset, valueBytes.Length);
            
            offset += valueBytes.Length;
        }
        
        WritePayload(buffer, ref offset, message.Payload.Length);
        Buffer.BlockCopy(message.Payload, 0, buffer, offset, message.Payload.Length);

        return buffer;
    }

    private static (Dictionary<String, String> Headers, byte[] Payload) DeserializeInner(byte[] data)
    {
        var offset = 0;
        var headerCount = data[offset++];
        var headers = new Dictionary<string, string>();

        for (int i = 0; i < headerCount; i++)
        {
            var nameLength = ReadHeader(data, ref offset);
            var name = Encoding.ASCII.GetString(data, offset, nameLength);
            offset += nameLength;

            var valueLength = ReadHeader(data, ref offset);
            var value = Encoding.ASCII.GetString(data, offset, valueLength);
            offset += valueLength;
            headers[name] = value;
        }

        int payloadLength = ReadPayload(data, ref offset);
        var payload = new byte[payloadLength];
        Buffer.BlockCopy(data, offset, payload, 0, payloadLength);

        return (headers, payload);
    }

    private static void WriteHeader(byte[] buffer, ref int offset, short value) 
    {
        buffer[offset++] = (byte) (value >> 8);
        buffer[offset++] = (byte) value;
    }
    
    private static short ReadHeader(byte[] buffer, ref int offset) 
    {
        short value = (short) ((buffer[offset] << 8) | buffer[offset + 1]);
        offset += 2;
        return value;
    }
    
    private static void WritePayload(byte[] buffer, ref int offset, int value) 
    {
        buffer[offset++] = (byte) (value >> 24);
        buffer[offset++] = (byte) (value >> 16);
        buffer[offset++] = (byte) (value >> 8);
        buffer[offset++] = (byte) value;
    }
    
    private static int ReadPayload(byte[] buffer, ref int offset)
    {
        int value = (buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | buffer[offset + 3];
        offset += 4;
        return value;
    }
}