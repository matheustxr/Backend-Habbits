namespace Habbits.Domain.Repositories.User;
public interface IUsertWriteOnlyRepository
{
    Task Add(Entities.User user);
    Task Delete(Entities.User user);
}