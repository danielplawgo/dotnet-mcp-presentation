using Microsoft.AspNetCore.Mvc.Testing;
using ModelContextProtocol.Client;

namespace McpDemo.Tests;

/// <summary>
/// Uruchamia całe API w pamięci i podpina do niego prawdziwego klienta MCP.
/// Testy sprawdzają kontrakt protokołu, a nie reguły biznesowe — te są testowane na handlerach.
/// </summary>
public sealed class McpDemoApplication : WebApplicationFactory<Program>
{
    public async Task<McpClient> ConnectAsync(string apiKey, CancellationToken cancellationToken = default)
    {
        var httpClient = CreateClient();
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = new Uri("http://localhost/mcp"),
                TransportMode = HttpTransportMode.StreamableHttp,
                AdditionalHeaders = new Dictionary<string, string> { ["X-Api-Key"] = apiKey }
            },
            httpClient,
            loggerFactory: null,
            ownsHttpClient: true);

        return await McpClient.CreateAsync(transport, cancellationToken: cancellationToken);
    }
}
