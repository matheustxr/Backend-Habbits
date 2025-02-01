using AutoMapper;
using Habbits.Communication.Requests.Habits;
using Habbits.Communication.Requests.User;
using Habbits.Communication.Responses.Habbits;
using Habbits.Communication.Responses.Users;
using Habbits.Domain.Entities;

namespace Habbits.Application.AutoMapper;

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
        
        CreateMap<RequestCreateHabitJson, Habit>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => (DateTime?)null))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
    }

    private void EntityResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Habit, ResponseHabitJson>();
    }
}
