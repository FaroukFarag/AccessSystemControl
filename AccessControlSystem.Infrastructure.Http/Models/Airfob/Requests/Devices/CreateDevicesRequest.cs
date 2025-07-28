namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Devices;

public class CreateDevicesRequest
{
    public IEnumerable<CreateDeviceRequest> Devices { get; set; } = default!;
}
