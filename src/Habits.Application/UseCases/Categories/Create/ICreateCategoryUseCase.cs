using Habits.Communication.Requests.Categories;
using Habits.Communication.Responses.Categories;

namespace Habits.Application.UseCases.Categories.Create
{
    public interface ICreateCategoryUseCase
    {
        Task<ResponseCategoryJson> Execute(RequestCategoryJson request);
    }
}
