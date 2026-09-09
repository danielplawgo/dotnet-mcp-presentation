using ModelContextProtocol.Client;

const string apiKey = "demo-user-key"; // Zmień na "demo-admin-key", aby zobaczyć również CancelOrder.
var endpoint = new Uri("http://localhost:5055/mcp");
var role = apiKey == "demo-admin-key" ? "Admin" : "User";

var transport = new HttpClientTransport(new HttpClientTransportOptions
{
    Endpoint = endpoint,
    TransportMode = HttpTransportMode.StreamableHttp,
    AdditionalHeaders = new Dictionary<string, string> { ["X-Api-Key"] = apiKey }
});

await using var client = await McpClient.CreateAsync(transport);
var tools = await client.ListToolsAsync();

Console.WriteLine($"Dostępne narzędzia dla roli {role}:");
foreach (var tool in tools)
{
    Console.WriteLine($"  - {tool.Name}");
    Console.WriteLine($"    {tool.Description}");
}
