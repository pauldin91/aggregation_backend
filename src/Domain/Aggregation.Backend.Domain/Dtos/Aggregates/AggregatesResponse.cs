namespace Aggregation.Backend.Domain.Dtos.Aggregates
{
    public record AggregateResponse(string Author, string Title, string Content, DateTime? Date);
}