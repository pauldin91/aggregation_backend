namespace Aggregation.Backend.Domain.Constants
{
    public static class ApiEndpoints
    {
        public const string Api = "api";
        public const string Version = "1";
        public const string GetAggrgatesRoute = $"{Api}/v{Version}/aggregates";
        public const string GetStatisticsRoute = $"{Api}/v{Version}/statistics";
    }
}