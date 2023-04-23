namespace SimpleBinarySerializer;

public static class MessageConst
{
    public const int MaxHeaderLength = 1023;
    public const int MaxHeaders = 63;
    public const int MaxPayloadLength = 256 * 1024; // 256 KiB
}