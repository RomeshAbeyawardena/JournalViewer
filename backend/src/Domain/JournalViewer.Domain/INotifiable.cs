namespace JournalViewer.Domain;

public interface INotifiableEntity
{
    string? GetKey(object model);
    Task<string> PrepareNotificationAsync(object result, NotificationType notificationType, 
        CancellationToken cancellationToken);
}

public interface INotifiableEntity<T> : INotifiableEntity
{
    TKey GetKey<TKey>(T model);
    
    Task<string> PrepareNotificationAsync(T result, NotificationType notificationType, 
        CancellationToken cancellationToken);

}
