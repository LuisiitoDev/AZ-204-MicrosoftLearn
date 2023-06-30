using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json");

var config = configuration.Build();


Console.WriteLine("Azure Blob Storage exercise");


BlobServiceClient client = new BlobServiceClient(config["Azure:Storage:ConnectionString"]);

var containerName = $"wrblob{Guid.NewGuid()}";

BlobContainerClient containerClient = await client.CreateBlobContainerAsync(containerName);

Console.WriteLine("A container named: " + containerName + " was created!!");


// uploading a file to blobs
string fileName = $"wtfile{Guid.NewGuid()}.txt";
string localFilePath = Path.Combine("./data/", fileName);

// writing a file

await File.WriteAllTextAsync(localFilePath, "Hello World, Azure Storage");

// Getting reference to the blob

var blobClient = containerClient.GetBlobClient(fileName);

Console.WriteLine("Uploading to Blob Storage as blob: " + blobClient.Uri);


// Open file and upload its data

using FileStream fileStream = File.OpenRead(localFilePath);
await blobClient.UploadAsync(fileStream);
fileStream.Close();

Console.WriteLine("The file was upload. We will verify by listing");


// Listing blobs

Console.WriteLine("Listing Blobs...");

await foreach (BlobItem blob in containerClient.GetBlobsAsync())
{
    Console.WriteLine("\t" + blob.Name);
}

// Downloading blobs

string downloadFilePath = localFilePath.Replace(".txt", "DONWLOADED.txt");

Console.WriteLine("Downloading blob to " + downloadFilePath);

BlobDownloadInfo download = await blobClient.DownloadAsync();

using FileStream downloadStream = File.OpenWrite(downloadFilePath);
await download.Content.CopyToAsync(downloadStream);

Console.WriteLine("\nLocate the local file in the data directory created earlier to verify it was downloaded.");

// Deleting the container and cleaning up

Console.WriteLine("Deleting blob container");

//await containerClient.DeleteAsync();

// Fetch some container properties and write out their values.
// var properties = await blobClient.GetPropertiesAsync();
// Console.WriteLine($"Properties for container {blobClient.Uri}");
// Console.WriteLine($"Public access level: {properties.Value.PublicAccess}");
// Console.WriteLine($"Last modified time in UTC: {properties.Value.LastModified}");


Console.WriteLine("Finished cleaning up.");

Console.ReadLine();

