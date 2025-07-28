using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;

public class AssignDevicesToDoorsResponse
{
    [JsonPropertyName("door_id")]
    public int DoorId { get; set; }

    [JsonPropertyName("in_reader_id")]
    public int? InReaderId { get; set; }

    [JsonPropertyName("out_reader_id")]
    public int? OutReaderId { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }
}
