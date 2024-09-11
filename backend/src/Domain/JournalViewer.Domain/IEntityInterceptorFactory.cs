using System.Collections.Concurrent;

namespace JournalViewer.Domain;

public interface IEntityInterceptorFactory<TContext>
{
    IEntityInterceptor GetInterceptor(Subject subject, Type entityType);
    IEntityInterceptor<TContext, TEntity> GetInterceptor<TEntity>(Subject subject);
}

public abstract class EntityInterceptorFactoryBase<TContext> : IEntityInterceptorFactory<TContext>
{
    private readonly ConcurrentDictionary<Subject, List<IEntityInterceptor>> factory = [];

    protected IEntityInterceptorFactory<TContext> AddSubjectInterceptor(Subject subject, IEntityInterceptor entityInterceptor)
    {
        factory.AddOrUpdate(subject, (s) => [], (s, l) => { 
            l.Add(entityInterceptor); 
            return l; });
        return this;
    }

    public IEntityInterceptor GetInterceptor(Subject subject, Type entityType)
    {
        var method = typeof(EntityInterceptorFactoryBase<TContext>)
            .GetMethod(nameof(GetInterceptor), [typeof(Subject)])?
            .MakeGenericMethod(entityType);

        if (method != null)
        {
            var obj = method.Invoke(this, [subject]);
            if(obj != null)
            {
                return (IEntityInterceptor)obj;
            }
        }
        // Call the method and return the result
        
        throw new InvalidOperationException($"Could not find method 'GetInterceptor' for entity type {entityType.Name}");
    }

    public abstract IEntityInterceptor<TContext, TEntity> GetInterceptor<TEntity>(Subject subject);
}
