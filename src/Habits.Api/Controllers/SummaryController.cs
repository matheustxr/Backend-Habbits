using Habits.Application.UseCases.Summary.GetMounthly;
using Habits.Communication.Requests.Summary;
using Habits.Communication.Responses.Summary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SummaryController : ControllerBase
{
    [HttpGet]
    [Route("{startDate}/{endDate}")]
    [ProducesResponseType(typeof(ResponseSummaryJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSummary(
    [FromRoute] DateOnly startDate,
    [FromRoute] DateOnly endDate,
    [FromServices] IGetMonthlySummaryUseCase useCase)
{
    var response = await useCase.Execute(startDate, endDate);

    if (response == null || response.Count == 0)
        return NoContent();

    return Ok(response);
}
}

