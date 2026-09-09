namespace McpDemo.Core;

public sealed record GetOrderQuery(string Number) : IQuery<Order?>;
public sealed record GetCustomerQuery(string Id) : IQuery<Customer?>;
public sealed record GetInvoiceQuery(string Number) : IQuery<Invoice?>;
public sealed record GetSupportTicketsQuery : IQuery<IReadOnlyCollection<SupportTicket>>;

public sealed record CancelOrderCommand(string Number) : ICommand<Order?>;
public sealed record CreateSupportTicketCommand(
    string CustomerId,
    string Subject,
    string Description) : ICommand<SupportTicket>;
