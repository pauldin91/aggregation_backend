using Aggregation.Backend.Domain.Constants;
using Aggregation.Backend.Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Aggregation.Backend.WebApi.Controllers
{
    [ApiController]
    public class StatisticsController(PerformanceStatisticsCache performanceStatisticsCache) : ControllerBase
    {
        [HttpGet(ApiEndpoints.GetStatisticsRoute)]
        public async Task<IActionResult> GetRequestStatistics()
        {
            return Ok(performanceStatisticsCache.GetStats());
        }
    }
}