using Habits.Domain.Entities;

namespace Habits.Application.UseCases.Categories.Delete
{
    public interface IDeleteCategoryUseCase
    {
        public Task Execute(long id);
    }
}
