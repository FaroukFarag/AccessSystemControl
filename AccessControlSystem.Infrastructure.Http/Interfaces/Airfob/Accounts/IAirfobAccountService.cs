using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Accounts;

public interface IAirfobAccountService
{
    Task<AirfobResponse<GetSelfAccountsResponse>> GetSelfAccountsAsync();
}
