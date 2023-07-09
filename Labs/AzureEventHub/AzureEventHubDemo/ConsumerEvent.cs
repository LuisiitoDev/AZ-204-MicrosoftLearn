using System.Text;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;

namespace AzureEventHubDemo;

public static class ConsumerEvent
{
    private static string connectionString = "";
    private static string eventHubName = "eventlusiitodev";
    private static string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
    public static async Task ListeningEvents()
    {
        await using var consumerClient = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName);

        // GET THE EVENTS
        await foreach (PartitionEvent partitionEvent in consumerClient.ReadEventsAsync())
        {
            string eventData = Encoding.UTF8.GetString(partitionEvent.Data.EventBody);
            Console.WriteLine($"Event received {eventData}");
        }

    }
}