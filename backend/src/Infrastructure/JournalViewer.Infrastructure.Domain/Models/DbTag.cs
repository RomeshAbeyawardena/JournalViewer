using JournalViewer.Domain;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbTag : NotifiableEntityBase<DbTag>, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public override TKey GetKey<TKey>(DbTag model)
    {
        return (TKey)(object)model.Id;
    }

    public override Task<string> PrepareNotificationAsync(DbTag result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
