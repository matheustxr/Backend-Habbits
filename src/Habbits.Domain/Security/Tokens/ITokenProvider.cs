namespace Habbits.Domain.Security.Tokens;
public interface ITokenProvider
{
    string TokenOnRequest();
}