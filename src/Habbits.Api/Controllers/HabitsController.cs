using Habbits.Application.UseCases.Habit.Create;
using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Responses;
using Habbits.Communication.Responses.Habbits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habbits.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HabitsController : ControllerBase
{
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ResponseCreateHabitJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHabit(
        [FromBody] RequestCreateHabitJson request,
        [FromServices] ICreateHabitUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
