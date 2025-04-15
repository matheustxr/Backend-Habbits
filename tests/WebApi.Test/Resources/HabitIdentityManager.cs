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
        public string GetTitle() => _habit.Title;
        public DateTime GetCreatedAt() => _habit.CreatedAt;
        public Guid GetUserId() => _habit.UserId;
        public bool IsActive() => _habit.IsActive;
        public long? GetCategoryId() => _habit.CategoryId;
    }
}
