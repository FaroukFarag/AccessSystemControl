using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses;

public class Visitors
{
    [JsonPropertyName("invite_token")]
    public string InviteToken { get; set; } = default!;
}
