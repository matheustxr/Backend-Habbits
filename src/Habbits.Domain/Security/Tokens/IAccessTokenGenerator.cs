using Habbits.Domain.Entities;

namespace Habbits.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string Generate(User user);
}