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
            .MaximumLength(50);
    }
}
