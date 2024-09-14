using System.Collections.Concurrent;

namespace JournalViewer.Domain;

public abstract class EntityInterceptorFactoryBase<TContext> : IEntityInterceptorFactory<TContext>
{
    public static IEntityInterceptor? GetFromServiceProviderFactory(
        Type type,
        Func<Type, object?> factory)
    {
        var interceptor = factory(type);
        
        if(interceptor == null) 
        {
            return null;
        }

        return (IEntityInterceptor)interceptor;
    }

    private readonly ConcurrentDictionary<Subject, List<Func<Type,IEntityInterceptor?>>> factory = [];

    protected IEntityInterceptorFactory<TContext> Add(Subject subject, Func<Type, IEntityInterceptor?> entityInterceptor)
    {
        factory.AddOrUpdate(subject, (s) => [entityInterceptor], (s, l) => { 
            l.Add(entityInterceptor); 
            return l; });
        return this;
    }

    public virtual Type ChangeType(Type type)
    {
        return type;
    }

    public IEnumerable<IEntityInterceptor?> GetInterceptors(Subject subject, Type entityType)
    {
        var method = typeof(EntityInterceptorFactoryBase<TContext>)
            .GetMethod(nameof(GetUnderliningInterceptors), [typeof(Subject)])?
            .MakeGenericMethod(ChangeType(entityType), entityType);

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

    public IEnumerable<IEntityInterceptor<TContext, TBaseEntity>?> GetUnderliningInterceptors<TBaseEntity, TEntity>(Subject subject)
    {
        if (factory.TryGetValue(subject, out var value))
        {
            return value.Select((type) =>
            {
                var result = type(typeof(TEntity));
                return (IEntityInterceptor<TContext, TBaseEntity>?)result;
            });
        }

        return [];
    }

    public IEnumerable<IEntityInterceptor<TContext, TEntity>?> GetInterceptors<TEntity>(Subject subject)
    {
       if(factory.TryGetValue(subject, out var value))
       {
            return value.Select((type) =>
            {
                var result = type(typeof(TEntity));
                return (IEntityInterceptor<TContext, TEntity>?)result;
            });
       }

       return [];
    }
}
