using System.ComponentModel;
using McpDemo.Core;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace McpDemo.Api.Tools;

[McpServerToolType]
public sealed class OrderTools(
    IQueryHandler<GetOrderQuery, Order?> getOrder,
    ICommandHandler<CancelOrderCommand, Order?> cancelOrder,
    ILogger<OrderTools> logger)
{
    [McpServerTool(Name = "GetOrder", Title = "Pobierz zamówienie", ReadOnly = true, OpenWorld = false)]
    [Authorize(Roles = "User,Admin")]
    [Description("Zwraca szczegóły zamówienia, jego status i notatkę diagnostyczną.")]
    public async Task<Order> GetOrder([Description("Numer zamówienia, na przykład 123")] string number, CancellationToken cancellationToken)
        => await getOrder.Handle(new(number), cancellationToken)
           ?? throw new McpException($"Order '{number}' was not found. Known demo orders are 123 and 456.");

    [McpServerTool(Name = "CancelOrder", Title = "Anuluj zamówienie", ReadOnly = false, Destructive = true, Idempotent = true, OpenWorld = false)]
    [Authorize(Roles = "Admin")]
    [Description("Anuluje istniejące zamówienie. Operacja jest destrukcyjna i wymaga roli Admin — host powinien poprosić użytkownika o potwierdzenie przed wywołaniem.")]
    public async Task<Order> CancelOrder([Description("Numer zamówienia do anulowania")] string number, CancellationToken cancellationToken)
    {
        var cancelled = await cancelOrder.Handle(new(number), cancellationToken)
                        ?? throw new McpException($"Order '{number}' was not found. Known demo orders are 123 and 456.");

        logger.LogWarning("AUDIT tool=CancelOrder order={OrderNumber} customer={CustomerId}", cancelled.Number, cancelled.CustomerId);
        return cancelled;
    }
}
