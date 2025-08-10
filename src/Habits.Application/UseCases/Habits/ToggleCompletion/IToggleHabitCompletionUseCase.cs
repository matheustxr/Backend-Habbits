namespace Habits.Application.UseCases.Habits.ToggleCompletion
{
    public interface IToggleHabitCompletionUseCase
    {
        Task Execute(long habitId, DateOnly date);
    }
}
