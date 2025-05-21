using Habits.Domain.Repositories;
using Habits.Domain.Repositories.Habits;
using Habits.Domain.Repositories.Users;
using Habits.Domain.Security.Cryptography;
using Habits.Domain.Security.Tokens;
using Habits.Domain.Services.LoggedUser;
using Habits.Infrastructure.DataAccess;
using Habits.Infrastructure.DataAccess.Repositories;
using Habits.Infrastructure.Extensions;
using Habits.Infrastructure.Security.Tokens;
using Habits.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.Infrastructure;

public static class DepedencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncrypter, Security.Cryptography.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        AddToken(services, configuration);
        AddRepositories(services);

        if (configuration.IsTestEnvironment() == false)
        {
            AddDbContext(services, configuration);
        }
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
    }

    private static void AddRepositories(IServiceCollection services) //aqui são as injeções de dependências e as interfaces que elas devem ter que é 
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();

        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUsertWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        services.AddScoped<IHabitReadOnlyRepository, HabitRepository>();
        services.AddScoped<IHabitWriteOnlyRepository, HabitRepository>();
        services.AddScoped<IHabitUpdateOnlyRepository, HabitRepository>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");

        services.AddDbContext<HabitsDbContext>(options => options.UseNpgsql(connectionString));
    }
}
