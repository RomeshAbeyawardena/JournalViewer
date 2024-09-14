using System.Linq.Expressions;
using JournalViewer.Domain.Bootstrap;

namespace JournalViewer.Domain.Features.Categories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetCategories(CategoryFilter filter);
}
