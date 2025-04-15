using Habits.Domain.Security.Tokens;

namespace Habits.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    //classe que pega o token da requisição e armazena na classe para passar para o loggedUser
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _contextAccessor = httpContextAccessor;
    }
    public string TokenOnRequest()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
