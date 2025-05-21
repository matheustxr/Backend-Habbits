using Habits.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public async static Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<HabitsDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}
