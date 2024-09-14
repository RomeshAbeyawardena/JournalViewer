using System.Reflection;

namespace JournalViewer.Domain.TypeCache
{
    public class TypeCache<T> : TypeCache, ITypeCache<T>
    {
        internal TypeCache(Lazy<IEnumerable<PropertyInfo>> properties,
            Lazy<IEnumerable<Attribute>> attributes)
            : base(typeof(T), properties, attributes)
        {

        }
    }

    public class TypeCache : ITypeCache
    {
        internal readonly Lazy<IEnumerable<PropertyInfo>> _properties;
        internal readonly Lazy<IEnumerable<Attribute>> _attributes;
        internal TypeCache(Type type,
            Lazy<IEnumerable<PropertyInfo>>? properties = null,
            Lazy<IEnumerable<Attribute>>? attributes = null)
        {
            Type = type;
            _properties = properties ?? new Lazy<IEnumerable<PropertyInfo>>(type.GetProperties);
            _attributes = attributes ?? new Lazy<IEnumerable<Attribute>>(type.GetCustomAttributes);
        }

        public Type Type { get; }
        public IEnumerable<PropertyInfo> Properties => _properties.Value;
        public IEnumerable<Attribute> Attributes => _attributes.Value;

        public override bool Equals(object? obj)
        {
          return obj is not null && obj is TypeCache typeCache && Equals(typeCache);
        }


        public bool Equals(TypeCache typeCache)
        {
            return typeCache.Type == Type
                && typeCache.Properties.SequenceEqual(Properties)
                && typeCache.Attributes.SequenceEqual(Attributes);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Type.GetHashCode();
                hash = (hash * 397) ^ Properties.Aggregate(0, (acc, prop) => acc ^ prop.GetHashCode());
                hash = (hash * 397) ^ Attributes.Aggregate(0, (acc, attr) => acc ^ attr.GetHashCode());
                return hash;
            }
        }

    }
}
