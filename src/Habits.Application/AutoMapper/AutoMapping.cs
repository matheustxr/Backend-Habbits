using AutoMapper;
using Habits.Communication.Requests.Habits;
using Habits.Communication.Requests.Users;
using Habits.Communication.Responses.Habits;
using Habits.Communication.Responses.Users;
using Habits.Domain.Entities;

namespace Habits.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestEntity();
        EntityResponse();
    }

    private void RequestEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

        CreateMap<RequestHabitJson, Habit>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => (DateTime?)null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true))
            .ForMember(dest => dest.UserId, opt => opt.Ignore());
    }

    private void EntityResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Habit, ResponseCreateHabitJson>();
        CreateMap<Habit, ResponseHabitsJson>();
        CreateMap<Habit, ResponseShortHabitJson>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.HabitCategory != null ? src.HabitCategory.Category : null));
        CreateMap<Habit, ResponseHabitJson>();
    }
}
