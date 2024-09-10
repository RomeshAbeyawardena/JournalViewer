namespace JournalViewer.Domain;

public abstract class NotifiableEntityBase<T> : INotifiableEntity<T>
{
    public abstract TKey GetKey<TKey>(T model);

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
