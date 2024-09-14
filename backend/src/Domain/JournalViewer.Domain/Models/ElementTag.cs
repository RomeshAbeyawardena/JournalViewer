namespace JournalViewer.Domain.Models;

public class ElementTag
{
    public Guid Id { get; set; }
    public Guid ElementId { get; set; }
    public Guid TagId { get; set; }
    public int? OrderIndex { get; set; }
}