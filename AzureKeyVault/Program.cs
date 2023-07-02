using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

string keyVaultUrl = "";

var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());


string secretName = "mysecret";

KeyVaultSecret secret = await client.GetSecretAsync(secretName);

Console.WriteLine("The value of the secret is: " + secret.Value);


