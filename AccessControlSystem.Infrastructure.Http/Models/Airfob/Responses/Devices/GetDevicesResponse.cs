namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class GetDevicesResponse
{
    public int Total { get; set; }
    public IEnumerable<GetDeviceResponse> Devices { get; set; } = default!;
}
