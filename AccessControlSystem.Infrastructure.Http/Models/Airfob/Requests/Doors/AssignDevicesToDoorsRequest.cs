using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;

public class AssignDevicesToDoorsRequest
{
    [JsonPropertyName("door_ids")]
    public IEnumerable<int> DoorIds { get; set; } = default!;

    [JsonPropertyName("in_reader_ids")]
    public IEnumerable<int> InReaderIds { get; set; } = default!;

    [JsonPropertyName("out_reader_ids")]
    public IEnumerable<int> OutReaderIds { get; set; } = default!;
}
