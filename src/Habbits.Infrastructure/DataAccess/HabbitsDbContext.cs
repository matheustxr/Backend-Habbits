using Habbits.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Habbits.Infrastructure.DataAccess;

public class HabbitsDbContext : DbContext
{
    public HabbitsDbContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Habit> Habits { get; set; }
}
