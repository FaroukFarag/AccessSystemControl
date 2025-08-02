using AccessControlSystem.Domain.Interfaces.Services.Users;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Application.Services.Users;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public bool IsAuthenticated()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.Identity?.IsAuthenticated ?? false;
    }

    public bool HasSubscriptionId()
    {
        return _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "subscriptionId")?.Value is not null;
    }

    public bool IsAdmin()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        return user?.IsInRole("Admin") ?? false;
    }

    public int GetSubscriptionId()
    {
        var subscriptionIdClaim = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == "subscriptionId")?.Value;

        if (int.TryParse(subscriptionIdClaim, out int subscriptionId))
        {
            return subscriptionId;
        }

        return default!;
    }
}
