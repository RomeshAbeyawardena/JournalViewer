
namespace JournalViewer.Domain;

public class Element : NotifiableEntityBase<Element>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Description { get; set; }

    public override TKey GetKey<TKey>(Element model)
    {
        return (TKey)(object)model.Id;
    }

    public override Task<string> PrepareNotificationAsync(Element result, NotificationType notificationType, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
