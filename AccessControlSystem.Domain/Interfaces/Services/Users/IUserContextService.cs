namespace AccessControlSystem.Domain.Interfaces.Services.Users;

public interface IUserContextService
{
    bool IsAuthenticated();
    bool HasSubscriptionId();
    bool IsAdmin();
    int GetSubscriptionId();
}
