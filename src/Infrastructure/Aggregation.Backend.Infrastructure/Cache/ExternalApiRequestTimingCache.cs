using System.Collections.Concurrent;

namespace Aggregation.Backend.Infrastructure.Cache
{
    public class ExternalApiRequestTimingCache
    {
        private static readonly ConcurrentDictionary<string, List<Tuple<DateTime, long>>> _responseTimes = new();
        private static readonly object _lock = new();

        public static void Record(string endpoint, long durationMs)
        {
            _responseTimes.AddOrUpdate(endpoint,
                new List<Tuple<DateTime, long>> { Tuple.Create(DateTime.UtcNow, durationMs) },
                (id, existing) =>
                {
                    lock (_lock)
                    {
                        existing = existing.Where(s => s.Item1 > DateTime.UtcNow.AddMinutes(-5)).ToList();
                        existing.Add(Tuple.Create(DateTime.UtcNow, durationMs));
                        return existing;
                    }
                }
            );
        }

        public static IReadOnlyDictionary<string, long> GetResponseTimes()
        {
            return _responseTimes.ToDictionary(s => s.Key, l =>
            {
                if (l.Value.Count == 0)
                {
                    return 0;
                }
                return l.Value.Sum(p => p.Item2) / l.Value.Count;
            });
        }
    }
}