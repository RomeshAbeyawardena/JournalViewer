namespace JournalViewer.Domain.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
}
