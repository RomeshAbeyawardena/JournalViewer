using JournalViewer.Domain;
using JournalViewer.Domain.Features.Categories;
using JournalViewer.Domain.TypeCache;
using JournalViewer.Infrastructure.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JournalViewer.Infrastructure.SqlServer.Repositories;

public class CategoryRepository(JournalViewDbContext dbContext, ITypeCacheProvider typeCacheProvider)
    : JournalViewerEntityRepositoryBase<DbCategory, Category>(dbContext,
        typeCacheProvider), ICategoryRepository
{
    public async Task<IPagedList<Category>> GetCategories(IPagedRequest request, CategoryFilter categoryFilter, CancellationToken cancellationToken)
    {
        var entity = Entity.Value;
        var query = Query.Value;

        if(!categoryFilter.ShowAll.HasValue || !categoryFilter.ShowAll.Value)
        {
            query.And(c => !c.IsSuppressed);
        }

        if (!string.IsNullOrWhiteSpace(categoryFilter.NameContains))
        {
            query.And(c => c.Name.Contains(categoryFilter.NameContains));
        }

        var items = await entity.Where(query).AsPaged(request).ToPagedList(cancellationToken);

        return items.Select(i => i.MapTo<Category>(i));
    }
}
