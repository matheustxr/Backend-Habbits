using Habits.Api.UserContext;
using Habits.Application.UseCases.Habits.Create;
using Habits.Application.UseCases.Habits.GetAll;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses;
using Habits.Communication.Responses.Habits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habits.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HabitsController : ControllerBase
{
    private readonly IUserContext _userContext;

    public HabitsController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreateHabitJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHabit(
        [FromBody] RequestCreateHabitJson request,
        [FromServices] ICreateHabitUseCase useCase)
    {
        request.UserId = _userContext.GetUserId();

        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseHabitsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllHabits([FromServices] IGetAllHabitsUseCase useCase)
    {
        var response = await useCase.Execute();

        if (response.Habits.Count != 0)
            return Ok(response);

        return NoContent();
    }
}
