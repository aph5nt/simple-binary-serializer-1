namespace SimpleBinarySerializer;

public record Message(Dictionary<string, string> Headers, byte[] Payload)
{
    public int Size()
    {
        var headerSize = Headers.Count * (2 + MessageConst.MaxHeaderLength + 2 + MessageConst.MaxHeaderLength);
        var payloadSize = Payload.Length;
        return 1 + headerSize + 4 + payloadSize;
    }

    public Message Validate()
    {
        if (Headers.Count > MessageConst.MaxHeaders)
        {
            throw new ArgumentException("Too many headers");
        }

        if (Payload.Length > MessageConst.MaxPayloadLength)
        {
            throw new ArgumentException($"Payload should be max {MessageConst.MaxPayloadLength} bytes length.");
        }

        foreach (var header in Headers)
        {
            if (header.Key.Length > MessageConst.MaxHeaderLength)
            {
                throw new ArgumentException($"Header Key should be max {MessageConst.MaxHeaderLength} bytes length.");
            }

            if (header.Value.Length > MessageConst.MaxHeaderLength)
            {
                throw new ArgumentException($"Header Value should be max {MessageConst.MaxHeaderLength} bytes length.");
            }
        }

        return this;
    }
}
