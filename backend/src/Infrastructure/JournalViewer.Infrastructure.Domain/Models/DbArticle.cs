﻿using JournalViewer.Domain.Bootstrap;
using JournalViewer.Domain.Characteristics;
using JournalViewer.Domain.Extensions;
using System.Text.Json.Serialization;

namespace JournalViewer.Infrastructure.Domain.Models;

public class DbArticle : NotifiableEntityBase<DbArticle>, 
    IIdentifier, ICreatedTimestamp, IModifiedTimestamp
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    //Do not include this in the notification - it may be too large, consumers will be able to query the API using the Key deduced from GetKey or EF Db Identity
    [JsonIgnore]
    public string? Content { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    public override Task<string> PrepareNotificationAsync(DbArticle result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        return this.PrepareAsJsonAsync(cancellationToken);
    }
}
