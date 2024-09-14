namespace JournalViewer.Domain.Models;

public class CategoryTag
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TagId { get; set; }
    public int? OrderIndex { get; set; }
}
