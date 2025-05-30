using Habits.Communication.Responses.Categories;

namespace Habits.Application.UseCases.Categories.GetById
{
    public interface IGetCategoriesByIdUseCase
    {
        Task<ResponseCategoryJson> Execute(long id);
    }
}
