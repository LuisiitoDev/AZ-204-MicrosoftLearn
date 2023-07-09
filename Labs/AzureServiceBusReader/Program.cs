using Azure.Messaging.ServiceBus;

string connection = "Endpoint=sb://sbnamespaceluisiitodev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=eMtVwnn1UjEbFtD1lOWVjldc+6XYfb898+ASbD8BJKo=";
string queueName = "messagequeue";

ServiceBusClient client = default!;
ServiceBusProcessor processor = default!;

client = new(connection);
processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

processor.ProcessMessageAsync += async (ProcessMessageEventArgs args) =>
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
    await args.CompleteMessageAsync(args.Message);
};

processor.ProcessErrorAsync += (ProcessErrorEventArgs args) =>
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
};

await processor.StartProcessingAsync();
Console.WriteLine("Wait for a minute and then press any key to end the processing");
Console.ReadKey();

Console.WriteLine("\nStopping the receiver...");
await processor.StopProcessingAsync();
Console.WriteLine("Stopped receiving messages");


await processor.DisposeAsync();
await client.DisposeAsync();