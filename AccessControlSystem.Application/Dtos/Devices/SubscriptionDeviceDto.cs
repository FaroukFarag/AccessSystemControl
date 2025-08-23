namespace AccessControlSystem.Application.Dtos.Devices;

public class SubscriptionDeviceDto
{
    public string DeviceName { get; set; } = default!;
    public DateOnly StartDate { get; set; } = default!;
    public DateOnly EndDate { get; set; } = default!;
    public string RemainingPeriod { get; set; } = default!;
}
