using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class ReactivateUserResponse
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("user_key")]
    public string UserKey { get; set; } = default!;

    [JsonPropertyName("activate_token")]
    public string ActivateToken { get; set; } = default!;

    [JsonPropertyName("short_link_id")]
    public string ShortLinkId { get; set; } = default!;
}
