using Habits.Application.UseCases.Habits.Create;
using Habits.Application.UseCases.Habits.Delete;
using Habits.Application.UseCases.Habits.GetAll;
using Habits.Application.UseCases.Habits.GetById;
using Habits.Application.UseCases.Habits.Update;
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
    [HttpPost]
    [ProducesResponseType(typeof(ResponseCreateHabitJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHabit(
        [FromBody] RequestHabitJson request,
        [FromServices] ICreateHabitUseCase useCase)
    {
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

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseHabitJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetById(
        [FromServices] IGetHabitByIdUseCase useCase,
        [FromRoute] long id)
    {
        var response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete(
        [FromServices] IDeleteHabitUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
            [FromServices] IUpdateHabitUseCase useCase,
            [FromRoute] long id,
            [FromBody] RequestHabitJson request)
    {
        await useCase.Execute(request, id);
        return NoContent();
    }
}
