using System.Reflection;

namespace JournalViewer.Domain.TypeCache
{
    public interface ITypeCache
    {
        Type Type { get; }
        IEnumerable<PropertyInfo> Properties { get; }
        IEnumerable<Attribute> Attributes { get; }
    }

    public interface ITypeCache<T> : ITypeCache
    {

    }
}
