using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;

public class AssignAccessLevelsByDevicesRequest
{
    [JsonPropertyName("device_ids")]
    public IEnumerable<int> DeviceIds { get; set; } = default!;

    [JsonPropertyName("access_level_ids")]
    public IEnumerable<int> AccessLevelIds { get; set; } = default!;
}
