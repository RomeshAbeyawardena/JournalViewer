using System.Collections.Concurrent;

namespace JournalViewer.Domain.Extensions
{
    public static class ObjectExtensions
    {
        // Lazy initialization of ConcurrentDictionary for caching the types
        private static readonly Lazy<ConcurrentDictionary<Type, Type>> GenericTypesCache =
            new(() => new());

        public static bool HasCreatedTimestamp(this object value, out ICreatedTimestamp? createdTimestamp)
        {
            createdTimestamp = null;
            if (value is ICreatedTimestamp created)
            {
                createdTimestamp = created;
                return true;
            }

            return false;
        }

        public static bool IsNotifiable(this object value, out INotifiableEntity? notifiableEntity)
        {
            notifiableEntity = null;
            var type = value.GetType();

            // Cache or retrieve the type that implements INotifiableEntity<>
            var genericType = GenericTypesCache.Value.GetOrAdd(type,
                t => typeof(INotifiableEntity<>).MakeGenericType(t));

            // Check if the provided type implements the cached generic interface
            if (genericType.IsAssignableFrom(type))
            {
                // Safely cast to INotifiableEntity if applicable
                notifiableEntity = value as INotifiableEntity;
                return notifiableEntity != null;
            }

            return false;
        }
    }
}
