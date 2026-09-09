using McpDemo.Api;
using McpDemo.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDemoCqrs();
builder.Services
    .AddAuthentication(DemoAuthenticationHandler.SchemeName)
    .AddScheme<DemoAuthenticationOptions, DemoAuthenticationHandler>(DemoAuthenticationHandler.SchemeName, _ => { });
builder.Services.AddAuthorization();
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options => options.Stateless = true)
    .AddAuthorizationFilters()
    .WithToolsFromAssembly();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => Results.Ok(new { service = "McpDemo.Api", rest = "/api/orders/123", mcp = "/mcp", auth = "X-Api-Key: demo-user-key | demo-admin-key" }));
app.MapOrderEndpoints();
app.MapCustomerEndpoints();
app.MapInvoiceEndpoints();
app.MapSupportTicketEndpoints();
app.MapMcp("/mcp");
app.Run();

// Udostępnia punkt wejścia dla WebApplicationFactory w testach kontraktu narzędzi.
public partial class Program;
