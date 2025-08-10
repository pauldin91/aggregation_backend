using System.Collections.Concurrent;

namespace Aggregation.Backend.Infrastructure.Cache
{
    public class ExternalApiRequestTimingCache
    {
        private static int _requestsCount = 0;
        private static long _accumulativeReponseTime = 0;
        private static readonly ConcurrentDictionary<string, List<long>> _responseTimes = new();
        private static readonly object _lock = new();

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

        public static void Record(string endpoint, long durationMs)
        {
            lock (_lock)
            {
                _requestsCount++;
                _accumulativeReponseTime += durationMs;

                _responseTimes.AddOrUpdate(endpoint,
                    new List<long> { durationMs },
                    (id, existing) => { 
                        existing.Add(durationMs); 
                        return existing; 
                    }
                );
            }
        }
    }
}