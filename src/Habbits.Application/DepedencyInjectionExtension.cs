using Habbits.Application.AutoMapper;
using Habbits.Application.UseCases.Login;
using Habbits.Application.UseCases.Users.ChangePassword;
using Habbits.Application.UseCases.Users.Delete;
using Habbits.Application.UseCases.Users.Profile;
using Habbits.Application.UseCases.Users.Register;
using Habbits.Application.UseCases.Users.Update;
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
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase> ();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
    }
}
