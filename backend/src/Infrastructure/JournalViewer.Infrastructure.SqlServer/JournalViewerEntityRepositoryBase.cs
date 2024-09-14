namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewerEntityRepositoryBase<TDb, T>(JournalViewDbContext journalViewDbContext)
    : EntityFrameworkRepositoryBase<JournalViewDbContext, TDb, T>(journalViewDbContext)
    where TDb : class
{
}
