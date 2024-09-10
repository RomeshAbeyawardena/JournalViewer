namespace JournalViewer.Domain.Extensions;

public static class ObjectExtensions
{
    public static bool IsNotifiable(this object value)
    {
        var type = value.GetType();
        var baseType = type.BaseType ?? throw new InvalidCastException();
        var genericType = typeof(INotifiableEntity<>).MakeGenericType(type);
        return baseType.IsAssignableFrom(genericType);
    }
}
