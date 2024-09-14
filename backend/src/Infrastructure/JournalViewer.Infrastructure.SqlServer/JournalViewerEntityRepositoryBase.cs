using JournalViewer.Domain.TypeCache;

namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewerEntityRepositoryBase<TDb, T>(JournalViewDbContext journalViewDbContext,
    ITypeCacheProvider typeCacheProvider)
    : EntityFrameworkRepositoryBase<JournalViewDbContext, TDb, T>(journalViewDbContext,
        typeCacheProvider)
    where TDb : class
{
}
