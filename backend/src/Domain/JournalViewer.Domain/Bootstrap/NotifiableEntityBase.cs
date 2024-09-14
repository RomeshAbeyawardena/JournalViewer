using JournalViewer.Domain.Characteristics;

namespace JournalViewer.Domain.Bootstrap;

public abstract class NotifiableEntityBase<T> : MappableBase<T>, INotifiableEntity<T>
{
    public virtual TKey GetKey<TKey>(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        if (model is not IIdentifier identifier || identifier.Id == null)
        {
            throw new NullReferenceException();
        }

        return (TKey)(object)identifier.Id;
    }

    public abstract Task<string> PrepareNotificationAsync(T result, NotificationType notificationType, CancellationToken cancellationToken);

    public NotificationType NotificationType { get; private set; }

    Task<string> INotifiableEntity.PrepareNotificationAsync(object result,
        NotificationType notificationType, CancellationToken cancellationToken)
    {
        NotificationType = notificationType;
        return PrepareNotificationAsync((T)result, notificationType, cancellationToken);
    }

    string? INotifiableEntity.GetKey(object model)
    {
        return GetKey<T>((T)model)?.ToString();
    }
}
