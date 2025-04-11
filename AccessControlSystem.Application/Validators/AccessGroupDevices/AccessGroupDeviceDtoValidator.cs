using AccessControlSystem.Application.Dtos.AccessGroupDevices;
using FluentValidation;

namespace AccessControlSystem.Application.Validators.AccessGroupDevices;

public class AccessGroupDeviceDtoValidator : AbstractValidator<AccessGroupDeviceDto>
{
    public AccessGroupDeviceDtoValidator()
    {
    }
}
