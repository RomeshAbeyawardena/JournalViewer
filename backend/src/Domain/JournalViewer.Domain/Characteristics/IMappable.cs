using JournalViewer.Domain.TypeCache;

namespace JournalViewer.Domain.Characteristics;

public interface IMappable<T>
{
    TDestination MapTo<TDestination>(T source, ITypeCacheProvider? typeCache = null);
}
