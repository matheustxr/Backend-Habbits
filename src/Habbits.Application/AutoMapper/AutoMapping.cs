using AutoMapper;
using Habbits.Communication.Requests;
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
    }

    private void EntityResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
    }
}
