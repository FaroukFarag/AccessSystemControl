using AccessControlSystem.Application.Dtos.Devices;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.Devices;

public class DeviceDtoValidator : AbstractValidator<DeviceDto>
{
    public DeviceDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(d => d.MacAddress)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$|^([0-9A-Fa-f]{4}\.){2}([0-9A-Fa-f]{4})$");
    }
}
