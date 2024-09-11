namespace JournalViewer.Domain;

public interface IEntityInterceptorFactory<TContext>
{
    IEntityInterceptor? GetInterceptor(Subject subject, Type entityType);
    IEntityInterceptor<TContext, TEntity> GetInterceptor<TEntity>(Subject subject);
}

public abstract class EntityInterceptorFactoryBase<TContext> : IEntityInterceptorFactory<TContext>
{
    public IEntityInterceptor? GetInterceptor(Subject subject, Type entityType)
    {
        var method = typeof(EntityInterceptorFactoryBase<TContext>)
            .GetMethod(nameof(GetInterceptor), new Type[] { typeof(Subject) })?
            .MakeGenericMethod(entityType);

        if (method == null)
        {
            throw new InvalidOperationException($"Could not find method 'GetInterceptor' for entity type {entityType.Name}");
        }
        // Call the method and return the result
        return (IEntityInterceptor)method.Invoke(this, new object[] { subject });
    }

    public abstract IEntityInterceptor<TContext, TEntity> GetInterceptor<TEntity>(Subject subject);
}
