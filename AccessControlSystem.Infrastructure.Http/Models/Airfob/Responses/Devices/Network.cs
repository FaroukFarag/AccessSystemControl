using System.Text.Json.Serialization;

namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Network
{
    public string Gateway { get; set; } = default!;

    [JsonPropertyName("use_dhcp")]
    public bool UseDhcp { get; set; } = default!;

    [JsonPropertyName("ip_address")]
    public string IpAddress { get; set; } = default!;

    [JsonPropertyName("subnet_mask")]
    public string SubnetMask { get; set; } = default!;

    [JsonPropertyName("dns_server_address")]
    public string DnsServerAddress { get; set; } = default!;
}
