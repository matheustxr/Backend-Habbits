namespace Habbits.Domain.Repositories.User;
public interface IHabitWriteOnlyRepository
{
    Task Add(Entities.User user);
    Task Delete(Entities.User user);
}