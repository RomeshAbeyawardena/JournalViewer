namespace JournalViewer.Domain.Bootstrap;

public interface IEntityInterceptorFactory<TContext>
{
    Type ChangeType(Type type);
    IEnumerable<IEntityInterceptor?> GetInterceptors(Subject subject, Type entityType);
    IEnumerable<IEntityInterceptor<TContext, TEntity>?> GetInterceptors<TEntity>(Subject subject);
}
