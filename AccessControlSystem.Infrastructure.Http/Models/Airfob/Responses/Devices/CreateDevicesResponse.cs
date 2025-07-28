namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class CreateDevicesResponse
{
    public IEnumerable<CreateDeviceResponse> Success { get; set; } = default!;
}
