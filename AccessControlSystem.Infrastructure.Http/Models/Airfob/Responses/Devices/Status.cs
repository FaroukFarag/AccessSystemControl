using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Status
{
    public bool Upgrading { get; set; }

    [JsonPropertyName("first_sync")]
    public bool FirstSync { get; set; }

    public bool Online { get; set; }
}
