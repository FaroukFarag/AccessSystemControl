using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;

public class AssignAccessLevelsByDoorsResponse
{
    [JsonPropertyName("door_id")]
    public int DoorId { get; set; }

    [JsonPropertyName("access_level_id")]
    public int AccessLevelId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
}
