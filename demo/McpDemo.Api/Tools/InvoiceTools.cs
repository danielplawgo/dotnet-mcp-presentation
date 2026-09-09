using System.ComponentModel;
using McpDemo.Core;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace McpDemo.Api.Tools;

[McpServerToolType]
public sealed class InvoiceTools(IQueryHandler<GetInvoiceQuery, Invoice?> getInvoice)
{
    [McpServerTool(Name = "GetInvoice", Title = "Pobierz fakturę", ReadOnly = true, OpenWorld = false)]
    [Authorize(Roles = "User,Admin")]
    [Description("Zwraca szczegóły faktury, w tym kwotę, status płatności i status przypomnień.")]
    public async Task<Invoice> GetInvoice([Description("Numer faktury, na przykład INV-123")] string number, CancellationToken cancellationToken)
        => await getInvoice.Handle(new(number), cancellationToken)
           ?? throw new McpException($"Invoice '{number}' was not found. Known demo invoices are INV-123 and INV-456.");
}
