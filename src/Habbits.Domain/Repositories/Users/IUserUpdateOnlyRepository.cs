namespace Habbits.Domain.Repositories.Users;
public interface IUserUpdateOnlyRepository
{
    Task<Entities.User> GetById(Guid id);
    void Update(Entities.User user);
}
