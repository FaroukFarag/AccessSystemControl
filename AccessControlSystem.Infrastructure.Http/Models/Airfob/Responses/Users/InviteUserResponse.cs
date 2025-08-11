using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class InviteUserResponse
{
    [JsonPropertyName("invite_token")]
    public string InviteToken { get; set; } = default!;
}
