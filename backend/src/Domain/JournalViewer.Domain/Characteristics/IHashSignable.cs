namespace JournalViewer.Domain.Characteristics;

public interface IHashSignable
{
    string SignatureHash { get; set; }
}
