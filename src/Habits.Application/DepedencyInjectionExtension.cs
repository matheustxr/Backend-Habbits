using Habits.Application.AutoMapper;
using Habits.Application.UseCases.Habits.Create;
using Habits.Application.UseCases.Habits.Delete;
using Habits.Application.UseCases.Habits.GetAll;
using Habits.Application.UseCases.Habits.GetById;
using Habits.Application.UseCases.Login;
using Habits.Application.UseCases.Users.ChangePassword;
using Habits.Application.UseCases.Users.Delete;
using Habits.Application.UseCases.Users.Profile;
using Habits.Application.UseCases.Users.Register;
using Habits.Application.UseCases.Users.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Habits.Application;

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
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IDeleteUserAccountUseCase, DeleteUserAccountUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

        services.AddScoped<ICreateHabitUseCase, CreateHabitUseCase>();
        services.AddScoped<IGetAllHabitsUseCase, GetAllHabitsUseCase>();
        services.AddScoped<IGetHabitByIdUseCase, GetHabitByIdUseCase>();
        services.AddScoped<IDeleteHabitUseCase, DeleteHabitUseCase>();
    }
}
