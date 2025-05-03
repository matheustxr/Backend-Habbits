namespace Habits.Application.UseCases.Habits.Delete
{
    public interface IDeleteHabitUseCase
    {
        Task Execute(long id);
    }
}
