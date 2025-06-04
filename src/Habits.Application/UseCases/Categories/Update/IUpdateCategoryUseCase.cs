using Habits.Communication.Requests.Categories;

namespace Habits.Application.UseCases.Categories.Update
{
    public interface IUpdateCategoryUseCase
    {
        Task Execute(RequestCategoryJson request, long id);
    }
}
