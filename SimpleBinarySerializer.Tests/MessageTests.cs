using System.Text;

namespace SimpleBinarySerializer.Tests;

[TestFixture]
public class MessageTests
{
    [Test]
    public void Message_Should_Return_Size() 
    {
        var size = new Message(new Dictionary<string, string>(), Array.Empty<byte>()).Size();
        Assert.That(size > 0);
    }
    
    [Test]
    public void Message_Should_Throw_When_Passing_Too_Many_Headers() 
    {
        var headers = new Dictionary<string, string>();
        
        for (var i = 0; i < MessageConst.MaxHeaders + 10; i++) {
            headers.Add($"header-{i}", $"value-{i}");
        }
        
        Assert.Throws<ArgumentException>(() => new Message(headers, Array.Empty<byte>()).Validate());
    }

    [Test]
    public void Message_Should_Throw_When_Header_Key_It_Too_Long()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < short.MaxValue; i++)
        {
            sb.Append("-------------------------------------------");
        }
        
        var headers = new Dictionary<string, string>
        {
            { sb.ToString(), "value" }
        };
        
        Assert.Throws<ArgumentException>(() => new Message(headers, Array.Empty<byte>()).Validate());
    }
    
    [Test]
    public void Message_Should_Throw_When_Header_Value_It_Too_Long()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < short.MaxValue; i++)
        {
            sb.Append("-------------------------------------------");
        }
        
        var headers = new Dictionary<string, string>
        {
            { "header", sb.ToString() }
        };
        
        Assert.Throws<ArgumentException>(() => new Message(headers, Array.Empty<byte>()).Validate());        
    }
    
    [Test]
    public void Message_Should_Throw_When_Payload_Is_Too_Big()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < short.MaxValue; i++)
        {
            sb.Append("-------------------------------------------");
        }

        var payload = Encoding.ASCII.GetBytes(sb.ToString());
        Assert.Throws<ArgumentException>(() => new Message(new Dictionary<string, string>(), payload).Validate());        
    }
}