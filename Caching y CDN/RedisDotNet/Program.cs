using StackExchange.Redis;


string CacheConnection = "";

var lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
{
    return ConnectionMultiplexer.Connect(CacheConnection);
});


IDatabase cache = lazyConnection.Value.GetDatabase();

Console.WriteLine($"Reading Cache: {cache.StringGet("Session33")}");
Console.WriteLine($"Writing Cache: {cache.StringSet("Session33", "Hello world from cache!")}");
Console.WriteLine($"Reading Cache: {cache.StringGet("Session33")}");
cache.KeyExpire("Session33", DateTime.Now.AddMinutes(1));


lazyConnection.Value.Dispose();
