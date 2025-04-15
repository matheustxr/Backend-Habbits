using Habits.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habits.Infrastructure.DataAccess;

public class HabbitsDbContext : DbContext
{
    public HabbitsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
}
