using JournalViewer.Domain.TypeCache;

namespace JournalViewer.Domain;

public interface IMappable<T>
{
    TDestination MapTo<TDestination>(T source);
}

public abstract class MappableBase<T>(ITypeCacheProvider typeCache) : IMappable<T>
{
    public TDestination MapTo<TDestination>(T source)
    {
        var destination = Activator.CreateInstance<TDestination>();
        var sourceProperties = typeCache.Get<T>().Properties;
        var destinationProperties = typeCache.Get<TDestination>().Properties;

        foreach(var sourceProperty in sourceProperties)
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
                || destinationProperty.PropertyType != sourceProperty.PropertyType)
            {
                continue;
            }

            destinationProperty.SetValue(destination, sourcePropertyValue);
        }

        return destination;
    }
}
