using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class SuspendUserResponse
{
    [JsonPropertyName("suspend_token")]
    public string SuspendToken { get; set; } = default!;
}
