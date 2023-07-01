using Azure.Identity;
using Microsoft.Graph;

var scopes = new[] { "https://graph.microsoft.com/.default" };
string tenantId = "";
string clientSecret = "";
string clientId = "";

var options = new TokenCredentialOptions()
{
    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
};

var clientSecretCredential = new ClientSecretCredential(
    tenantId, clientId, clientSecret, options);

var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

var users = await graphClient.Users.GetAsync();

foreach (var user in users!.Value!)
{
    Console.WriteLine(user.DisplayName + " , " + user.GivenName);
}

