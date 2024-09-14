using System.Collections.Concurrent;

namespace JournalViewer.Domain.TypeCache
{
    public interface ITypeCacheProvider
    {
        ITypeCache this[Type type] { get; }
        ITypeCache<T> Get<T>();
    }

    public class TypeCacheProvider : ITypeCacheProvider
    {
        private static readonly Lazy<TypeCacheProvider> _provider =
            new(() => new TypeCacheProvider());
        private static ITypeCacheProvider Instance = _provider.Value;
        private readonly ConcurrentDictionary<Type, ITypeCache> _typeCacheDictionary;

        public TypeCacheProvider()
        {
            _typeCacheDictionary = new ConcurrentDictionary<Type, ITypeCache>();
        }

        public ITypeCache this[Type type]
        {
            get => _typeCacheDictionary.GetOrAdd(type, new TypeCache(type));
        }

        public ITypeCache<T> Get<T>()
        {
            var typeCache = this[typeof(T)] as TypeCache ?? throw new NullReferenceException();

            if(typeCache is TypeCache<T> genericTypeCache)
            {
                return genericTypeCache;
            }

            genericTypeCache = new TypeCache<T>(typeCache._properties, typeCache._attributes);

            if(!_typeCacheDictionary.TryUpdate(typeof(T), genericTypeCache, typeCache))
            {
                throw new InvalidOperationException("Unable to update cache");
            }

            return genericTypeCache;
        }
    }
}
