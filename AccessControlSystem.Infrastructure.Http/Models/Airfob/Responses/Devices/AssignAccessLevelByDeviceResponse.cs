using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class AssignAccessLevelByDeviceResponse
{
    [JsonPropertyName("device_id")]
    public int DeviceId { get; set; }

    [JsonPropertyName("access_level_id")]
    public int AccessLevelId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
}
