using Habits.Domain.Entities;
using Habits.Domain.Repositories.Categories;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Habits.Domain.Repositories.Habits;

namespace Habits.Infrastructure.DataAccess.Repositories
{
    public class CategoryRepository : ICategoriesReadOnlyRepository, ICategoriesWriteOnlyRepository, ICategoriesUpdateOnlyRepository
    {
        private readonly HabitsDbContext _dbContext;

        public CategoryRepository(HabitsDbContext dbContext) => _dbContext = dbContext;

        public async Task<bool> AlreadyCategoryExist(string category, Guid userId, long? excludeId = null)
        {
            var query = _dbContext.HabitCategories
                .AsNoTracking()
                .Where(c => c.Category == category && c.UserId == userId);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<List<HabitCategory>> GetAll(User user)
        {
            return await _dbContext.HabitCategories
                .AsNoTracking()
                .Where(category => category.UserId == user.Id).ToListAsync();
        }

        public async Task<HabitCategory?> GetByCategory(string category, Guid userId)
        {
            return await _dbContext.HabitCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Category == category && c.UserId == userId);
        }

        public async Task<HabitCategory?> GetById(User user, long id)
        {
            return await _dbContext.HabitCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(category => category.Id == id && category.UserId == user.Id);
        }

        public async Task Add(HabitCategory category)
        {
            await _dbContext.HabitCategories.AddAsync(category);
        }

        public async Task Delete(User user, long id)
        {
            var categoryToRemove = await _dbContext.HabitCategories
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == user.Id);

            if (categoryToRemove is not null)
                _dbContext.HabitCategories.Remove(categoryToRemove);
        }

        public void Update(HabitCategory category)
        {
            _dbContext.HabitCategories.Update(category);
        }
    }
}
