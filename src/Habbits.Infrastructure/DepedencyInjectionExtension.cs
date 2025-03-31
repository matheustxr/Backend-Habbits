using Habbits.Domain.Repositories;
using Habbits.Domain.Repositories.Habits;
using Habbits.Domain.Repositories.Users;
using Habbits.Domain.Security.Cryptography;
using Habbits.Domain.Security.Tokens;
using Habbits.Domain.Services.LoggedUser;
using Habbits.Infrastructure.DataAccess;
using Habbits.Infrastructure.DataAccess.Repositories;
using Habbits.Infrastructure.Extensions;
using Habbits.Infrastructure.Security.Tokens;
using Habbits.Infrastructure.Services.LoggedUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Habbits.Infrastructure;

public static class DepedencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.Cryptography.BCrypt>();
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

        services.AddDbContext<HabbitsDbContext>(options => options.UseNpgsql(connectionString));
    }
}
