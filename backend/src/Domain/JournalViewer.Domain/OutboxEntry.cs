namespace JournalViewer.Domain;

public class OutboxEntry
{ 
    public Guid Id { get; set; }
    public required string EntityId { get; set; }
    public NotificationType NotificationType { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? LastProcessedDate { get; set; }
    public string LastError { get; set; } = string.Empty;
    public DateTimeOffset? CompletionDate { get; set; }
}
