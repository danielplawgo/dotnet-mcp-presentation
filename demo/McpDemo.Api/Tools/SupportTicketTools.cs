using System.ComponentModel;
using McpDemo.Core;
using Microsoft.AspNetCore.Authorization;
using ModelContextProtocol;
using ModelContextProtocol.Server;

namespace McpDemo.Api.Tools;

[McpServerToolType]
public sealed class SupportTicketTools(
    ICommandHandler<CreateSupportTicketCommand, SupportTicket> createTicket,
    ILogger<SupportTicketTools> logger)
{
    // Potwierdzenie użytkownika należy do hosta, nie do argumentu wypełnianego przez model.
    // Serwer deklaruje intencję adnotacjami, waliduje wejście i zapisuje audyt.
    [McpServerTool(Name = "CreateSupportTicket", Title = "Utwórz zgłoszenie", ReadOnly = false, Destructive = false, Idempotent = false, OpenWorld = false)]
    [Authorize(Roles = "User,Admin")]
    [Description("Tworzy zgłoszenie serwisowe dla klienta. Operacja zapisuje dane — host powinien poprosić użytkownika o potwierdzenie przed wywołaniem.")]
    public async Task<SupportTicket> CreateSupportTicket(
        [Description("Identyfikator klienta, na przykład C-001")] string customerId,
        [Description("Krótkie podsumowanie problemu, 5-120 znaków")] string subject,
        [Description("Dowody i oczekiwane rozwiązanie")] string description,
        CancellationToken cancellationToken)
    {
        if (subject.Length is < 5 or > 120)
            throw new McpException($"Subject must contain 5-120 characters, but got {subject.Length}. Shorten or expand the subject and call again.");

        var ticket = await createTicket.Handle(new(customerId, subject, description), cancellationToken);
        logger.LogWarning("AUDIT tool=CreateSupportTicket ticket={TicketId} customer={CustomerId}", ticket.Id, customerId);
        return ticket;
    }
}
