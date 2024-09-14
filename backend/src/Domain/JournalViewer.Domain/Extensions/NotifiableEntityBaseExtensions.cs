using System.Text;
using System.Text.Json;
using JournalViewer.Domain.Bootstrap;

namespace JournalViewer.Domain.Extensions;

public static class NotifiableEntityBaseExtensions
{
    public static async Task<string> PrepareAsJsonAsync<T>(this T item, 
        CancellationToken cancellationToken, JsonSerializerOptions? jsonSerializerOptions = null)
        where T : NotifiableEntityBase<T>
    {
        using var memoryStream = new MemoryStream();
        await JsonSerializer.SerializeAsync(memoryStream, item,
            jsonSerializerOptions, cancellationToken);
        return Encoding.UTF8.GetString(memoryStream.ToArray());
    }
}
