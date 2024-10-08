﻿using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using JournalViewer.Domain.Extensions;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbElement : NotifiableEntityBase<DbElement>, 
    IIdentifier, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public virtual ICollection<DbElementTag> ElementTags { get; set; } = [];

    public override Task<string> PrepareNotificationAsync(DbElement result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        return result.PrepareAsJsonAsync(cancellationToken);
    }
}
