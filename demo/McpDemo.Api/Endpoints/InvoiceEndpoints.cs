using McpDemo.Core;

namespace McpDemo.Api.Endpoints;

public static class InvoiceEndpoints
{
    public static IEndpointRouteBuilder MapInvoiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/invoices/{number}", GetInvoice);
        return endpoints;
    }

    private static async Task<IResult> GetInvoice(
        string number,
        IQueryHandler<GetInvoiceQuery, Invoice?> handler,
        CancellationToken cancellationToken)
        => await handler.Handle(new(number), cancellationToken) is { } invoice
            ? Results.Ok(invoice)
            : Results.NotFound();
}
