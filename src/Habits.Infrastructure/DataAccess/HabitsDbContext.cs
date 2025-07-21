using Habits.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess;

public class HabitsDbContext : DbContext
{
    public HabitsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitCategory> HabitCategories { get; set; }
    public DbSet<DayHabit> DayHabits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento explícito para evitar HabitCategoryId extra
        modelBuilder.Entity<Habit>()
            .HasOne(h => h.HabitCategory)
            .WithMany(c => c.Habits)
            .HasForeignKey(h => h.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
