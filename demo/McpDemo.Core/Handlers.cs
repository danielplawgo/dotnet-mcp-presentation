namespace McpDemo.Core;

public sealed class GetOrderHandler(DemoDataStore store) : IQueryHandler<GetOrderQuery, Order?>
{
    public Task<Order?> Handle(GetOrderQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        store.Orders.TryGetValue(query.Number, out var order);
        return Task.FromResult(order);
    }
}

public sealed class GetCustomerHandler(DemoDataStore store) : IQueryHandler<GetCustomerQuery, Customer?>
{
    public Task<Customer?> Handle(GetCustomerQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        store.Customers.TryGetValue(query.Id, out var customer);
        return Task.FromResult(customer);
    }
}

public sealed class GetInvoiceHandler(DemoDataStore store) : IQueryHandler<GetInvoiceQuery, Invoice?>
{
    public Task<Invoice?> Handle(GetInvoiceQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        store.Invoices.TryGetValue(query.Number, out var invoice);
        return Task.FromResult(invoice);
    }
}

public sealed class GetSupportTicketsHandler(DemoDataStore store)
    : IQueryHandler<GetSupportTicketsQuery, IReadOnlyCollection<SupportTicket>>
{
    public Task<IReadOnlyCollection<SupportTicket>> Handle(
        GetSupportTicketsQuery query,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        IReadOnlyCollection<SupportTicket> tickets = store.Tickets.Values
            .OrderBy(ticket => ticket.CreatedAt)
            .ToArray();
        return Task.FromResult(tickets);
    }
}

public sealed class CancelOrderHandler(DemoDataStore store) : ICommandHandler<CancelOrderCommand, Order?>
{
    public Task<Order?> Handle(CancelOrderCommand command, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!store.Orders.TryGetValue(command.Number, out var order))
        {
            return Task.FromResult<Order?>(null);
        }

        var cancelled = order with
        {
            Status = "Cancelled",
            Note = "Cancelled through an admin-only command."
        };
        store.Orders[command.Number] = cancelled;
        return Task.FromResult<Order?>(cancelled);
    }
}

public sealed class CreateSupportTicketHandler(DemoDataStore store)
    : ICommandHandler<CreateSupportTicketCommand, SupportTicket>
{
    public Task<SupportTicket> Handle(
        CreateSupportTicketCommand command,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var ticket = new SupportTicket(
            Guid.NewGuid(),
            command.CustomerId,
            command.Subject,
            command.Description,
            "Open",
            DateTimeOffset.UtcNow);
        store.Tickets[ticket.Id] = ticket;
        return Task.FromResult(ticket);
    }
}
