using Habits.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess;

public class HabbitsDbContext : DbContext
{
    public HabbitsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
    public DbSet<HabitCategory> HabitCategories { get; set; }

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
