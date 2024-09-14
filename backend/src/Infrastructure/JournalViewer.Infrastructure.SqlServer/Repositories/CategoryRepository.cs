using JournalViewer.Domain.Features.Categories;
using JournalViewer.Domain.TypeCache;
using JournalViewer.Infrastructure.Domain.Models;

namespace JournalViewer.Infrastructure.SqlServer.Repositories;

public class CategoryRepository(JournalViewDbContext dbContext, ITypeCacheProvider typeCacheProvider)
    : JournalViewerEntityRepositoryBase<DbCategory, Category>(dbContext,
        typeCacheProvider), ICategoryRepository
{
    public Task<IEnumerable<Category>> GetCategories(CategoryFilter categoryFilter)
    {
        return Task.FromResult(Array.Empty<Category>().AsEnumerable());
    }
}
