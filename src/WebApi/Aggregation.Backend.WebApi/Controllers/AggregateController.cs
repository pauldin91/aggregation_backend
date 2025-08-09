using Aggregation.Backend.Application.Features.Aggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aggregation.Backend.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AggregateController(IMediator mediator) : ControllerBase
    {
        [HttpGet("aggregates")]
        public async Task<IActionResult> GetNewsAsync([FromQuery] string category, [FromQuery] string? filter, [FromQuery] string? orderBy, CancellationToken cancellationToken, [FromQuery] bool asc = true)
        {
            var result = await mediator.Send(new AggregatesQuery(category, filter, orderBy, asc), cancellationToken);

            return Ok(result);
        }
    }
}