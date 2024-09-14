using System.Linq.Expressions;

namespace JournalViewer.Domain.Features.Categories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategories(CategoryFilter filter);
}
