using Aggregation.Backend.WebApi.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Aggregation.Backend.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet("stats")]
        public async Task<IActionResult> GetRequestStatistics()
        {
            return Ok(StatisticsCache.GetStats());
        }
    }
}