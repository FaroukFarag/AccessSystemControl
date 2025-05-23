namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;

public interface IAirfobAuthService
{
    Task<string> GetAccessTokenAsync();
}
