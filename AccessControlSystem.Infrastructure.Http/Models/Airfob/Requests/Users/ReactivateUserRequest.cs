using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class ReactivateUserRequest
{
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    public string Mobile { get; set; } = default!;
    public string Email { get; set; } = default!;

    [JsonPropertyName("certify_by")]
    public string CertifyBy { get; set; } = "none";

    [JsonPropertyName("use_site_template")]
    public bool UseSiteTemplate { get; set; } = true;
}
