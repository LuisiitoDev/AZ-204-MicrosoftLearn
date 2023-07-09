using Microsoft.Azure.Management.Cdn.Fluent;
using Microsoft.Rest;

CdnManagementClient cdn = new CdnManagementClient(new TokenCredentials("access-token")){ SubscriptionId = "suscription-id" };