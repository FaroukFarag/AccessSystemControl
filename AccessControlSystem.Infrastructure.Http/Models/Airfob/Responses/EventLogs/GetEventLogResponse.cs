using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.EventLogs;

public class GetEventLogResponse
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("visitor_id")]
    public int VisitorId { get; set; }

    [JsonPropertyName("card_number")]
    public string CardNumber { get; set; } = default!;

    [JsonPropertyName("device_serial")]
    public string DeviceSerial { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    public DateTime DateTime { get; set; }
    public string Timezone { get; set; } = default!;
    public string Code { get; set; } = default!;

    [JsonPropertyName("card_type")]
    public string CardType { get; set; } = default!;

    public int Tna { get; set; }
    public int Offset { get; set; }
    public bool Dst { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    public int Battery { get; set; }

    [JsonPropertyName("_id")]
    public string Id { get; set; } = default!;
}
