namespace JournalViewer.Domain.Extensions;

public static class ObjectExtensions
{
    public static bool IsNotifiable(this object value, out INotifiableEntity notifiableEntity)
    {
        notifiableEntity = null;
        var type = value.GetType();
        var baseType = type.BaseType ?? throw new InvalidCastException();
        var genericType = typeof(INotifiableEntity<>).MakeGenericType(type);
        
        if (baseType.IsAssignableFrom(genericType))
        {
            notifiableEntity = (INotifiableEntity)value;
            return true;
        }

        return false;
    }
}
