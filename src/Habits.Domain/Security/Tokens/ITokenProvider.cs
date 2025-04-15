namespace Habits.Domain.Security.Tokens;
public interface ITokenProvider
{
    string TokenOnRequest();
}