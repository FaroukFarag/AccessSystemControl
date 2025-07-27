using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Settings
{
    public int Dst { get; set; }
    public Tna Tna { get; set; } = default!;
    public Card Card { get; set; } = default!;
    public Sound Sound { get; set; } = default!;
    public Network Network { get; set; } = default!;
    public Integrity Integrity { get; set; } = default!;

    [JsonPropertyName("sio2_conf")]
    public Sio2Conf Sio2Conf { get; set; } = default!;

    [JsonPropertyName("walk_through")]
    public bool WalkThrough { get; set; }

    [JsonPropertyName("automatic_door")]
    public bool AutomaticDoor { get; set; }

    [JsonPropertyName("rssi_calibration")]
    public int RssiCalibration { get; set; }

    [JsonPropertyName("wiegand_ignore_guard_time")]
    public bool WiegandIgnoreGuardTime { get; set; }
}
