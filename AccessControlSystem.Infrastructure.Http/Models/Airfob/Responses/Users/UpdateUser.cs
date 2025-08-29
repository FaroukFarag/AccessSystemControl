using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class UpdateUser
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("mobile_id")]
    public string? MobileId { get; set; }

    [JsonPropertyName("user_key")]
    public string UserKey { get; set; } = default!;

    [JsonPropertyName("inviter_user_id")]
    public int? InviterUserId { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Mobile { get; set; } = default!;

    [JsonPropertyName("used_mobile_card")]
    public bool UsedMobileCard { get; set; }

    [JsonPropertyName("mobile_card_id")]
    public string? MobileCardId { get; set; }

    [JsonPropertyName("used_mobile_qr_card")]
    public bool UsedMobileQrCard { get; set; }

    [JsonPropertyName("used_web_qr_card")]
    public bool UsedWebQrCard { get; set; }

    [JsonPropertyName("used_web_link_card")]
    public bool UsedWebLinkCard { get; set; }

    [JsonPropertyName("used_apple_wallet_card")]
    public bool UsedAppleWalletCard { get; set; }

    [JsonPropertyName("used_face_credential")]
    public bool UsedFaceCredential { get; set; }

    public string? Pin { get; set; }

    [JsonPropertyName("used_wiegand_ignore_guard_time")]
    public bool UsedWiegandIgnoreGuardTime { get; set; }

    [JsonPropertyName("card_template_id")]
    public int? CardTemplateId { get; set; }

    [JsonPropertyName("properties")]
    public Dictionary<string, object> Properties { get; set; } = default!;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
