using System.Linq.Expressions;

namespace JournalViewer.Domain.Features.Categories
{
    internal interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategories(Expression<Func<Category, bool>> filter);
    }
}
