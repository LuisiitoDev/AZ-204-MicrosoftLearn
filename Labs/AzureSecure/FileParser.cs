using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Storage.Blobs;

namespace AzureSecure
{
    public static class FileParser
    {
        /* Append an attribute to the Run method of type FunctionNameAttribute
         that has its name parameter set to a value of FileParser */
        [FunctionName("FileParser")]
        public static async Task<IActionResult> Run(
        /* Append an attribute to the request parameter of type  HttpTriggerAttribute
          that has its methods parameter array set to a single value of GET */
            [HttpTrigger("GET")] HttpRequest req)
        {
            /* Retrieve the value of the StorageConnectionString application setting by
             using the Environment.GetEnvironmentVariable method and to store the result
             in a string variable named connectionString */
            string connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");


         /* Create a new instance of the BlobClient class by passing in your 
            connectionString variable, a  "drop" string value, and a 
            "records.json" string value to the constructor */
         BlobClient blob = new BlobClient(connectionString, "drop", "records.json");

         /* Use the BlobClient.DownloadAsync method to download the contents of 
            the referenced blob asynchronously, and then store the result in 
            a variable named "response" */
         var response = await blob.DownloadAsync();

          /* Return the value of the various content stored in the content 
             variable by using the FileStreamResult class constructor */
         return new FileStreamResult(response?.Value?.Content, response?.Value?.ContentType);
        }
    }
}
