namespace Aggregation.Backend.Contracts.Aggregates
{
    public record AggregateRequest(string? filterBy,string? orderBy);
}