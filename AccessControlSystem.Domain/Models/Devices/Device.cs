using AccessControlSystem.Domain.Models.Abstraction;

namespace AccessControlSystem.Domain.Models.Devices;

public class Device : BaseModel<int>
{
    public string Name { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public bool Active { get; set; }
}
