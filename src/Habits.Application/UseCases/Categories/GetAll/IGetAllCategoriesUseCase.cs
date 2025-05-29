using Habits.Communication.Responses.Categories;

namespace Habits.Application.UseCases.Categories.GetAll
{
    public interface IGetAllCategoriesUseCase
    {
        Task<ResponseListCategoriesJson> Execute();
    }
}
