using AzureEventHubDemo;

await ProducerEvent.SendEvent()
    .ContinueWith(async response => {
        await ConsumerEvent.ListeningEvents();
    });


Console.ReadLine();