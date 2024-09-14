namespace JournalViewer.Domain.Characteristics;

public interface ICreatedTimestamp
{
    DateTimeOffset Created { get; set; }
}
