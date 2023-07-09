using Azure.Messaging.ServiceBus;


string serviceBusConnectionString = "Endpoint=sb://sbnamespaceluisiitodev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=eMtVwnn1UjEbFtD1lOWVjldc+6XYfb898+ASbD8BJKo=";
string queueName = "messagequeue";
int numOfMessages = 3;

ServiceBusClient client = default!;
ServiceBusSender sender = default!;

client = new(serviceBusConnectionString);
sender = client.CreateSender(queueName);

using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    if(!batch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
        throw new Exception($"The message {i} is too large to fit in the batch.");
}

await sender.SendMessagesAsync(batch);
Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");

await sender.DisposeAsync();
await client.DisposeAsync();