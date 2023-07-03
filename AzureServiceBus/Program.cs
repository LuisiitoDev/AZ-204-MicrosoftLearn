using Azure.Messaging.ServiceBus;

string connectionString = "";
string queueName = "az204-queue";

ServiceBusClient client = new ServiceBusClient(connectionString);


// CODE FOR SENDING MESSAGES TO A QUEUE

// ServiceBusSender sender = client.CreateSender(queueName);

// using ServiceBusMessageBatch batch = await sender.CreateMessageBatchAsync();

// for (int i = 0; i <= 3; i++)
// {
//     if (!batch.TryAddMessage(new ServiceBusMessage($"Message {i}"))){
//         throw new Exception($"Exception {i} has occurred.");
//     }
// }


// await sender.SendMessagesAsync(batch);
// Console.WriteLine("A batch of three messages has been published to the queue.");

// await sender.DisposeAsync();
// await client.DisposeAsync();


// CODE FOR READING MESSAGES FROM A QUEUE


ServiceBusProcessor processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    // add handler to process messages
    processor.ProcessMessageAsync += MessageHandler;

    // add handler to process any errors
    processor.ProcessErrorAsync += ErrorHandler;

    // start processing 
    await processor.StartProcessingAsync();

    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    // stop processing 
    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await processor.DisposeAsync();
    await client.DisposeAsync();
}


async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
    await args.CompleteMessageAsync(args.Message);
}

Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}