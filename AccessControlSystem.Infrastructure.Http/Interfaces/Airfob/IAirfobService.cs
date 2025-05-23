using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;

public interface IAirfobService
{
    Task<GetSelfAccountResponse> GetSelfAccounts();
}
