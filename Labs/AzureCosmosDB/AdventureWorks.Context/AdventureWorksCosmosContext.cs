using AdventureWorks.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks.Context;

public class AdventureWorksProductContext : IAdventureWorksProductContext
{

    private readonly Container container;

    public AdventureWorksProductContext(string connectionString, string dataBase = "Retail", string container = "Online")
    {
        this.container = new CosmosClient(connectionString)
        .GetDatabase(dataBase)
        .GetContainer(container);

    }

    public async Task<Model> FindModelAsync(Guid id)
    {
        var iterator = this.container.GetItemLinqQueryable<Model>()
        .Where(m => m.id == id).ToFeedIterator<Model>();

        var matches = new List<Model>();
        while (iterator.HasMoreResults)
        {
            var next = await iterator.ReadNextAsync();
            matches.AddRange(next);
        }
        return matches.SingleOrDefault();
    }

    public async Task<Product> FindProductAsync(Guid id)
    {
         /* Run an SQL query, get the query result iterator, iterate over the result set,
            and then return the single item in the result set */
         string query = $@"SELECT VALUE products
                     FROM models
                     JOIN products in models.Products
                     WHERE products.id = '{id}'";
         var iterator = this.container.GetItemQueryIterator<Product>(query);
         List<Product> matches = new List<Product>();
         while (iterator.HasMoreResults)
         {
             var next = await iterator.ReadNextAsync();
             matches.AddRange(next);
         }

         return matches.SingleOrDefault();
    }

    public async Task<List<Model>> GetModelsAsync()
    {
         /* Run an SQL query, get the query result iterator, iterate over the result set,
             and then return the union of all results */
         string query = $@"SELECT * FROM items";
         var iterator = this.container.GetItemQueryIterator<Model>(query);
         List<Model> matches = new List<Model>();
         while (iterator.HasMoreResults)
         {
             var next = await iterator.ReadNextAsync();
             matches.AddRange(next);
         }

         return matches;
    }
}