using JournalViewer.Domain.TypeCache;

namespace JournalViewer.Domain.Features.Categories;

public class Category : MappableBase<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
