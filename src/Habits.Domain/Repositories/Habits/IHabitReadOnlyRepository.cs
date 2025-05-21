using Habits.Domain.Entities;
using Habits.Domain.Enums;

namespace Habits.Domain.Repositories.Habits;
public interface IHabitReadOnlyRepository
{
    Task<List<Habit>> GetAll(User user);
    Task<Habit?> GetById(User user, long id);
    Task<bool> ExistHabitWithTitle(string title, User user, long? excludeId = null);
    Task<Habit?> GetHabitByTitle(string habit);
    Task<List<Habit>> FilterByWeekday(User user, WeekDays weekday);
}