namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceTrafficDto
{
    public string TrafficType { get; set; } = default!;
    public TimeOnly Time { get; set; }
    public DateOnly Date { get; set; }
    public string MacAddress { get; set; } = default!;
    public string ImagePath { get; set; } = default!;
}
