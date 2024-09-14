using JournalViewer.Domain.Bootstrap;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbElementTag : NotifiableEntityBase<DbElementTag>
{
    public Guid Id { get; set; }
    public Guid ElementId { get; set; }
    public Guid TagId { get; set; }
    public int? OrderIndex { get; set; }

    public override TKey GetKey<TKey>(DbElementTag model)
    {
        return (TKey)(object)model.Id;
    }

    public override Task<string> PrepareNotificationAsync(DbElementTag result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
