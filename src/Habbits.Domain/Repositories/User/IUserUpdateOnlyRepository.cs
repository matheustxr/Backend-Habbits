namespace Habbits.Domain.Repositories.User;
public interface IHabitUpdateOnlyRepository
{
    Task<Entities.User> GetById(Guid id);
    void Update(Entities.User user);
}
