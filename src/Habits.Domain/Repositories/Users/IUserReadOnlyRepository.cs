namespace Habits.Domain.Repositories.Users;
public interface IUserReadOnlyRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);

    Task<Entities.User?> GetUserByEmail(string email);
}