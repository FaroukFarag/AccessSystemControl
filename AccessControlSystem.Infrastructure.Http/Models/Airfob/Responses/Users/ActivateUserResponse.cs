using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class ActivateUserResponse
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("user_key")]
    public string UserKey { get; set; } = default!;

    [JsonPropertyName("user_status")]
    public string UserStatus { get; set; } = default!;

    [JsonPropertyName("activate_token")]
    public string ActivateToken { get; set; } = default!;
}
