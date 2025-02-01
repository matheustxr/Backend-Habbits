namespace Habbits.Domain.Repositories.User;
public interface IHabitReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);

    Task<Entities.User?> GetUserByEmail(string email);
}