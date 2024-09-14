namespace JournalViewer.Domain.Models;

public class DbElement
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Description { get; set; }
}
