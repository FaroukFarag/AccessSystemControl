using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob;

public class AirfobService(AirfobClient client) : IAirfobService
{
    private readonly AirfobClient _client = client;

    public async Task<GetSelfAccountResponse> GetSelfAccounts()
    {
        return await _client.GetAsync<GetSelfAccountResponse>("v1/accounts/self");
    }
}
