using Habits.Communication.Responses.Categories;

namespace Habits.Application.UseCases.Categories.GetById
{
    public interface IGetCategoryByIdUseCase
    {
        Task<ResponseCategoryJson> Execute(long id);
    }
}
