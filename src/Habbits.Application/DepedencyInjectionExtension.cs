using Habbits.Application.AutoMapper;
using Habbits.Application.UseCases.Users.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Habbits.Application;

public static class DepedencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddAutoMapper(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }
}
