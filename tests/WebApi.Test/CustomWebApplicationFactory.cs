using CommonTestUtilities.Entities;
using Habbits.Domain.Entities;
using Habbits.Domain.Security.Cryptography;
using Habbits.Domain.Security.Tokens;
using Habbits.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public string TestUserToken { get; private set; }
        public UserIdentityManager? TestUser { get; private set; }
        public HabitIdentityManager? TestHabit { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<HabbitsDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<HabbitsDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncrypter>();
                    var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
                });
        }

        private void StartDatabase(
            HabbitsDbContext dbContext,
            IPasswordEncrypter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var user = AddUser(dbContext, passwordEncripter, accessTokenGenerator);
            var habit = AddHabit(dbContext, user, habitId: 1);

            TestUser = new UserIdentityManager(user, "12345678", TestUserToken!);
            TestHabit = new HabitIdentityManager(habit);

            dbContext.SaveChanges();
        }

        private User AddUser(
            HabbitsDbContext dbContext,
            IPasswordEncrypter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            var user = UserBuilder.Build();
            dbContext.Users.Add(user);
            TestUserToken = accessTokenGenerator.Generate(user);
            return user;
        }

        private Habit AddHabit(HabbitsDbContext dbContext, User user, long habitId)
        {
            var habit = HabitBuilder.Build(user);
            habit.Id = habitId;
            dbContext.Habits.Add(habit);
            return habit;
        }
    }
}
