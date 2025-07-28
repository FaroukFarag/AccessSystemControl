using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Integrity
{
    [JsonPropertyName("mmc_EOL")]
    public string MmcEOL { get; set; } = default!;

    [JsonPropertyName("mmc_lifeA")]
    public string MmcLifeA { get; set; } = default!;

    [JsonPropertyName("mmc_lifeB")]
    public string MmcLifeB { get; set; } = default!;
}
