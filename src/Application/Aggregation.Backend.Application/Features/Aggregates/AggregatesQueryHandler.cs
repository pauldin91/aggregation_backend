using Aggregation.Backend.Application.Interfaces;
using Aggregation.Backend.Domain.Dtos.Aggregates;
using MediatR;
using System.Globalization;
using System.Text.Json;

namespace Aggregation.Backend.Application.Features.Aggregates
{
    public class AggregatesQueryHandler(IEnumerable<IExternalApiService> services) : IRequestHandler<AggregatesQuery, string>
    {
        public async Task<string> Handle(AggregatesQuery request, CancellationToken cancellationToken)
        {
            var tasks = new List<Task<IList<AggregateResponse>>>();

            foreach (var service in services)
            {
                tasks.Add(service.ListAsync(request.Category, cancellationToken));
            }

            var results = await Task.WhenAll(tasks);
            
            IEnumerable<AggregateResponse> allData = results.SelectMany(r => r).ToList();

            if (!string.IsNullOrEmpty(request.FilterBy))
            {
                var filter = request.FilterBy.Split('=');
                allData = filter[0] switch
                {
                    "author" => allData.Where(s => s.Author.Equals(filter[1], StringComparison.OrdinalIgnoreCase)),
                    "title" => allData.Where(s => s.Title.Equals(filter[1], StringComparison.OrdinalIgnoreCase)),
                    "date" => allData.Where(s => s.Date == DateTime.ParseExact( filter[1],"yyyy-MM-ddTHH:mm:ss.ffffffZ",CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal)),
                    _ => allData
                };
            }

            if (!string.IsNullOrEmpty(request.SortBy))
            {
                allData = request.SortBy switch
                {
                    "author" => request.Asc ? allData.OrderBy(s => s.Author) : allData.OrderByDescending(s => s.Author),
                    "title" => request.Asc ? allData.OrderBy(s => s.Title) : allData.OrderByDescending(s => s.Title),
                    "date" => request.Asc ? allData.OrderBy(s => s.Date) : allData.OrderByDescending(s => s.Date),
                    _ => allData
                };
            }

            return JsonSerializer.Serialize(allData);
        }
    }
}