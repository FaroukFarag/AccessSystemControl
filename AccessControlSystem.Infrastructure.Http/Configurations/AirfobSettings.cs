namespace AccessControlSystem.Infrastructure.Http.Configurations;

public class AirfobSettings
{
    public const string SectionName = "AirfobSettings";

    public string BaseUrl { get; set; } = string.Empty;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
