using JournalViewer.Domain;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbElement : NotifiableEntityBase<DbElement>, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public override TKey GetKey<TKey>(DbElement model)
    {
        return (TKey)(object)model.Id;
    }

    public override Task<string> PrepareNotificationAsync(DbElement result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
