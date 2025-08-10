using Aggregation.Backend.Application.Features.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Aggregation.Backend.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AggregateController(IMediator mediator) : ControllerBase
    {
        [HttpGet("aggregates")]
        [OutputCache(PolicyName = "AggregatePolicy")]
        public async Task<IActionResult> GetNewsAsync([FromQuery] string keyword, [FromQuery] string? filter, [FromQuery] string? orderBy, CancellationToken cancellationToken, [FromQuery] bool asc = true)
        {
            var result = await mediator.Send(new AggregatesQuery(keyword, filter, orderBy, asc), cancellationToken);

            return Ok(result);
        }
    }
}