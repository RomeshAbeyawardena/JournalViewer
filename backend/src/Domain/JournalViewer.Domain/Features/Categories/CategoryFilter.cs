namespace JournalViewer.Domain.Features.Categories;

public class CategoryFilter
{
    public Guid Id { get; set; }
    public bool? ShowAll { get; set; }
    public string? NameContains { get; set; }
}
