## SimpleBinarySerializer

### Characteristics
Here are the characteristics of the simple binary serializer:

- Designed for a signaling protocol used in real-time communication applications.
- Allows for passing messages between peers in the application.
- Supports a simple message model consisting of headers and a payload.
- Headers are name-value pairs that are ASCII-encoded strings.
- Header names and values are both limited to 1023 bytes each.
- A message can have a maximum of 63 headers.
- The message payload is limited to 256 KiB.
- Designed to be simple and efficient.

### Build int validation
This serializer has built-in validation rules to ensure that messages are correctly serialized and deserialized. If any of the following validation rules are not met, the serializer will throw an exception:

- Header names and values must be no more than 1023 bytes each.
- A message can have a maximum of 63 headers.
- The message payload is limited to 256 KiB.


### SimpleBinarySerializer sample usage:

    var headers = new Dictionary<string, string> 
    {
        { "header-1", "value" },
        { "header-2", "value" }
    };

    var payload = "Hello, world"u8.ToArray();

    // create message
    var messageToEncode = new Message(headers, payload);

    // serialize message
    var data = Serializer.Serialize(messageToEncode);
    
    // deserialize message
    Message decodedMessage = Serializer.Deserialize(data);

