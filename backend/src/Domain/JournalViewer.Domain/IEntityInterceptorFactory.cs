namespace JournalViewer.Domain;

public interface IEntityInterceptorFactory<TContext>
{
    IEnumerable<IEntityInterceptor?> GetInterceptors(Subject subject, Type entityType);
    IEnumerable<IEntityInterceptor<TContext, TEntity>?> GetInterceptors<TEntity>(Subject subject);
}
