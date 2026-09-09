namespace McpDemo.Core;

public sealed record Order(string Number, string CustomerId, string Status, DateTimeOffset CreatedAt, string? Note = null);
public sealed record Customer(string Id, string Name, string Email);
public sealed record Invoice(string Number, string CustomerId, decimal Amount, string Status, string ReminderStatus);
public sealed record SupportTicket(Guid Id, string CustomerId, string Subject, string Description, string Status, DateTimeOffset CreatedAt);
