using AccessControlSystem.Infrastructure.Http.Clients.Airfob;
using AccessControlSystem.Infrastructure.Http.Configurations;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Auth;
using AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Auth;
using Microsoft.Extensions.Options;

namespace AccessControlSystem.Infrastructure.Http.Services.Airfob;

public class AirfobAuthService(AirfobClient client, IOptions<AirfobSettings> settings) : IAirfobAuthService
{
    private readonly AirfobClient _client = client;
    private readonly AirfobSettings _settings = settings.Value;
    private string _accessToken = string.Empty;
    private DateTime _tokenExpiry;

    public async Task<string> GetAccessTokenAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiry)
        {
            await RefreshTokenAsync();
        }

        return _accessToken;
    }

    private async Task RefreshTokenAsync()
    {
        var response = await _client.PostAsync<AirfobLoginRequest, AirfobAuthResponse>(
            "v1/local/login",
            new AirfobLoginRequest
            {
                Username = _settings.Username,
                Password = _settings.Password
            });

        _accessToken = response.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(response.TokenExpirySec - 60);
    }
}
