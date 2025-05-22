using Habits.Application.UseCases.Categories.Create;
using Habits.Application.UseCases.Habits.Create;
using Habits.Communication.Requests.Categories;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Responses;
using Habits.Communication.Responses.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Habits.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCategory(
        [FromBody] RequestCategoryJson request,
        [FromServices] ICreateCategoryUseCase useCase)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
