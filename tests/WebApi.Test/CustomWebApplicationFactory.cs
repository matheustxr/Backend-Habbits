using CommonTestUtilities.Entities;
using Habbits.Domain.Entities;
using Habbits.Domain.Security.Cryptography;
using Habbits.Domain.Security.Tokens;
using Habbits.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public string? TestUserToken { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test")
                .ConfigureServices(services =>
                {
                    //configuração para iniciar o bd inMemory para testes
                    var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                    services.AddDbContext<HabbitsDbContext>(config =>
                    {
                        config.UseInMemoryDatabase("InMemoryDbForTesting");
                        config.UseInternalServiceProvider(provider);
                    });

                    //configuração para sempre inicar o bd com um usuario
                    var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<HabbitsDbContext>();
                    var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                    var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                    StartDatabase(dbContext, passwordEncripter, accessTokenGenerator);
                });
        }

        private void StartDatabase(
            HabbitsDbContext dbContext,
            IPasswordEncripter passwordEncripter,
            IAccessTokenGenerator accessTokenGenerator)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            var user = AddUser(dbContext, passwordEncripter, accessTokenGenerator);

            if (user == null || string.IsNullOrEmpty(TestUserToken))
            {
                throw new InvalidOperationException("Erro ao criar usuário de teste e gerar token.");
            }

            var habit = AddHabit(dbContext, user, habitId: 1);

            dbContext.SaveChanges();
        }

        private User AddUser(
            HabbitsDbContext dbContext,
            IPasswordEncripter passwordEncripter,
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
