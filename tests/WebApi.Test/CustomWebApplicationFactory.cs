using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Habbits.Domain.Repositories;
using Moq;
using Habbits.Infrastructure.DataAccess;
using Habbits.Domain.Repositories.Habit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace WebApi.Test
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Test.json");
            });

            builder.ConfigureServices(services =>
            {
                // Remove o contexto de banco de dados real
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<HabbitsDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adiciona um banco de dados em memória para testes
                services.AddDbContext<HabbitsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("HabbitsTestDb")
                        .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning)); // Evita erro de transações
                });

                // Configuração de mocks para repositórios se necessário
                var habitReadOnlyRepoMock = new Mock<IHabitReadOnlyRepository>();
                var habitWriteOnlyRepoMock = new Mock<IHabitWriteOnlyRepository>();
                var habitUpdateOnlyRepoMock = new Mock<IHabitUpdateOnlyRepository>();
                var unitOfWorkMock = new Mock<IUnityOfWork>();

                services.RemoveAll<IHabitReadOnlyRepository>();
                services.RemoveAll<IHabitWriteOnlyRepository>();
                services.RemoveAll<IHabitUpdateOnlyRepository>();
                services.RemoveAll<IUnityOfWork>();

                services.AddSingleton(habitReadOnlyRepoMock.Object);
                services.AddSingleton(habitWriteOnlyRepoMock.Object);
                services.AddSingleton(habitUpdateOnlyRepoMock.Object);
                services.AddSingleton(unitOfWorkMock.Object);

                // Criar o escopo de serviços para inicializar o banco de dados de testes
                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<HabbitsDbContext>();
                dbContext.Database.EnsureCreated();
            });
        }
    }
}
