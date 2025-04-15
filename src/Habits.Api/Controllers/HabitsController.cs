using Habits.Api.UserContext;
using Habits.Application.UseCases.Habits.Create;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses;
using Habits.Communication.Responses.Habits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habits.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HabitsController : ControllerBase
{
    private readonly IUserContext _userContext;

    public HabitsController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpPost]
    [Authorize]
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
}
