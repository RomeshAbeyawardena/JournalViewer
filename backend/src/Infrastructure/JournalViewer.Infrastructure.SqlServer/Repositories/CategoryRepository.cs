using JournalViewer.Domain;
using JournalViewer.Domain.Features.Categories;
using JournalViewer.Domain.TypeCache;
using JournalViewer.Infrastructure.Domain.Models;

namespace JournalViewer.Infrastructure.SqlServer.Repositories;

public class CategoryRepository(JournalViewDbContext dbContext, ITypeCacheProvider typeCacheProvider)
    : JournalViewerEntityRepositoryBase<DbCategory, Category>(dbContext,
        typeCacheProvider), ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetCategories(IPagedRequest request, CategoryFilter categoryFilter)
    {
        var query = Entity.Value;

    }
}
