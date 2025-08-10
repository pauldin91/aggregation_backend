using Aggregation.Backend.Application.Features.Aggregates;
using Aggregation.Backend.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Aggregation.Backend.WebApi.Controllers
{
    [ApiController]
    public class AggregateController(IMediator mediator) : ControllerBase
    {
        [HttpGet(ApiEndpoints.GetAggrgatesRoute)]
        [OutputCache(PolicyName = Policies.AggregatesCachePolicy)]
        public async Task<IActionResult> GetNewsAsync([FromQuery] string keyword, [FromQuery] string? filterBy, [FromQuery] string? orderBy, CancellationToken cancellationToken, [FromQuery] bool asc = true)
        {
            var result = await mediator.Send(new AggregatesQuery(keyword, filterBy, orderBy, asc), cancellationToken);

            return Ok(result);
        }
    }
}