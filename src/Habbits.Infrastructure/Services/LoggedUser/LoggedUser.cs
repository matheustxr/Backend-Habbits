using Habbits.Domain.Entities;
using Habbits.Domain.Security.Tokens;
using Habbits.Domain.Services.LoggedUser;
using Habbits.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Habbits.Infrastructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly HabbitsDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(HabbitsDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}
