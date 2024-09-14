using System.Collections.Concurrent;
using System.Reflection;

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

    private static readonly ConcurrentDictionary<Type, MethodInfo?> MethodCache = new();

    public IEnumerable<IEntityInterceptor?> GetInterceptors(Subject subject, Type entityType)
    {
        var method = MethodCache.GetOrAdd(entityType, et =>
        {
            return typeof(EntityInterceptorFactoryBase<TContext>)
                .GetMethod(nameof(GetUnderliningInterceptors), [typeof(Subject)])?
                .MakeGenericMethod(ChangeType(et), et);
        });

        if (method != null)
        {
            var obj = method.Invoke(this, [subject]);
            if (obj != null)
            {
                return (IEnumerable<IEntityInterceptor>)obj;
            }
        }

        throw new InvalidOperationException($"Could not find method 'GetInterceptor' for entity type {entityType.Name}");
    }


    [Obsolete("This is only to be used internally in reflection, it should not be used in production code")]
    public IEnumerable<IEntityInterceptor<TContext, TBaseEntity>?> GetUnderliningInterceptors<TBaseEntity, TEntity>(Subject subject)
    {
        if (factory.TryGetValue(subject, out var value))
        {
            var appliedInterceptors = value.Select((type) =>
            {
                var result = type(typeof(TEntity));
                return (IEntityInterceptor<TContext, TBaseEntity>?)result;
            });

            return appliedInterceptors.All(a => a == null) ? [] : appliedInterceptors;
        }

        return [];
    }

    public IEnumerable<IEntityInterceptor<TContext, TEntity>?> GetInterceptors<TEntity>(Subject subject)
    {
       if(factory.TryGetValue(subject, out var value))
       {
            var appliedInterceptors = value.Select((type) =>
            {
                var result = type(typeof(TEntity));
                return (IEntityInterceptor<TContext, TEntity>?)result;
            });

            return appliedInterceptors.All(a => a == null) ? [] : appliedInterceptors;
       }

       return [];
    }
}
