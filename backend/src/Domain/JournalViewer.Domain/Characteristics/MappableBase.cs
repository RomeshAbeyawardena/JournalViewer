using JournalViewer.Domain.TypeCache;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace JournalViewer.Domain.Characteristics;

public abstract class MappableBase<T> : IMappable<T>
{
    public TDestination MapTo<TDestination>(T source, ITypeCacheProvider? typeCache = null)
    {
        typeCache ??= TypeCacheProvider.Instance;
        var destination = Activator.CreateInstance<TDestination>();
        var sourceProperties = typeCache.Get<T>().Properties;
        var destinationProperties = typeCache.Get<TDestination>().Properties;

        foreach (var sourceProperty in sourceProperties)
        {
            if (!sourceProperty.CanRead)
            {
                continue;
            }

            var destinationProperty = destinationProperties.FirstOrDefault(p => p.Name == sourceProperty.Name);

            var sourcePropertyValue = sourceProperty.GetValue(source);
            if (sourcePropertyValue == null
                || destinationProperty == null
                || !destinationProperty.CanWrite
                || destinationProperty.PropertyType != sourceProperty.PropertyType
                || sourceProperty.GetCustomAttribute<NotMappedAttribute>() != null
                )
            {
                continue;
            }

            destinationProperty.SetValue(destination, sourcePropertyValue);
        }

        return destination;
    }
}
