using McpDemo.Core;

namespace McpDemo.Api.Endpoints;

public static class SupportTicketEndpoints
{
    public static IEndpointRouteBuilder MapSupportTicketEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/tickets", CreateSupportTicket);
        endpoints.MapGet("/api/tickets", GetSupportTickets);
        return endpoints;
    }

    private static async Task<IResult> CreateSupportTicket(
        CreateSupportTicketCommand command,
        ICommandHandler<CreateSupportTicketCommand, SupportTicket> handler,
        CancellationToken cancellationToken)
        => Results.Created("/api/tickets", await handler.Handle(command, cancellationToken));

    private static async Task<IResult> GetSupportTickets(
        IQueryHandler<GetSupportTicketsQuery, IReadOnlyCollection<SupportTicket>> handler,
        CancellationToken cancellationToken)
        => Results.Ok(await handler.Handle(new(), cancellationToken));
}
