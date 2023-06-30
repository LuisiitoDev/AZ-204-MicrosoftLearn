using Azure.Storage.Blobs.Specialized;


Console.WriteLine("Moving file from container 1 to container 2");

var connectionString = "";

var sourceContainer = "test";
var targetContainer = "second";

var sourceFile = "background_jarvis.jpg";
var targetFile = "background_jarvis-COPY2.jpg";

var sourceClient = new BlockBlobClient(connectionString, sourceContainer, sourceFile);
var targetClient = new BlockBlobClient(connectionString,targetContainer, targetFile);

await targetClient.StartCopyFromUriAsync(sourceClient.Uri);


Console.WriteLine("File moved");
Console.ReadLine();

