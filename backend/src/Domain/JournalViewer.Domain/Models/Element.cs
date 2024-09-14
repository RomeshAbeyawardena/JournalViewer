namespace JournalViewer.Domain.Models;

public class Element
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Description { get; set; }

    public virtual Category Category { get; set; } = new();
}
