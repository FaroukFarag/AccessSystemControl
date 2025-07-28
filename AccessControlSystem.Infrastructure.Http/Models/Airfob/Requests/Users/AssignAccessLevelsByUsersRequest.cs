using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class AssignAccessLevelsByUsersRequest
{
    [JsonPropertyName("user_ids")]
    public IEnumerable<int> UserIds { get; set; } = default!;

    [JsonPropertyName("access_level_ids")]
    public IEnumerable<int> AccessLevelIds { get; set; } = default!;
}
