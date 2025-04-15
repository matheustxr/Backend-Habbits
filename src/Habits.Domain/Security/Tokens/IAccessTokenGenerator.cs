using Habits.Domain.Entities;

namespace Habits.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string Generate(User user);
}