using System.Text.Json.Serialization;

namespace Aggregation.Backend.Domain.Dtos.External
{
    public record MusicDto(
                [property: JsonPropertyName("author")] string Author,
        [property: JsonPropertyName("title")] string Title,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("url")] string Url,
        [property: JsonPropertyName("published_at")] DateTime? PublishedAt);
}