using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;

public class AssignAccessLevelsByDoorsRequest
{
    [JsonPropertyName("door_ids")]
    public IEnumerable<int> DoorIds { get; set; } = default!;

    [JsonPropertyName("access_level_ids")]
    public IEnumerable<int> AccessLevelIds { get; set; } = default!;
}
