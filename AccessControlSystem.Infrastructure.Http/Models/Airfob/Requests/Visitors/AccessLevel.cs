using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Visitors;

public class AccessLevel
{
    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("access_level_id")]
    public int AccessLevelId { get; set; }
}
