using System.Collections.Concurrent;

namespace JournalViewer.Domain;

public abstract class EntityInterceptorFactoryBase<TContext> : IEntityInterceptorFactory<TContext>
{
    private readonly ConcurrentDictionary<Subject, List<Func<Type,IEntityInterceptor?>>> factory = [];

    protected IEntityInterceptorFactory<TContext> AddSubjectInterceptor(Subject subject, Func<Type, IEntityInterceptor?> entityInterceptor)
    {
        factory.AddOrUpdate(subject, (s) => [], (s, l) => { 
            l.Add(entityInterceptor); 
            return l; });
        return this;
    }

    public IEnumerable<IEntityInterceptor?> GetInterceptors(Subject subject, Type entityType)
    {
        var method = typeof(EntityInterceptorFactoryBase<TContext>)
            .GetMethod(nameof(GetInterceptors), [typeof(Subject)])?
            .MakeGenericMethod(entityType);

        if (method != null)
        {
            var obj = method.Invoke(this, [subject]);
            if (obj != null)
            {
                return (IEnumerable<IEntityInterceptor>)obj;
            }
        }
        // Call the method and return the result

        throw new InvalidOperationException($"Could not find method 'GetInterceptor' for entity type {entityType.Name}");
    }

    public IEnumerable<IEntityInterceptor<TContext, TEntity>?> GetInterceptors<TEntity>(Subject subject)
    {
        return factory.TryGetValue(subject, out var value)
            ? value.Select(i => (IEntityInterceptor<TContext, TEntity>?)i(typeof(TEntity)))
            : [];
    }
}
