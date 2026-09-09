using McpDemo.Core;

namespace McpDemo.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoCqrs(this IServiceCollection services) => services
        .AddSingleton<DemoDataStore>()
        .AddSingleton<IQueryHandler<GetOrderQuery, Order?>, GetOrderHandler>()
        .AddSingleton<IQueryHandler<GetCustomerQuery, Customer?>, GetCustomerHandler>()
        .AddSingleton<IQueryHandler<GetInvoiceQuery, Invoice?>, GetInvoiceHandler>()
        .AddSingleton<IQueryHandler<GetSupportTicketsQuery, IReadOnlyCollection<SupportTicket>>, GetSupportTicketsHandler>()
        .AddSingleton<ICommandHandler<CancelOrderCommand, Order?>, CancelOrderHandler>()
        .AddSingleton<ICommandHandler<CreateSupportTicketCommand, SupportTicket>, CreateSupportTicketHandler>();
}
