using Habits.Domain.Entities;

namespace WebApi.Test.Resources
{
    public class HabitIdentityManager
    {
        private readonly Habit _habit;

        public HabitIdentityManager(Habit habit)
        {
            _habit = habit;
        }

        public long GetId() => _habit.Id;
    }
}
