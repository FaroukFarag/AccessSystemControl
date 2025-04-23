using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Devices;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Application.Dtos.Devices;

public class DeviceDto : BaseModelDto<int>
{
    public string? ImagePath { get; set; }
    public string? ImageEncode { get; set; }
    public string Name { get; set; } = default!;
    public string MacAddress { get; set; } = default!;
    public DeviceType DeviceType { get; set; }
    public string DeviceTypeName { get; set; } = default!;
    public bool Active { get; set; }

    public IFormFile ImageFile { get; set; } = default!;
}
