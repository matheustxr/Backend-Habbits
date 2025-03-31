namespace Habbits.Domain.Repositories.Users;
public interface IUsertWriteOnlyRepository
{
    Task Add(Entities.User user);
    Task Delete(Entities.User user);
}