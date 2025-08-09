using Aggregation.Backend.Domain.Dtos.Statistics;
using System.Collections.Concurrent;

namespace Aggregation.Backend.WebApi.Stores
{
    public class StatisticsCache
    {
        private const string _fast = "Fast";
        private const string _average = "Average";
        private const string _slow = "Slow";
        private static int _requestsCount = 0;
        private static long _accumulativeReponseTime = 0;
        private static readonly ConcurrentDictionary<string, Tuple<long, int>> _responseTimes = new();
        private static readonly object _lock = new();

        public static int TotalRequestCount => _requestsCount;

        public static long AverageResponseTime
        {
            get
            {
                if (_requestsCount == 0)
                {
                    return 0;
                }
                else
                {
                    return _accumulativeReponseTime / _requestsCount;
                }
            }
        }

        public static void Record(long durationMs)
        {
            lock (_lock)
            {
                _requestsCount++;
                _accumulativeReponseTime += durationMs;
                string key = string.Empty;
                if (durationMs < 100)
                {
                    key = _fast;
                }
                else if (durationMs >= 100 && durationMs <= 200)
                {
                    key = _average;
                }
                else
                {
                    key = _slow;
                }
                _responseTimes.AddOrUpdate(key,
                    Tuple.Create(durationMs, 1),
                    (id, existing) => Tuple.Create(existing.Item1 + durationMs, existing.Item2 + 1)
                );
            }
        }

        public static RequestStatistics GetStats()
        {
            var result = new RequestStatistics
            {
                AverageResponseTimeMs = AverageResponseTime,
                TotalRequests = TotalRequestCount,
                Buckets = _responseTimes.ToDictionary(s => s.Key, s => s.Value.Item2)
            };

            return result;
        }
    }
}