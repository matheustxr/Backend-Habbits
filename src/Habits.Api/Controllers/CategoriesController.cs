using Habits.Application.UseCases.Categories.Create;
using Habits.Application.UseCases.Categories.Delete;
using Habits.Application.UseCases.Categories.GetAll;
using Habits.Application.UseCases.Categories.GetById;
using Habits.Application.UseCases.Categories.Update;
using Habits.Application.UseCases.Habits.Delete;
using Habits.Application.UseCases.Habits.Update;
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

        [HttpGet]
        [ProducesResponseType(typeof(ResponseListCategoriesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllCategories([FromServices] IGetAllCategoriesUseCase useCase)
        {
            var response = await useCase.Execute();

            if (response.Categories.Count != 0)
                return Ok(response);

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseCategoryJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GeyCategoryById([FromServices] IGetCategoryByIdUseCase useCase, [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Delete(
            [FromServices] IDeleteCategoryUseCase useCase,
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
            [FromServices] IUpdateCategoryUseCase useCase,
            [FromRoute] long id,
            [FromBody] RequestCategoryJson request)
        {
            await useCase.Execute(request, id);
            return NoContent();
        }
    }
}
