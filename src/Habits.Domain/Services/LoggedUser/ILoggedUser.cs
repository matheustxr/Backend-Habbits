using Habits.Domain.Entities;

namespace Habits.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
