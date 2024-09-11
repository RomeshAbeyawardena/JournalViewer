
using JournalViewer.Domain;

namespace JournalViewer.Infrastructure.SqlServer;

internal class JournalViewerDbContextEntityInterceptorFactory : EntityInterceptorFactoryBase<JournalViewDbContext>
{
    
    public override IEntityInterceptor<JournalViewDbContext, TEntity> 
        GetInterceptor<TEntity>(Subject subject)
    {
        throw new NotImplementedException();
    }
}
