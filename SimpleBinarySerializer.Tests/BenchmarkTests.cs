using System.Diagnostics;

namespace SimpleBinarySerializer.Tests;

[TestFixture]
public class BenchmarkTests
{
    private const int Iterations = 1000000;

    private static readonly Dictionary<string, string> Headers = new()
    {
        { "header-1", "value" },
        { "header-2", "value" }
    };

    private static readonly byte[] Payload = "Hello, world"u8.ToArray();
    private static readonly Message Message = new Message(Headers, Payload);
    private static byte[] SerializedMessage => Serializer.Serialize(Message);
    
    [Test]
    public void Serialize_Performance_Measurement()
    {
        Run(() =>
        {
            Serializer.Serialize(new Message(Headers, Payload));
        });
    }
    
    [Test]
    public void Deserialize_Performance_Measurement()
    {
        Run(() =>
        {
            Serializer.Deserialize(SerializedMessage);
        });
    }

    private void Run(Action action)
    {
        List<double> measurements = new();
        for (var i = 0; i < Iterations; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            action.Invoke();
            stopwatch.Stop();
            measurements.Add(stopwatch.Elapsed.TotalMilliseconds);
        }
        
        Console.WriteLine($"Number of executions: {Iterations}");
        Console.WriteLine($"Avg Execution time: {measurements.Average()} ms");
        Console.WriteLine($"Min Execution time: {measurements.Min()} ms");
        Console.WriteLine($"Max Execution time: {measurements.Max()} ms");
        
        
    }
}