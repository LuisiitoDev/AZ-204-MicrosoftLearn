using System.Text.Json;
using System.Diagnostics;
using Microsoft.Azure.Cosmos;

const string EndpointUrl = "https://polycosmoslusiitodev.documents.azure.com:443/";
const string AuthorizationKey = "C9a3YFLCSbXJiNiCQ6kNA5frBGMnEEFccyslHgFrGxAgyMn0Fpqta5PvoTnkFVtjfHjK0bwJLNZbACDbsZBasA==";
const string DataBaseName = "Retail";
const string ContainerName = "Online";
const string PartitionKey = "/Category";
const string JsonFilePath = @"C:\Users\Luis Sandoval\OneDrive\Escritorio\AZ204\Labs\AzureCosmosDB\AdventureWorks.Upload\models.json";
int amountToInsert;
List<Model> models;

try
{
    // CREATING CLIENT
    CosmosClient client = new CosmosClient(EndpointUrl, AuthorizationKey, new CosmosClientOptions() { AllowBulkExecution = true });

    // INIT
    Console.WriteLine("Creaint a database if not exists");
    Database database = await client.CreateDatabaseIfNotExistsAsync(DataBaseName);

    // Configure indexing policy to exclude all attributes to maximaze RU/s usage
    Console.WriteLine("Creating a contaier if no already exists....");
    await database.DefineContainer(ContainerName, PartitionKey)
    .WithIndexingPolicy()
    .WithIndexingMode(IndexingMode.Consistent)
    .WithIncludedPaths()
    .Attach()
    .WithExcludedPaths()
    .Path("/*")
    .Attach()
    .Attach()
    .CreateAsync();

    using StreamReader reader = new StreamReader(File.OpenRead(JsonFilePath));
    var json = await reader.ReadToEndAsync();
    models = JsonSerializer.Deserialize<List<Model>>(json);
    amountToInsert = models.Count;


    // Prepare items for insertion
    Console.WriteLine($"Preparing {amountToInsert} items to insert...");

    Console.WriteLine("Starting...");

    Stopwatch stp = Stopwatch.StartNew();

    Container container = database.GetContainer(ContainerName);

    var tasks = new List<Task>(amountToInsert);

    foreach (Model model in models)
    {
        tasks.Add(container.CreateItemAsync(model, new PartitionKey(model.Category))
            .ContinueWith(itemResponse =>
            {
                if (!itemResponse.IsCompletedSuccessfully)
                {
                    AggregateException innerExceptions = itemResponse.Exception.Flatten();
                    if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                    {
                        Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                    }
                    else
                    {
                        Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                    }
                }
            }));
    }

    await Task.WhenAll(tasks);

    stp.Stop();


    Console.WriteLine($"Finished writing {amountToInsert} items in {stp.Elapsed}.");
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
public class Model
{
    public string id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public string Photo { get; set; }
    public IList<Product> Products { get; set; }
}

public class Product
{
    public string id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public decimal? Weight { get; set; }
    public decimal ListPrice { get; set; }
    public string Photo { get; set; }
}