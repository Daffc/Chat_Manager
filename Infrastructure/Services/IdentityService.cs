using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public sealed class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {

        var subClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (subClaim == null)
        {
            subClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        if (string.IsNullOrEmpty(subClaim) || !Guid.TryParse(subClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User identity is invalid (missing or malformed sub claim)");
        }

        return userId;
    }

    public string? GeCurrentUserEmail()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
    }

    public string? GeCurrentUserNickname()
    {
        return _httpContextAccessor.HttpContext?.User.FindFirst("nickname")?.Value;
    }
}