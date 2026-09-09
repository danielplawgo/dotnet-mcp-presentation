using McpDemo.Core;

namespace McpDemo.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/orders/{number}", GetOrder);
        return endpoints;
    }

    private static async Task<IResult> GetOrder(
        string number,
        IQueryHandler<GetOrderQuery, Order?> handler,
        CancellationToken cancellationToken)
        => await handler.Handle(new(number), cancellationToken) is { } order
            ? Results.Ok(order)
            : Results.NotFound();
}
