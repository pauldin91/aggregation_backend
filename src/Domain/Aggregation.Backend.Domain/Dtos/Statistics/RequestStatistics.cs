namespace Aggregation.Backend.Domain.Dtos.Statistics
{
    public class RequestStatistics
    {
        public int TotalRequests { get; set; }
        public double AverageResponseTimeMs { get; set; }
        public Dictionary<string, int> Buckets { get; set; } = new();
    }
}