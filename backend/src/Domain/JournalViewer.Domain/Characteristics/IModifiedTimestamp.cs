namespace JournalViewer.Domain.Characteristics;

public interface IModifiedTimestamp
{
    DateTimeOffset? Modified { get; set; }
}