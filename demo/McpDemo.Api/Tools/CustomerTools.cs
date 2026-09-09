using System.ComponentModel;
using McpDemo.Core;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace McpDemo.Api.Tools;

[McpServerToolType]
public sealed class CustomerTools(IQueryHandler<GetCustomerQuery, Customer?> getCustomer)
{
    [McpServerTool(Name = "GetCustomer", Title = "Pobierz klienta", ReadOnly = true, OpenWorld = false)]
    [Authorize(Roles = "User,Admin")]
    [Description("Zwraca dane kontaktowe klienta na podstawie jego identyfikatora.")]
    public async Task<Customer> GetCustomer([Description("Identyfikator klienta, na przykład C-001")] string id, CancellationToken cancellationToken)
        => await getCustomer.Handle(new(id), cancellationToken)
           ?? throw new McpException($"Customer '{id}' was not found. Known demo customers are C-001 and C-002.");
}
