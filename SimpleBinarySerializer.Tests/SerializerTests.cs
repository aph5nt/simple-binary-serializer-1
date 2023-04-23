namespace SimpleBinarySerializer.Tests;

[TestFixture]
public class SerializerTests 
{
    [Test]
    public void Message_Should_Be_Serialized_And_Deserialized() 
    {
        // Arrange
        var headers = new Dictionary<string, string> 
        {
            { "header-1", "value" },
            { "header-2", "value" }
        };
        
        var payload = "Hello, world"u8.ToArray();
        
        // Act
        var message = new Message(headers, payload);
        var data = Serializer.Serialize(message);
        var decoded = Serializer.Deserialize(data);
        
        // Assert
        Assert.That(decoded.Headers, Is.EqualTo(message.Headers));
        Assert.That(decoded.Payload, Is.EqualTo(message.Payload));
    }
    
    [Test]
    public void Deserialize_Should_Throw_When_Data_Is_Invalid() 
    {
        Assert.Throws<ArgumentException>(() => Serializer.Deserialize("1"u8.ToArray()));
    }
}