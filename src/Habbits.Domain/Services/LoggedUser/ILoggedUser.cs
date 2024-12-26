using Habbits.Domain.Entities;

namespace Habbits.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
