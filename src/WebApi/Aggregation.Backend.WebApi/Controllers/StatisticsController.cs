using Aggregation.Backend.WebApi.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Aggregation.Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetRequestStatistics() {
            return Ok(StatisticsCache.GetStats());
        } 
    }
}
