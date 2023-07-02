using Microsoft.Identity.Client;

const string clientId = "bf25271b-fd76-452e-9f69-15812d6ee8b2";
const string tenantId = "4fd96409-78dc-4e07-9b56-8095f2a1f2a5";

var app = PublicClientApplicationBuilder
.Create(clientId)
.WithAuthority(AzureCloudInstance.AzurePublic,tenantId)
.WithRedirectUri("http://localhost")
.Build();

string[] scopes = { "user.read" };

AuthenticationResult result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();

Console.WriteLine($"Token: {result.AccessToken}");

