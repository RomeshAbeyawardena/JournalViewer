using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using JournalViewer.Domain.Extensions;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbTag : NotifiableEntityBase<DbTag>, IIdentifier, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OrderIndex { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public override Task<string> PrepareNotificationAsync(DbTag result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        return result.PrepareAsJsonAsync(cancellationToken);
    }
}
