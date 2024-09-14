using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using JournalViewer.Domain.Extensions;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbCategory : NotifiableEntityBase<DbCategory>, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public virtual ICollection<DbCategoryTag> CategoryTags { get; set; } = [];

    public override TKey GetKey<TKey>(DbCategory model)
    {
        return (TKey)(object)model.Id;
    }

    public override Task<string> PrepareNotificationAsync(DbCategory result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        return result.PrepareAsJsonAsync(cancellationToken);
    }
}
