using McpDemo.Core;

namespace McpDemo.Api.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/customers/{id}", GetCustomer);
        return endpoints;
    }

    private static async Task<IResult> GetCustomer(
        string id,
        IQueryHandler<GetCustomerQuery, Customer?> handler,
        CancellationToken cancellationToken)
        => await handler.Handle(new(id), cancellationToken) is { } customer
            ? Results.Ok(customer)
            : Results.NotFound();
}
