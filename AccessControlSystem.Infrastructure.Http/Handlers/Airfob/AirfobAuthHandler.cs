using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using System.Net.Http.Headers;

namespace AccessControlSystem.Infrastructure.Http.Handlers.Airfob;

public class AirfobAuthHandler : DelegatingHandler
{
    private readonly IAirfobAuthService _authService;

    public AirfobAuthHandler(IAirfobAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _authService.GetAccessTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}
