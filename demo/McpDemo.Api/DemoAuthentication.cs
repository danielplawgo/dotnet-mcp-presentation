using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace McpDemo.Api;

public sealed class DemoAuthenticationOptions : AuthenticationSchemeOptions;

public sealed class DemoAuthenticationHandler(IOptionsMonitor<DemoAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : AuthenticationHandler<DemoAuthenticationOptions>(options, logger, encoder)
{
    public const string SchemeName = "DemoHeader";
    public const string HeaderName = "X-Api-Key";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Request.Headers[HeaderName].FirstOrDefault();
        var role = apiKey switch
        {
            "demo-user-key" => "User",
            "demo-admin-key" => "Admin",
            _ => null
        };
        if (role is null) return Task.FromResult(AuthenticateResult.NoResult());

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, $"demo-{role.ToLowerInvariant()}"),
            new Claim(ClaimTypes.Name, $"Demo {role}"),
            new Claim(ClaimTypes.Role, role)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName));
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName)));
    }
}
