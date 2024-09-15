namespace JournalViewer.Domain.Features.Categories;

public class CategoryFilter
{
    public Guid Id { get; set; }
    public string? NameContains { get; set; }
}
