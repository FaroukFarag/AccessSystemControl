using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Visitors;

public class InviteVisitorRequest
{
    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    public string Name { get; set; } = default!;
    public string Mobile { get; set; } = default!;
    public string Purpose { get; set; } = default!;

    [JsonPropertyName("invite_msg")]
    public string InviteMessage { get; set; } = default!;

    [JsonPropertyName("certify_by")]
    public string CertifyBy { get; set; } = default!;

    [JsonPropertyName("use_site_template")]
    public bool UseSiteTemplate { get; set; }

    [JsonPropertyName("access_levels")]
    public IEnumerable<AccessLevel> AccessLevels { get; set; } = default!;
}
