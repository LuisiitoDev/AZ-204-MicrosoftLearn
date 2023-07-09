using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace AzureEventHubDemo;

public class ProducerEvent
{
    private static string connectionString = "";
    private static string eventHubName = "eventlusiitodev";

    public static async Task SendEvent()
    {
        await using var producerClient = new EventHubProducerClient(connectionString, eventHubName);

        using EventDataBatch batch = await producerClient.CreateBatchAsync();

        // Add events to bulk
        batch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Message 1")));
        batch.TryAdd(new EventData(Encoding.UTF8.GetBytes("Message 2")));
        
        // Send events
        await producerClient.SendAsync(batch);

        Console.WriteLine("Events sent successfully");
    }
}