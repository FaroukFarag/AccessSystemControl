namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Auth;

public class AirfobAuthResponse
{
    public string AccessToken { get; set; } = default!;
    public int TokenExpirySec { get; set; }
    public string WebsocketServerUrl { get; set; } = default!;
}
