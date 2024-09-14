namespace JournalViewer.Domain;

public interface IMappable<T, TDestination>
{
    TDestination MapTo(T source);
}
