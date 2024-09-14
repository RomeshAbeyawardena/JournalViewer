namespace JournalViewer.Infrastructure.SqlServer;

public class JournalViewerEntityRepositoryBase<T>(JournalViewDbContext journalViewDbContext)
    : EntityFrameworkRepositoryBase<JournalViewDbContext,T>(journalViewDbContext)
    where T : class
{
}
