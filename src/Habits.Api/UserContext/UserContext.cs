using System.Security.Claims;

namespace Habits.Api.UserContext
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Sid);

            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("Usuário não autenticado.");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("ID de usuário inválido.");
            }

            return userId;
        }
    }
}
