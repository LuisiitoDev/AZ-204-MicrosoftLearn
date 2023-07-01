using Microsoft.Azure.Cosmos;

string endpointuri = "";
string primaryKey = "";

string databaseid = "az204DataBase";
string containerid = "az204Container";

CosmosClient cosmosClient = new CosmosClient(endpointuri, primaryKey);


// Creating the database
Database db = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseid);
Console.WriteLine("Database was created: " + db.Id);


// Creating the container
Container container = await db.CreateContainerIfNotExistsAsync(containerid,"/LastName");
Console.WriteLine("Container was created: " + container.Id);



