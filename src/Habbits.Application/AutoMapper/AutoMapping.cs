using AutoMapper;
using Habbits.Communication.Requests;
using Habbits.Domain.Entities;

namespace Habbits.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestEntity();
    }

    private void RequestEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());
    }
}
