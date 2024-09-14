namespace JournalViewer.Infrastructure.Domain.Models;

public class DbCategoryTag
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TagId { get; set; }
}
