namespace JournalViewer.Domain;

public interface IIdentifier
{
    public Guid Id { get; set; }
}


public interface IHashSignable
{
    string SignatureHash { get; set; }
}

public interface ICreatedTimestamp
{
    DateTimeOffset Created { get; set; }
}

public interface IModifiedTimestamp
{
    DateTimeOffset? Modified { get; set; }
}