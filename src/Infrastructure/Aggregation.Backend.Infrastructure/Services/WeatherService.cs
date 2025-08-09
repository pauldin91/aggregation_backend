using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.Aggregates;

namespace Aggregation.Backend.Infrastructure.Services
{
    public class WeatherService : IExternalApiService
    {
        public async Task<IList<Dictionary<string,string>>> ListAsync(string category,CancellationToken cancellationToken)
        {
            var result = new List<Dictionary<string, string>>();
            return result;
        }
    }
}