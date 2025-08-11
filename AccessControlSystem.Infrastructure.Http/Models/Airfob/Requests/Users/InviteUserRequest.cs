using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class InviteUserRequest
{
    public string Name { get; set; } = default!;
    public string Mobile { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public long SiteId { get; set; }

    public string Email { get; set; } = default!;

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("access_level_ids")]
    public IEnumerable<int> AccessLevelIds { get; set; } = default!;

    [JsonPropertyName("certify_by")]
    public string CertifyBy { get; set; } = "none";

    [JsonPropertyName("use_site_template")]
    public bool UseSiteTemplate { get; set; } = true;

    [JsonPropertyName("requiredFields")]
    public IEnumerable<string> RequiredFields { get; set; } = [
        "name",
        "mobile"
    ];

    [JsonPropertyName("requiredProperties")]
    public IEnumerable<string> RequiredProperties { get; set; } = [
        "name",
        "mobile"
    ];

    [JsonPropertyName("properties")]
    public object Properties { get; set; } = default!;
}
